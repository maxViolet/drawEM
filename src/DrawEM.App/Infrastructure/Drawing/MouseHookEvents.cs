using System.Runtime.InteropServices;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

/// <summary>
/// Routes low-level mouse events: a move reports the point, and a button or wheel event asks its activity
/// handler whether to hide the event.
/// </summary>
public sealed class MouseHookEvents : IMouseHookSource
{
    public event Action<ScreenPoint>? PointerMoved;

    public event Func<bool>? PointerButtonActivity;

    public event Func<bool>? PointerWheelActivity;

    /// <summary>
    /// Routes the raw arguments of a low-level mouse hook callback. A negative <paramref name="nCode"/> passes
    /// through unread.
    /// </summary>
    /// <returns><c>true</c> to hide the event from the window under the cursor.</returns>
    internal bool HandleHook(int nCode, int message, IntPtr lParam)
    {
        if (nCode < NativeMethods.HC_ACTION)
        {
            return false;
        }

        if (message == NativeMethods.WM_MOUSEMOVE)
        {
            Move(Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam).pt);
            return false;
        }

        if (NativeMethods.IsPointerButtonMessage(message))
        {
            return Ask(PointerButtonActivity);
        }

        return NativeMethods.IsPointerWheelMessage(message) && Ask(PointerWheelActivity);
    }

    private void Move(NativeMethods.POINT point) => PointerMoved?.Invoke(new ScreenPoint(point.X, point.Y));

    private static bool Ask(Func<bool>? activity) => activity?.Invoke() ?? false;
}
