using DrawEM.App.Application.Drawing;
using DrawEM.App.Infrastructure;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class GlobalShortcutAdapter
{
    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly ICursorPositionSource cursorPositionSource;
    private readonly IMonitorBoundsSource monitorBoundsSource;
    private readonly Action<Action> dispatch;
    private readonly HashSet<int> pressedKeys = [];
    private bool clearShortcutActive;

    public GlobalShortcutAdapter(
        IKeyboardHookSource source,
        DrawingSessionController controller,
        DrawingModeInputGate inputGate,
        ICursorPositionSource cursorPositionSource,
        Action<Action> dispatch,
        IMonitorBoundsSource monitorBoundsSource)
    {
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
        this.monitorBoundsSource = monitorBoundsSource;
        this.dispatch = dispatch;
        source.KeyDown += OnKeyChanged;
        source.KeyUp += OnKeyUp;
        source.KeySuppressionRequested += ShouldSuppressKey;
    }

    private void OnKeyChanged(int vkCode)
    {
        pressedKeys.Add(vkCode);
        UpdateChords();
    }

    private void OnKeyUp(int vkCode)
    {
        pressedKeys.Remove(vkCode);
        UpdateChords();
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
        (clearShortcutActive && vkCode == VirtualKeys.X);

}
