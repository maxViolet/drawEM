using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class GlobalShortcutAdapter
{
    private static readonly Dictionary<int, SoundSlot> SoundSlotKeys = new()
    {
        [VirtualKeys.D1] = SoundSlot.Slot1,
        [VirtualKeys.D2] = SoundSlot.Slot2,
        [VirtualKeys.D3] = SoundSlot.Slot3,
        [VirtualKeys.D4] = SoundSlot.Slot4,
        [VirtualKeys.D5] = SoundSlot.Slot5,
        [VirtualKeys.D6] = SoundSlot.Slot6,
        [VirtualKeys.D7] = SoundSlot.Slot7,
        [VirtualKeys.D8] = SoundSlot.Slot8,
    };

    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly ICursorPositionSource cursorPositionSource;
    private readonly IMonitorBoundsSource monitorBoundsSource;
    private readonly Action<Action> dispatch;
    private readonly Func<SoundSlot, PlaySoundCommand?> resolveSound;
    private readonly Action<PlaySoundCommand> playSound;
    private readonly HashSet<int> pressedKeys = [];
    private readonly HashSet<int> startedSoundKeys = [];
    private readonly HashSet<int> soundKeysSuppressedOnPress = [];
    private bool clearShortcutActive;

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
    {
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
        this.monitorBoundsSource = monitorBoundsSource;
        this.dispatch = dispatch;
        this.resolveSound = resolveSound;
        this.playSound = playSound;
        source.KeyDown += OnKeyChanged;
        source.KeyUp += OnKeyUp;
        source.KeySuppressionRequested += ShouldSuppressKey;
    }

    private void OnKeyChanged(int vkCode)
    {
        var firstKeyDown = pressedKeys.Add(vkCode);
        UpdateChords();
        UpdateSoundShortcuts(firstKeyDown ? vkCode : null);
    }

    private void OnKeyUp(int vkCode)
    {
        pressedKeys.Remove(vkCode);
        startedSoundKeys.Remove(vkCode);
        soundKeysSuppressedOnPress.Remove(vkCode);
        UpdateChords();
    }

    /// <summary>
    /// Starts an assigned slot once per press: when its key and Ctrl+Alt are first down together.
    /// Auto-repeat does nothing; the slot key must be released before it can start again. Unassigned
    /// slots are ignored. The drawing gate is untouched.
    /// </summary>
    /// <param name="firstKeyDown">The key whose first key-down is being handled, if any.</param>
    /// <remarks>
    /// A started slot key's later key-downs are suppressed until release. Its key-up is suppressed only
    /// when its own first key-down started the sound. If the chord forms while the key is already held,
    /// the application saw that key-down, so it also receives the matching key-up.
    /// </remarks>
    private void UpdateSoundShortcuts(int? firstKeyDown)
    {
        if (!IsCtrlDown() || !IsAltDown())
        {
            return;
        }

        foreach (var (vkCode, slot) in SoundSlotKeys)
        {
            if (!pressedKeys.Contains(vkCode) || startedSoundKeys.Contains(vkCode)
                || resolveSound(slot) is not { } command)
            {
                continue;
            }

            startedSoundKeys.Add(vkCode);
            if (vkCode == firstKeyDown)
            {
                soundKeysSuppressedOnPress.Add(vkCode);
            }

            playSound(command);
        }
    }

    private void UpdateChords()
    {
        var modifiersDown = IsCtrlDown() && IsAltDown();

        var drawShortcutDown = modifiersDown && pressedKeys.Contains(VirtualKeys.Z);
        var clearShortcutDown = modifiersDown && pressedKeys.Contains(VirtualKeys.X);
        if (drawShortcutDown && !inputGate.IsActive && !inputGate.IsBlockedUntilReleased
            && cursorPositionSource.TryGetCurrentPosition(out var startingPoint))
        {
            if (monitorBoundsSource.TryGetBounds(startingPoint, out var bounds))
            {
                inputGate.Begin(bounds);
                dispatch(() => controller.EnterDrawMode(startingPoint, bounds));
            }
        }
        else if (!drawShortcutDown && inputGate.IsActive)
        {
            inputGate.SetActive(false);
            dispatch(controller.ExitDrawMode);
        }

        if (!drawShortcutDown && !clearShortcutDown)
        {
            inputGate.ReleaseBlock();
        }

        if (clearShortcutDown && !clearShortcutActive)
        {
            clearShortcutActive = true;
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
        else if (!clearShortcutDown)
        {
            clearShortcutActive = false;
        }
    }

    private bool IsCtrlDown() =>
        pressedKeys.Contains(VirtualKeys.LeftControl) || pressedKeys.Contains(VirtualKeys.RightControl);

    private bool IsAltDown() =>
        pressedKeys.Contains(VirtualKeys.LeftMenu) || pressedKeys.Contains(VirtualKeys.RightMenu);

    private bool ShouldSuppressKey(int vkCode, KeyDirection direction) =>
        (inputGate.IsActive && vkCode is not (
            VirtualKeys.LeftControl or
            VirtualKeys.RightControl or
            VirtualKeys.LeftMenu or
            VirtualKeys.RightMenu or
            VirtualKeys.Z)) ||
        (clearShortcutActive && vkCode == VirtualKeys.X) ||
        (direction == KeyDirection.Down && startedSoundKeys.Contains(vkCode)) ||
        soundKeysSuppressedOnPress.Contains(vkCode);

}
