using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

public static class SettingsPersistence
{
    /// <summary>
    /// Persists <paramref name="draft"/>, makes it the running configuration through <paramref name="activate"/>,
    /// then retires managed sounds that <paramref name="previous"/> or the draft referenced and the new saved
    /// settings do not. Media is retired only after the persisted settings and the active configuration agree
    /// and <paramref name="activate"/> confirms nothing running can still use a removed copy. Without that
    /// confirmation the draft ends with every copy kept (<see cref="ISoundLibrary.CommitSaveKeepingCopies"/>),
    /// and startup cleanup removes the unreferenced ones. When the save or <paramref name="activate"/> fails
    /// nothing is removed: the previously saved media and the draft's imports stay, so the user can retry or
    /// cancel. When the save fails <paramref name="activate"/> is not called.
    /// </summary>
    /// <param name="activate">
    /// Publishes the saved snapshot to the running app, for example through <see cref="ActiveSettings.Publish"/>.
    /// Returns <c>true</c> when no request made under the previous configuration can still play, so its copies
    /// may be removed.
    /// </param>
    /// <returns>Sounds that could not be removed; they stay until the next startup cleanup.</returns>
    /// <exception cref="SettingsStoreException">The settings could not be saved.</exception>
    public static IReadOnlyList<SoundCleanupFailure> Save(
        ISettingsStore store,
        ISoundLibrary library,
        SettingsSnapshot previous,
        SettingsSnapshot draft,
        Func<SettingsSnapshot, bool> activate)
    {
        ArgumentNullException.ThrowIfNull(activate);
        store.Save(draft);
        if (!activate(draft))
        {
            library.CommitSaveKeepingCopies();
            return [];
        }

        return library.CommitSave(previous, draft);
    }
}
