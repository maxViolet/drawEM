using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Effects;
using SkiaSharp;
using SkiaSharp.Skottie;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// One built-in effect on screen: its window, bitmap, Skia surface, and frame callback. Each
/// <see cref="CompositionTarget.Rendering"/> callback draws the Skottie frame for the monotonic time since
/// the start into the effect's own <see cref="WriteableBitmap"/>, so a slow frame never lengthens the effect.
/// The frame callback is subscribed only while the effect plays. All members run on the UI thread.
/// </summary>
internal sealed class LottiePlayback : IEffectPlayback
{
    private readonly Animation animation;
    private readonly Action dpiChanged;
    private readonly long started = Stopwatch.GetTimestamp();
    private readonly LottieFrameLoop frames;
    private EffectSurfaceWindow? window;
    private LottieFrameRenderer? renderer;
    private bool subscribed;
    private bool released;

    private LottiePlayback(Animation animation, bool loop, Action frameStarting, Action dpiChanged)
    {
        this.animation = animation;
        this.dpiChanged = dpiChanged;
        frames = new LottieFrameLoop(
            animation.Duration, loop, () => Stopwatch.GetElapsedTime(started), frameStarting, Draw, Complete, Fail);
    }

    public event Action? Completed;

    public event Action<string>? Failed;

    /// <summary>Shows the window for <paramref name="command"/> and starts playing from the first frame.</summary>
    /// <param name="frameStarting">Called before each frame is drawn.</param>
    /// <param name="dpiChanged">Called if the target monitor is rescaled after the window is visible.</param>
    /// <exception cref="EffectPlaybackException">The window could not be shown; everything created is released.</exception>
    public static LottiePlayback Start(
        StartEffectCommand command, Animation animation, Action frameStarting, Action dpiChanged)
    {
        var playback = new LottiePlayback(animation, command.Loop, frameStarting, dpiChanged);
        try
        {
            playback.Open(command);
            return playback;
        }
        catch (Exception exception)
        {
            try
            {
                playback.Dispose();
            }
            catch (EffectPlaybackException)
            {
                // The show failure below is the one reported; the release counts as done.
            }

            throw new EffectPlaybackException($"The effect window could not be shown: {exception.Message}", exception);
        }
    }

    public void Dispose()
    {
        if (released)
        {
            return;
        }

        released = true;
        Unsubscribe();
        try
        {
            window?.Close();
        }
        catch (Exception exception)
        {
            throw new EffectPlaybackException($"The effect window could not be closed: {exception.Message}", exception);
        }
        finally
        {
            window = null;
            renderer?.Dispose();
            renderer = null;
        }
    }

    private void Open(StartEffectCommand command)
    {
        var canvas = new Size(animation.Size.Width, animation.Size.Height);
        window = new EffectSurfaceWindow(command.Monitor, dpiChanged);
        EffectRenderLayout layout;
        switch (command.Placement)
        {
            case EffectPlacement.Monitor:
                window.ShowOnMonitor();
                layout = EffectRenderLayout.Monitor(command.Monitor, canvas);
                break;
            case EffectPlacement.Cursor:
                EffectRenderLayout? placed = null;
                window.ShowOnMonitor(dpiScale =>
                {
                    placed = EffectRenderLayout.Cursor(command.Monitor, dpiScale, canvas, command.CursorPoint);
                    return placed.Value.Window;
                });
                layout = placed!.Value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command), command.Placement, "Unknown effect placement.");
        }

        renderer = new LottieFrameRenderer(
            animation,
            window.ShowBitmap(layout),
            SKRect.Create(
                (float)layout.Destination.X, (float)layout.Destination.Y,
                (float)layout.Destination.Width, (float)layout.Destination.Height));
        CompositionTarget.Rendering += OnRendering;
        subscribed = true;
    }

    private void Unsubscribe()
    {
        if (subscribed)
        {
            CompositionTarget.Rendering -= OnRendering;
            subscribed = false;
        }
    }

    private void OnRendering(object? sender, EventArgs e)
    {
        if (!released)
        {
            frames.Render(((RenderingEventArgs)e).RenderingTime);
        }
    }

    private void Complete()
    {
        Unsubscribe();
        Completed?.Invoke();
    }

    private void Fail(string reason)
    {
        Unsubscribe();
        Failed?.Invoke(reason);
    }

    internal enum PlaybackState
    {
        Playing,
        Completed,
        /// <summary>A looping effect without length has no frame to repeat, so it must end as a failure.</summary>
        CannotLoop,
    }

    internal static (TimeSpan Position, PlaybackState State) PlaybackPosition(
        TimeSpan elapsed,
        TimeSpan duration,
        bool loop)
    {
        if (loop && duration <= TimeSpan.Zero)
        {
            return (elapsed, PlaybackState.CannotLoop);
        }

        if (elapsed < duration)
        {
            return (elapsed, PlaybackState.Playing);
        }

        if (!loop)
        {
            return (elapsed, PlaybackState.Completed);
        }

        return (TimeSpan.FromTicks(elapsed.Ticks % duration.Ticks), PlaybackState.Playing);
    }

    private void Draw(double seconds) => renderer!.Draw(seconds);
}
