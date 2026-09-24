using DrawEM.App.Application.Drawing;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

public class GlobalShortcutAdapterTests
{
    private static readonly MonitorBounds DefaultMonitor = new(-1000, -1000, 1000, 1000);
    private static readonly IMonitorBoundsSource DefaultMonitorSource = new FakeMonitorBoundsSource(DefaultMonitor);

    [Fact]
    public void CtrlAltX_ClearsOnlyMonitorUnderCursor()
    {
        var left = new MonitorBounds(0, 0, 100, 100);
        var right = new MonitorBounds(100, 0, 200, 100);
        var controller = new DrawingSessionController();
        controller.EnterDrawMode(new ScreenPoint(10, 10), left);
        controller.ExitDrawMode();
        controller.EnterDrawMode(new ScreenPoint(110, 10), right);
        controller.ExitDrawMode();
        var source = new FakeKeyboardHookSource();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(),
            new FakeCursorPositionSource(new ScreenPoint(110, 10)), action => action(),
            new FakeMonitorBoundsSource(left, right));

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.X);

        Assert.Equal(left, Assert.Single(controller.CompletedStrokes).Bounds);
    }

    [Fact]
    public void BoundaryCrossing_RequiresShortcutReleaseBeforeDrawingAgain()
    {
        var left = new MonitorBounds(0, 0, 100, 100);
        var right = new MonitorBounds(100, 0, 200, 100);
        var source = new FakeKeyboardHookSource();
        var cursor = new MutableCursorPositionSource(new ScreenPoint(90, 50));
        var gate = new DrawingModeInputGate();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, gate, cursor, action => action(),
            new FakeMonitorBoundsSource(left, right));

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        Assert.True(gate.StopAtBoundary(new ScreenPoint(101, 50)));
        controller.ExitDrawMode();
        cursor.Position = new ScreenPoint(110, 50);
        var countAfterCrossing = states.Count;

        source.PressKey(VirtualKeys.Z);
        Assert.Equal(countAfterCrossing, states.Count);
        source.ReleaseKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);
        Assert.Equal(right, states[^1].ActiveStroke!.Bounds);
    }

    [Fact]
    public void CtrlAltZ_Pressed_EntersDrawModeOnce_DespiteAutoRepeat()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);

        Assert.Single(states.Where(state => state.IsDrawModeActive));
    }

    [Fact]
    public void CtrlAltZ_Released_ExitsDrawMode()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.ReleaseKey(VirtualKeys.Z);

        Assert.Equal([true, false], states.Select(state => state.IsDrawModeActive));
    }

    [Fact]
    public void CtrlAltX_Pressed_InvokesClear()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        controller.EnterDrawMode(new ScreenPoint(1, 1), DefaultMonitor);
        controller.ReportPointer(new ScreenPoint(2, 2));
        controller.ExitDrawMode();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.X);

        Assert.Empty(controller.CompletedStrokes);
    }

    [Fact]
    public void CtrlAltZ_Pressed_DefersDrawModeChangeUntilDispatcherRuns()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        var queuedActions = new Queue<Action>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), queuedActions.Enqueue, DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Empty(states);
        Assert.Single(queuedActions);

        queuedActions.Dequeue().Invoke();

        Assert.True(Assert.Single(states).IsDrawModeActive);
    }

    [Fact]
    public void DrawMode_SuppressesNonShortcutKeys_ButAllowsDrawingChord()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.False(source.ShouldSuppressKey(VirtualKeys.LeftControl));
        Assert.False(source.ShouldSuppressKey(VirtualKeys.LeftMenu));
        Assert.False(source.ShouldSuppressKey(VirtualKeys.Z));
        Assert.True(source.ShouldSuppressKey(VirtualKeys.A));

        source.ReleaseKey(VirtualKeys.Z);

        Assert.False(source.ShouldSuppressKey(VirtualKeys.A));
    }

    [Fact]
    public void CtrlAltX_SuppressesXAndClearsThenExitsDrawMode()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));

        var xWasSuppressed = source.PressKey(VirtualKeys.X);

        var state = states.Last();
        Assert.True(xWasSuppressed);
        Assert.False(state.IsDrawModeActive);
        Assert.Empty(state.CompletedStrokes);
        Assert.Null(state.ActiveStroke);
    }

    [Fact]
    public void CtrlAltZ_Pressed_StartsActiveStrokeAtCurrentCursorPosition()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        var cursorPosition = new ScreenPoint(120, 240);
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            new DrawingModeInputGate(),
            new FakeCursorPositionSource(cursorPosition),
            action => action(),
            DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Equal([cursorPosition], Assert.Single(states).ActiveStroke!.Points);
    }

    [Fact]
    public void CtrlAltZ_Pressed_DoesNotEnterDrawMode_WhenCursorPositionUnavailable()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var inputGate = new DrawingModeInputGate();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            inputGate,
            new FailingCursorPositionSource(),
            action => action(),
            DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Empty(states);
        Assert.False(inputGate.IsActive);
    }

    [Fact]
    public void CtrlAltZ_Pressed_EntersDrawMode_OnceCursorPositionBecomesAvailable()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        var cursorSource = new RecoveringCursorPositionSource(failuresBeforeSuccess: 2, new ScreenPoint(5, 9));
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            new DrawingModeInputGate(),
            cursorSource,
            action => action(),
            DefaultMonitorSource);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        Assert.Empty(states);

        source.PressKey(VirtualKeys.Z);
        Assert.Empty(states);

        source.PressKey(VirtualKeys.Z);
        var state = Assert.Single(states);
        Assert.True(state.IsDrawModeActive);
        Assert.Equal([new ScreenPoint(5, 9)], state.ActiveStroke!.Points);
    }

    private sealed class FakeKeyboardHookSource : IKeyboardHookSource
    {
        public event Action<int>? KeyDown;

        public event Action<int>? KeyUp;

        public event Func<int, bool>? KeySuppressionRequested;

        public bool PressKey(int vkCode)
        {
            KeyDown?.Invoke(vkCode);
            return ShouldSuppressKey(vkCode);
        }

        public void ReleaseKey(int vkCode) => KeyUp?.Invoke(vkCode);

        public bool ShouldSuppressKey(int vkCode) =>
            KeySuppressionRequested?
                .GetInvocationList()
                .Cast<Func<int, bool>>()
                .Any(handler => handler(vkCode))
            ?? false;
    }

    private sealed class FakeCursorPositionSource(ScreenPoint point) : ICursorPositionSource
    {
        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = point;
            return true;
        }
    }

    private sealed class MutableCursorPositionSource(ScreenPoint position) : ICursorPositionSource
    {
        public ScreenPoint Position { get; set; } = position;

        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = Position;
            return true;
        }
    }

    private sealed class FakeMonitorBoundsSource(params MonitorBounds[] monitors) : IMonitorBoundsSource
    {
        public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
        {
            foreach (var monitor in monitors)
            {
                if (monitor.Contains(point))
                {
                    bounds = monitor;
                    return true;
                }
            }

            bounds = default;
            return false;
        }
    }

    private sealed class FailingCursorPositionSource : ICursorPositionSource
    {
        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = default;
            return false;
        }
    }

    private sealed class RecoveringCursorPositionSource(int failuresBeforeSuccess, ScreenPoint point)
        : ICursorPositionSource
    {
        private int remainingFailures = failuresBeforeSuccess;

        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            if (remainingFailures > 0)
            {
                remainingFailures--;
                position = default;
                return false;
            }

            position = point;
            return true;
        }
    }
}
