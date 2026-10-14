using System.Diagnostics.CodeAnalysis;

namespace DrawEM.App.Domain.Effects;

/// <summary>The built-in effects. Settings name an effect by ID; placement and file come from here.</summary>
public static class EffectCatalog
{
    public static IReadOnlyList<BuiltInEffect> BuiltIn { get; } = Array.AsReadOnly(
    [
        new BuiltInEffect(new EffectId("confetti"), "Confetti", EffectPlacement.Monitor, "confetti.json"),
        new BuiltInEffect(new EffectId("focus-ring"), "Focus ring", EffectPlacement.Cursor, "focus-ring.json"),
        new BuiltInEffect(new EffectId("confetti-transparent"), "Confetti shower", EffectPlacement.Monitor, "confetti-transparent.json"),
        new BuiltInEffect(new EffectId("confetti-effects"), "Confetti effects", EffectPlacement.Monitor, "confetti-effects.json"),
        new BuiltInEffect(new EffectId("confetti-party"), "Confetti party", EffectPlacement.Monitor, "confetti-party.json"),
        new BuiltInEffect(new EffectId("celebration-confetti"), "Celebration confetti", EffectPlacement.Monitor, "celebration-confetti.json"),
        new BuiltInEffect(new EffectId("confetti-02"), "Confetti burst", EffectPlacement.Monitor, "confetti-02.json"),
        new BuiltInEffect(new EffectId("fireworks-teal-red"), "Fireworks teal and red", EffectPlacement.Monitor, "fireworks-teal-red.json"),
        new BuiltInEffect(new EffectId("winner-fireworks"), "Winner fireworks", EffectPlacement.Monitor, "winner-fireworks.json"),
        new BuiltInEffect(new EffectId("season-fireworks"), "Season fireworks", EffectPlacement.Monitor, "season-fireworks.json"),
        new BuiltInEffect(new EffectId("fireworks-red-white-blue"), "Red, white and blue fireworks", EffectPlacement.Monitor, "fireworks-red-white-blue.json"),
        new BuiltInEffect(new EffectId("big-bada-boom"), "Big bada boom", EffectPlacement.Monitor, "big-bada-boom.json"),
        new BuiltInEffect(new EffectId("snow-falling"), "Snow falling", EffectPlacement.Monitor, "snow-falling.json"),
        new BuiltInEffect(new EffectId("let-it-snow"), "Let it snow", EffectPlacement.Monitor, "let-it-snow.json"),
        new BuiltInEffect(new EffectId("snow"), "Snow", EffectPlacement.Monitor, "snow.json"),
        new BuiltInEffect(new EffectId("loader-cat"), "Loader cat", EffectPlacement.Cursor, "loader-cat.json"),
    ]);

    /// <summary>Finds the entry with exactly this ID; IDs are case-sensitive.</summary>
    public static bool TryFind(EffectId id, [NotNullWhen(true)] out BuiltInEffect? effect)
    {
        effect = BuiltIn.FirstOrDefault(entry => entry.Id == id);
        return effect is not null;
    }
}
