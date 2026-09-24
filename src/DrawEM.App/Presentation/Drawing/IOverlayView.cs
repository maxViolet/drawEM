using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Presentation.Drawing;

public interface IOverlayView
{
    void Render(DrawingState state);

}
