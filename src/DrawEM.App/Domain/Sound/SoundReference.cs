namespace DrawEM.App.Domain.Sound;

/// <summary>
/// A managed library copy of a WAV or MP3 file. <see cref="LibraryFileName"/> names the copy inside the
/// library (never a path); <see cref="DisplayName"/> is the original file name shown to the user.
/// </summary>
public sealed record SoundReference
{
    /// <exception cref="ArgumentException">A name is blank, or the library name contains a path.</exception>
    public SoundReference(string libraryFileName, string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(libraryFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        if (ContainsPath(libraryFileName))
        {
            throw new ArgumentException("Library file name must not contain a path.", nameof(libraryFileName));
        }

        LibraryFileName = libraryFileName;
        DisplayName = displayName;
    }

    private static bool ContainsPath(string name) =>
        name.Contains('\\') || name.Contains('/') || name.Contains(':') || name is "." or "..";

    public string LibraryFileName { get; }

    public string DisplayName { get; }
}
