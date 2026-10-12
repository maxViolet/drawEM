using DrawEM.App.Domain.Drawing;
using DrawEM.App.Presentation.Effects;
using Rect = System.Windows.Rect;
using Size = System.Windows.Size;

namespace DrawEM.Tests.Presentation.Effects;

public class EffectRenderLayoutTests
{
    private static readonly Size WideCanvas = new(1920, 1080);
    private static readonly Size RingCanvas = new(240, 240);

    [Fact]
    public void Monitor_CoversTheMonitorWithAHalfResolutionBitmap()
    {
        var monitor = new MonitorBounds(0, 0, 1920, 1200);

        var layout = EffectRenderLayout.Monitor(monitor, WideCanvas);

        Assert.Equal(monitor, layout.Window);
        Assert.Equal(0.5, layout.RenderScale);
        Assert.Equal((960, 600), (layout.BitmapWidth, layout.BitmapHeight));
        AssertRect(new Rect(-53.333333, 0, 1066.666667, 600), layout.Destination);
    }

    [Fact]
    public void Monitor_RoundsAnOddBitmapSizeUpAtNegativeCoordinates()
    {
        var layout = EffectRenderLayout.Monitor(new MonitorBounds(-1365, -767, 0, 0), WideCanvas);

        Assert.Equal((683, 384), (layout.BitmapWidth, layout.BitmapHeight));
        AssertRect(EffectCanvasPlacement.Cover(WideCanvas, new Size(683, 384)), layout.Destination);
    }

    [Fact]
    public void Cursor_DrawsTheWholeCanvasAtFullResolutionInsideTheClippedWindow()
    {
        var monitor = new MonitorBounds(0, 0, 1920, 1080);

        var layout = EffectRenderLayout.Cursor(monitor, 1.5, RingCanvas, new ScreenPoint(1915, 1075));

        Assert.Equal(new MonitorBounds(1735, 895, 1920, 1080), layout.Window);
        Assert.Equal(1.0, layout.RenderScale);
        Assert.Equal((185, 185), (layout.BitmapWidth, layout.BitmapHeight));
        Assert.Equal(new Rect(0, 0, 360, 360), layout.Destination);
    }

    [Fact]
    public void Cursor_KeepsTheCanvasOnThePointAtATopLeftCornerAtNegativeCoordinates()
    {
        var layout = EffectRenderLayout.Cursor(
            new MonitorBounds(-3840, -1080, 0, 1080), 2.0, RingCanvas, new ScreenPoint(-3830, -1070));

        Assert.Equal(new MonitorBounds(-3840, -1080, -3590, -830), layout.Window);
        Assert.Equal(new Rect(-230, -230, 480, 480), layout.Destination);
    }

    private static void AssertRect(Rect expected, Rect actual)
    {
        Assert.Equal(expected.X, actual.X, precision: 4);
        Assert.Equal(expected.Y, actual.Y, precision: 4);
        Assert.Equal(expected.Width, actual.Width, precision: 4);
        Assert.Equal(expected.Height, actual.Height, precision: 4);
    }
}
