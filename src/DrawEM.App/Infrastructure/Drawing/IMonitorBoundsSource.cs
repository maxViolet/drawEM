using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public interface IMonitorBoundsSource
{
    bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds);
}
