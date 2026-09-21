using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace DrawEM.App.Presentation;

public partial class OverlayWindow : Window, IOverlayView, IOverlayLifetime
{
    private readonly StrokeRenderElement renderElement = new();
    private readonly object renderLock = new();
    private DrawingState? pendingState;
    private bool renderScheduled;

    public event Action<ScreenPoint>? PointerMoved;

    public OverlayWindow()
    {
        InitializeComponent();

        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;
        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;

        Content = renderElement;
        SourceInitialized += (_, _) => SetClickThrough(true);
        MouseMove += OnMouseMove;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (!NativeMethods.GetCursorPos(out var cursor))
        {
            return;
        }

        PointerMoved?.Invoke(new ScreenPoint(cursor.X, cursor.Y));
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

    public void SetInputCapture(bool captureInput)
    {
        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send, () => SetInputCapture(captureInput));
            return;
        }

        SetClickThrough(!captureInput);
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

    private void SetClickThrough(bool clickThrough)
    {
        var handle = new WindowInteropHelper(this).Handle;
        var style = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);

        style = clickThrough
            ? style | NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_LAYERED
            : style & ~NativeMethods.WS_EX_TRANSPARENT;

        NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE, style);
    }
}
