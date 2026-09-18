using DrawEM.App.Domain;
using DrawEM.App.Presentation;

namespace DrawEM.Tests.Presentation;

public class OverlayWindowAdapterTests
{
    [Fact]
    public void End_ForwardsCompletedStrokeWithNoActiveStroke()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));
        controller.End();

        Assert.Equal(controller.CompletedStrokes, view.LastRenderedState!.CompletedStrokes);
        Assert.Null(view.LastRenderedState.ActiveStroke);
    }

    [Fact]
    public void Start_ThenMove_ForwardsActiveStrokeBeforeEnd()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.Start(new ScreenPoint(5, 5));
        controller.Move(new ScreenPoint(9, 9));

        var state = view.LastRenderedState!;
        Assert.Empty(state.CompletedStrokes);
        Assert.NotNull(state.ActiveStroke);
        Assert.Equal(
            [new ScreenPoint(5, 5), new ScreenPoint(9, 9)],
            state.ActiveStroke!.Points);
    }

    [Fact]
    public void Clear_ForwardsEmptyState()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));
        controller.End();
        controller.Start(new ScreenPoint(3, 3));

        controller.Clear();

        var state = view.LastRenderedState!;
        Assert.Empty(state.CompletedStrokes);
        Assert.Null(state.ActiveStroke);
    }

    [Fact]
    public void PointerMovement_DuringDrawMode_BecomesCompletedStroke()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.EnterDrawMode();
        view.SimulateMove(new ScreenPoint(10, 20));
        view.SimulateMove(new ScreenPoint(15, 25));
        controller.ExitDrawMode();

        var stroke = Assert.Single(controller.CompletedStrokes);
        Assert.Equal(
            [new ScreenPoint(10, 20), new ScreenPoint(15, 25)],
            stroke.Points);
    }

    [Fact]
    public void PointerMovement_OutsideDrawMode_IsIgnored()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        view.SimulateMove(new ScreenPoint(10, 20));

        Assert.Empty(controller.CompletedStrokes);
    }

    private sealed class FakeOverlayView : IOverlayView
    {
        public DrawingState? LastRenderedState { get; private set; }

        public event Action<ScreenPoint>? PointerMoved;

        public void Render(DrawingState state)
        {
            LastRenderedState = state;
        }

        public void SetInputCapture(bool captureInput)
        {
        }

        public void SimulateMove(ScreenPoint point) => PointerMoved?.Invoke(point);
    }
}
