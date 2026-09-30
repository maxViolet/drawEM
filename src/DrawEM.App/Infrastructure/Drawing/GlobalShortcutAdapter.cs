using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Input;
using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.App.Infrastructure.Drawing;

/// <summary>
/// Hook side of the shortcuts: translates virtual-key codes for <see cref="ShortcutDecisionEngine"/>,
/// performs its commands against the input gate, cursor, drawing controller, and sound channel, and
/// returns its suppression answer before the hook callback returns.
/// </summary>
public sealed class GlobalShortcutAdapter
{
    private readonly ShortcutDecisionEngine engine;
    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly ICursorPositionSource cursorPositionSource;
    private readonly IMonitorBoundsSource monitorBoundsSource;
    private readonly Action<Action> dispatch;
    private readonly Func<SoundSlot, PlaySoundCommand?> resolveSound;
    private readonly Action<PlaySoundCommand> playSound;

    /// <param name="resolveSound">
    /// Maps a slot to its command, or <c>null</c> for an unassigned slot. Runs inside the hook
    /// callback, so it must only look up the assignment: no file access or player creation.
    /// </param>
    /// <param name="playSound">
    /// Queues the command on the sound thread. Called directly inside the hook callback, so it must
    /// return promptly without file access, decoding, or player creation. Production uses SoundChannelHost.Play.
    /// </param>
    public GlobalShortcutAdapter(
        IKeyboardHookSource source,
        DrawingSessionController controller,
        DrawingModeInputGate inputGate,
        ICursorPositionSource cursorPositionSource,
        Action<Action> dispatch,
        IMonitorBoundsSource monitorBoundsSource,
        Func<SoundSlot, PlaySoundCommand?> resolveSound,
        Action<PlaySoundCommand> playSound)
        : this(
            source,
            new ShortcutDecisionEngine(slot => resolveSound((SoundSlot)slot) is not null),
            controller,
            inputGate,
            cursorPositionSource,
            dispatch,
            monitorBoundsSource,
            resolveSound,
            playSound)
    {
    }

    /// <param name="engine">
    /// The engine shared with <see cref="Application.Settings.SettingsSaver"/>, so Save can require fresh
    /// presses. Its slot lookup must agree with <paramref name="resolveSound"/>.
    /// </param>
    public GlobalShortcutAdapter(
        IKeyboardHookSource source,
        ShortcutDecisionEngine engine,
        DrawingSessionController controller,
        DrawingModeInputGate inputGate,
        ICursorPositionSource cursorPositionSource,
        Action<Action> dispatch,
        IMonitorBoundsSource monitorBoundsSource,
        Func<SoundSlot, PlaySoundCommand?> resolveSound,
        Action<PlaySoundCommand> playSound)
    {
        this.engine = engine;
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
        this.monitorBoundsSource = monitorBoundsSource;
        this.dispatch = dispatch;
        this.resolveSound = resolveSound;
        this.playSound = playSound;
        source.KeyEvent += OnKeyEvent;
    }

    private bool OnKeyEvent(int vkCode, KeyDirection direction)
    {
        var gate = new DrawGateStatus(inputGate.IsActive, inputGate.IsBlockedUntilReleased);
        var decision = engine.Handle(VirtualKeys.ToInputKey(vkCode), direction, gate);
        foreach (var command in decision.Commands)
        {
            Perform(command);
        }

        return decision.Suppress;
    }

    private void Perform(ShortcutCommand command)
    {
        switch (command)
        {
            case ShortcutCommand.BeginDraw:
                if (cursorPositionSource.TryGetCurrentPosition(out var startingPoint)
                    && monitorBoundsSource.TryGetBounds(startingPoint, out var bounds))
                {
                    inputGate.Begin(bounds);
                    dispatch(() => controller.EnterDrawMode(startingPoint, bounds));
                }

                break;
            case ShortcutCommand.EndDraw:
                inputGate.SetActive(false);
                dispatch(controller.ExitDrawMode);
                break;
            case ShortcutCommand.ReleaseDrawBlock:
                inputGate.ReleaseBlock();
                break;
            case ShortcutCommand.ClearMonitor:
                inputGate.SetActive(false);
                inputGate.BlockUntilReleased();
                if (cursorPositionSource.TryGetCurrentPosition(out var clearPoint)
                    && monitorBoundsSource.TryGetBounds(clearPoint, out var clearBounds))
                {
                    dispatch(() => controller.ClearMonitorAndExitDrawMode(clearBounds));
                }
                else
                {
                    dispatch(controller.ExitDrawMode);
                }

                break;
            case ShortcutCommand.PlaySlot play:
                if (resolveSound((SoundSlot)play.Slot) is { } sound)
                {
                    playSound(sound);
                }

                break;
        }
    }
}
