using DrawEM.App.Domain;

namespace DrawEM.App.Presentation;

public interface IOverlayView
{
    void Render(DrawingState state);

    void SetInputCapture(bool captureInput);

    event Action<ScreenPoint>? PointerMoved;
}
