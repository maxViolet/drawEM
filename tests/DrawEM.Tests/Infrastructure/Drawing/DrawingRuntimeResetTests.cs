using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

public class DrawingRuntimeResetTests
{
    private static readonly MonitorBounds Left = new(0, 0, 100, 100);

    [Fact]
    public void ExitDrawingAndClearAllMonitors_ClosesGateBeforeQueuedWorkRuns()
    {
        var gate = new DrawingModeInputGate();
        gate.Begin(Left);
        var queued = new Queue<Action>();
        IDrawingReset reset = new DrawingRuntimeReset(gate, new DrawingSessionController(), queued.Enqueue);

        reset.ExitDrawingAndClearAllMonitors();

        Assert.False(gate.IsActive);
    }

    [Fact]
    public void ExitDrawingAndClearAllMonitors_OnDispatcher_EndsActiveStrokeAndClearsEveryMonitor()
    {
        var right = new MonitorBounds(100, 0, 200, 100);
        var controller = new DrawingSessionController();
        controller.EnterDrawMode(new ScreenPoint(10, 10), Left);
        controller.ExitDrawMode();
        controller.EnterDrawMode(new ScreenPoint(110, 10), right);
        controller.ReportPointer(new ScreenPoint(120, 20));
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        var queued = new Queue<Action>();
        var reset = new DrawingRuntimeReset(new DrawingModeInputGate(), controller, queued.Enqueue);

        reset.ExitDrawingAndClearAllMonitors();
        Assert.Empty(states);
        Assert.Single(queued).Invoke();

        var state = Assert.Single(states);
        Assert.False(state.IsDrawModeActive);
        Assert.Null(state.ActiveStroke);
        Assert.Empty(state.CompletedStrokes);
        Assert.Empty(controller.CompletedStrokes);
    }
}
