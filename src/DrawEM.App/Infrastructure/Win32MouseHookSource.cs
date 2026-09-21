using System.Runtime.InteropServices;
using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public sealed class Win32MouseHookSource : IMouseHookSource, IDisposable
{
    private readonly NativeMethods.LowLevelHookProc proc;
    private IntPtr hookHandle;

    public Win32MouseHookSource()
    {
        proc = HookCallback;
        hookHandle = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_MOUSE_LL, proc, NativeMethods.GetModuleHandle(null), 0);

        if (hookHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to register the global mouse hook.");
        }
    }

    public event Action<ScreenPoint>? PointerMoved;

    public event Func<bool>? PointerButtonActivity;

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var data = Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam);
            var message = wParam.ToInt32();

            if (message == NativeMethods.WM_MOUSEMOVE)
            {
                PointerMoved?.Invoke(new ScreenPoint(data.pt.X, data.pt.Y));
            }
            else if (NativeMethods.IsPointerButtonMessage(message) && ShouldSuppressPointerButton())
            {
                return new IntPtr(1);
            }
        }

        return NativeMethods.CallNextHookEx(hookHandle, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (hookHandle != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(hookHandle);
            hookHandle = IntPtr.Zero;
        }
    }

    private bool ShouldSuppressPointerButton() =>
        PointerButtonActivity?
            .GetInvocationList()
            .Cast<Func<bool>>()
            .Any(handler => handler())
        ?? false;
}
