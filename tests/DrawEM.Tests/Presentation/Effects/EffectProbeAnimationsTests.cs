using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

public class EffectProbeAnimationsTests
{
    [Theory]
    [InlineData(EffectProbePlacement.Monitor, 1920, 1080, 3)]
    [InlineData(EffectProbePlacement.Cursor, 240, 240, 2)]
    public void Load_ParsesTheEmbeddedAnimationForThePlacement(
        EffectProbePlacement placement, float width, float height, double seconds)
    {
        using var animation = EffectProbeAnimations.Load(placement);

        Assert.Equal(width, animation.Size.Width);
        Assert.Equal(height, animation.Size.Height);
        Assert.Equal(seconds, animation.Duration.TotalSeconds, precision: 3);
    }
}
