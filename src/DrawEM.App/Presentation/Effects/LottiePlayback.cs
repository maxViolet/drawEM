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
    private readonly Action frameStarting;
    private readonly long started = Stopwatch.GetTimestamp();
    private EffectSurfaceWindow? window;
    private WriteableBitmap? bitmap;
    private SKSurface? surface;
    private IntPtr surfaceBuffer;
    private SKRect destination;
    private Int32Rect dirty;
    private TimeSpan lastRenderingTime = TimeSpan.MinValue;
    private bool subscribed;
    private bool released;

    private LottiePlayback(Animation animation, Action frameStarting)
    {
        this.animation = animation;
        this.frameStarting = frameStarting;
    }

    public event Action? Completed;

    public event Action<string>? Failed;

    /// <summary>Shows the window for <paramref name="command"/> and starts playing from the first frame.</summary>
    /// <param name="frameStarting">Called before each frame is drawn.</param>
    /// <exception cref="EffectPlaybackException">The window could not be shown; everything created is released.</exception>
    public static LottiePlayback Start(StartEffectCommand command, Animation animation, Action frameStarting)
    {
        var playback = new LottiePlayback(animation, frameStarting);
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
            surface?.Dispose();
            surface = null;
            bitmap = null;
        }
    }

    private void Open(StartEffectCommand command)
    {
        var canvas = new Size(animation.Size.Width, animation.Size.Height);
        window = new EffectSurfaceWindow(command.Monitor);
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

        bitmap = window.ShowBitmap(layout);
        dirty = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
        destination = SKRect.Create(
            (float)layout.Destination.X, (float)layout.Destination.Y,
            (float)layout.Destination.Width, (float)layout.Destination.Height);
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
        // Rendering can be raised more than once for one frame; only the first call draws.
        var renderingTime = ((RenderingEventArgs)e).RenderingTime;
        if (released || renderingTime == lastRenderingTime)
        {
            return;
        }

        lastRenderingTime = renderingTime;
        var elapsed = Stopwatch.GetElapsedTime(started);
        if (elapsed >= animation.Duration)
        {
            Unsubscribe();
            Completed?.Invoke();
            return;
        }

        frameStarting();

        try
        {
            Draw(elapsed.TotalSeconds);
        }
        catch (Exception exception)
        {
            Unsubscribe();
            Failed?.Invoke($"The effect could not be drawn: {exception.Message}");
        }
    }

    private void Draw(double seconds)
    {
        var target = bitmap!;
        target.Lock();
        try
        {
            // The back buffer normally keeps its address; a new address needs a new surface over it.
            if (surface is null || surfaceBuffer != target.BackBuffer)
            {
                surface?.Dispose();
                var info = new SKImageInfo(
                    target.PixelWidth, target.PixelHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
                surface = SKSurface.Create(info, target.BackBuffer, target.BackBufferStride)
                    ?? throw new InvalidOperationException("Skia could not create a surface over the bitmap.");
                surfaceBuffer = target.BackBuffer;
            }

            var canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);
            animation.SeekFrameTime(seconds);
            animation.Render(canvas, destination);
            canvas.Flush();
            target.AddDirtyRect(dirty);
        }
        finally
        {
            target.Unlock();
        }
    }
}
