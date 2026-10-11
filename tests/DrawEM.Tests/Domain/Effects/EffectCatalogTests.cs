using DrawEM.App.Domain.Effects;

namespace DrawEM.Tests.Domain.Effects;

public class EffectCatalogTests
{
    [Fact]
    public void BuiltIn_ListsConfettiOnMonitorAndFocusRingOnCursor()
    {
        Assert.Equal(
            [
                (new EffectId("confetti"), EffectPlacement.Monitor),
                (new EffectId("focus-ring"), EffectPlacement.Cursor),
            ],
            EffectCatalog.BuiltIn.Select(effect => (effect.Id, effect.Placement)));
    }

    [Theory]
    [InlineData("confetti", EffectPlacement.Monitor)]
    [InlineData("focus-ring", EffectPlacement.Cursor)]
    public void TryFind_ReturnsTheEntryForAKnownId(string id, EffectPlacement placement)
    {
        Assert.True(EffectCatalog.TryFind(new EffectId(id), out var effect));
        Assert.Equal(placement, effect.Placement);
    }

    [Theory]
    [InlineData("sparkles")]
    [InlineData("Confetti")]
    public void TryFind_RejectsAnUnknownId(string id)
    {
        Assert.False(EffectCatalog.TryFind(new EffectId(id), out var effect));
        Assert.Null(effect);
    }
}
