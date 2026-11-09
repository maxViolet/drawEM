namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// What one playing effect does on each <see cref="System.Windows.Media.CompositionTarget.Rendering"/>
/// callback: draw the frame for the elapsed time, complete, or fail. A throw while drawing ends the effect as a
/// failure, so it never escapes into the static event.
/// </summary>
internal sealed class LottieFrameLoop(
    TimeSpan duration,
    bool loop,
    Func<TimeSpan> elapsed,
    Action frameStarting,
    Action<double> draw,
    Action completed,
    Action<string> failed)
{
    private TimeSpan lastRenderingTime = TimeSpan.MinValue;

    public void Render(TimeSpan renderingTime)
    {
        // Rendering can be raised more than once for one frame; only the first call draws.
        if (renderingTime == lastRenderingTime)
        {
            return;
        }

        lastRenderingTime = renderingTime;
        var frame = LottiePlayback.PlaybackPosition(elapsed(), duration, loop);
        switch (frame.State)
        {
            case LottiePlayback.PlaybackState.Completed:
                completed();
                return;
            case LottiePlayback.PlaybackState.CannotLoop:
                failed("The effect has no length, so it cannot loop.");
                return;
        }

        try
        {
            frameStarting();
            draw(frame.Position.TotalSeconds);
        }
        catch (Exception exception)
        {
            failed($"The effect could not be drawn: {exception.Message}");
        }
    }
}
