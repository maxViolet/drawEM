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

    public bool StopAtBoundary(ScreenPoint point)
    {
        lock (sync)
        {
            if (active == Inactive || activeBounds is not { } bounds || bounds.Contains(point))
            {
                return false;
            }

            activeBounds = null;
            blockedUntilReleased = true;
            Volatile.Write(ref active, Inactive);
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
