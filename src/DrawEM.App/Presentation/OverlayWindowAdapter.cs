using DrawEM.App.Application;

namespace DrawEM.App.Presentation;

public sealed class OverlayWindowAdapter
{
    public OverlayWindowAdapter(DrawingSessionController controller, IOverlayView view)
    {
        controller.StateChanged += view.Render;
    }
}
