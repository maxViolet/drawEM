using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public interface IMonitorBoundsSource
{
    bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds);
}
