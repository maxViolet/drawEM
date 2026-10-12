using System.Diagnostics.CodeAnalysis;
using DrawEM.App.Domain.Effects;
using SkiaSharp.Skottie;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// The parsed Lottie file of every built-in effect, embedded under <c>BuiltIn\</c> by its catalog resource
/// name. Each file is parsed once, at construction, outside the hook callback, and reused by every instance.
/// A file that fails to parse disables only its own effect.
/// </summary>
public sealed class BuiltInEffectAnimations : IDisposable
{
    private readonly Dictionary<EffectId, Animation> animations = [];
    private readonly Dictionary<EffectId, string> failures = [];

    public BuiltInEffectAnimations()
    {
        foreach (var effect in EffectCatalog.BuiltIn)
        {
            try
            {
                animations[effect.Id] = Load(effect);
            }
            catch (InvalidOperationException exception)
            {
                failures[effect.Id] = exception.Message;
            }
        }
    }

    /// <summary>Parses the embedded file of <paramref name="effect"/>; the caller disposes it.</summary>
    /// <exception cref="InvalidOperationException">The file is missing or is not a valid Lottie file.</exception>
    public static Animation Load(BuiltInEffect effect)
    {
        using var stream = typeof(BuiltInEffectAnimations).Assembly.GetManifestResourceStream(
            $"DrawEM.App.Presentation.Effects.BuiltIn.{effect.ResourceName}")
            ?? throw new InvalidOperationException($"The effect file {effect.ResourceName} is not embedded.");
        return Animation.TryCreate(stream, out var animation) && animation is not null
            ? animation
            : throw new InvalidOperationException($"The effect file {effect.ResourceName} is not a valid Lottie file.");
    }

    /// <summary>The parsed animation of <paramref name="id"/>, or why it is unavailable.</summary>
    public bool TryGet(EffectId id, [NotNullWhen(true)] out Animation? animation, [NotNullWhen(false)] out string? failure)
    {
        if (animations.TryGetValue(id, out animation))
        {
            failure = null;
            return true;
        }

        failure = failures.GetValueOrDefault(id) ?? $"The effect {id.Value} is not a built-in effect.";
        return false;
    }

    public void Dispose()
    {
        foreach (var animation in animations.Values)
        {
            animation.Dispose();
        }

        animations.Clear();
    }
}
