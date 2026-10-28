namespace DrawEM.App.Infrastructure.Drawing;

public sealed class Win32MouseHookSource : IDisposable
{
    private readonly NativeMethods.LowLevelHookProc proc;
    private readonly MouseHookEvents events = new();
    private IntPtr hookHandle;

    public Win32MouseHookSource()
    {
        proc = HookCallback;
        hookHandle = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_MOUSE_LL, proc, NativeMethods.GetModuleHandle(null), NativeMethods.AllThreads);

        if (hookHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to register the global mouse hook.");
        }
    }

    public IMouseHookSource Events => events;

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) =>
        events.HandleHook(nCode, wParam, lParam)
            ? NativeMethods.SuppressMessage
            : NativeMethods.CallNextHookEx(hookHandle, nCode, wParam, lParam);

    public void Dispose()
    {
        if (hookHandle != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(hookHandle);
            hookHandle = IntPtr.Zero;
        }
    }
}
