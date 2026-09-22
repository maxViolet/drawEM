using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public interface ICursorPositionSource
{
    ScreenPoint GetCurrentPosition();
}
