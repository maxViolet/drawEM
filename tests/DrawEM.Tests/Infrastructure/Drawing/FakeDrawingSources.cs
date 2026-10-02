using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

/// <summary>Reports one fixed cursor position.</summary>
internal sealed class FakeCursorPositionSource(ScreenPoint point) : ICursorPositionSource
{
    public bool TryGetCurrentPosition(out ScreenPoint position)
    {
        position = point;
        return true;
    }
}

/// <summary>Finds the first monitor that contains the point.</summary>
internal sealed class FakeMonitorBoundsSource(params MonitorBounds[] monitors) : IMonitorBoundsSource
{
    public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
    {
        foreach (var monitor in monitors)
        {
            if (monitor.Contains(point))
            {
                bounds = monitor;
                return true;
            }
        }

        bounds = default;
        return false;
    }
}
