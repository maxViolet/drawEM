using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class Win32MonitorBoundsSource : IMonitorBoundsSource
{
    public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
    {
        var monitor = NativeMethods.MonitorFromPoint(new NativeMethods.POINT { X = point.X, Y = point.Y }, NativeMethods.MONITOR_DEFAULTTONULL);
        var info = new NativeMethods.MONITORINFO { cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf<NativeMethods.MONITORINFO>() };
        if (monitor == IntPtr.Zero || !NativeMethods.GetMonitorInfo(monitor, ref info))
        {
            bounds = default;
            return false;
        }

        bounds = new MonitorBounds(info.rcMonitor.Left, info.rcMonitor.Top,
            info.rcMonitor.Right, info.rcMonitor.Bottom);
        return true;
    }
}
