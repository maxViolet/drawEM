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
    /// <exception cref="SoundPlaybackException">The media engine cannot create, configure, or open the player.</exception>
    public MediaSoundPlayer(string path)
    {
        MediaPlayer? created = null;
        try
        {
            created = new MediaPlayer();
            created.MediaEnded += OnMediaEnded;
            created.MediaFailed += OnMediaFailed;
            created.Volume = 1.0;
            created.Open(new Uri(path, UriKind.Absolute));
        }
        catch (Exception exception)
        {
            // Nobody else holds this player yet, so release whatever part of it was built.
            if (created is not null)
            {
                Release(created);
            }

            throw new SoundPlaybackException(exception.Message, exception);
        }

        player = created;
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

        disposed = true;
        Unsubscribe(player);
        Engine(player.Close);
    }

    private void Release(MediaPlayer partial)
    {
        Unsubscribe(partial);
        try
        {
            partial.Close();
        }
        catch (Exception)
        {
            // The construction failure is the error worth reporting; a failed close adds nothing.
        }
    }

    private void Unsubscribe(MediaPlayer target)
    {
        target.MediaEnded -= OnMediaEnded;
        target.MediaFailed -= OnMediaFailed;
    }

    private void OnMediaEnded(object? sender, EventArgs e) => Completed?.Invoke();

    private void OnMediaFailed(object? sender, ExceptionEventArgs e) => Failed?.Invoke(e.ErrorException.Message);

    private static void Engine(Action call)
    {
        try
        {
            call();
        }
        catch (Exception exception)
        {
            throw new SoundPlaybackException(exception.Message, exception);
        }
    }
}
