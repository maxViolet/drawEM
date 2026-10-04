using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawEM.App.Application.Drawing;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Presentation.Drawing;

namespace DrawEM.Tests.Presentation.Drawing;

public class StrokeRenderElementTests
{
    /// <summary>OrangeRed <c>#FF4500</c>, 4 physical pixels.</summary>
    private static readonly DrawingStyle DefaultStyle = SettingsSnapshot.Default.Style;

    /// <summary>Alpha of a fully opaque pixel (Pbgra32 byte 3).</summary>
    private const byte OpaqueAlpha = 0xFF;

    /// <summary>WPF units per inch; a bitmap at this DPI has one pixel per unit.</summary>
    private const double UnitsPerInch = 96;

    /// <summary>Pbgra32 stores blue, green, red, and alpha bytes per pixel.</summary>
    private const int BytesPerPixel = 4;

    /// <summary>Size, in pixels, of the square element that the incremental-rendering tests use.</summary>
    private const int SmallSurfaceSize = 20;

    /// <summary>Widths 1, 4 (default), and 20 at overlay scales 100% and 150%.</summary>
    public static TheoryData<int, double> WidthsAndScales => new()
    {
        { StrokeWidth.Min, 1.0 },
        { 4, 1.0 },
        { StrokeWidth.Max, 1.0 },
        { StrokeWidth.Min, 1.5 },
        { 4, 1.5 },
        { StrokeWidth.Max, 1.5 },
    };

    /// <summary>
    /// Allowed difference, in physical pixels, between a measured and a chosen width. Anti-aliasing
    /// spreads an edge over partial pixels; coverage counts them fractionally but keeps rounding error.
    /// </summary>
    private const double WidthTolerance = 0.25;

    [Fact]
    public void UpdateState_ClipsStrokeThicknessToItsStartingMonitor()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var bounds = new MonitorBounds(0, 0, 10, 20);
            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(9, 10)], DefaultStyle, bounds), true, 0),
                new PhysicalToLocalTransform(0, 0, Matrix.Identity));

            Assert.True(PixelAlphaAt(element, 9, 10) > 0);
            Assert.Equal((byte)0, PixelAlphaAt(element, 10, 10));
        });
    }

    [Fact]
    public void UpdateState_RendersSinglePointStroke()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement
            {
                Width = 20,
                Height = 20,
            };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            element.UpdateState(
                new DrawingState(
                    [],
                    new Stroke([new ScreenPoint(10, 10)], DefaultStyle),
                    true,
                    0),
                new PhysicalToLocalTransform(0, 0, Matrix.Identity));

            var pixels = RenderPixels(element, SmallSurfaceSize, SmallSurfaceSize, 1.0);

            Assert.Equal(Opaque(DefaultStyle.Color), PixelAt(pixels, SmallSurfaceSize, 10, 10));
        });
    }

    [Fact]
    public void UpdateState_GrowingActiveStroke_RendersEachIncrementalSegment()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);

            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(2, 10)], DefaultStyle), true, 0),
                transform);
            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(2, 10), new ScreenPoint(10, 10)], DefaultStyle), true, 0),
                transform);
            element.UpdateState(
                new DrawingState(
                    [],
                    new Stroke([new ScreenPoint(2, 10), new ScreenPoint(10, 10), new ScreenPoint(17, 10)], DefaultStyle),
                    true,
                    0),
                transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 17, 10));
            Assert.Equal((byte)255, PixelAlphaAt(element, 10, 10));
        });
    }

    [Fact]
    public void UpdateState_StrokeCompletesThenNewOneStarts_KeepsCompletedPixelsVisible()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var completed = new Stroke([new ScreenPoint(1, 1), new ScreenPoint(5, 1)], DefaultStyle);

            element.UpdateState(new DrawingState([], completed, true, 0), transform);
            element.UpdateState(new DrawingState([completed], null, false, 0), transform);
            element.UpdateState(
                new DrawingState([completed], new Stroke([new ScreenPoint(15, 15)], DefaultStyle), true, 0),
                transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 5, 1));
            Assert.Equal((byte)255, PixelAlphaAt(element, 15, 15));
        });
    }

    [Fact]
    public void UpdateState_FewerCompletedStrokesThanBefore_ResetsAndDropsOldPixels()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var completed = new Stroke([new ScreenPoint(1, 1), new ScreenPoint(5, 1)], DefaultStyle);

            element.UpdateState(new DrawingState([], completed, true, 0), transform);
            element.UpdateState(new DrawingState([completed], null, false, 0), transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 5, 1));

            element.UpdateState(new DrawingState([], null, false, 1), transform);

            Assert.Equal((byte)0, PixelAlphaAt(element, 5, 1));
        });
    }

    [Fact]
    public void UpdateState_CoalescedClearThenNewStrokeOfSameLength_DropsOldAndDrawsNew()
    {
        // Regression for a coalesced pendingState update that folds clear -> new
        // stroke -> end into one call: CompletedStrokes.Count is unchanged (1 -> 1),
        // so only Generation distinguishes this from "nothing happened".
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var oldStroke = new Stroke([new ScreenPoint(1, 1), new ScreenPoint(5, 1)], DefaultStyle);
            var newStroke = new Stroke([new ScreenPoint(12, 12), new ScreenPoint(16, 12)], DefaultStyle);

            element.UpdateState(new DrawingState([oldStroke], null, false, 0), transform);
            Assert.Equal((byte)255, PixelAlphaAt(element, 5, 1));

            element.UpdateState(new DrawingState([newStroke], null, false, 1), transform);

            Assert.Equal((byte)0, PixelAlphaAt(element, 5, 1));
            Assert.Equal((byte)255, PixelAlphaAt(element, 16, 12));
        });
    }

    [Fact]
    public void UpdateState_StrokeEndsWithUnseenPoint_DropsStalePartialVisualAndDrawsFullStroke()
    {
        // Regression for a coalesced update where the active stroke gained a point the
        // renderer never saw as "active" (e.g. a move batched together with the End),
        // so the match heuristic in UpdateState cannot recognize the completed stroke
        // as a continuation of what it already drew.
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);

            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(2, 2), new ScreenPoint(6, 2)], DefaultStyle), true, 0),
                transform);

            var completedWithExtraPoint = new Stroke(
                [new ScreenPoint(2, 2), new ScreenPoint(6, 2), new ScreenPoint(10, 2)],
                DefaultStyle);
            element.UpdateState(new DrawingState([completedWithExtraPoint], null, false, 0), transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 10, 2));

            // A brand-new stroke starting at the same first point must not be treated
            // as a continuation of the stale (and now removed) tracking above.
            element.UpdateState(
                new DrawingState(
                    [completedWithExtraPoint],
                    new Stroke([new ScreenPoint(2, 2)], DefaultStyle),
                    true,
                    0),
                transform);
            element.UpdateState(
                new DrawingState(
                    [completedWithExtraPoint],
                    new Stroke([new ScreenPoint(2, 2), new ScreenPoint(2, 15)], DefaultStyle),
                    true,
                    0),
                transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 2, 15));
        });
    }

    [Fact]
    public void ClearDuringFirstActiveStroke_DropsPixelsThroughRealController()
    {
        // Regression: CompletedStrokes.Count stays 0 -> 0 when a clear happens before the
        // first stroke ever completes, so only DrawingState.Generation can signal the
        // reset. Wires a real DrawingSessionController to a real StrokeRenderElement, the
        // same way OverlayWindowAdapter/OverlayWindow.Render do, instead of hand-built
        // DrawingState values.
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var controller = new DrawingSessionController();
            controller.StateChanged += state => element.UpdateState(state, transform);

            controller.EnterDrawMode(new ScreenPoint(5, 5));
            controller.Move(new ScreenPoint(15, 5));

            Assert.Equal((byte)255, PixelAlphaAt(element, 15, 5));
            Assert.Empty(controller.CompletedStrokes);

            controller.ClearAndExitDrawMode();

            Assert.Equal((byte)0, PixelAlphaAt(element, 15, 5));
            Assert.Empty(controller.CompletedStrokes);
        });
    }

    [Theory]
    [MemberData(nameof(WidthsAndScales))]
    public void UpdateState_LineHasChosenPhysicalWidth(int width, double overlayScale)
    {
        RunOnStaThread(() =>
        {
            var surface = new DeviceSurface(60, 60, overlayScale);
            surface.Render(new Stroke([new ScreenPoint(10, 30), new ScreenPoint(50, 30)], StyleOfWidth(width)));

            AssertWidth(width, surface.ColumnCoverage(30, 0, 60));
        });
    }

    [Theory]
    [MemberData(nameof(WidthsAndScales))]
    public void UpdateState_DotHasChosenPhysicalDiameter(int width, double overlayScale)
    {
        RunOnStaThread(() =>
        {
            var surface = new DeviceSurface(60, 60, overlayScale);
            surface.Render(new Stroke([new ScreenPoint(30, 30)], StyleOfWidth(width)));

            AssertWidth(width, surface.DotDiameter(0, 0, 60, 60));
        });
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    public void UpdateState_StrokesOnTwoMonitors_KeepPhysicalWidthOnEachMonitor(double overlayScale)
    {
        // The overlay is one per-monitor-aware window, so it has one DPI even when its monitors differ.
        // Windows does not stretch it on the other monitor: a physical pixel is one surface pixel everywhere.
        // Mixed DPI is therefore handled by design, not by a monitor DPI input. This test shows that
        // clipping each stroke to its monitor keeps the physical width at either window DPI.
        const int width = 6;
        RunOnStaThread(() =>
        {
            var surface = new DeviceSurface(80, 60, overlayScale);
            var left = new MonitorBounds(0, 0, 40, 60);
            var right = new MonitorBounds(40, 0, 80, 60);
            var style = StyleOfWidth(width);

            surface.Render(
                new Stroke([new ScreenPoint(5, 15), new ScreenPoint(35, 15)], style, left),
                new Stroke([new ScreenPoint(20, 45)], style, left),
                new Stroke([new ScreenPoint(45, 15), new ScreenPoint(75, 15)], style, right),
                new Stroke([new ScreenPoint(60, 45)], style, right));

            AssertWidth(width, surface.ColumnCoverage(20, 0, 30));
            AssertWidth(width, surface.ColumnCoverage(60, 0, 30));
            AssertWidth(width, surface.DotDiameter(0, 30, 40, 60));
            AssertWidth(width, surface.DotDiameter(40, 30, 80, 60));
        });
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    public void UpdateState_UsesStrokeHexColor(double overlayScale)
    {
        RunOnStaThread(() =>
        {
            var surface = new DeviceSurface(20, 20, overlayScale);
            var color = new HexColor(0x1E, 0x90, 0xC8);
            surface.Render(new Stroke([new ScreenPoint(10, 10)], new DrawingStyle(color, new StrokeWidth(8))));

            Assert.Equal(Opaque(color), surface.PixelAt(10, 10));
        });
    }

    [Fact]
    public void UpdateState_StrokesKeepTheirOwnStyle()
    {
        RunOnStaThread(() =>
        {
            var surface = new DeviceSurface(60, 20, 1.0);
            var blue = new HexColor(0x00, 0x00, 0xFF);

            surface.Render(
                new Stroke([new ScreenPoint(10, 10)], DefaultStyle),
                new Stroke([new ScreenPoint(40, 10)], new DrawingStyle(blue, new StrokeWidth(StrokeWidth.Max))));

            Assert.Equal(Opaque(DefaultStyle.Color), surface.PixelAt(10, 10));
            Assert.Equal(Opaque(blue), surface.PixelAt(40, 10));
            AssertWidth(DefaultStyle.Width.Pixels, surface.DotDiameter(0, 0, 20, 20));
        });
    }

    private static DrawingStyle StyleOfWidth(int width) => DefaultStyle with { Width = new StrokeWidth(width) };

    private static void AssertWidth(int expected, double measured) =>
        Assert.InRange(measured, expected - WidthTolerance, expected + WidthTolerance);

    private static (byte Blue, byte Green, byte Red, byte Alpha) Opaque(HexColor color) =>
        (color.Blue, color.Green, color.Red, OpaqueAlpha);

    /// <summary>Renders <paramref name="element"/> into a bitmap with one pixel per physical pixel.</summary>
    private static byte[] RenderPixels(StrokeRenderElement element, int width, int height, double scale)
    {
        var bitmap = new RenderTargetBitmap(
            width, height, UnitsPerInch * scale, UnitsPerInch * scale, PixelFormats.Pbgra32);
        bitmap.Render(element);
        var pixels = new byte[width * height * BytesPerPixel];
        bitmap.CopyPixels(pixels, width * BytesPerPixel, 0);
        return pixels;
    }

    private static (byte Blue, byte Green, byte Red, byte Alpha) PixelAt(byte[] pixels, int width, int x, int y)
    {
        var offset = ((y * width) + x) * BytesPerPixel;
        return (pixels[offset], pixels[offset + 1], pixels[offset + 2], pixels[offset + 3]);
    }

    /// <summary>
    /// A stroke element rendered at an overlay scale into a bitmap with one pixel per physical pixel.
    /// Coverage is the sum of alpha over a region, so anti-aliased edge pixels count fractionally.
    /// </summary>
    private sealed class DeviceSurface
    {
        private readonly int width;
        private readonly int height;
        private readonly double scale;
        private readonly StrokeRenderElement element;
        private byte[] pixels = [];

        public DeviceSurface(int width, int height, double scale)
        {
            this.width = width;
            this.height = height;
            this.scale = scale;
            var size = new Size(width / scale, height / scale);
            element = new StrokeRenderElement { Width = size.Width, Height = size.Height };
            element.Measure(size);
            element.Arrange(new Rect(size));
        }

        public void Render(params Stroke[] strokes)
        {
            element.UpdateState(
                new DrawingState(strokes, null, false, 0),
                new PhysicalToLocalTransform(0, 0, new Matrix(1 / scale, 0, 0, 1 / scale, 0, 0)));
            pixels = RenderPixels(element, width, height, scale);
        }

        public (byte Blue, byte Green, byte Red, byte Alpha) PixelAt(int x, int y) =>
            StrokeRenderElementTests.PixelAt(pixels, width, x, y);

        /// <summary>Covered pixels in column <paramref name="x"/>: a horizontal line's width.</summary>
        public double ColumnCoverage(int x, int top, int bottom) => Coverage(x, top, x + 1, bottom);

        /// <summary>The diameter of a circle with the covered area of the region.</summary>
        public double DotDiameter(int left, int top, int right, int bottom) =>
            2 * Math.Sqrt(Coverage(left, top, right, bottom) / Math.PI);

        private double Coverage(int left, int top, int right, int bottom)
        {
            var alpha = 0d;
            for (var y = top; y < bottom; y++)
            {
                for (var x = left; x < right; x++)
                {
                    alpha += PixelAt(x, y).Alpha;
                }
            }

            return alpha / byte.MaxValue;
        }
    }

    private static byte PixelAlphaAt(StrokeRenderElement element, int x, int y) =>
        PixelAt(RenderPixels(element, SmallSurfaceSize, SmallSurfaceSize, 1.0), SmallSurfaceSize, x, y).Alpha;

    private static void RunOnStaThread(Action action)
    {
        Exception? exception = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception caught)
            {
                exception = caught;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (exception is not null)
        {
            throw new InvalidOperationException("The STA render assertion failed.", exception);
        }
    }
}
