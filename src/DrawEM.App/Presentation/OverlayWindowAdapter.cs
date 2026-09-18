using DrawEM.App.Domain;

namespace DrawEM.App.Presentation;

public sealed class OverlayWindowAdapter
{
    public OverlayWindowAdapter(DrawingSessionController controller, IOverlayView view)
    {
        controller.StateChanged += view.Render;
        controller.InputCaptureRequested += view.SetInputCapture;
        view.PointerMoved += controller.ReportPointer;
    }
}
