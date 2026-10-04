using System.Windows.Media;
using DrawEM.App.Domain.Drawing;
using Point = System.Windows.Point;
using Vector = System.Windows.Vector;

namespace DrawEM.App.Presentation.Drawing;

/// <summary>
/// Maps physical desktop pixels to the overlay's local coordinates. The overlay is one per-monitor-aware
/// window, which Windows never bitmap-scales: a physical pixel has the same local size on every monitor,
/// including a monitor whose DPI differs from the window's. Points and lengths therefore share
/// <see cref="DeviceToLogical"/>; the DPI of the monitor under a stroke does not enter the conversion.
/// </summary>
public readonly record struct PhysicalToLocalTransform(int OriginX, int OriginY, Matrix DeviceToLogical)
{
    public Point ToLocalPoint(ScreenPoint point) =>
        DeviceToLogical.Transform(new Point(point.X - OriginX, point.Y - OriginY));

    /// <summary>The local length of <paramref name="physicalPixels"/>, for a pen width or dot diameter.</summary>
    public double ToLocalLength(int physicalPixels) =>
        DeviceToLogical.Transform(new Vector(physicalPixels, 0)).Length;
}
