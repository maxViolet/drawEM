using System.Windows.Media;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Plays one WAV or MP3 file through WPF <see cref="MediaPlayer"/>. Create, use, and dispose it on the
/// UI thread; <see cref="MediaPlayer"/> raises its events on that thread.
/// </summary>
public sealed class MediaSoundPlayer : ISoundPlayer
{
    private readonly MediaPlayer player = new() { Volume = 1.0 };
    private bool disposed;

    /// <param name="path">Absolute path to an existing file.</param>
    public MediaSoundPlayer(string path)
    {
        player.MediaEnded += OnMediaEnded;
        player.MediaFailed += OnMediaFailed;
        player.Open(new Uri(path, UriKind.Absolute));
    }

    public event Action? Completed;

    public event Action<string>? Failed;

    public void Play() => player.Play();

    public void Stop() => player.Stop();

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        player.MediaEnded -= OnMediaEnded;
        player.MediaFailed -= OnMediaFailed;
        player.Close();
    }

    private void OnMediaEnded(object? sender, EventArgs e) => Completed?.Invoke();

    private void OnMediaFailed(object? sender, ExceptionEventArgs e) => Failed?.Invoke(e.ErrorException.Message);
}
