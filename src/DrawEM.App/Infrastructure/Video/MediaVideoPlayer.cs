using System.Windows;
using System.Windows.Media;
using DrawEM.App.Application.Video;

namespace DrawEM.App.Infrastructure.Video;

/// <summary>
/// Plays one MP4 file once through WPF <see cref="MediaPlayer"/>, the path S5-01 chose. Create, use, and
/// dispose it on one Dispatcher thread; <see cref="MediaPlayer"/> raises its events there. Engine
/// exceptions from calls become <see cref="VideoPlaybackException"/>; open and decode errors arrive as
/// <see cref="Failed"/>, so a bad file cannot end drawEM. After <see cref="Dispose"/> no event is raised and
/// the file is no longer held open.
/// </summary>
public sealed class MediaVideoPlayer : IDisposable
{
    private readonly MediaPlayer player;
    private bool disposed;

    /// <param name="path">Absolute path to the file.</param>
    /// <param name="muted">Mutes embedded audio; playback otherwise uses full volume, so Windows sets the level.</param>
    /// <exception cref="VideoPlaybackException">The media engine cannot create or configure the player.</exception>
    public MediaVideoPlayer(string path, bool muted = false)
    {
        MediaPlayer? created = null;
        try
        {
            created = new MediaPlayer();
            created.MediaOpened += OnMediaOpened;
            created.MediaEnded += OnMediaEnded;
            created.MediaFailed += OnMediaFailed;
            created.Volume = 1.0;
            created.IsMuted = muted;
            created.Open(new Uri(path, UriKind.Absolute));
        }
        catch (Exception exception)
        {
            if (created is not null)
            {
                _ = Release(created);
            }

            throw new VideoPlaybackException(exception.Message, exception);
        }

        player = created;
    }

    /// <summary>The engine opened the file and knows its tracks and frame size.</summary>
    public event Action<VideoOpened>? Opened;

    /// <summary>Playback reached the natural end of the file.</summary>
    public event Action? Completed;

    /// <summary>The engine could not open or decode the file. Dispose the player afterwards.</summary>
    public event Action<VideoPlaybackFailure>? Failed;

    /// <summary>A drawing that shows this player's frames scaled into <paramref name="bounds"/>.</summary>
    public VideoDrawing CreateDrawing(Rect bounds) => new() { Player = player, Rect = bounds };

    public void Play() => Engine(player.Play);

    public void Stop() => Engine(player.Stop);

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        if (Release(player) is { } failure)
        {
            throw new VideoPlaybackException(failure.Message, failure);
        }
    }

    /// <summary>
    /// Detaches every handler, stops, and closes <paramref name="target"/>. Every step runs even if an
    /// earlier one throws. Returns the first failure, or <c>null</c>.
    /// </summary>
    private Exception? Release(MediaPlayer target)
    {
        Exception? first = null;
        Action[] steps =
        [
            () => target.MediaOpened -= OnMediaOpened,
            () => target.MediaEnded -= OnMediaEnded,
            () => target.MediaFailed -= OnMediaFailed,
            target.Stop,
            target.Close,
        ];

        foreach (var step in steps)
        {
            try
            {
                step();
            }
            catch (Exception exception)
            {
                first ??= exception;
            }
        }

        return first;
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        if (disposed)
        {
            return;
        }

        var duration = player.NaturalDuration.HasTimeSpan ? player.NaturalDuration.TimeSpan : (TimeSpan?)null;
        Opened?.Invoke(new VideoOpened(
            player.HasVideo, player.HasAudio, player.NaturalVideoWidth, player.NaturalVideoHeight, duration));
    }

    private void OnMediaEnded(object? sender, EventArgs e)
    {
        if (!disposed)
        {
            Completed?.Invoke();
        }
    }

    private void OnMediaFailed(object? sender, ExceptionEventArgs e)
    {
        if (disposed)
        {
            return;
        }

        var error = e.ErrorException;
        var code = $"0x{error.HResult:X8}";
        var detail = error.Message.Contains(code, StringComparison.OrdinalIgnoreCase) ? error.Message : $"{error.Message} ({code})";
        Failed?.Invoke(new VideoPlaybackFailure($"The video could not be played: {detail}", error.HResult));
    }

    private static void Engine(Action call)
    {
        try
        {
            call();
        }
        catch (Exception exception)
        {
            throw new VideoPlaybackException(exception.Message, exception);
        }
    }
}
