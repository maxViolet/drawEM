using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// A transparent window above the drawing overlay that covers one monitor or a part of it. It never takes
/// focus, is absent from Alt+Tab and Win+Tab, and passes mouse input to the windows below it in any process.
/// Content is drawn into one <see cref="DrawingVisual"/> without layout, and clipped to the window.
/// </summary>
public sealed class EffectSurfaceWindow : Window
{
    private readonly EffectSurfaceHost host = new();
    private MonitorBounds bounds;
    private IntPtr handle;

    public EffectSurfaceWindow(MonitorBounds monitor)
    {
        bounds = monitor;
        Title = "drawEM effect";
        WindowStyle = WindowStyle.None;
        ResizeMode = ResizeMode.NoResize;
        AllowsTransparency = true;
        Background = Brushes.Transparent;
        ShowInTaskbar = false;
        ShowActivated = false;
        Topmost = true;
        Focusable = false;
        IsHitTestVisible = false;
        WindowStartupLocation = WindowStartupLocation.Manual;
        Content = host;
        Layout = new EffectSurfaceLayout(monitor, 1d);

        SourceInitialized += (_, _) =>
        {
            handle = new WindowInteropHelper(this).Handle;
            ApplyExtendedStyles();
            HwndSource.FromHwnd(handle).AddHook(WindowProcedure);
        };
    }

    /// <summary>Maps physical pixels to window DIPs; its bounds are the window's, which lie on the monitor.</summary>
    public EffectSurfaceLayout Layout { get; private set; }

    /// <summary>The window's physical desktop bounds: the monitor, or the part chosen when it was shown.</summary>
    public MonitorBounds Bounds => bounds;

    /// <summary>Whether the native window covers exactly <see cref="Bounds"/> in physical pixels.</summary>
    public bool PlacementMatchesBounds =>
        NativeMethods.GetWindowRect(handle, out var rect)
        && rect.Left == bounds.Left && rect.Top == bounds.Top
        && rect.Right == bounds.Right && rect.Bottom == bounds.Bottom;

    /// <summary>Shows the window without activating it, covering its monitor or the part <paramref name="part"/> chooses.</summary>
    /// <param name="part">Maps the monitor's DPI scale to physical bounds on the monitor; <c>null</c> covers the monitor.</param>
    public void ShowOnMonitor(Func<double, MonitorBounds>? part = null)
    {
        new WindowInteropHelper(this).EnsureHandle();

        // The first placement can move the window to a monitor with another DPI. WPF then applies the
        // system's suggested size for the new DPI, so the second placement restores the exact bounds.
        Place();
        Place();
        if (part is not null)
        {
            // The window now has the monitor's DPI; moving within the monitor keeps it.
            bounds = part(NativeMethods.GetDpiForWindow(handle) / 96d);
            Place();
        }

        Show();
        Place();
        RefreshLayout();
    }

    /// <summary>
    /// Creates a bitmap that fills the window, with <paramref name="renderScale"/> bitmap pixels per physical
    /// pixel, and makes it the window content. WPF scales it to the window and shows each change to it.
    /// </summary>
    public WriteableBitmap ShowBitmap(double renderScale)
    {
        var dpi = 96d * Layout.DpiScale * renderScale;
        var bitmap = new WriteableBitmap(
            Math.Max(1, (int)Math.Ceiling((bounds.Right - bounds.Left) * renderScale)),
            Math.Max(1, (int)Math.Ceiling((bounds.Bottom - bounds.Top) * renderScale)),
            dpi,
            dpi,
            PixelFormats.Pbgra32,
            null);
        RenderOptions.SetBitmapScalingMode(host.Visual, BitmapScalingMode.Linear);
        using var context = RenderOpen();
        context.DrawImage(bitmap, Layout.LocalBounds);
        return bitmap;
    }

    /// <summary>Replaces the window content with what is drawn into the returned context before it is closed.</summary>
    public DrawingContext RenderOpen() => host.Visual.RenderOpen();

    protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
    {
        base.OnDpiChanged(oldDpi, newDpi);
        RefreshLayout();
    }

    private void Place() =>
        NativeMethods.SetWindowPos(
            handle,
            NativeMethods.HWND_TOPMOST,
            bounds.Left,
            bounds.Top,
            bounds.Right - bounds.Left,
            bounds.Bottom - bounds.Top,
            NativeMethods.SWP_NOACTIVATE | NativeMethods.SWP_NOOWNERZORDER);

    private void RefreshLayout()
    {
        Layout = new EffectSurfaceLayout(bounds, VisualTreeHelper.GetDpi(this).DpiScaleX);
        host.Clip = new RectangleGeometry(Layout.LocalBounds);
    }

    private void ApplyExtendedStyles()
    {
        var exStyle = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);
        exStyle |= NativeMethods.WS_EX_TOOLWINDOW | NativeMethods.WS_EX_NOACTIVATE
            | NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_LAYERED;
        exStyle &= ~NativeMethods.WS_EX_APPWINDOW;
        NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE, exStyle);
    }

    private static IntPtr WindowProcedure(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (message)
        {
            case NativeMethods.WM_NCHITTEST:
                handled = true;
                return new IntPtr(NativeMethods.HTTRANSPARENT);
            case NativeMethods.WM_MOUSEACTIVATE:
                handled = true;
                return new IntPtr(NativeMethods.MA_NOACTIVATE);
            default:
                return IntPtr.Zero;
        }
    }

    private sealed class EffectSurfaceHost : FrameworkElement
    {
        public EffectSurfaceHost()
        {
            Visual = new DrawingVisual();
            AddVisualChild(Visual);
            IsHitTestVisible = false;
        }

        public DrawingVisual Visual { get; }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index) => Visual;
    }
}
