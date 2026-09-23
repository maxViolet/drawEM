using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;

namespace DrawEM.App.Presentation;

public partial class OverlayWindow : Window, IOverlayView, IOverlayLifetime
{
    private readonly StrokeRenderElement renderElement = new();
    private readonly object renderLock = new();
    private DrawingState? pendingState;
    private bool renderScheduled;

    public OverlayWindow()
    {
        InitializeComponent();

        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;
        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;

        Content = renderElement;
        SourceInitialized += (_, _) =>
        {
            HideFromAltTab();
            ((HwndSource)PresentationSource.FromVisual(this)!).AddHook(WindowProcedure);
        };
    }

    public void Render(DrawingState state)
    {
        if (CheckAccess())
        {
            renderElement.UpdateState(state, GetPhysicalToLocalTransform());
            return;
        }

        lock (renderLock)
        {
            pendingState = state;
            if (renderScheduled)
            {
                return;
            }

            renderScheduled = true;
        }

        Dispatcher.BeginInvoke(DispatcherPriority.Render, FlushPendingRender);
    }

    private void FlushPendingRender()
    {
        DrawingState state;
        lock (renderLock)
        {
            state = pendingState!;
            pendingState = null;
            renderScheduled = false;
        }

        renderElement.UpdateState(state, GetPhysicalToLocalTransform());
    }

    private PhysicalToLocalTransform GetPhysicalToLocalTransform()
    {
        var matrix = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice ?? Matrix.Identity;

        if (!NativeMethods.GetWindowRect(new WindowInteropHelper(this).Handle, out var windowRect))
        {
            return new PhysicalToLocalTransform(0, 0, matrix);
        }

        return new PhysicalToLocalTransform(windowRect.Left, windowRect.Top, matrix);
    }

    // ShowInTaskbar="False" removes only the taskbar button; a borderless top-level
    // window still shows in Alt+Tab and Win+Tab unless it is marked as a tool window.
    private void HideFromAltTab()
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        var exStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
        exStyle = (exStyle | NativeMethods.WS_EX_TOOLWINDOW) & ~NativeMethods.WS_EX_APPWINDOW;
        NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, exStyle);
    }

    private IntPtr WindowProcedure(
        IntPtr hwnd,
        int message,
        IntPtr wParam,
        IntPtr lParam,
        ref bool handled)
    {
        if (message == NativeMethods.WM_NCHITTEST)
        {
            handled = true;
            return new IntPtr(NativeMethods.HTTRANSPARENT);
        }

        return IntPtr.Zero;
    }
}
