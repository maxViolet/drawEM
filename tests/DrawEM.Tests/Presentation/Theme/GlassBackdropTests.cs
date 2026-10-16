using DrawEM.App.Presentation.Theme;

namespace DrawEM.Tests.Presentation.Theme;

public class GlassBackdropTests
{
    /// <summary>Windows 10 22H2, Windows 11 21H2, Windows 11 22H2, and Windows 11 24H2.</summary>
    [Theory]
    [InlineData(19045, false)]
    [InlineData(22000, false)]
    [InlineData(22621, true)]
    [InlineData(26100, true)]
    public void IsSupported_OnlyFromTheBuildWithSystemBackdrops(int build, bool expected)
    {
        Assert.Equal(expected, GlassBackdrop.IsSupported(new Version(10, 0, build)));
    }
}
