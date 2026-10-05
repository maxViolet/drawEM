using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>
/// drawEM's managed copies of user-selected WAV and MP3 files. Importing copies the source; the source
/// file is never moved or deleted. Identical content with the same extension shares one copy, so several
/// slots may reference it. Imports belong to the open draft until <see cref="CommitSave"/> or
/// <see cref="DiscardDraft"/>. Removal
/// never touches a copy referenced by the saved snapshot passed in, and never throws: each copy that
/// cannot be removed is returned as a failure and left for the next startup cleanup.
/// </summary>
public interface ISoundLibrary
{
    /// <param name="sourceFile">The user's selected file, as the file picker reports it.</param>
    /// <exception cref="SoundImportException">The file is not a WAV or MP3, or the copy failed.</exception>
    SoundReference Import(string sourceFile);

    /// <summary>
    /// Cancel: removes this draft's imports that <paramref name="saved"/> does not reference.
    /// </summary>
    IReadOnlyList<SoundCleanupFailure> DiscardDraft(SettingsSnapshot saved);

    /// <summary>
    /// Ends this draft after <paramref name="saved"/> was persisted. With
    /// <paramref name="removeUnreferencedCopies"/>, removes copies that <paramref name="previous"/> or this
    /// draft referenced and <paramref name="saved"/> does not; otherwise removes nothing and leaves them to
    /// startup cleanup.
    /// </summary>
    IReadOnlyList<SoundCleanupFailure> CommitSave(
        SettingsSnapshot previous, SettingsSnapshot saved, bool removeUnreferencedCopies);

    /// <summary>
    /// Startup: removes every managed copy that <paramref name="saved"/> does not reference, including
    /// draft imports left by a forced exit. Call only with settings that were loaded from disk; see
    /// <see cref="SettingsStartup.RemoveOrphanSounds"/>.
    /// </summary>
    IReadOnlyList<SoundCleanupFailure> RemoveOrphans(SettingsSnapshot saved);
}

/// <summary>
/// A managed copy that could not be removed; it stays in the library. <see cref="LibraryFileName"/> is
/// <c>null</c> when the library itself could not be listed.
/// </summary>
public sealed record SoundCleanupFailure(string? LibraryFileName, string Reason);

/// <summary>Plays a draft sound on the global sound channel without changing active shortcuts.</summary>
public interface ISoundSampler
{
    void Sample(SoundReference sound);
}

public sealed class SoundImportException : Exception
{
    public SoundImportException(string message)
        : base(message)
    {
    }

    public SoundImportException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
