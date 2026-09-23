using System.Threading;
using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public sealed class DrawingModeInputGate
{
    private readonly object sync = new();
    private int active;
    private MonitorBounds? activeBounds;
    private bool blockedUntilReleased;

    public bool IsActive => Volatile.Read(ref active) == 1;

    public bool IsBlockedUntilReleased
    {
        get { lock (sync) { return blockedUntilReleased; } }
    }

    public void SetActive(bool isActive)
    {
        lock (sync)
        {
            activeBounds = null;
            Volatile.Write(ref active, isActive ? 1 : 0);
        }
    }

    public void Begin(MonitorBounds bounds)
    {
        lock (sync)
        {
            activeBounds = bounds;
            Volatile.Write(ref active, 1);
        }
    }

    public bool StopAtBoundary(ScreenPoint point)
    {
        lock (sync)
        {
            if (active == 0 || activeBounds is not { } bounds || bounds.Contains(point))
            {
                return false;
            }

            activeBounds = null;
            blockedUntilReleased = true;
            Volatile.Write(ref active, 0);
            return true;
        }
    }

    public void BlockUntilReleased()
    {
        lock (sync) { blockedUntilReleased = true; }
    }

    public void ReleaseBlock()
    {
        lock (sync) { blockedUntilReleased = false; }
    }
}
