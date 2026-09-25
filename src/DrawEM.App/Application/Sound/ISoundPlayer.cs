namespace DrawEM.App.Application.Sound;

/// <summary>
/// One playback of one sound file. Created stopped; <see cref="Play"/> starts it from the beginning.
/// Events are raised on the thread that owns the controller.
/// </summary>
public interface ISoundPlayer : IDisposable
{
    /// <summary>The file reached its natural end.</summary>
    event Action? Completed;

    /// <summary>Playback failed after it started, for example a decode or output-device error. Carries the reason.</summary>
    event Action<string>? Failed;

    /// <exception cref="SoundPlaybackException">Playback cannot start.</exception>
    void Play();

    void Stop();
}
