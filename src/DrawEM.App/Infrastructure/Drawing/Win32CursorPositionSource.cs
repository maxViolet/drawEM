using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class Win32CursorPositionSource : ICursorPositionSource
{
    public bool TryGetCurrentPosition(out ScreenPoint position)
    {
        if (!NativeMethods.GetCursorPos(out var point))
        {
            position = default;
            return false;
        }

        position = new ScreenPoint(point.X, point.Y);
        return true;
    }
}
