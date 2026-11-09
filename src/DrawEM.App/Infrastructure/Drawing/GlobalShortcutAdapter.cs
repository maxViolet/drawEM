using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Effects;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Effects;

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
    private readonly IEffectStartQueue effects;
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
    /// The configuration generation, advanced by <see cref="Interrupt"/>. A queued drawing or effect command
    /// runs only if it was queued under the current value. Written on the hook thread, read on the dispatcher
    /// thread.
    /// </summary>
    private int configurationGeneration;

    private Action<ShortcutCaptureResult>? reportCapture;

    /// <param name="bindings">The shortcuts to start with, read inside the hook callback. <see cref="Bind"/> replaces them.</param>
    /// <param name="playSound">
    /// Queues the command on the sound thread. Called directly inside the hook callback, so it must
    /// return promptly without file access, decoding, or player creation. Production uses SoundChannelHost.Play.
    /// </param>
    /// <param name="effects">
    /// Takes each effect start: enqueued inside the hook callback at key-down, dequeued on the dispatcher thread.
    /// Production uses <see cref="EffectMonitorGuard"/>, which starts it on the EffectChannelController.
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
        Action<PlaySoundCommand> playSound,
        IEffectStartQueue effects)
    {
        this.neutralKeys = neutralKeys;
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
        this.monitorBoundsSource = monitorBoundsSource;
        this.dispatch = dispatch;
        this.bindings = bindings;
        this.playSound = playSound;
        this.effects = effects;
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
            DispatchCommand(controller.ExitDrawMode);
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
        Volatile.Write(ref configurationGeneration, configurationGeneration + 1);
        dispatchPaused = pressedKeys.Count > 0;

        // A dropped command may have been an exit, so drawing is reset here rather than queued.
        controller.ClearAndExitDrawMode();
    }

    /// <remarks>Call after <see cref="Interrupt"/>, so no press started under the previous shortcuts continues.</remarks>
    public void Bind(SettingsSnapshot snapshot, Func<SoundReference, PlaySoundCommand> commandFor) =>
        bindings = ShortcutBindings.FromSnapshot(snapshot, commandFor);

    private bool HandleKey(int vkCode, KeyDirection direction) =>
        direction == KeyDirection.Up ? HandleKeyUp(vkCode) : HandleKeyDown(vkCode);

    private bool HandleKeyUp(int vkCode)
    {
        var suppressKeyUp = keysWithHiddenFirstDown.Remove(vkCode);
        OnKeyUp(vkCode);
        return suppressKeyUp;
    }

    private bool HandleKeyDown(int vkCode)
    {
        var isFirstKeyDown = pressedKeys.Add(vkCode);
        var suppress = DispatchKeyDown(vkCode, isFirstKeyDown) || IsSuppressed(vkCode);
        if (isFirstKeyDown && suppress)
        {
            keysWithHiddenFirstDown.Add(vkCode);
        }

        return suppress;
    }

    /// <returns>Whether capture took the key.</returns>
    private bool DispatchKeyDown(int vkCode, bool isFirstKeyDown)
    {
        if (IsCapturing)
        {
            return isFirstKeyDown && Capture(vkCode);
        }

        if (!dispatchPaused)
        {
            int? firstKeyDown = isFirstKeyDown ? vkCode : null;
            UpdateChords(firstKeyDown);
            UpdateSlotShortcuts(firstKeyDown);
        }

        return false;
    }

    private bool IsSuppressed(int vkCode) =>
        keysWithHiddenFirstDown.Contains(vkCode)
        || (inputGate.IsActive && IsHiddenInDrawMode(vkCode))
        || startedShortcutKeys.Contains(vkCode);

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
        if (!IsCaptureCandidate(vkCode, modifiers, out var key))
        {
            return false;
        }

        if (pressedKeys.Contains(VirtualKeys.RightMenu))
        {
            RejectCapture(ShortcutCaptureRejection.RightAlt);
            return false;
        }

        ProtectLayoutSwitch(modifiers);
        Record(modifiers, key);
        return true;
    }

    /// <summary>
    /// A letter, digit, or F1–F12 key while capture is not blocked, outside a Win chord and Alt+F4.
    /// </summary>
    private bool IsCaptureCandidate(int vkCode, ShortcutModifiers modifiers, out ShortcutKey key)
    {
        key = default;
        return keysBlockingCapture.Count == 0
            && KeyChord.TryGetShortcutKey(vkCode, out key)
            && !IsWindowsKeyDown()
            && !(vkCode == VirtualKeys.F4 && modifiers == ShortcutModifiers.Alt);
    }

    private bool IsWindowsKeyDown() =>
        pressedKeys.Contains(VirtualKeys.LeftWindows) || pressedKeys.Contains(VirtualKeys.RightWindows);

    private void Record(ShortcutModifiers modifiers, ShortcutKey key)
    {
        if (Shortcut.TryCreate(modifiers, key, out var shortcut, out _))
        {
            FinishCapture(new ShortcutCaptureResult.Captured(shortcut));
        }
        else
        {
            RejectCapture(ShortcutCaptureRejection.TooFewModifiers);
        }
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
    /// Starts a bound sound or effect once per press: when its key and exactly its modifiers are first down
    /// together. Auto-repeat does nothing; the key must be released before it can start again. The drawing
    /// gate is untouched.
    /// </summary>
    /// <param name="firstKeyDown">The key whose first key-down is being handled, if any.</param>
    /// <remarks>
    /// Suppression follows <see cref="TryStartOncePerPress"/>.
    /// </remarks>
    private void UpdateSlotShortcuts(int? firstKeyDown)
    {
        StartSounds(firstKeyDown);
        StartEffects(firstKeyDown);
    }

    private void StartSounds(int? firstKeyDown)
    {
        // Indexed loops: the hook callback avoids allocating an enumerator per key-down.
        for (var i = 0; i < bindings.Sounds.Count; i++)
        {
            var (chord, command) = bindings.Sounds[i];
            if (TryStartOncePerPress(chord, firstKeyDown))
            {
                playSound(command);
            }
        }
    }

    private void StartEffects(int? firstKeyDown)
    {
        for (var i = 0; i < bindings.Effects.Count; i++)
        {
            var effect = bindings.Effects[i];
            if (TryStartOncePerPress(effect.Chord, firstKeyDown)
                && cursorPositionSource.TryGetCurrentPosition(out var point))
            {
                DispatchEffect(effect, point);
            }
        }
    }

    /// <summary>
    /// Queues an effect start at the cursor point captured at key-down. The monitor is resolved on the
    /// dispatcher, outside the hook; a point outside every monitor, or a Save before dispatch, starts nothing.
    /// The start is dequeued either way, so the guard knows when nothing is queued.
    /// </summary>
    private void DispatchEffect(EffectBinding effect, ScreenPoint point)
    {
        var generation = configurationGeneration;
        var monitorEpoch = effects.Enqueue(point);
        dispatch(() =>
        {
            StartEffectCommand? command = null;
            if (generation == Volatile.Read(ref configurationGeneration)
                && monitorBoundsSource.TryGetBounds(point, out var monitor))
            {
                command = new StartEffectCommand(
                    effect.Slot,
                    effect.Effect,
                    effect.Placement,
                    monitor,
                    point,
                    effect.Loop,
                    effect.MaxLength,
                    generation,
                    monitorEpoch);
            }

            effects.Dequeue(monitorEpoch, command);
        });
    }

    /// <summary>
    /// Starts a one-shot chord (clear, a sound, or an effect) when it is pressed and its key has not started a
    /// chord since the key went down.
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
        UpdateDrawMode(drawShortcutHeld);
        if (!drawShortcutHeld && !clearShortcutHeld)
        {
            inputGate.ReleaseBlock();
        }

        if (TryStartOncePerPress(bindings.Clear, firstKeyDown))
        {
            Clear();
        }
    }

    private void UpdateDrawMode(bool drawShortcutHeld)
    {
        if (IsDrawPressedWhileIdle() && cursorPositionSource.TryGetCurrentPosition(out var startingPoint))
        {
            EnterDrawMode(startingPoint);
        }
        else if (!drawShortcutHeld && inputGate.IsActive)
        {
            inputGate.SetActive(false);
            DispatchCommand(controller.ExitDrawMode);
        }
    }

    private bool IsDrawPressedWhileIdle() =>
        IsPressed(bindings.Draw) && !inputGate.IsActive && !inputGate.IsBlockedUntilReleased;

    private void EnterDrawMode(ScreenPoint startingPoint)
    {
        if (monitorBoundsSource.TryGetBounds(startingPoint, out var bounds))
        {
            inputGate.Begin(bounds);
            DispatchCommand(() => controller.EnterDrawMode(startingPoint, bounds));
        }
    }

    private void Clear()
    {
        inputGate.SetActive(false);
        inputGate.BlockUntilReleased();
        if (cursorPositionSource.TryGetCurrentPosition(out var clearPoint)
            && monitorBoundsSource.TryGetBounds(clearPoint, out var clearBounds))
        {
            DispatchCommand(() => controller.ClearMonitorAndExitDrawMode(clearBounds));
        }
        else
        {
            DispatchCommand(controller.ExitDrawMode);
        }
    }

    /// <summary>
    /// Queues a drawing command that is dropped if <see cref="Interrupt"/> runs before it, so a press made under
    /// the previous shortcuts cannot draw under the new configuration. Effect starts use the same check in
    /// <see cref="DispatchEffect"/>.
    /// </summary>
    private void DispatchCommand(Action command)
    {
        var generation = configurationGeneration;
        dispatch(() =>
        {
            if (generation == Volatile.Read(ref configurationGeneration))
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
    private ShortcutModifiers CurrentModifiers() =>
        IfDown(VirtualKeys.LeftControl, VirtualKeys.RightControl, ShortcutModifiers.Control)
        | IfDown(VirtualKeys.LeftMenu, ShortcutModifiers.Alt)
        | IfDown(VirtualKeys.LeftShift, VirtualKeys.RightShift, ShortcutModifiers.Shift);

    private ShortcutModifiers IfDown(int key, ShortcutModifiers modifier) =>
        pressedKeys.Contains(key) ? modifier : ShortcutModifiers.None;

    private ShortcutModifiers IfDown(int key, int otherKey, ShortcutModifiers modifier) =>
        IfDown(key, modifier) | IfDown(otherKey, modifier);

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
