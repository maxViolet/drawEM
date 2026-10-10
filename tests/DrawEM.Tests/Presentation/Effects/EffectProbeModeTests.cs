using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

public class EffectProbeModeTests
{
    [Theory]
    [InlineData(null, EffectProbePlacement.Monitor, 1.0, "monitor")]
    [InlineData("", EffectProbePlacement.Monitor, 1.0, "monitor")]
    [InlineData("monitor", EffectProbePlacement.Monitor, 1.0, "monitor")]
    [InlineData("monitor-half", EffectProbePlacement.Monitor, 0.5, "monitor-half")]
    [InlineData("cursor", EffectProbePlacement.Cursor, 1.0, "cursor")]
    [InlineData("CURSOR", EffectProbePlacement.Cursor, 1.0, "cursor")]
    public void Parse_ReadsPlacementAndRenderScale(
        string? value, EffectProbePlacement placement, double renderScale, string name)
    {
        var mode = EffectProbeMode.Parse(value);

        Assert.Equal(placement, mode.Placement);
        Assert.Equal(renderScale, mode.RenderScale);
        Assert.Equal(name, mode.Name);
    }

    [Theory]
    [InlineData("cursor-half")]
    [InlineData("monitors")]
    public void Parse_RejectsUnknownValue(string value)
    {
        var exception = Assert.Throws<ArgumentException>(() => EffectProbeMode.Parse(value));

        Assert.Contains(EffectProbeMode.Variable, exception.Message);
    }
}
