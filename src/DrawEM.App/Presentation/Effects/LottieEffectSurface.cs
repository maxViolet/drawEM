using DrawEM.App.Application.Effects;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// Shows built-in effects in an <see cref="EffectSurfaceWindow"/>: a Monitor effect covers its monitor and a
/// Cursor effect is centered on the captured cursor point, clipped to the monitor. Call on the UI thread.
/// </summary>
public sealed class LottieEffectSurface(BuiltInEffectAnimations animations) : IEffectSurface
{
    public IEffectPlayback Show(EffectInstance instance)
    {
        if (!animations.TryGet(instance.Command.Effect, out var animation, out var failure))
        {
            throw new EffectPlaybackException(failure);
        }

        return LottiePlayback.Start(instance.Command, animation);
    }
}
