using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

public static class SettingsPersistence
{
    /// <summary>
    /// Persists <paramref name="draft"/>, then retires managed sounds that <paramref name="previous"/> or the
    /// draft referenced and the new saved settings do not. When the save fails nothing is removed: the
    /// previously saved media and the draft's imports stay, so the user can retry or cancel.
    /// </summary>
    /// <returns>Sounds that could not be removed; they stay until the next startup cleanup.</returns>
    /// <exception cref="SettingsStoreException">The settings could not be saved.</exception>
    public static IReadOnlyList<SoundCleanupFailure> Save(
        ISettingsStore store,
        ISoundLibrary library,
        SettingsSnapshot previous,
        SettingsSnapshot draft)
    {
        store.Save(draft);
        return library.CommitSave(previous, draft);
    }
}
