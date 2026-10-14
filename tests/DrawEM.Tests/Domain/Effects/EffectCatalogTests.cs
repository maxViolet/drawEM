using DrawEM.App.Domain.Effects;

namespace DrawEM.Tests.Domain.Effects;

public class EffectCatalogTests
{
    [Fact]
    public void BuiltIn_ListsEveryEffectWithItsPlacement()
    {
        Assert.Equal(
            [
                (new EffectId("confetti"), EffectPlacement.Monitor),
                (new EffectId("focus-ring"), EffectPlacement.Cursor),
                (new EffectId("confetti-transparent"), EffectPlacement.Monitor),
                (new EffectId("confetti-effects"), EffectPlacement.Monitor),
                (new EffectId("confetti-party"), EffectPlacement.Monitor),
                (new EffectId("celebration-confetti"), EffectPlacement.Monitor),
                (new EffectId("confetti-02"), EffectPlacement.Monitor),
                (new EffectId("fireworks-teal-red"), EffectPlacement.Monitor),
                (new EffectId("winner-fireworks"), EffectPlacement.Monitor),
                (new EffectId("season-fireworks"), EffectPlacement.Monitor),
                (new EffectId("fireworks-red-white-blue"), EffectPlacement.Monitor),
                (new EffectId("big-bada-boom"), EffectPlacement.Monitor),
                (new EffectId("snow-falling"), EffectPlacement.Monitor),
                (new EffectId("let-it-snow"), EffectPlacement.Monitor),
                (new EffectId("snow"), EffectPlacement.Monitor),
                (new EffectId("loader-cat"), EffectPlacement.Cursor),
            ],
            EffectCatalog.BuiltIn.Select(effect => (effect.Id, effect.Placement)));
    }

    [Theory]
    [InlineData("confetti", EffectPlacement.Monitor)]
    [InlineData("focus-ring", EffectPlacement.Cursor)]
    [InlineData("let-it-snow", EffectPlacement.Monitor)]
    [InlineData("loader-cat", EffectPlacement.Cursor)]
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
