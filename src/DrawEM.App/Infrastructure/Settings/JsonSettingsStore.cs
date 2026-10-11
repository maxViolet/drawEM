using System.Globalization;
using System.IO;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Infrastructure.Settings;

/// <summary>
/// Stores <c>settings.json</c> in one directory. Save writes a flushed temporary file, then moves it over
/// the saved file, so a failed save leaves the previous file intact. Before Save replaces a valid schema 1
/// file it keeps the file's bytes as a backup for rollback; the first backup, <see cref="Schema1BackupFileName"/>,
/// is never overwritten. Load never writes the settings file; when it cannot use the file it keeps a
/// timestamped copy of the bytes beside it.
/// </summary>
public sealed class JsonSettingsStore : ISettingsStore
{
    public const string FileName = "settings.json";

    /// <summary>The first schema 1 file a Save replaced, for rollback.</summary>
    public const string Schema1BackupFileName = "settings.schema1-backup.json";

    private const string TempSuffix = ".tmp";
    private const string RecoveryPattern = "settings.unreadable-*.json";
    private const string NumberedBackupPrefix = "settings.schema1-backup-";

    private readonly string directory;
    private readonly TimeProvider time;

    public JsonSettingsStore(string directory, TimeProvider? time = null)
    {
        this.directory = Path.GetFullPath(directory);
        this.time = time ?? TimeProvider.System;
    }

    /// <summary><c>%LOCALAPPDATA%\drawEM</c> for the current Windows user.</summary>
    public static string DefaultDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "drawEM");

    public string SettingsPath => Path.Combine(directory, FileName);

    public SettingsLoadResult Load()
    {
        byte[] data;
        try
        {
            data = File.ReadAllBytes(SettingsPath);
        }
        catch (Exception failure) when (failure is FileNotFoundException or DirectoryNotFoundException)
        {
            return new SettingsLoadResult.Missing();
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            return new SettingsLoadResult.Unreadable($"Settings file cannot be read: {failure.Message}", null);
        }

        try
        {
            return new SettingsLoadResult.Loaded(SettingsJson.Deserialize(data));
        }
        catch (SettingsFormatException failure)
        {
            return new SettingsLoadResult.Unreadable(failure.Message, TryKeepRecoveryCopy(data));
        }
    }

    public void Save(SettingsSnapshot snapshot)
    {
        var temp = SettingsPath + TempSuffix;
        try
        {
            Directory.CreateDirectory(directory);
            BackUpSchema1File();
            WriteFlushed(temp, SettingsJson.Serialize(snapshot), FileMode.Create);
            File.Move(temp, SettingsPath, overwrite: true);
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            TryDelete(temp);
            throw new SettingsStoreException($"Settings could not be saved: {failure.Message}", failure);
        }
    }

    /// <summary>
    /// Keeps the current file's bytes before Save replaces a valid schema 1 file. The first backup is
    /// <see cref="Schema1BackupFileName"/> (number 1); each later one is <c>settings.schema1-backup-N.json</c>
    /// with the next number, so the highest number always holds the file the latest migration replaced.
    /// Nothing is copied when the highest-numbered backup already has these bytes. No backup is overwritten,
    /// and each appears complete or not at all.
    /// </summary>
    /// <exception cref="SettingsStoreException">The backup could not be made; nothing was saved.</exception>
    private void BackUpSchema1File()
    {
        byte[] current;
        try
        {
            current = File.ReadAllBytes(SettingsPath);
        }
        catch (FileNotFoundException)
        {
            return;
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            throw new SettingsStoreException(
                $"Settings could not be saved: the current settings could not be read for a backup: {failure.Message}",
                failure);
        }

        if (!SettingsJson.IsValidSchema1(current))
        {
            return;
        }

        string backup;
        try
        {
            var (latest, number) = LatestSchema1Backup();
            if (latest is not null && SameBytes(latest, current))
            {
                return;
            }

            backup = number == 0
                ? Path.Combine(directory, Schema1BackupFileName)
                : Path.Combine(directory, $"{NumberedBackupPrefix}{(number + 1).ToString(CultureInfo.InvariantCulture)}.json");
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            throw new SettingsStoreException(
                $"Settings could not be saved: the schema 1 backups could not be read: {failure.Message}",
                failure);
        }

        var temp = backup + TempSuffix;
        try
        {
            WriteFlushed(temp, current, FileMode.Create);
            File.Move(temp, backup, overwrite: false);
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            TryDelete(temp);
            throw new SettingsStoreException(
                $"Settings could not be saved: the schema 1 backup {backup} could not be created: {failure.Message}",
                failure);
        }
    }

    /// <summary>The highest-numbered schema 1 backup and its number, or <c>(null, 0)</c> when there is none.</summary>
    private (string? Path, int Number) LatestSchema1Backup()
    {
        var first = Path.Combine(directory, Schema1BackupFileName);
        (string? Path, int Number) latest = File.Exists(first) ? (first, 1) : (null, 0);
        foreach (var path in Directory.EnumerateFiles(directory, NumberedBackupPrefix + "*.json"))
        {
            var suffix = Path.GetFileNameWithoutExtension(path)[NumberedBackupPrefix.Length..];
            if (int.TryParse(suffix, NumberStyles.None, CultureInfo.InvariantCulture, out var number) &&
                number > latest.Number)
            {
                latest = (path, number);
            }
        }

        return latest;
    }

    private string? TryKeepRecoveryCopy(byte[] data)
    {
        var existing = FindRecoveryCopy(data);
        if (existing is not null)
        {
            return existing;
        }

        var stamp = time.GetUtcNow().ToString("yyyyMMdd'T'HHmmssfff'Z'", CultureInfo.InvariantCulture);
        for (var attempt = 0; attempt < 100; attempt++)
        {
            var suffix = attempt == 0 ? "" : "-" + attempt.ToString(CultureInfo.InvariantCulture);
            var path = Path.Combine(directory, $"settings.unreadable-{stamp}{suffix}.json");
            try
            {
                WriteFlushed(path, data, FileMode.CreateNew);
                return path;
            }
            catch (IOException) when (File.Exists(path))
            {
                // Another copy has this name; try the next one.
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
            {
                return null;
            }
        }

        return null;
    }

    /// <summary>Reuses a copy of the same bytes, so repeated launches with one damaged file keep one copy.</summary>
    private string? FindRecoveryCopy(byte[] data)
    {
        try
        {
            return Directory.EnumerateFiles(directory, RecoveryPattern)
                .FirstOrDefault(path => SameBytes(path, data));
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    private static bool SameBytes(string path, byte[] data) =>
        new FileInfo(path).Length == data.Length && File.ReadAllBytes(path).AsSpan().SequenceEqual(data);

    private static void WriteFlushed(string path, byte[] data, FileMode mode)
    {
        using var stream = new FileStream(path, mode, FileAccess.Write, FileShare.None);
        stream.Write(data);
        stream.Flush(flushToDisk: true);
    }

    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            // The next save overwrites a leftover temporary file.
        }
    }
}
