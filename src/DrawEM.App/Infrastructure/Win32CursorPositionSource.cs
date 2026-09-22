using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

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
