using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class GlobalShortcutAdapter
{
    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly ICursorPositionSource cursorPositionSource;
    private readonly IMonitorBoundsSource monitorBoundsSource;
    private readonly Action<Action> dispatch;
    private readonly ShortcutBindings bindings;
    private readonly Action<PlaySoundCommand> playSound;
    private readonly HashSet<int> pressedKeys = [];
    private readonly HashSet<int> startedShortcutKeys = [];
    private readonly HashSet<int> keysSuppressedOnPress = [];

    /// <param name="bindings">The active shortcuts, read inside the hook callback.</param>
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
        ShortcutBindings bindings,
        Action<PlaySoundCommand> playSound)
    {
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
        this.monitorBoundsSource = monitorBoundsSource;
        this.dispatch = dispatch;
        this.bindings = bindings;
        this.playSound = playSound;
        source.KeyDown += OnKeyChanged;
        source.KeyUp += OnKeyUp;
        source.KeySuppressionRequested += ShouldSuppressKey;
    }

    private void OnKeyChanged(int vkCode)
    {
        int? firstKeyDown = pressedKeys.Add(vkCode) ? vkCode : null;
        UpdateChords(firstKeyDown);
        UpdateSoundShortcuts(firstKeyDown);
    }

    private void OnKeyUp(int vkCode)
    {
        pressedKeys.Remove(vkCode);
        startedShortcutKeys.Remove(vkCode);
        keysSuppressedOnPress.Remove(vkCode);
        UpdateChords(firstKeyDown: null);
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
            keysSuppressedOnPress.Add(chord.VirtualKey);
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
                dispatch(() => controller.EnterDrawMode(startingPoint, bounds));
            }
        }
        else if (!drawShortcutHeld && inputGate.IsActive)
        {
            inputGate.SetActive(false);
            dispatch(controller.ExitDrawMode);
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
                dispatch(() => controller.ClearMonitorAndExitDrawMode(clearBounds));
            }
            else
            {
                dispatch(controller.ExitDrawMode);
            }
        }
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
    /// Modifier keys always pass through, so the system never sees a key-down without its key-up.
    /// </remarks>
    private bool ShouldSuppressKey(int vkCode, KeyDirection direction) =>
        (inputGate.IsActive && vkCode != bindings.Draw.VirtualKey && vkCode is not (
            VirtualKeys.LeftControl or
            VirtualKeys.RightControl or
            VirtualKeys.LeftMenu or
            VirtualKeys.RightMenu or
            VirtualKeys.LeftShift or
            VirtualKeys.RightShift)) ||
        (direction == KeyDirection.Down && startedShortcutKeys.Contains(vkCode)) ||
        keysSuppressedOnPress.Contains(vkCode);

}
