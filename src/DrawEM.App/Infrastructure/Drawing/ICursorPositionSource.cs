using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public interface ICursorPositionSource
{
    bool TryGetCurrentPosition(out ScreenPoint position);
}
