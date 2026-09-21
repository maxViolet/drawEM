using DrawEM.App.Application;
using DrawEM.App.Domain;
using DrawEM.App.Presentation;

namespace DrawEM.Tests.Presentation;

public class OverlayInputCaptureTests
{
    [Fact]
    public void EnterDrawMode_RequestsInputCapture()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.EnterDrawMode();

        Assert.True(view.IsInputCaptured);
    }

    [Fact]
    public void ExitDrawMode_ReleasesInputCapture()
    {
        var controller = new DrawingSessionController();
        var view = new FakeOverlayView();
        _ = new OverlayWindowAdapter(controller, view);

        controller.EnterDrawMode();
        controller.ExitDrawMode();

        Assert.False(view.IsInputCaptured);
    }

    private sealed class FakeOverlayView : IOverlayView
    {
        public bool IsInputCaptured { get; private set; }

        public event Action<ScreenPoint>? PointerMoved;

        public void Render(DrawingState state)
        {
        }

        public void SetInputCapture(bool captureInput)
        {
            IsInputCaptured = captureInput;
        }
    }
}
