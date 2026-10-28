using System.Runtime.InteropServices;

namespace DrawEM.App.Infrastructure;

public sealed class Win32KeyboardHookSource : INeutralKeyEmitter, IDisposable
{
    private readonly NativeMethods.LowLevelHookProc proc;
    private readonly KeyboardHookEvents events = new();
    private IntPtr hookHandle;

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

    public IKeyboardHookSource Events => events;

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

    /// <remarks>
    /// Called inside the hook callback. SendInput only queues the events, so they arrive after the event
    /// being handled and before any key the user presses or releases later. A failure is ignored: the hook
    /// cannot wait or retry, and a lone key-down of an unassigned key has no effect.
    /// </remarks>
    public void EmitNeutralKey()
    {
        NativeMethods.INPUT[] inputs =
        [
            NativeMethods.INPUT.Key(VirtualKeys.Neutral, keyUp: false, KeyboardHookEvents.NeutralKeyTag),
            NativeMethods.INPUT.Key(VirtualKeys.Neutral, keyUp: true, KeyboardHookEvents.NeutralKeyTag),
        ];
        _ = NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<NativeMethods.INPUT>());
    }
}
