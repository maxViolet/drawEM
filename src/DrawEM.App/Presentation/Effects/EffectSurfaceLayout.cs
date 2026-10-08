using DrawEM.App.Domain.Drawing;
using Point = System.Windows.Point;
using Rect = System.Windows.Rect;
using Size = System.Windows.Size;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// Maps physical desktop pixels to the local DIPs of an effect window that covers exactly one monitor.
/// The window is per-monitor-aware and placed on <see cref="Monitor"/>, so its DPI scale is that monitor's
/// scale and its local origin is the monitor's top-left corner, including at negative desktop coordinates.
/// </summary>
public readonly record struct EffectSurfaceLayout
{
    public EffectSurfaceLayout(MonitorBounds monitor, double dpiScale)
    {
        if (monitor.Right <= monitor.Left || monitor.Bottom <= monitor.Top)
        {
            throw new ArgumentException("The monitor bounds must not be empty.", nameof(monitor));
        }

        if (!double.IsFinite(dpiScale) || dpiScale <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dpiScale), dpiScale, "The DPI scale must be positive.");
        }

        Monitor = monitor;
        DpiScale = dpiScale;
    }

    public MonitorBounds Monitor { get; }

    /// <summary>Physical pixels per DIP: 1.0 at 100% scaling, 1.5 at 150%, 2.0 at 200%.</summary>
    public double DpiScale { get; }

    public Size LocalSize => new((Monitor.Right - Monitor.Left) / DpiScale, (Monitor.Bottom - Monitor.Top) / DpiScale);

    /// <summary>The drawable area of the surface. Content outside it would belong to another monitor.</summary>
    public Rect LocalBounds => new(LocalSize);

    public Point ToLocalPoint(ScreenPoint point) =>
        new((point.X - Monitor.Left) / DpiScale, (point.Y - Monitor.Top) / DpiScale);

    /// <summary>The part of <paramref name="localRect"/> that lies on this monitor, or an empty rectangle.</summary>
    public Rect Clip(Rect localRect) => Rect.Intersect(localRect, LocalBounds);
}
