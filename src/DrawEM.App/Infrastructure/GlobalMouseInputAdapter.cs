using System.Threading;
using DrawEM.App.Application;
using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public sealed class GlobalMouseInputAdapter
{
    private readonly DrawingSessionController controller;
    private readonly Action<Action> dispatch;
    private int drawModeActive;

    public GlobalMouseInputAdapter(IMouseHookSource source, DrawingSessionController controller)
        : this(source, controller, action => action())
    {
    }

    public GlobalMouseInputAdapter(
        IMouseHookSource source,
        DrawingSessionController controller,
        Action<Action> dispatch)
    {
        this.controller = controller;
        this.dispatch = dispatch;
        controller.InputCaptureRequested += SetDrawMode;
        source.PointerMoved += OnPointerMoved;
        source.PointerButtonActivity += ShouldSuppressPointerButton;
        source.PointerWheelActivity += ShouldSuppressPointerButton;
    }

    private void SetDrawMode(bool active) => Interlocked.Exchange(ref drawModeActive, active ? 1 : 0);

    private void OnPointerMoved(ScreenPoint point)
    {
        if (Volatile.Read(ref drawModeActive) == 1)
        {
            dispatch(() => controller.ReportPointer(point));
        }
    }

    private bool ShouldSuppressPointerButton() => Volatile.Read(ref drawModeActive) == 1;
}
