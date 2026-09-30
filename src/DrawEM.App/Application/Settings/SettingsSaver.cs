using DrawEM.App.Application.Input;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

public sealed record SaveSettingsResult(bool Succeeded, string? Error)
{
    public static SaveSettingsResult Saved { get; } = new(true, null);

    public static SaveSettingsResult Failed(string error) => new(false, error);
}

/// <summary>
/// The only way the Settings window applies a configuration. A save either changes nothing (and reports
/// why) or completes every step in order.
/// </summary>
public interface ISettingsSaver
{
    SaveSettingsResult Save(SettingsSnapshot snapshot);
}

/// <summary>Stops the global sound channel's current playback. Must return promptly and not throw.</summary>
public interface ISoundStopper
{
    void StopSound();
}

/// <summary>
/// Closes the drawing input gate, ends any active stroke, and clears strokes on every monitor.
/// Must return promptly and not throw.
/// </summary>
public interface IDrawingReset
{
    void ExitDrawingAndClearAllMonitors();
}

/// <summary>The configuration that runtime consumers read. Changes only through <see cref="SettingsSaver"/>.</summary>
public sealed class ActiveSettings(SettingsSnapshot initial)
{
    public SettingsSnapshot Current { get; private set; } = initial;

    internal void Publish(SettingsSnapshot snapshot) => Current = snapshot;
}

/// <summary>
/// Save in the order the v3 contract requires: persist durably, stop sound, exit drawing and clear every
/// monitor, then require fresh key presses and publish the new snapshot. A persistence failure stops
/// before any runtime change. Call on the thread that runs the keyboard hook, so no key event is decided
/// between publication and the fresh-press boundary.
/// </summary>
public sealed class SettingsSaver(
    ISettingsStore store,
    ISoundStopper sound,
    IDrawingReset drawing,
    ShortcutDecisionEngine shortcuts,
    ActiveSettings active) : ISettingsSaver
{
    public SaveSettingsResult Save(SettingsSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        try
        {
            store.Save(snapshot);
        }
        catch (SettingsStoreException failure)
        {
            return SaveSettingsResult.Failed(failure.Message);
        }

        // The file now holds the new snapshot. Activate it even if a runtime step throws, so the running
        // configuration never disagrees with the saved one.
        try
        {
            sound.StopSound();
            drawing.ExitDrawingAndClearAllMonitors();
        }
        finally
        {
            shortcuts.RequireFreshPress();
            active.Publish(snapshot);
        }

        return SaveSettingsResult.Saved;
    }
}
