using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public interface ICursorPositionSource
{
    bool TryGetCurrentPosition(out ScreenPoint position);
}
