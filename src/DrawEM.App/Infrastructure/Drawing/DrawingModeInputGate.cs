using System.Threading;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class DrawingModeInputGate
{
    /// <summary>Flag value for an inactive gate. The shared flag uses int for volatile access.</summary>
    private const int Inactive = 0;

    /// <summary>Flag value for an active gate.</summary>
    private const int Active = 1;

    private readonly object sync = new();
    private int active = Inactive;
    private MonitorBounds? activeBounds;
    private bool blockedUntilReleased;

    public bool IsActive => Volatile.Read(ref active) == Active;

    public bool IsBlockedUntilReleased
    {
        get { lock (sync) { return blockedUntilReleased; } }
    }

    public void SetActive(bool isActive)
    {
        lock (sync)
        {
            activeBounds = null;
            Volatile.Write(ref active, isActive ? Active : Inactive);
        }
    }

    public void Begin(MonitorBounds bounds)
    {
        lock (sync)
        {
            activeBounds = bounds;
            Volatile.Write(ref active, Active);
        }
    }

    /// <summary>
    /// Closes the gate when the pointer enters a monitor other than the starting one.
    /// A point outside every monitor counts as a position on the starting monitor: the gate
    /// stays open and <paramref name="drawPoint"/> is that point clamped to the starting monitor.
    /// </summary>
    public bool StopAtBoundary(ScreenPoint point, IMonitorBoundsSource monitors, out ScreenPoint drawPoint)
    {
        drawPoint = point;
        lock (sync)
        {
            if (StartingMonitorLeftBy(point) is not { } bounds)
            {
                return false;
            }

            if (!IsOnAnotherMonitor(monitors, point, bounds))
            {
                drawPoint = bounds.Clamp(point);
                return false;
            }

            activeBounds = null;
            blockedUntilReleased = true;
            Volatile.Write(ref active, Inactive);
            return true;
        }
    }

    /// <summary>The starting monitor while the gate is open and <paramref name="point"/> is outside it.</summary>
    private MonitorBounds? StartingMonitorLeftBy(ScreenPoint point) =>
        active != Inactive && activeBounds is { } bounds && !bounds.Contains(point) ? bounds : null;

    private static bool IsOnAnotherMonitor(IMonitorBoundsSource monitors, ScreenPoint point, MonitorBounds bounds) =>
        monitors.TryGetBounds(point, out var monitor) && monitor != bounds;

    public void BlockUntilReleased()
    {
        lock (sync) { blockedUntilReleased = true; }
    }

    public void ReleaseBlock()
    {
        lock (sync) { blockedUntilReleased = false; }
    }
}
