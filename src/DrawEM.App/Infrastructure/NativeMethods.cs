using System.Runtime.InteropServices;

namespace DrawEM.App.Infrastructure;

internal static class NativeMethods
{
    /// <summary>Hook type for a system-wide low-level keyboard hook (SetWindowsHookEx).</summary>
    internal const int WH_KEYBOARD_LL = 13;

    /// <summary>
    /// dwThreadId value for SetWindowsHookEx that associates the hook with all threads
    /// on the desktop. Low-level hooks require it.
    /// </summary>
    internal const uint AllThreads = 0;

    /// <summary>
    /// nCode value meaning the hook callback carries a real message. A negative nCode
    /// must be passed to CallNextHookEx without processing.
    /// </summary>
    internal const int HC_ACTION = 0;

    /// <summary>
    /// Any non-zero value returned from a low-level hook callback stops the message:
    /// the system does not pass it to the rest of the hook chain or to the target window.
    /// </summary>
    internal static readonly IntPtr SuppressMessage = new(1);

    /// <summary>A non-system key was pressed.</summary>
    internal const int WM_KEYDOWN = 0x0100;

    /// <summary>A non-system key was released.</summary>
    internal const int WM_KEYUP = 0x0101;

    /// <summary>A key was pressed while Alt is held, or F10 was pressed.</summary>
    internal const int WM_SYSKEYDOWN = 0x0104;

    /// <summary>A key was released while Alt is held.</summary>
    internal const int WM_SYSKEYUP = 0x0105;

    internal delegate IntPtr LowLevelHookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    internal struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr SetWindowsHookEx(
        int idHook, LowLevelHookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr GetModuleHandle(string? lpModuleName);

}
