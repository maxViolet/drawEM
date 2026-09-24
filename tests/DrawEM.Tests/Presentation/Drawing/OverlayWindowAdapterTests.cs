using DrawEM.App.Application.Drawing;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Presentation.Drawing;

namespace DrawEM.Tests.Presentation.Drawing;

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
    public void ClearAndExitDrawMode_ForwardsEmptyState()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));
        controller.End();
        controller.Start(new ScreenPoint(3, 3));

        controller.ClearAndExitDrawMode();

        var state = view.LastRenderedState!;
        Assert.Empty(state.CompletedStrokes);
        Assert.Null(state.ActiveStroke);
    }

    private sealed class FakeOverlayView : IOverlayView
    {
        public DrawingState? LastRenderedState { get; private set; }

        public void Render(DrawingState state)
        {
            LastRenderedState = state;
        }

    }
}
