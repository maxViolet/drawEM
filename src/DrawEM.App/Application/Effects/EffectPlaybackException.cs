namespace DrawEM.App.Application.Effects;

/// <summary>An effect cannot be shown or released. Recoverable: the channel logs it and stays usable.</summary>
public sealed class EffectPlaybackException : Exception
{
    public EffectPlaybackException(string message)
        : base(message)
    {
    }

    public EffectPlaybackException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
