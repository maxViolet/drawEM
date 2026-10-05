using DrawEM.App.Application.Drawing;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public sealed class GlobalMouseInputAdapter
{
    private readonly DrawingSessionController controller;
    private readonly DrawingModeInputGate inputGate;
    private readonly Action<Action> dispatch;
    private readonly IMonitorBoundsSource monitorBoundsSource;

    public GlobalMouseInputAdapter(
        IMouseHookSource source,
        DrawingSessionController controller,
        DrawingModeInputGate inputGate,
        Action<Action> dispatch,
        IMonitorBoundsSource monitorBoundsSource)
    {
        this.controller = controller;
        this.inputGate = inputGate;
        this.dispatch = dispatch;
        this.monitorBoundsSource = monitorBoundsSource;
        source.PointerMoved += OnPointerMoved;
        source.PointerButtonActivity += ShouldSuppressPointerButton;
        source.PointerWheelActivity += ShouldSuppressPointerButton;
    }

    private void OnPointerMoved(ScreenPoint point)
    {
        if (inputGate.StopAtBoundary(point, monitorBoundsSource, out var drawPoint))
        {
            dispatch(controller.ExitDrawMode);
            return;
        }

        if (inputGate.IsActive)
        {
            dispatch(() => controller.ReportPointer(drawPoint));
        }
    }

    private bool ShouldSuppressPointerButton() => inputGate.IsActive;
}
