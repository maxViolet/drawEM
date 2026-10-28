using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

/// <summary>Asks Windows for the monitors of this machine; the primary monitor always contains (0, 0).</summary>
public sealed class Win32MonitorBoundsSourceTests
{
    [Fact]
    public void TryGetBounds_ReturnsThePrimaryMonitorForTheScreenOrigin()
    {
        Assert.True(new Win32MonitorBoundsSource().TryGetBounds(new ScreenPoint(0, 0), out var bounds));

        Assert.Equal(0, bounds.Left);
        Assert.Equal(0, bounds.Top);
        Assert.True(bounds.Right > 0 && bounds.Bottom > 0);
    }

    [Fact]
    public void TryGetBounds_RejectsAPointOutsideEveryMonitor()
    {
        Assert.False(new Win32MonitorBoundsSource().TryGetBounds(new ScreenPoint(-1_000_000, -1_000_000), out var bounds));

        Assert.Equal(default, bounds);
    }
}
