using DrawEM.App.Domain.Effects;

namespace DrawEM.Tests.Domain.Effects;

public class EffectCatalogTests
{
    [Fact]
    public void BuiltIn_ListsConfettiOnMonitorAndFocusRingOnCursor()
    {
        Assert.Equal(
            [
                new BuiltInEffect(new EffectId("confetti"), "Confetti", EffectPlacement.Monitor, "confetti.json"),
                new BuiltInEffect(new EffectId("focus-ring"), "Focus ring", EffectPlacement.Cursor, "focus-ring.json"),
            ],
            EffectCatalog.BuiltIn);
    }

    [Theory]
    [InlineData("confetti", EffectPlacement.Monitor)]
    [InlineData("focus-ring", EffectPlacement.Cursor)]
    public void TryFind_ReturnsTheEntryForAKnownId(string id, EffectPlacement placement)
    {
        Assert.True(EffectCatalog.TryFind(new EffectId(id), out var effect));
        Assert.Equal(placement, effect!.Placement);
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
