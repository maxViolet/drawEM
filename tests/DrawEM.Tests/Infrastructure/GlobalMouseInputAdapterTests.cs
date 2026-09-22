using DrawEM.App.Application;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class GlobalMouseInputAdapterTests
{
    [Fact]
    public void PointerMovement_DuringDrawMode_BecomesCompletedStroke()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        var inputGate = new DrawingModeInputGate();
        _ = new GlobalMouseInputAdapter(source, controller, inputGate, action => action());

        inputGate.SetActive(true);
        controller.EnterDrawMode();
        source.Move(new ScreenPoint(10, 20));
        source.Move(new ScreenPoint(15, 25));
        inputGate.SetActive(false);
        controller.ExitDrawMode();

        var stroke = Assert.Single(controller.CompletedStrokes);
        Assert.Equal([new ScreenPoint(10, 20), new ScreenPoint(15, 25)], stroke.Points);
    }

    [Fact]
    public void PointerButtons_AreSuppressedOnlyDuringDrawMode()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        var inputGate = new DrawingModeInputGate();
        _ = new GlobalMouseInputAdapter(source, controller, inputGate, action => action());

        Assert.False(source.ShouldSuppressPointerButton());

        inputGate.SetActive(true);

        Assert.True(source.ShouldSuppressPointerButton());

        inputGate.SetActive(false);

        Assert.False(source.ShouldSuppressPointerButton());
    }

    [Fact]
    public void PointerWheel_IsSuppressedOnlyDuringDrawMode()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        var inputGate = new DrawingModeInputGate();
        _ = new GlobalMouseInputAdapter(source, controller, inputGate, action => action());

        Assert.False(source.ShouldSuppressPointerWheel());

        inputGate.SetActive(true);

        Assert.True(source.ShouldSuppressPointerWheel());

        inputGate.SetActive(false);

        Assert.False(source.ShouldSuppressPointerWheel());
    }

    [Fact]
    public void CtrlAltX_StopsMouseSuppressionBeforeQueuedControllerActionsRun()
    {
        var keyboardSource = new FakeKeyboardHookSource();
        var mouseSource = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        var inputGate = new DrawingModeInputGate();
        var queuedActions = new Queue<Action>();
        _ = new GlobalShortcutAdapter(keyboardSource, controller, inputGate, queuedActions.Enqueue);
        _ = new GlobalMouseInputAdapter(mouseSource, controller, inputGate, queuedActions.Enqueue);

        keyboardSource.PressKey(VirtualKeys.LeftControl);
        keyboardSource.PressKey(VirtualKeys.LeftMenu);
        keyboardSource.PressKey(VirtualKeys.Z);

        Assert.True(mouseSource.ShouldSuppressPointerButton());
        Assert.True(mouseSource.ShouldSuppressPointerWheel());

        keyboardSource.PressKey(VirtualKeys.X);

        Assert.False(mouseSource.ShouldSuppressPointerButton());
        Assert.False(mouseSource.ShouldSuppressPointerWheel());
        Assert.Equal(2, queuedActions.Count);
    }

    private sealed class FakeMouseHookSource : IMouseHookSource
    {
        public event Action<ScreenPoint>? PointerMoved;

        public event Func<bool>? PointerButtonActivity;

        public event Func<bool>? PointerWheelActivity;

        public void Move(ScreenPoint point) => PointerMoved?.Invoke(point);

        public bool ShouldSuppressPointerButton() =>
            PointerButtonActivity?
                .GetInvocationList()
                .Cast<Func<bool>>()
                .Any(handler => handler())
            ?? false;

        public bool ShouldSuppressPointerWheel() =>
            PointerWheelActivity?
                .GetInvocationList()
                .Cast<Func<bool>>()
                .Any(handler => handler())
            ?? false;
    }

    private sealed class FakeKeyboardHookSource : IKeyboardHookSource
    {
        public event Action<int>? KeyDown;

        public event Action<int>? KeyUp;

        public event Func<int, bool>? KeySuppressionRequested;

        public void PressKey(int vkCode) => KeyDown?.Invoke(vkCode);

        public void ReleaseKey(int vkCode) => KeyUp?.Invoke(vkCode);

        public bool ShouldSuppressKey(int vkCode) => KeySuppressionRequested?.Invoke(vkCode) ?? false;
    }
}
