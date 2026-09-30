using System.Diagnostics.CodeAnalysis;
using System.IO;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Maps a <see cref="ManagedSoundId"/> to its file in the managed library. The only place that knows an
/// identity is stored as a file name directly inside the library directory. An identity that is not such
/// a name (a path, <c>..</c>, a device name, or invalid characters) resolves to nothing, so a hand-edited
/// settings file cannot point outside the library.
/// </summary>
public sealed class ManagedSoundLocator
{
    private static readonly HashSet<string> DeviceNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "CON", "PRN", "AUX", "NUL",
        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
    };

    public ManagedSoundLocator(string directory) => Directory = Path.GetFullPath(directory);

    /// <summary><c>%LOCALAPPDATA%\drawEM\media</c> for the current Windows user.</summary>
    public static string DefaultDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "drawEM", "media");

    public string Directory { get; }

    /// <summary>
    /// Returns the library path for <paramref name="id"/>. Does not check that the file exists.
    /// </summary>
    public bool TryResolve(ManagedSoundId id, [NotNullWhen(true)] out string? path)
    {
        path = null;
        var name = id.Value;
        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            name.EndsWith('.') || name.EndsWith(' ') || name.StartsWith(' ') ||
            DeviceNames.Contains(Path.GetFileNameWithoutExtension(name)))
        {
            return false;
        }

        var candidate = Path.GetFullPath(Path.Combine(Directory, name));
        if (!string.Equals(Path.GetDirectoryName(candidate), Directory, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        path = candidate;
        return true;
    }
}
