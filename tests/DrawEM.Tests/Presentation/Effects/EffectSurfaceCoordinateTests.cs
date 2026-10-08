using DrawEM.App.Domain.Drawing;
using DrawEM.App.Presentation.Effects;
using Point = System.Windows.Point;
using Rect = System.Windows.Rect;

namespace DrawEM.Tests.Presentation.Effects;

public class EffectSurfaceCoordinateTests
{
    /// <summary>A 3840x2160 monitor left of and above the primary monitor's origin.</summary>
    private static readonly MonitorBounds NegativeMonitor = new(-3840, -1080, 0, 1080);

    [Theory]
    [InlineData(1.0, 3840, 2160)]
    [InlineData(1.5, 2560, 1440)]
    [InlineData(2.0, 1920, 1080)]
    public void LocalSize_IsPhysicalSizeDividedByScale(double scale, double width, double height)
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, scale);

        Assert.Equal(width, layout.LocalSize.Width, precision: 6);
        Assert.Equal(height, layout.LocalSize.Height, precision: 6);
        Assert.Equal(new Rect(0, 0, width, height), layout.LocalBounds);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void ToLocalPoint_MapsNegativeMonitorOriginToSurfaceOrigin(double scale)
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, scale);

        Assert.Equal(new Point(0, 0), layout.ToLocalPoint(new ScreenPoint(-3840, -1080)));
    }

    [Theory]
    [InlineData(1.0, 1920, 1080)]
    [InlineData(1.5, 1280, 720)]
    [InlineData(2.0, 960, 540)]
    public void ToLocalPoint_DividesOffsetFromMonitorOriginByScale(double scale, double x, double y)
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, scale);

        var local = layout.ToLocalPoint(new ScreenPoint(-1920, 0));

        Assert.Equal(x, local.X, precision: 6);
        Assert.Equal(y, local.Y, precision: 6);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void ToLocalPoint_PointOnNeighborMonitorFallsOutsideLocalBounds(double scale)
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, scale);

        // The primary monitor starts at this monitor's exclusive right edge, x = 0.
        Assert.True(layout.ToLocalPoint(new ScreenPoint(0, 0)).X >= layout.LocalSize.Width);
        Assert.True(layout.ToLocalPoint(new ScreenPoint(-3841, -1081)) is { X: < 0, Y: < 0 });
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void Clip_ShapeCrossingRightEdge_KeepsOnlyThePartOnThisMonitor(double scale)
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, scale);
        var width = layout.LocalSize.Width;
        var shape = new Rect(width - 50, 100, 160, 160);

        var clipped = layout.Clip(shape);

        Assert.Equal(new Rect(width - 50, 100, 50, 160), clipped);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void Clip_ShapeCrossingTopLeftCorner_KeepsOnlyThePartOnThisMonitor(double scale)
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, scale);

        var clipped = layout.Clip(new Rect(-80, -80, 160, 160));

        Assert.Equal(new Rect(0, 0, 80, 80), clipped);
    }

    [Fact]
    public void Clip_ShapeEntirelyOnNeighborMonitor_IsEmpty()
    {
        var layout = new EffectSurfaceLayout(NegativeMonitor, 1.5);
        var neighbor = layout.ToLocalPoint(new ScreenPoint(100, 100));

        Assert.True(layout.Clip(new Rect(neighbor.X, neighbor.Y, 160, 160)).IsEmpty);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void Constructor_RejectsInvalidScale(double scale)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new EffectSurfaceLayout(NegativeMonitor, scale));
    }

    [Fact]
    public void Constructor_RejectsEmptyMonitor()
    {
        Assert.Throws<ArgumentException>(() => new EffectSurfaceLayout(new MonitorBounds(0, 0, 0, 100), 1.0));
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("0", false)]
    [InlineData("true", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Probe_IsEnabledOnlyByOne(string? value, bool enabled)
    {
        Assert.Equal(enabled, EffectSurfaceProbe.IsEnabled(value));
    }
}
