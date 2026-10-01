using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>Receives a settings failure the user must see.</summary>
public interface ISettingsFailureReporter
{
    void SettingsUnreadable(string reason, string? recoveryCopy);
}

/// <summary>
/// The configuration to start with. <see cref="SavedSettingsLoaded"/> is <c>false</c> after a first launch
/// or a load failure; media cleanup must not run then.
/// </summary>
public sealed record StartupSettings(SettingsSnapshot Active, bool SavedSettingsLoaded);

public static class SettingsStartup
{
    /// <summary>
    /// Loads saved settings, or falls back to <see cref="SettingsSnapshot.Default"/> when none exist or they
    /// cannot be used. A load failure is reported. Fallback defaults are never saved here, so a damaged file
    /// is not overwritten until the user saves.
    /// </summary>
    public static StartupSettings Load(ISettingsStore store, ISettingsFailureReporter failures)
    {
        switch (store.Load())
        {
            case SettingsLoadResult.Loaded loaded:
                return new StartupSettings(loaded.Snapshot, SavedSettingsLoaded: true);
            case SettingsLoadResult.Unreadable unreadable:
                failures.SettingsUnreadable(unreadable.Reason, unreadable.RecoveryCopy);
                return new StartupSettings(SettingsSnapshot.Default, SavedSettingsLoaded: false);
            default:
                return new StartupSettings(SettingsSnapshot.Default, SavedSettingsLoaded: false);
        }
    }

    /// <summary>
    /// Removes managed sounds the loaded settings do not reference. Skipped after a first launch or a load
    /// failure, so media referenced by a damaged settings file survives until the user recovers or saves.
    /// </summary>
    public static IReadOnlyList<SoundCleanupFailure> RemoveOrphanSounds(StartupSettings startup, ISoundLibrary library) =>
        startup.SavedSettingsLoaded ? library.RemoveOrphans(startup.Active) : [];
}
