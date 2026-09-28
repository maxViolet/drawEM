namespace DrawEM.App.Application.Sound;

/// <summary>A sound cannot be played. Recoverable: the channel logs it and stays usable.</summary>
public sealed class SoundPlaybackException : Exception
{
    public SoundPlaybackException(string message)
        : base(message)
    {
    }

    public SoundPlaybackException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
