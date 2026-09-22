using System.Runtime.InteropServices;

namespace DrawEM.App.Infrastructure;

public sealed class Win32KeyboardHookSource : IKeyboardHookSource, IDisposable
{
    private readonly NativeMethods.LowLevelHookProc proc;
    private IntPtr hookHandle;

    public event Action<int>? KeyDown;

    public event Action<int>? KeyUp;

    public event Func<int, bool>? KeyActivity;

    public Win32KeyboardHookSource()
    {
        proc = HookCallback;
        hookHandle = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_KEYBOARD_LL, proc, NativeMethods.GetModuleHandle(null), 0);

        if (hookHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to register the global keyboard hook.");
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var data = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
            var vkCode = (int)data.vkCode;
            var message = wParam.ToInt32();

            if (message is NativeMethods.WM_KEYDOWN or NativeMethods.WM_SYSKEYDOWN)
            {
                KeyDown?.Invoke(vkCode);
                if (ShouldSuppressKey(vkCode))
                {
                    return new IntPtr(1);
                }
            }
            else if (message is NativeMethods.WM_KEYUP or NativeMethods.WM_SYSKEYUP)
            {
                var suppress = ShouldSuppressKey(vkCode);
                KeyUp?.Invoke(vkCode);
                if (suppress)
                {
                    return new IntPtr(1);
                }
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

    private bool ShouldSuppressKey(int vkCode) =>
        KeyActivity?
            .GetInvocationList()
            .Cast<Func<int, bool>>()
            .Any(handler => handler(vkCode))
        ?? false;
}
