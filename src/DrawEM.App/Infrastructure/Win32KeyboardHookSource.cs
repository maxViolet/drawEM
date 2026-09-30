using System.Runtime.InteropServices;
using DrawEM.App.Application.Input;

namespace DrawEM.App.Infrastructure;

public sealed class Win32KeyboardHookSource : IKeyboardHookSource, IDisposable
{
    private readonly NativeMethods.LowLevelHookProc proc;
    private IntPtr hookHandle;

    public event Func<int, KeyDirection, bool>? KeyEvent;

    public Win32KeyboardHookSource()
    {
        proc = HookCallback;
        hookHandle = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_KEYBOARD_LL, proc, NativeMethods.GetModuleHandle(null), NativeMethods.AllThreads);

        if (hookHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to register the global keyboard hook.");
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= NativeMethods.HC_ACTION)
        {
            var data = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
            var vkCode = (int)data.vkCode;
            var message = wParam.ToInt32();

            KeyDirection? direction = message switch
            {
                NativeMethods.WM_KEYDOWN or NativeMethods.WM_SYSKEYDOWN => KeyDirection.Down,
                NativeMethods.WM_KEYUP or NativeMethods.WM_SYSKEYUP => KeyDirection.Up,
                _ => null,
            };

            if (direction is { } keyDirection && (KeyEvent?.Invoke(vkCode, keyDirection) ?? false))
            {
                return NativeMethods.SuppressMessage;
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
}
