using System.Windows.Media;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Plays one WAV or MP3 file through WPF <see cref="MediaPlayer"/>. Create, use, and dispose it on the
/// sound thread; <see cref="MediaPlayer"/> raises its events on that thread. Every WPF exception is
/// reported as <see cref="SoundPlaybackException"/>, so a media-engine error cannot end drawEM.
/// </summary>
public sealed class MediaSoundPlayer : ISoundPlayer
{
    private readonly MediaPlayer player;
    private bool disposed;

    /// <param name="path">Absolute path to an existing file.</param>
    /// <exception cref="SoundPlaybackException">The media engine cannot open the file.</exception>
    public MediaSoundPlayer(string path)
    {
        player = Engine(() => new MediaPlayer { Volume = 1.0 });
        player.MediaEnded += OnMediaEnded;
        player.MediaFailed += OnMediaFailed;

        try
        {
            Engine(() => player.Open(new Uri(path, UriKind.Absolute)));
        }
        catch (SoundPlaybackException)
        {
            // Nobody else holds this player yet, so release it here.
            Release();
            throw;
        }
    }

    public event Action? Completed;

    public event Action<string>? Failed;

    public void Play() => Engine(player.Play);

    public void Stop() => Engine(player.Stop);

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        Unsubscribe();
        Engine(player.Close);
    }

    private void Release()
    {
        Unsubscribe();
        try
        {
            player.Close();
        }
        catch (Exception)
        {
            // The open failure is the error worth reporting; a failed close adds nothing.
        }
    }

    private void Unsubscribe()
    {
        disposed = true;
        player.MediaEnded -= OnMediaEnded;
        player.MediaFailed -= OnMediaFailed;
    }

    private void OnMediaEnded(object? sender, EventArgs e) => Completed?.Invoke();

    private void OnMediaFailed(object? sender, ExceptionEventArgs e) => Failed?.Invoke(e.ErrorException.Message);

    private static void Engine(Action call) => Engine(() =>
    {
        call();
        return 0;
    });

    private static T Engine<T>(Func<T> call)
    {
        try
        {
            return call();
        }
        catch (Exception exception) when (exception is not SoundPlaybackException)
        {
            throw new SoundPlaybackException(exception.Message, exception);
        }
    }
}
