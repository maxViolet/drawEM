using DrawEM.App.Application.Drawing;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>Outcome of <see cref="SettingsSaveOperation.Save"/>.</summary>
public abstract record SettingsSaveResult
{
    private SettingsSaveResult()
    {
    }

    /// <summary>The snapshot was persisted and is now the active configuration.</summary>
    public sealed record Saved(SettingsSnapshot Snapshot) : SettingsSaveResult;

    /// <summary>The draft is invalid. Nothing was persisted and nothing running changed.</summary>
    public sealed record Invalid(IReadOnlyList<SettingsError> Errors) : SettingsSaveResult;

    /// <summary>
    /// The settings could not be persisted. The failure was reported and the previous configuration stays
    /// active: sound, drawing, shortcuts, and saved media.
    /// </summary>
    public sealed record NotSaved(string Reason) : SettingsSaveResult;
}

/// <summary>Receives a Save failure the user must see.</summary>
public interface ISettingsSaveFailureReporter
{
    void SettingsNotSaved(string reason);
}

/// <summary>
/// The keyboard shortcuts of the running app. Call every member on the thread that runs the keyboard hook
/// callbacks, so no key event interleaves with a change.
/// </summary>
public interface IActiveShortcuts
{
    /// <summary>
    /// Ends every press in progress: closes the drawing gate, drops drawing commands queued but not yet run,
    /// and ignores keys until all held keys are released, so the next command needs a fresh press. The
    /// caller resets the drawing controller, because a dropped command may have been an exit.
    /// </summary>
    void Interrupt();

    /// <summary>Replaces the shortcuts and their sound commands with those of <paramref name="configuration"/>.</summary>
    void Bind(ActiveConfiguration configuration);
}

/// <summary>
/// Saves a settings draft and makes it the running configuration. Call <see cref="Save"/> on the thread that
/// runs the keyboard hook callbacks and the drawing controller (the WPF UI thread), never on the sound thread.
/// </summary>
public sealed class SettingsSaveOperation
{
    private readonly ISettingsStore store;
    private readonly ISoundLibrary library;
    private readonly ActiveSettings settings;
    private readonly Func<bool> stopSound;
    private readonly IActiveShortcuts shortcuts;
    private readonly DrawingSessionController drawing;
    private readonly ISettingsSaveFailureReporter failures;
    private readonly Action<SoundCleanupFailure> reportCleanup;
    private readonly Action reportUnconfirmedStop;

    /// <param name="stopSound">
    /// Stops the global sound channel within a bounded wait, for example SoundChannelHost.Stop. Before it
    /// returns, every play request made earlier is barred from starting playback, so none plays under the
    /// new configuration. Returns <c>true</c> once those requests have ended and the active player is
    /// released, <c>false</c> when that was not confirmed in time.
    /// </param>
    /// <param name="reportCleanup">Records a managed sound the Save could not remove.</param>
    /// <param name="reportUnconfirmedStop">Records that the stop was not confirmed, so no copy was removed.</param>
    public SettingsSaveOperation(
        ISettingsStore store,
        ISoundLibrary library,
        ActiveSettings settings,
        Func<bool> stopSound,
        IActiveShortcuts shortcuts,
        DrawingSessionController drawing,
        ISettingsSaveFailureReporter failures,
        Action<SoundCleanupFailure> reportCleanup,
        Action reportUnconfirmedStop)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(library);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(stopSound);
        ArgumentNullException.ThrowIfNull(shortcuts);
        ArgumentNullException.ThrowIfNull(drawing);
        ArgumentNullException.ThrowIfNull(failures);
        ArgumentNullException.ThrowIfNull(reportCleanup);
        ArgumentNullException.ThrowIfNull(reportUnconfirmedStop);
        this.store = store;
        this.library = library;
        this.settings = settings;
        this.stopSound = stopSound;
        this.shortcuts = shortcuts;
        this.drawing = drawing;
        this.failures = failures;
        this.reportCleanup = reportCleanup;
        this.reportUnconfirmedStop = reportUnconfirmedStop;
    }

    /// <summary>
    /// Validates the complete draft, persists it, then stops sound, ends drawing, clears every monitor, and
    /// publishes it. No play request made before the Save starts afterwards, and every command needs a fresh
    /// press, even when only an unrelated setting changed. Copies the new settings no longer reference are
    /// removed only when the sound stop was confirmed; otherwise, because an old request may still hold a
    /// copy open, the unconfirmed stop is reported, every copy stays until the next startup cleanup, and the
    /// result is still <see cref="SettingsSaveResult.Saved"/>. An invalid draft or a failed persist changes
    /// nothing running.
    /// </summary>
    public SettingsSaveResult Save(DrawingStyle style, Shortcut? draw, Shortcut? clear, IEnumerable<ActionSlot> slots)
    {
        var validation = SettingsSnapshot.Validate(style, draw, clear, slots);
        if (validation.Snapshot is not { } draft)
        {
            return new SettingsSaveResult.Invalid(validation.Errors);
        }

        IReadOnlyList<SoundCleanupFailure> cleanupFailures;
        try
        {
            cleanupFailures = SettingsPersistence.Save(store, library, settings.Current.Snapshot, draft, Activate);
        }
        catch (SettingsStoreException exception)
        {
            failures.SettingsNotSaved(exception.Message);
            return new SettingsSaveResult.NotSaved(exception.Message);
        }

        foreach (var failure in cleanupFailures)
        {
            reportCleanup(failure);
        }

        return new SettingsSaveResult.Saved(draft);
    }

    /// <summary>Runs only after the snapshot was persisted.</summary>
    /// <returns>Whether the copies of the previous configuration may be removed.</returns>
    private bool Activate(SettingsSnapshot saved)
    {
        var stopped = stopSound();
        shortcuts.Interrupt();
        drawing.ClearAndExitDrawMode();
        shortcuts.Bind(settings.Publish(saved));
        if (!stopped)
        {
            reportUnconfirmedStop();
        }

        return stopped;
    }
}
