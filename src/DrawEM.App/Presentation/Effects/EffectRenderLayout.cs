using DrawEM.App.Domain.Drawing;
using Rect = System.Windows.Rect;
using Size = System.Windows.Size;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// Where one effect draws: the effect window's physical bounds, the bitmap that fills it, and the Lottie
/// canvas destination in bitmap pixels.
/// </summary>
/// <param name="Window">Physical desktop bounds of the effect window; always on the target monitor.</param>
/// <param name="RenderScale">Bitmap pixels per physical window pixel; WPF scales the bitmap to the window.</param>
public readonly record struct EffectRenderLayout(
    MonitorBounds Window, double RenderScale, int BitmapWidth, int BitmapHeight, Rect Destination)
{
    /// <summary>
    /// Monitor effects render at half resolution: at full resolution the S4-03.1 benchmark missed the 4 ms
    /// render-callback target; the project owner chose this fallback on 2026-10-12.
    /// </summary>
    public const double MonitorRenderScale = 0.5;

    /// <summary>Monitor surface: covers <paramref name="monitor"/>, with cover scaling of the canvas.</summary>
    public static EffectRenderLayout Monitor(MonitorBounds monitor, Size canvas)
    {
        var (width, height) = BitmapSize(monitor, MonitorRenderScale);
        return new EffectRenderLayout(
            monitor, MonitorRenderScale, width, height, EffectCanvasPlacement.Cover(canvas, new Size(width, height)));
    }

    /// <summary>
    /// Cursor surface: the canvas in DIP at <paramref name="dpiScale"/>, centered on <paramref name="center"/>,
    /// in a full-resolution window clipped to <paramref name="monitor"/>.
    /// </summary>
    public static EffectRenderLayout Cursor(MonitorBounds monitor, double dpiScale, Size canvas, ScreenPoint center)
    {
        var bounds = EffectCanvasPlacement.Cursor(monitor, dpiScale, canvas, center);
        var (width, height) = BitmapSize(bounds.Window, 1.0);
        return new EffectRenderLayout(bounds.Window, 1.0, width, height, bounds.CanvasRectInWindow(1.0));
    }

    private static (int Width, int Height) BitmapSize(MonitorBounds window, double renderScale) => (
        Math.Max(1, (int)Math.Ceiling((window.Right - window.Left) * renderScale)),
        Math.Max(1, (int)Math.Ceiling((window.Bottom - window.Top) * renderScale)));
}
