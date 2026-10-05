using DrawEM.App.Application.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.App.Infrastructure.Settings;

/// <summary>
/// Shows a failed Save to the user and records sound copy cleanup failures and an unconfirmed sound stop in
/// the sound log.
/// </summary>
public sealed class SettingsSaveNotifications(
    SettingsFailureDialog dialog,
    LoggingSoundFailureReporter soundLog,
    ManagedSoundLibrary library) : ISettingsSaveNotifications
{
    public void NotSaved(string reason) => dialog.SettingsNotSaved(reason);

    /// <remarks>Also used for startup cleanup failures.</remarks>
    public void CleanupFailed(SoundCleanupFailure failure) => soundLog.ReportCleanup(
        failure.LibraryFileName is { } name ? library.PathFor(name) : library.LibraryDirectory, failure.Reason);

    public void StopUnconfirmed() => soundLog.ReportUnconfirmedStop();
}
