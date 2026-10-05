using DrawEM.App.Application.Sound;
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
    /// The settings were not persisted. The failure was reported and the previous configuration stays
    /// active: sound, drawing, shortcuts, and saved media.
    /// </summary>
    public sealed record NotSaved(string Reason) : SettingsSaveResult;
}

/// <summary>What a Save reports.</summary>
public interface ISettingsSaveNotifications
{
    /// <summary>Nothing was saved; the user must see why.</summary>
    void NotSaved(string reason);

    /// <summary>A managed copy could not be removed; it stays until the next startup cleanup.</summary>
    void CleanupFailed(SoundCleanupFailure failure);

    /// <summary>The sound stop was not confirmed in time, so the Save removed no copy.</summary>
    void StopUnconfirmed();
}

/// <summary>
/// The keyboard shortcuts of the running app. Call every member on the thread that runs the keyboard hook
/// callbacks and the drawing controller, so no key event interleaves with a change.
/// </summary>
public interface IActiveShortcuts
{
    /// <summary>
    /// Ends every press in progress: closes the drawing gate, drops drawing commands queued but not yet run,
    /// ends drawing and clears strokes on every monitor, and ignores keys until all held keys are released,
    /// so the next command needs a fresh press.
    /// </summary>
    void Interrupt();

    /// <summary>Replaces the shortcuts with those of <paramref name="snapshot"/>.</summary>
    /// <param name="commandFor">Builds the play command for each of the snapshot's sounds.</param>
    void Bind(SettingsSnapshot snapshot, Func<SoundReference, PlaySoundCommand> commandFor);
}

/// <summary>The global sound channel as a Save drives it. Call every member outside the sound thread.</summary>
public interface ISaveSoundChannel
{
    /// <summary>
    /// Waits a bounded time until no play request is starting its player, then keeps every request from
    /// starting until the hold is disposed. Dispose the hold on the thread that took it.
    /// </summary>
    /// <returns>The hold, or <c>null</c> when a start did not finish in time.</returns>
    ISoundStartHold? TryHoldStarts();

    /// <summary>
    /// Stops the active sound on the sound thread after every earlier queued request, within a bounded wait.
    /// </summary>
    /// <returns>
    /// <c>true</c> when the stop ran; <c>false</c> when it was not confirmed in time. A timed-out stop stays
    /// queued and still runs before any later request.
    /// </returns>
    bool Stop();
}

/// <summary>Keeps play requests from starting their players until disposed. Disposing again does nothing.</summary>
public interface ISoundStartHold : IDisposable
{
    /// <summary>Ends every play request made so far: none of them will ever start its player.</summary>
    void EndEarlierRequests();
}

/// <summary>
/// Saves a settings draft and makes it the running configuration. Call <see cref="Save"/> on the thread that
/// runs the keyboard hook callbacks and the drawing controller (the WPF UI thread), never on the sound thread.
/// </summary>
public sealed class SettingsSaveOperation
{
    /// <summary>The reason a Save reports when a sound was still starting and nothing was saved.</summary>
    public const string SoundStartingReason =
        "A sound was still starting, so nothing was saved. Try again.";

    private readonly ISettingsStore store;
    private readonly ISoundLibrary library;
    private readonly ActiveSettings settings;
    private readonly ISaveSoundChannel sound;
    private readonly IActiveShortcuts shortcuts;
    private readonly ISettingsSaveNotifications notifications;

    public SettingsSaveOperation(
        ISettingsStore store,
        ISoundLibrary library,
        ActiveSettings settings,
        ISaveSoundChannel sound,
        IActiveShortcuts shortcuts,
        ISettingsSaveNotifications notifications)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(library);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(sound);
        ArgumentNullException.ThrowIfNull(shortcuts);
        ArgumentNullException.ThrowIfNull(notifications);
        this.store = store;
        this.library = library;
        this.settings = settings;
        this.sound = sound;
        this.shortcuts = shortcuts;
        this.notifications = notifications;
    }

    /// <summary>
    /// Validates the draft, persists it, then stops sound, ends drawing on every monitor, publishes the
    /// snapshot, and rebinds shortcuts, so every command needs a fresh press.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>The snapshot is persisted before anything running changes. An invalid draft, a failed write, or a
    /// sound start that does not finish in time changes nothing.</item>
    /// <item>Sound starts are held from before the write until every earlier play request is ended, so no
    /// request made before the Save starts playback afterwards.</item>
    /// <item>Copies no longer referenced are removed only after the sound stop was confirmed, because an old
    /// request may still hold a copy open. Otherwise the result is still <see cref="SettingsSaveResult.Saved"/>
    /// and startup cleanup removes them.</item>
    /// </list>
    /// </remarks>
    public SettingsSaveResult Save(DrawingStyle style, Shortcut? draw, Shortcut? clear, IEnumerable<ActionSlot> slots)
    {
        var validation = SettingsSnapshot.Validate(style, draw, clear, slots);
        if (validation.Snapshot is not { } draft)
        {
            return new SettingsSaveResult.Invalid(validation.Errors);
        }

        var hold = sound.TryHoldStarts();
        if (hold is null)
        {
            return NotSaved(SoundStartingReason);
        }

        var previous = settings.Current;
        try
        {
            store.Save(draft);
            hold.EndEarlierRequests();
        }
        catch (SettingsStoreException exception)
        {
            return NotSaved(exception.Message);
        }
        finally
        {
            // Released before the stop: the sound thread may need it to drop an ended request.
            hold.Dispose();
        }

        var stopped = sound.Stop();
        shortcuts.Interrupt();
        settings.Publish(draft);
        shortcuts.Bind(draft, settings.CommandFor);
        if (!stopped)
        {
            notifications.StopUnconfirmed();
        }

        foreach (var failure in library.CommitSave(previous, draft, removeUnreferencedCopies: stopped))
        {
            notifications.CleanupFailed(failure);
        }

        return new SettingsSaveResult.Saved(draft);
    }

    private SettingsSaveResult.NotSaved NotSaved(string reason)
    {
        notifications.NotSaved(reason);
        return new SettingsSaveResult.NotSaved(reason);
    }
}
