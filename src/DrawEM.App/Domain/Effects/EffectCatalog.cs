using System.Diagnostics.CodeAnalysis;

namespace DrawEM.App.Domain.Effects;

/// <summary>The built-in effects. Settings name an effect by ID; placement and file come from here.</summary>
public static class EffectCatalog
{
    public static IReadOnlyList<BuiltInEffect> BuiltIn { get; } = Array.AsReadOnly(
    [
        new BuiltInEffect(new EffectId("confetti"), "Confetti", EffectPlacement.Monitor, "confetti.json"),
        new BuiltInEffect(new EffectId("focus-ring"), "Focus ring", EffectPlacement.Cursor, "focus-ring.json"),
    ]);

    /// <summary>Finds the entry with exactly this ID; IDs are case-sensitive.</summary>
    public static bool TryFind(EffectId id, [NotNullWhen(true)] out BuiltInEffect? effect)
    {
        effect = BuiltIn.FirstOrDefault(entry => entry.Id == id);
        return effect is not null;
    }
}
