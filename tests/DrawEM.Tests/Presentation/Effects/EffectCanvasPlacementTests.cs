using DrawEM.App.Domain.Drawing;
using DrawEM.App.Presentation.Effects;
using Rect = System.Windows.Rect;
using Size = System.Windows.Size;

namespace DrawEM.Tests.Presentation.Effects;

public class EffectCanvasPlacementTests
{
    private static readonly Size WideCanvas = new(1920, 1080);
    private static readonly Size RingCanvas = new(240, 240);
    private static readonly MonitorBounds Primary = new(0, 0, 1920, 1080);

    [Theory]
    [InlineData(1920, 1080, 0, 0, 1920, 1080)] // 16:9, same size
    [InlineData(3840, 2160, 0, 0, 3840, 2160)] // 16:9, larger
    [InlineData(1920, 1200, -106.666667, 0, 2133.333333, 1200)] // 16:10
    [InlineData(3440, 1440, 0, -247.5, 3440, 1935)] // 21:9
    [InlineData(1080, 1920, -1166.666667, 0, 3413.333333, 1920)] // portrait
    public void Cover_FillsTargetKeepsProportionsAndCentersOverflow(
        double targetWidth, double targetHeight, double x, double y, double width, double height)
    {
        var destination = EffectCanvasPlacement.Cover(WideCanvas, new Size(targetWidth, targetHeight));

        Assert.Equal(x, destination.X, precision: 5);
        Assert.Equal(y, destination.Y, precision: 5);
        Assert.Equal(width, destination.Width, precision: 5);
        Assert.Equal(height, destination.Height, precision: 5);
        Assert.Equal(WideCanvas.Width / WideCanvas.Height, destination.Width / destination.Height, precision: 9);
    }

    [Fact]
    public void Cover_RejectsEmptyCanvas()
    {
        Assert.Throws<ArgumentException>(() => EffectCanvasPlacement.Cover(new Size(0, 100), WideCanvas));
    }

    [Theory]
    [InlineData(1.0, 880, 380, 1120, 620)]
    [InlineData(1.5, 820, 320, 1180, 680)]
    [InlineData(2.0, 760, 260, 1240, 740)]
    public void Cursor_SizesCanvasInDipAndCentersItOnThePoint(
        double scale, int left, int top, int right, int bottom)
    {
        var bounds = EffectCanvasPlacement.Cursor(Primary, scale, RingCanvas, new ScreenPoint(1000, 500));

        Assert.Equal(new MonitorBounds(left, top, right, bottom), bounds.Canvas);
        Assert.Equal(bounds.Canvas, bounds.Window);
    }

    [Fact]
    public void Cursor_ClipsAtBottomRightEdgeWithoutShiftingTheCanvas()
    {
        var bounds = EffectCanvasPlacement.Cursor(Primary, 1.0, RingCanvas, new ScreenPoint(1915, 1075));

        Assert.Equal(new MonitorBounds(1795, 955, 2035, 1195), bounds.Canvas);
        Assert.Equal(new MonitorBounds(1795, 955, 1920, 1080), bounds.Window);
    }

    [Fact]
    public void Cursor_ClipsAtTopLeftCornerOfMonitorAtNegativeCoordinates()
    {
        var monitor = new MonitorBounds(-3840, -1080, 0, 1080);

        var bounds = EffectCanvasPlacement.Cursor(monitor, 2.0, RingCanvas, new ScreenPoint(-3830, -1070));

        Assert.Equal(new MonitorBounds(-4070, -1310, -3590, -830), bounds.Canvas);
        Assert.Equal(new MonitorBounds(-3840, -1080, -3590, -830), bounds.Window);
    }

    [Fact]
    public void Cursor_RoundsCanvasToWholePixelsAndKeepsItOnThePoint()
    {
        var bounds = EffectCanvasPlacement.Cursor(Primary, 1.25, new Size(101, 51), new ScreenPoint(500, 500));

        // 101 x 1.25 = 126.25 -> 126 px, 51 x 1.25 = 63.75 -> 64 px.
        Assert.Equal(new MonitorBounds(437, 468, 563, 532), bounds.Canvas);
    }

    [Fact]
    public void Cursor_RejectsPointOutsideTheMonitor()
    {
        Assert.Throws<ArgumentException>(
            () => EffectCanvasPlacement.Cursor(Primary, 1.0, RingCanvas, new ScreenPoint(1920, 500)));
    }

    [Fact]
    public void CanvasRectIn_MapsCanvasToWindowPixelsAtRenderScale()
    {
        var bounds = EffectCanvasPlacement.Cursor(Primary, 1.0, RingCanvas, new ScreenPoint(1915, 1075));

        Assert.Equal(new Rect(0, 0, 240, 240), bounds.CanvasRectInWindow(1.0));
        Assert.Equal(new Rect(0, 0, 120, 120), bounds.CanvasRectInWindow(0.5));

        var clipped = EffectCanvasPlacement.Cursor(
            new MonitorBounds(-3840, -1080, 0, 1080), 1.0, RingCanvas, new ScreenPoint(-3830, -1070));
        Assert.Equal(new Rect(-110, -110, 240, 240), clipped.CanvasRectInWindow(1.0));
    }
}
