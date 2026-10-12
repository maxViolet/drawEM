using DrawEM.App.Application.Effects;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// Shows built-in effects in an <see cref="EffectSurfaceWindow"/>: a Monitor effect covers its monitor and a
/// Cursor effect is centered on the captured cursor point, clipped to the monitor. Call on the UI thread.
/// </summary>
public sealed class LottieEffectSurface(BuiltInEffectAnimations animations) : IEffectSurface
{
    /// <summary>Raised on the UI thread before each animation frame is drawn, while an effect plays.</summary>
    public event Action? FrameStarting;

    public IEffectPlayback Show(EffectInstance instance)
    {
        if (!animations.TryGet(instance.Command.Effect, out var animation, out var failure))
        {
            throw new EffectPlaybackException(failure);
        }

        return LottiePlayback.Start(instance.Command, animation, () => FrameStarting?.Invoke());
    }
}
