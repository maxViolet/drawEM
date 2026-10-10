namespace DrawEM.App.Application.Effects;

/// <summary>
/// Rendering port of the effect channel. The implementation chooses the surface for the instance's placement;
/// the channel never branches on placement.
/// </summary>
public interface IEffectSurface
{
    /// <summary>Starts showing <paramref name="instance"/> from its beginning.</summary>
    /// <exception cref="EffectPlaybackException">The effect cannot be shown.</exception>
    IEffectPlayback Show(EffectInstance instance);
}

/// <summary>
/// One running effect on screen. Events are raised on the thread that owns the controller.
/// <see cref="IDisposable.Dispose"/> removes the effect and releases its window, frame callback, and player;
/// it reports an engine error as <see cref="EffectPlaybackException"/>, after which the playback is still unusable.
/// </summary>
public interface IEffectPlayback : IDisposable
{
    /// <summary>The effect reached its natural end.</summary>
    event Action? Completed;

    /// <summary>Rendering failed after the effect started. Carries the reason.</summary>
    event Action<string>? Failed;
}
