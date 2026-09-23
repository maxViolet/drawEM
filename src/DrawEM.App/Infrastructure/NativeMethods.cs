using System.Runtime.InteropServices;

namespace DrawEM.App.Infrastructure;

internal static class NativeMethods
{
    /// <summary>Hook type for a system-wide low-level keyboard hook (SetWindowsHookEx).</summary>
    internal const int WH_KEYBOARD_LL = 13;

    /// <summary>Hook type for a system-wide low-level mouse hook (SetWindowsHookEx).</summary>
    internal const int WH_MOUSE_LL = 14;

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

    /// <summary>The cursor moved.</summary>
    internal const int WM_MOUSEMOVE = 0x0200;

    /// <summary>The left mouse button was pressed.</summary>
    internal const int WM_LBUTTONDOWN = 0x0201;

    /// <summary>The left mouse button was released.</summary>
    internal const int WM_LBUTTONUP = 0x0202;

    /// <summary>The right mouse button was pressed.</summary>
    internal const int WM_RBUTTONDOWN = 0x0204;

    /// <summary>The right mouse button was released.</summary>
    internal const int WM_RBUTTONUP = 0x0205;

    /// <summary>The middle mouse button was pressed.</summary>
    internal const int WM_MBUTTONDOWN = 0x0207;

    /// <summary>The middle mouse button was released.</summary>
    internal const int WM_MBUTTONUP = 0x0208;

    /// <summary>An extra mouse button (X1 or X2) was pressed.</summary>
    internal const int WM_XBUTTONDOWN = 0x020B;

    /// <summary>An extra mouse button (X1 or X2) was released.</summary>
    internal const int WM_XBUTTONUP = 0x020C;

    /// <summary>The vertical mouse wheel was rotated.</summary>
    internal const int WM_MOUSEWHEEL = 0x020A;

    /// <summary>The horizontal mouse wheel was tilted or rotated.</summary>
    internal const int WM_MOUSEHWHEEL = 0x020E;

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

    [StructLayout(LayoutKind.Sequential)]
    internal struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }

    internal static bool IsPointerButtonMessage(int message) => message is
        WM_LBUTTONDOWN or WM_LBUTTONUP or
        WM_RBUTTONDOWN or WM_RBUTTONUP or
        WM_MBUTTONDOWN or WM_MBUTTONUP or
        WM_XBUTTONDOWN or WM_XBUTTONUP;

    internal static bool IsPointerWheelMessage(int message) =>
        message is WM_MOUSEWHEEL or WM_MOUSEHWHEEL;

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

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(out POINT point);

    [DllImport("user32.dll")]
    internal static extern IntPtr MonitorFromPoint(POINT point, uint flags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetMonitorInfo(IntPtr monitor, ref MONITORINFO info);
}
