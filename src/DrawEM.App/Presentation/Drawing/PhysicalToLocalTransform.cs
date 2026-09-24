using System.Windows.Media;
using DrawEM.App.Domain.Drawing;
using Point = System.Windows.Point;

namespace DrawEM.App.Presentation.Drawing;

public readonly record struct PhysicalToLocalTransform(int OriginX, int OriginY, Matrix DeviceToLogical)
{
    public Point ToLocalPoint(ScreenPoint point) =>
        DeviceToLogical.Transform(new Point(point.X - OriginX, point.Y - OriginY));
}
