using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public sealed class Win32CursorPositionSource : ICursorPositionSource
{
    public ScreenPoint GetCurrentPosition()
    {
        if (!NativeMethods.GetCursorPos(out var point))
        {
            throw new InvalidOperationException("Failed to read the cursor position.");
        }

        return new ScreenPoint(point.X, point.Y);
    }
}
