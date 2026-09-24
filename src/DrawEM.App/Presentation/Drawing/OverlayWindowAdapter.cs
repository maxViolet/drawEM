using DrawEM.App.Application.Drawing;

namespace DrawEM.App.Presentation.Drawing;

public sealed class OverlayWindowAdapter
{
    public OverlayWindowAdapter(DrawingSessionController controller, IOverlayView view)
    {
        controller.StateChanged += view.Render;
    }
}
