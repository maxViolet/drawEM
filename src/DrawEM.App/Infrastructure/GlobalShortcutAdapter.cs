using DrawEM.App.Application;

namespace DrawEM.App.Infrastructure;

public sealed class GlobalShortcutAdapter
{
    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly ICursorPositionSource cursorPositionSource;
    private readonly Action<Action> dispatch;
    private readonly HashSet<int> pressedKeys = [];
    private bool drawShortcutBlockedUntilReleased;
    private bool clearShortcutActive;

    public GlobalShortcutAdapter(
        IKeyboardHookSource source,
        DrawingSessionController controller,
        DrawingModeInputGate inputGate,
        ICursorPositionSource cursorPositionSource,
        Action<Action> dispatch)
    {
        this.controller = controller;
        this.inputGate = inputGate;
        this.cursorPositionSource = cursorPositionSource;
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
        if (drawShortcutDown && !inputGate.IsActive && !drawShortcutBlockedUntilReleased)
        {
            inputGate.SetActive(true);
            if (cursorPositionSource.TryGetCurrentPosition(out var startingPoint))
            {
                dispatch(() => controller.EnterDrawMode(startingPoint));
            }
            else
            {
                dispatch(controller.EnterDrawMode);
            }
        }
        else if (!drawShortcutDown && inputGate.IsActive)
        {
            inputGate.SetActive(false);
            dispatch(controller.ExitDrawMode);
        }

        if (!drawShortcutDown)
        {
            drawShortcutBlockedUntilReleased = false;
        }

        var clearShortcutDown = modifiersDown && pressedKeys.Contains(VirtualKeys.X);
        if (clearShortcutDown && !clearShortcutActive)
        {
            clearShortcutActive = true;
            inputGate.SetActive(false);
            drawShortcutBlockedUntilReleased = true;
            dispatch(controller.ClearAndExitDrawMode);
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

    private bool ShouldSuppressKey(int vkCode) =>
        (inputGate.IsActive && vkCode is not (
            VirtualKeys.LeftControl or
            VirtualKeys.RightControl or
            VirtualKeys.LeftMenu or
            VirtualKeys.RightMenu or
            VirtualKeys.Z)) ||
        (clearShortcutActive && vkCode == VirtualKeys.X);

}
