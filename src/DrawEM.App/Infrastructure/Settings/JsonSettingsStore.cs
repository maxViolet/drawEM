using System.Globalization;
using System.IO;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Infrastructure.Settings;

/// <summary>
/// Stores <c>settings.json</c> in one directory. Save writes a flushed temporary file, then moves it over
/// the saved file, so a failed save leaves the previous file intact. Load never writes the settings file;
/// when it cannot use the file it keeps a timestamped copy of the bytes beside it.
/// </summary>
public sealed class JsonSettingsStore : ISettingsStore
{
    public const string FileName = "settings.json";
    private const string TempSuffix = ".tmp";
    private const string RecoveryPattern = "settings.unreadable-*.json";

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
            WriteFlushed(temp, SettingsJson.Serialize(snapshot), FileMode.Create);
            File.Move(temp, SettingsPath, overwrite: true);
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            TryDelete(temp);
            throw new SettingsStoreException($"Settings could not be saved: {failure.Message}", failure);
        }
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
                .FirstOrDefault(path => new FileInfo(path).Length == data.Length &&
                    File.ReadAllBytes(path).AsSpan().SequenceEqual(data));
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }

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
