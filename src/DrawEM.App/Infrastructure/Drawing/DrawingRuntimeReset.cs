using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;

namespace DrawEM.App.Infrastructure.Drawing;

/// <summary>
/// Save's drawing reset. Closes the input gate at once, so hooks stop suppressing input before the
/// dispatcher runs, then queues exit from draw mode and a clear of every monitor on the UI dispatcher.
/// </summary>
public sealed class DrawingRuntimeReset(
    DrawingModeInputGate inputGate,
    DrawingSessionController controller,
    Action<Action> dispatch) : IDrawingReset
{
    public void ExitDrawingAndClearAllMonitors()
    {
        inputGate.SetActive(false);
        dispatch(controller.ClearAndExitDrawMode);
    }
}
