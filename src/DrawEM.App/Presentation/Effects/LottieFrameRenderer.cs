using System.Windows;
using System.Windows.Media.Imaging;
using SkiaSharp;
using SkiaSharp.Skottie;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// Draws Skottie frames into one <see cref="WriteableBitmap"/> through a Skia surface over its back buffer.
/// Runs on the bitmap's thread.
/// </summary>
internal sealed class LottieFrameRenderer(Animation animation, WriteableBitmap bitmap, SKRect destination) : IDisposable
{
    private readonly Int32Rect dirty = new(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
    private SKSurface? surface;
    private IntPtr surfaceBuffer;

    /// <summary>Replaces the bitmap content with the frame at <paramref name="seconds"/>.</summary>
    public void Draw(double seconds)
    {
        bitmap.Lock();
        try
        {
            var canvas = Surface().Canvas;
            canvas.Clear(SKColors.Transparent);
            animation.SeekFrameTime(seconds);
            animation.Render(canvas, destination);
            canvas.Flush();
            bitmap.AddDirtyRect(dirty);
        }
        finally
        {
            bitmap.Unlock();
        }
    }

    public void Dispose()
    {
        surface?.Dispose();
        surface = null;
    }

    /// <summary>The back buffer normally keeps its address; a new address needs a new surface over it.</summary>
    private SKSurface Surface()
    {
        if (surface is not null && surfaceBuffer == bitmap.BackBuffer)
        {
            return surface;
        }

        surface?.Dispose();
        var info = new SKImageInfo(bitmap.PixelWidth, bitmap.PixelHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
        surface = SKSurface.Create(info, bitmap.BackBuffer, bitmap.BackBufferStride)
            ?? throw new InvalidOperationException("Skia could not create a surface over the bitmap.");
        surfaceBuffer = bitmap.BackBuffer;
        return surface;
    }
}
