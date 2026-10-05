using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure;

namespace DrawEM.App.Infrastructure.Drawing;

/// <remarks>
/// Hook callbacks, the <see cref="IShortcutCapture"/> calls, and the <see cref="IActiveShortcuts"/> calls
/// must run on the thread that installed the keyboard hook, so they never interleave.
/// </remarks>
public sealed class GlobalShortcutAdapter : IShortcutCapture, IActiveShortcuts
{
    private readonly INeutralKeyEmitter neutralKeys;
    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly ICursorPositionSource cursorPositionSource;
    private readonly IMonitorBoundsSource monitorBoundsSource;
    private readonly Action<Action> dispatch;
    private ShortcutBindings bindings;
    private readonly Action<PlaySoundCommand> playSound;
    private readonly HashSet<int> pressedKeys = [];
    private readonly HashSet<int> startedShortcutKeys = [];

    /// <summary>
    /// Keys whose first key-down was hidden. Their key-up stays hidden even if the input mode changes.
    /// </summary>
    private readonly HashSet<int> keysWithHiddenFirstDown = [];

    /// <summary>Keys that must be released before capture evaluates another chord.</summary>
    private readonly HashSet<int> keysBlockingCapture = [];

    /// <summary>Capture or a Save ended with keys held. No command starts until every key is released.</summary>
    private bool dispatchPaused;

    /// <summary>
    /// Advanced by <see cref="Interrupt"/>. A queued drawing command runs only if it was queued under the
    /// current value. Written on the hook thread, read on the dispatcher thread.
    /// </summary>
    private int drawingEpoch;

    private Action<ShortcutCaptureResult>? reportCapture;

    /// <param name="bindings">The shortcuts to start with, read inside the hook callback. <see cref="Bind"/> replaces them.</param>
    /// <param name="playSound">
    /// Queues the command on the sound thread. Called directly inside the hook callback, so it must
    /// return promptly without file access, decoding, or player creation. Production uses SoundChannelHost.Play.
    /// </param>
    public GlobalShortcutAdapter(
        IKeyboardHookSource source,
        INeutralKeyEmitter neutralKeys,
        DrawingSessionController controller,
        DrawingModeInputGate inputGate,
        ICursorPositionSource cursorPositionSource,
        Action<Action> dispatch,
        IMonitorBoundsSource monitorBoundsSource,
        ShortcutBindings bindings,
        Action<PlaySoundCommand> playSound)
    {
        this.neutralKeys = neutralKeys;
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
        this.monitorBoundsSource = monitorBoundsSource;
        this.dispatch = dispatch;
        this.bindings = bindings;
        this.playSound = playSound;
        source.SetKeyHandler(HandleKey);
    }

    private bool IsCapturing => reportCapture is not null;

    /// <remarks>Held keys keep the suppression decision they already have until they are released.</remarks>
    public void Begin(Action<ShortcutCaptureResult> report)
    {
        ArgumentNullException.ThrowIfNull(report);
        if (inputGate.IsActive)
        {
            inputGate.SetActive(false);
            DispatchDrawing(controller.ExitDrawMode);
        }

        reportCapture = report;
        keysBlockingCapture.Clear();
        keysBlockingCapture.UnionWith(pressedKeys);
    }

    public void End()
    {
        if (IsCapturing)
        {
            StopCapture();
        }
    }

    /// <remarks>Resets the drawing controller directly, so call it on the controller's thread.</remarks>
    public void Interrupt()
    {
        inputGate.SetActive(false);
        inputGate.ReleaseBlock();
        Volatile.Write(ref drawingEpoch, drawingEpoch + 1);
        dispatchPaused = pressedKeys.Count > 0;

        // A dropped command may have been an exit, so drawing is reset here rather than queued.
        controller.ClearAndExitDrawMode();
    }

    /// <remarks>Call after <see cref="Interrupt"/>, so no press started under the previous shortcuts continues.</remarks>
    public void Bind(SettingsSnapshot snapshot, Func<SoundReference, PlaySoundCommand> commandFor) =>
        bindings = ShortcutBindings.FromSnapshot(snapshot, commandFor);

    private bool HandleKey(int vkCode, KeyDirection direction)
    {
        if (direction == KeyDirection.Up)
        {
            var suppressKeyUp = keysWithHiddenFirstDown.Remove(vkCode);
            OnKeyUp(vkCode);
            return suppressKeyUp;
        }

        var isFirstKeyDown = pressedKeys.Add(vkCode);
        var capturedKey = false;
        if (IsCapturing)
        {
            if (isFirstKeyDown)
            {
                capturedKey = Capture(vkCode);
            }
        }
        else if (!dispatchPaused)
        {
            int? firstKeyDown = isFirstKeyDown ? vkCode : null;
            UpdateChords(firstKeyDown);
            UpdateSoundShortcuts(firstKeyDown);
        }

        var suppress = capturedKey || keysWithHiddenFirstDown.Contains(vkCode)
            || (inputGate.IsActive && IsHiddenInDrawMode(vkCode))
            || startedShortcutKeys.Contains(vkCode);
        if (isFirstKeyDown && suppress)
        {
            keysWithHiddenFirstDown.Add(vkCode);
        }

        return suppress;
    }

    private void OnKeyUp(int vkCode)
    {
        pressedKeys.Remove(vkCode);
        startedShortcutKeys.Remove(vkCode);
        keysBlockingCapture.Remove(vkCode);
        var dispatchWasPaused = dispatchPaused;
        dispatchPaused &= pressedKeys.Count > 0;
        if (IsCapturing || dispatchWasPaused)
        {
            return;
        }

        UpdateChords(firstKeyDown: null);
    }

    /// <summary>Evaluates the first key-down of a key while capture is on.</summary>
    /// <remarks>
    /// Escape cancels. Only a letter, digit, or F1–F12 key is a candidate. Win chords and Alt+F4 pass
    /// through untouched. A Right Alt chord passes through and is rejected. Any other candidate is hidden
    /// down and up, then recorded if valid or rejected. A rejected attempt blocks capture until all its keys
    /// are released.
    /// </remarks>
    private bool Capture(int vkCode)
    {
        if (vkCode == VirtualKeys.Escape)
        {
            FinishCapture(new ShortcutCaptureResult.Cancelled());
            return true;
        }

        var modifiers = CurrentModifiers();
        if (keysBlockingCapture.Count > 0
            || !KeyChord.TryGetShortcutKey(vkCode, out var key)
            || pressedKeys.Contains(VirtualKeys.LeftWindows)
            || pressedKeys.Contains(VirtualKeys.RightWindows)
            || (vkCode == VirtualKeys.F4 && modifiers == ShortcutModifiers.Alt))
        {
            return false;
        }

        if (pressedKeys.Contains(VirtualKeys.RightMenu))
        {
            RejectCapture(ShortcutCaptureRejection.RightAlt);
            return false;
        }

        ProtectLayoutSwitch(modifiers);
        if (Shortcut.TryCreate(modifiers, key, out var shortcut, out _))
        {
            FinishCapture(new ShortcutCaptureResult.Captured(shortcut));
        }
        else
        {
            RejectCapture(ShortcutCaptureRejection.TooFewModifiers);
        }

        return true;
    }

    private void RejectCapture(ShortcutCaptureRejection reason)
    {
        keysBlockingCapture.UnionWith(pressedKeys);
        var report = reportCapture!;
        dispatch(() => report(new ShortcutCaptureResult.Rejected(reason)));
    }

    private void FinishCapture(ShortcutCaptureResult result)
    {
        var report = reportCapture!;
        StopCapture();
        dispatch(() => report(result));
    }

    private void StopCapture()
    {
        reportCapture = null;
        keysBlockingCapture.Clear();
        dispatchPaused = pressedKeys.Count > 0;
    }

    /// <summary>
    /// Windows switches the input layout when Alt+Shift or Ctrl+Shift is released and it saw no other key
    /// in between. After a candidate key is hidden under such a pair, a neutral key fills that gap while the
    /// modifiers are still down. A chord without Shift needs none.
    /// </summary>
    private void ProtectLayoutSwitch(ShortcutModifiers modifiers)
    {
        if (modifiers.HasFlag(ShortcutModifiers.Shift)
            && (modifiers & (ShortcutModifiers.Control | ShortcutModifiers.Alt)) != 0)
        {
            neutralKeys.EmitNeutralKey();
        }
    }

    /// <summary>
    /// Starts a bound sound once per press: when its key and exactly its modifiers are first down together.
    /// Auto-repeat does nothing; the key must be released before it can start again. The drawing gate is
    /// untouched.
    /// </summary>
    /// <param name="firstKeyDown">The key whose first key-down is being handled, if any.</param>
    /// <remarks>
    /// Suppression follows <see cref="TryStartOncePerPress"/>.
    /// </remarks>
    private void UpdateSoundShortcuts(int? firstKeyDown)
    {
        // Indexed loop: the hook callback avoids allocating an enumerator per key-down.
        for (var i = 0; i < bindings.Sounds.Count; i++)
        {
            var (chord, command) = bindings.Sounds[i];
            if (TryStartOncePerPress(chord, firstKeyDown))
            {
                playSound(command);
            }
        }
    }

    /// <summary>
    /// Starts a one-shot chord (clear or a sound) when it is pressed and its key has not started a chord
    /// since the key went down.
    /// </summary>
    /// <remarks>
    /// A started key's later key-downs are suppressed until release, whatever happens to its modifiers.
    /// Its key-up is suppressed only when its own first key-down started the chord. If the chord forms
    /// while the key is already held, the application saw that key-down, so it also receives the key-up.
    /// </remarks>
    private bool TryStartOncePerPress(KeyChord chord, int? firstKeyDown)
    {
        if (!IsPressed(chord) || !startedShortcutKeys.Add(chord.VirtualKey))
        {
            return false;
        }

        if (chord.VirtualKey == firstKeyDown)
        {
            ProtectLayoutSwitch(chord.Modifiers);
        }

        return true;
    }

    /// <remarks>
    /// Draw starts only on an exact chord, but stays held while its key and modifiers are down, even if
    /// other keys join. Clear fires once per press of its key.
    /// </remarks>
    private void UpdateChords(int? firstKeyDown)
    {
        var drawShortcutHeld = IsHeld(bindings.Draw);
        var clearShortcutHeld = IsHeld(bindings.Clear);
        if (IsPressed(bindings.Draw) && !inputGate.IsActive && !inputGate.IsBlockedUntilReleased
            && cursorPositionSource.TryGetCurrentPosition(out var startingPoint))
        {
            if (monitorBoundsSource.TryGetBounds(startingPoint, out var bounds))
            {
                inputGate.Begin(bounds);
                DispatchDrawing(() => controller.EnterDrawMode(startingPoint, bounds));
            }
        }
        else if (!drawShortcutHeld && inputGate.IsActive)
        {
            inputGate.SetActive(false);
            DispatchDrawing(controller.ExitDrawMode);
        }

        if (!drawShortcutHeld && !clearShortcutHeld)
        {
            inputGate.ReleaseBlock();
        }

        if (TryStartOncePerPress(bindings.Clear, firstKeyDown))
        {
            inputGate.SetActive(false);
            inputGate.BlockUntilReleased();
            if (cursorPositionSource.TryGetCurrentPosition(out var clearPoint)
                && monitorBoundsSource.TryGetBounds(clearPoint, out var clearBounds))
            {
                DispatchDrawing(() => controller.ClearMonitorAndExitDrawMode(clearBounds));
            }
            else
            {
                DispatchDrawing(controller.ExitDrawMode);
            }
        }
    }

    /// <summary>
    /// Queues a drawing command that is dropped if <see cref="Interrupt"/> runs before it, so a press made
    /// under the previous shortcuts cannot draw under the new configuration.
    /// </summary>
    private void DispatchDrawing(Action command)
    {
        var epoch = drawingEpoch;
        dispatch(() =>
        {
            if (epoch == Volatile.Read(ref drawingEpoch))
            {
                command();
            }
        });
    }

    /// <summary>
    /// The chord's key is down with exactly its modifiers, and Right Alt is up. Right Alt is reserved for
    /// AltGr, which Windows also reports as Left Ctrl, so no command starts while it is held.
    /// </summary>
    private bool IsPressed(KeyChord chord) =>
        !pressedKeys.Contains(VirtualKeys.RightMenu)
        && pressedKeys.Contains(chord.VirtualKey) && CurrentModifiers() == chord.Modifiers;

    /// <summary>The chord's key and all its modifiers are down; other keys may be down too.</summary>
    private bool IsHeld(KeyChord chord) =>
        pressedKeys.Contains(chord.VirtualKey) && CurrentModifiers().HasFlag(chord.Modifiers);

    /// <remarks>Alt means Left Alt only: Right Alt is reserved for AltGr.</remarks>
    private ShortcutModifiers CurrentModifiers()
    {
        var modifiers = ShortcutModifiers.None;
        if (pressedKeys.Contains(VirtualKeys.LeftControl) || pressedKeys.Contains(VirtualKeys.RightControl))
        {
            modifiers |= ShortcutModifiers.Control;
        }

        if (pressedKeys.Contains(VirtualKeys.LeftMenu))
        {
            modifiers |= ShortcutModifiers.Alt;
        }

        if (pressedKeys.Contains(VirtualKeys.LeftShift) || pressedKeys.Contains(VirtualKeys.RightShift))
        {
            modifiers |= ShortcutModifiers.Shift;
        }

        return modifiers;
    }

    /// <remarks>
    /// Draw mode hides every key except the draw key and modifiers. Modifier keys always pass through, so
    /// the system never sees a key-down without its key-up.
    /// </remarks>
    private bool IsHiddenInDrawMode(int vkCode) =>
        vkCode != bindings.Draw.VirtualKey && vkCode is not (
            VirtualKeys.LeftControl or
            VirtualKeys.RightControl or
            VirtualKeys.LeftMenu or
            VirtualKeys.RightMenu or
            VirtualKeys.LeftShift or
            VirtualKeys.RightShift);

}
