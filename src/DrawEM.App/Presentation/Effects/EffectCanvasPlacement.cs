using DrawEM.App.Domain.Drawing;
using Rect = System.Windows.Rect;
using Size = System.Windows.Size;

namespace DrawEM.App.Presentation.Effects;

/// <summary>Where a Lottie canvas goes on each surface placement.</summary>
public static class EffectCanvasPlacement
{
    /// <summary>
    /// Monitor surface: scales <paramref name="canvas"/> until it fills <paramref name="target"/>, keeps its
    /// proportions, and centers it, so the overflow on one axis is cropped equally on both sides.
    /// </summary>
    /// <returns>The canvas destination in <paramref name="target"/> coordinates.</returns>
    public static Rect Cover(Size canvas, Size target)
    {
        if (!(canvas.Width > 0 && canvas.Height > 0))
        {
            throw new ArgumentException("The canvas size must not be empty.", nameof(canvas));
        }

        var scale = Math.Max(target.Width / canvas.Width, target.Height / canvas.Height);
        var width = canvas.Width * scale;
        var height = canvas.Height * scale;
        return new Rect((target.Width - width) / 2, (target.Height - height) / 2, width, height);
    }

    /// <summary>
    /// Cursor surface: <paramref name="canvas"/> in DIP at <paramref name="dpiScale"/>, rounded to whole
    /// pixels and centered on <paramref name="center"/>. The window is the part on <paramref name="monitor"/>;
    /// the canvas is clipped there, never shifted inward.
    /// </summary>
    public static CursorSurfaceBounds Cursor(MonitorBounds monitor, double dpiScale, Size canvas, ScreenPoint center)
    {
        if (!monitor.Contains(center))
        {
            throw new ArgumentException("The cursor point must lie on the monitor.", nameof(center));
        }

        var width = (int)Math.Round(canvas.Width * dpiScale, MidpointRounding.AwayFromZero);
        var height = (int)Math.Round(canvas.Height * dpiScale, MidpointRounding.AwayFromZero);
        var left = center.X - (width / 2);
        var top = center.Y - (height / 2);
        var canvasBounds = new MonitorBounds(left, top, left + width, top + height);
        var window = new MonitorBounds(
            Math.Max(canvasBounds.Left, monitor.Left),
            Math.Max(canvasBounds.Top, monitor.Top),
            Math.Min(canvasBounds.Right, monitor.Right),
            Math.Min(canvasBounds.Bottom, monitor.Bottom));
        return new CursorSurfaceBounds(canvasBounds, window);
    }
}

/// <summary>Physical desktop bounds of a Cursor surface; <see cref="MonitorBounds"/> serves as a pixel rectangle.</summary>
/// <param name="Canvas">The whole canvas, which can extend past the monitor.</param>
/// <param name="Window">The canvas clipped to its monitor: the effect window's bounds.</param>
public readonly record struct CursorSurfaceBounds(MonitorBounds Canvas, MonitorBounds Window)
{
    /// <summary>The canvas in the window's bitmap pixels, where the bitmap has <paramref name="renderScale"/> pixels per physical pixel.</summary>
    public Rect CanvasRectInWindow(double renderScale) => new(
        (Canvas.Left - Window.Left) * renderScale,
        (Canvas.Top - Window.Top) * renderScale,
        (Canvas.Right - Canvas.Left) * renderScale,
        (Canvas.Bottom - Canvas.Top) * renderScale);
}
