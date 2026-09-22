using DrawEM.App.Application;

namespace DrawEM.App.Infrastructure;

public sealed class GlobalShortcutAdapter
{
    private readonly DrawingSessionController controller;
    private readonly Action<Action> dispatch;
    private readonly HashSet<int> pressedKeys = [];
    private bool drawModeActive;
    private bool drawShortcutBlockedUntilReleased;
    private bool clearShortcutActive;

    public GlobalShortcutAdapter(IKeyboardHookSource source, DrawingSessionController controller)
        : this(source, controller, action => action())
    {
    }

    public GlobalShortcutAdapter(IKeyboardHookSource source, DrawingSessionController controller, Action<Action> dispatch)
    {
        this.controller = controller;
        this.dispatch = dispatch;
        source.KeyDown += OnKeyChanged;
        source.KeyUp += OnKeyUp;
        source.KeyActivity += ShouldSuppressKey;
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
        if (drawShortcutDown && !drawModeActive && !drawShortcutBlockedUntilReleased)
        {
            drawModeActive = true;
            dispatch(controller.EnterDrawMode);
        }
        else if (!drawShortcutDown && drawModeActive)
        {
            drawModeActive = false;
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
            drawModeActive = false;
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
        (drawModeActive && vkCode is not (
            VirtualKeys.LeftControl or
            VirtualKeys.RightControl or
            VirtualKeys.LeftMenu or
            VirtualKeys.RightMenu or
            VirtualKeys.Z)) ||
        (clearShortcutActive && vkCode == VirtualKeys.X);
}
