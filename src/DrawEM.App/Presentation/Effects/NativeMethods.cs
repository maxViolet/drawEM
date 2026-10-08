using System.Runtime.InteropServices;

namespace DrawEM.App.Presentation.Effects;

internal static class NativeMethods
{
    /// <summary>The system asks which part of the window is under the cursor.</summary>
    internal const int WM_NCHITTEST = 0x0084;

    /// <summary>WM_NCHITTEST result: the system sends mouse input to the window below.</summary>
    internal const int HTTRANSPARENT = -1;

    /// <summary>A click on an inactive window asks whether to activate it.</summary>
    internal const int WM_MOUSEACTIVATE = 0x0021;

    /// <summary>WM_MOUSEACTIVATE result: do not activate the window, and do not discard the message.</summary>
    internal const int MA_NOACTIVATE = 3;

    /// <summary>GetWindowLong/SetWindowLong index of the extended window style.</summary>
    internal const int GWL_EXSTYLE = -20;

    /// <summary>No taskbar button, not listed in Alt+Tab or Win+Tab.</summary>
    internal const int WS_EX_TOOLWINDOW = 0x00000080;

    /// <summary>Forces a window onto the taskbar and into Alt+Tab; it overrides WS_EX_TOOLWINDOW.</summary>
    internal const int WS_EX_APPWINDOW = 0x00040000;

    /// <summary>
    /// With WS_EX_LAYERED, mouse input passes through the window to windows below it, including
    /// windows of other processes. WM_NCHITTEST alone reaches only windows of the same thread.
    /// </summary>
    internal const int WS_EX_TRANSPARENT = 0x00000020;

    /// <summary>Layered window; WPF sets it for AllowsTransparency and WS_EX_TRANSPARENT needs it.</summary>
    internal const int WS_EX_LAYERED = 0x00080000;

    /// <summary>The window never becomes the foreground window through user interaction.</summary>
    internal const int WS_EX_NOACTIVATE = 0x08000000;

    /// <summary>SetWindowPos insert-after value: above all non-topmost windows, and topmost itself.</summary>
    internal static readonly IntPtr HWND_TOPMOST = new(-1);

    /// <summary>SetWindowPos flag: do not activate the window.</summary>
    internal const uint SWP_NOACTIVATE = 0x0010;

    /// <summary>SetWindowPos flag: do not change the owner window's position in the Z order.</summary>
    internal const uint SWP_NOOWNERZORDER = 0x0200;

    /// <summary>GetGuiResources flag: count of GDI objects.</summary>
    internal const uint GR_GDIOBJECTS = 0;

    /// <summary>GetGuiResources flag: count of USER objects.</summary>
    internal const uint GR_USEROBJECTS = 1;

    [DllImport("user32.dll")]
    internal static extern int GetWindowLong(IntPtr hWnd, int index);

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hWnd, int index, int newLong);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowPos(
        IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    internal static extern uint GetGuiResources(IntPtr hProcess, uint flags);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetCurrentProcess();

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        internal int Left;
        internal int Top;
        internal int Right;
        internal int Bottom;
    }
}
