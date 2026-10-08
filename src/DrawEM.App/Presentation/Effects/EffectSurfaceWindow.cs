using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Presentation.Effects;

/// <summary>
/// A transparent window that covers exactly one monitor above the drawing overlay. It never takes focus,
/// is absent from Alt+Tab and Win+Tab, and passes mouse input to the windows below it in any process.
/// Content is drawn into one <see cref="DrawingVisual"/> without layout, and clipped to the monitor.
/// </summary>
public sealed class EffectSurfaceWindow : Window
{
    private readonly MonitorBounds monitor;
    private readonly EffectSurfaceHost host = new();
    private IntPtr handle;

    public EffectSurfaceWindow(MonitorBounds monitor)
    {
        this.monitor = monitor;
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

    public EffectSurfaceLayout Layout { get; private set; }

    /// <summary>Whether the native window covers exactly the target monitor in physical pixels.</summary>
    public bool PlacementMatchesMonitor =>
        NativeMethods.GetWindowRect(handle, out var rect)
        && rect.Left == monitor.Left && rect.Top == monitor.Top
        && rect.Right == monitor.Right && rect.Bottom == monitor.Bottom;

    /// <summary>Shows the window on its monitor without activating it.</summary>
    public void ShowOnMonitor()
    {
        new WindowInteropHelper(this).EnsureHandle();

        // The first placement can move the window to a monitor with another DPI. WPF then applies the
        // system's suggested size for the new DPI, so the second placement restores the exact bounds.
        Place();
        Place();
        Show();
        Place();
        RefreshLayout();
    }

    public void Draw(Action<DrawingContext, EffectSurfaceLayout> draw)
    {
        using var context = host.Visual.RenderOpen();
        draw(context, Layout);
    }

    protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
    {
        base.OnDpiChanged(oldDpi, newDpi);
        RefreshLayout();
    }

    private void Place() =>
        NativeMethods.SetWindowPos(
            handle,
            NativeMethods.HWND_TOPMOST,
            monitor.Left,
            monitor.Top,
            monitor.Right - monitor.Left,
            monitor.Bottom - monitor.Top,
            NativeMethods.SWP_NOACTIVATE | NativeMethods.SWP_NOOWNERZORDER);

    private void RefreshLayout()
    {
        Layout = new EffectSurfaceLayout(monitor, VisualTreeHelper.GetDpi(this).DpiScaleX);
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
