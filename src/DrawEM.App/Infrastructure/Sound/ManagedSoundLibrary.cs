using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure.Settings;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Keeps managed sound copies in one directory. A copy is named by the SHA-256 of its content plus the
/// lower-case source extension, so the name is stable and identical content with the same extension
/// shares one copy; the same bytes imported as <c>.wav</c> and as <c>.mp3</c> make two copies. Import
/// streams the source into a flushed temporary file, then moves it into place; the source is only read.
/// Removal deletes only names this library creates, even when settings name another file, and never a
/// copy the given saved snapshot references.
/// </summary>
public sealed partial class ManagedSoundLibrary : ISoundLibrary
{
    private const string TempPrefix = "import-";
    private const string TempSuffix = ".tmp";

    private readonly string directory;
    private readonly object gate = new();
    private readonly HashSet<string> draftImports = new(StringComparer.OrdinalIgnoreCase);

    public ManagedSoundLibrary(string directory)
    {
        this.directory = Path.GetFullPath(directory);
    }

    /// <summary><c>%LOCALAPPDATA%\drawEM\sounds</c> for the current Windows user.</summary>
    public static string DefaultDirectory { get; } = Path.Combine(JsonSettingsStore.DefaultDirectory, "sounds");

    public string LibraryDirectory => directory;

    /// <summary>The path of the copy named <paramref name="libraryFileName"/>. Pure; touches no file.</summary>
    public string PathFor(string libraryFileName) => Path.Combine(directory, libraryFileName);

    public SoundReference Import(string sourceFile)
    {
        ArgumentNullException.ThrowIfNull(sourceFile);
        var displayName = Path.GetFileName(sourceFile);
        var extension = Path.GetExtension(sourceFile).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(displayName) || extension is not (".wav" or ".mp3"))
        {
            throw new SoundImportException($"'{sourceFile}' is not a WAV or MP3 file.");
        }

        var temp = PathFor(TempPrefix + Guid.NewGuid().ToString("N") + TempSuffix);
        try
        {
            if (IsInLibrary(sourceFile))
            {
                throw new SoundImportException(
                    $"'{displayName}' is already in drawEM's sound library. Select the original file instead.");
            }

            Directory.CreateDirectory(directory);
            var (hash, length) = CopyHashed(sourceFile, temp, displayName);
            var name = hash + extension;
            var target = PathFor(name);
            lock (gate)
            {
                if (!IsIntactCopy(target, hash, length))
                {
                    File.Move(temp, target, overwrite: true);
                }

                draftImports.Add(name);
            }

            return new SoundReference(name, displayName);
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException or NotSupportedException or SecurityException or ArgumentException)
        {
            throw new SoundImportException($"'{displayName}' could not be copied: {failure.Message}", failure);
        }
        finally
        {
            TryDelete(temp);
        }
    }

    public IReadOnlyList<SoundCleanupFailure> DiscardDraft(SettingsSnapshot saved)
    {
        ArgumentNullException.ThrowIfNull(saved);
        lock (gate)
        {
            var candidates = draftImports.ToArray();
            draftImports.Clear();
            return Remove(candidates, References(saved));
        }
    }

    public IReadOnlyList<SoundCleanupFailure> CommitSave(
        SettingsSnapshot previous, SettingsSnapshot saved, bool removeUnreferencedCopies)
    {
        ArgumentNullException.ThrowIfNull(previous);
        ArgumentNullException.ThrowIfNull(saved);
        lock (gate)
        {
            var candidates = References(previous);
            candidates.UnionWith(draftImports);
            draftImports.Clear();
            return removeUnreferencedCopies ? Remove(candidates, References(saved)) : [];
        }
    }

    public IReadOnlyList<SoundCleanupFailure> RemoveOrphans(SettingsSnapshot saved)
    {
        ArgumentNullException.ThrowIfNull(saved);
        lock (gate)
        {
            string[] names;
            try
            {
                if (!Directory.Exists(directory))
                {
                    return [];
                }

                names = Directory.EnumerateFiles(directory)
                    .Select(path => Path.GetFileName(path))
                    .Where(IsManagedName)
                    .ToArray();
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException or SecurityException)
            {
                return [new SoundCleanupFailure(null, $"Sound library cannot be listed: {failure.Message}")];
            }

            var keep = References(saved);
            keep.UnionWith(draftImports);
            return Remove(names, keep);
        }
    }

    private static (string Hash, long Length) CopyHashed(string sourceFile, string temp, string displayName)
    {
        using var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var copy = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        var buffer = new byte[81920];
        long length = 0;
        int read;
        while ((read = source.Read(buffer)) > 0)
        {
            hash.AppendData(buffer, 0, read);
            copy.Write(buffer, 0, read);
            length += read;
        }

        if (length == 0)
        {
            throw new SoundImportException($"'{displayName}' is empty.");
        }

        copy.Flush(flushToDisk: true);
        return (Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant(), length);
    }

    /// <summary>
    /// A file picked from inside the library is a managed copy, not a source; importing it would let Cancel
    /// or cleanup delete the file the user selected.
    /// </summary>
    private bool IsInLibrary(string sourceFile) =>
        string.Equals(
            Path.GetDirectoryName(Path.GetFullPath(sourceFile)),
            directory,
            StringComparison.OrdinalIgnoreCase);

    /// <summary>Reuses an existing copy only when its content still matches its name.</summary>
    private static bool IsIntactCopy(string path, string hash, long length)
    {
        var file = new FileInfo(path);
        if (!file.Exists || file.Length != length)
        {
            return false;
        }

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return Convert.ToHexString(SHA256.HashData(stream)).Equals(hash, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsManagedName(string name) => ManagedName().IsMatch(name) || TempName().IsMatch(name);

    private static HashSet<string> References(SettingsSnapshot snapshot) =>
        snapshot.Slots
            .Select(slot => slot.Action)
            .OfType<SoundAction>()
            .Select(action => action.Sound.LibraryFileName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private List<SoundCleanupFailure> Remove(IEnumerable<string> candidates, HashSet<string> keep)
    {
        var failures = new List<SoundCleanupFailure>();
        foreach (var name in candidates.Where(name => IsManagedName(name) && !keep.Contains(name)))
        {
            try
            {
                File.Delete(PathFor(name));
            }
            catch (DirectoryNotFoundException)
            {
                // Nothing to remove.
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException or SecurityException)
            {
                failures.Add(new SoundCleanupFailure(name, $"Sound copy could not be removed: {failure.Message}"));
            }
        }

        return failures;
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            // Startup cleanup removes a leftover temporary file.
        }
    }

    [GeneratedRegex("^[0-9a-f]{64}\\.(wav|mp3)$", RegexOptions.IgnoreCase)]
    private static partial Regex ManagedName();

    [GeneratedRegex("^import-[0-9a-f]{32}\\.tmp$", RegexOptions.IgnoreCase)]
    private static partial Regex TempName();
}
