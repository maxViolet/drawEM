using System.Runtime.InteropServices;

namespace DrawEM.App.Presentation;

internal static class NativeMethods
{
    /// <summary>The system asks which part of the window is under the cursor.</summary>
    internal const int WM_NCHITTEST = 0x0084;

    /// <summary>
    /// WM_NCHITTEST result: the window is transparent to input, so the system sends
    /// mouse input to the window below it.
    /// </summary>
    internal const int HTTRANSPARENT = -1;

    /// <summary>GetWindowLong/SetWindowLong index of the extended window style.</summary>
    internal const int GWL_EXSTYLE = -20;

    /// <summary>
    /// Extended style for a tool window: no taskbar button, not listed in Alt+Tab or Win+Tab.
    /// </summary>
    internal const int WS_EX_TOOLWINDOW = 0x00000080;

    /// <summary>
    /// Extended style that forces a top-level window onto the taskbar and into Alt+Tab.
    /// It must be cleared, or it overrides WS_EX_TOOLWINDOW.
    /// </summary>
    internal const int WS_EX_APPWINDOW = 0x00040000;

    [DllImport("user32.dll")]
    internal static extern bool GetCursorPos(out POINT point);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    [DllImport("user32.dll")]
    internal static extern int GetWindowLong(IntPtr hWnd, int index);

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hWnd, int index, int newLong);

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        internal int X;
        internal int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        internal int Left;
        internal int Top;
        internal int Right;
        internal int Bottom;
    }
}
