using DrawEM.App.Application.Drawing;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

public class GlobalMouseInputAdapterTests
{
    [Fact]
    public void BoundaryCrossing_ClosesInputGateBeforeQueuedControllerAction()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        var gate = new DrawingModeInputGate();
        var queued = new Queue<Action>();
        _ = new GlobalMouseInputAdapter(source, controller, gate, queued.Enqueue);
        var left = new MonitorBounds(0, 0, 100, 100);

        gate.Begin(left);
        controller.EnterDrawMode(new ScreenPoint(90, 50), left);
        source.Move(new ScreenPoint(101, 50));

        Assert.False(gate.IsActive);
        Assert.True(gate.IsBlockedUntilReleased);
        Assert.False(source.ShouldSuppressPointerButton());
        Assert.Single(queued);
        queued.Dequeue().Invoke();
        Assert.Equal([new ScreenPoint(90, 50)], Assert.Single(controller.CompletedStrokes).Points);
    }

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
        _ = new GlobalShortcutAdapter(keyboardSource, keyboardSource, controller, inputGate, new FakeCursorPositionSource(default), queuedActions.Enqueue,
            new FakeMonitorBoundsSource(new MonitorBounds(-1000, -1000, 1000, 1000)),
            new ShortcutBindings(SettingsSnapshot.Default.DrawShortcut, SettingsSnapshot.Default.ClearShortcut, []), _ => { });
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
}
