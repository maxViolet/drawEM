using System.Runtime.InteropServices;

namespace DrawEM.App.Presentation.Theme;

internal static class NativeMethods
{
    /// <summary>DwmSetWindowAttribute: draw the window frame and backdrop in dark colors.</summary>
    internal const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    /// <summary>DwmSetWindowAttribute: the corner style of the window.</summary>
    internal const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;

    /// <summary>DwmSetWindowAttribute: the system-drawn material behind the window. Windows 11 22H2 and later.</summary>
    internal const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

    /// <summary>Round corners, as other Windows 11 windows and menus have.</summary>
    internal const int DWMWCP_ROUND = 2;

    /// <summary>Acrylic: a blurred, tinted copy of what is behind the window.</summary>
    internal const int DWMSBT_TRANSIENTWINDOW = 3;

    /// <summary>GetWindowLong/SetWindowLong index of the window style.</summary>
    internal const int GWL_STYLE = -16;

    /// <summary>Window style that gives the title bar its system menu and the system window buttons.</summary>
    internal const int WS_SYSMENU = 0x00080000;

    /// <summary>GetWindowLong/SetWindowLong index of the extended window style.</summary>
    internal const int GWL_EXSTYLE = -20;

    /// <summary>Extended style for a tool window: no taskbar button, not listed in Alt+Tab.</summary>
    internal const int WS_EX_TOOLWINDOW = 0x00000080;

    /// <summary>MonitorFromPoint: the monitor nearest to the point when none contains it.</summary>
    internal const int MONITOR_DEFAULTTONEAREST = 2;

    /// <summary>GetDpiForMonitor: the DPI that scaling uses.</summary>
    internal const int MDT_EFFECTIVE_DPI = 0;

    [DllImport("dwmapi.dll")]
    internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

    [DllImport("user32.dll")]
    internal static extern int GetWindowLong(IntPtr hWnd, int index);

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hWnd, int index, int newLong);

    [DllImport("user32.dll")]
    internal static extern IntPtr MonitorFromPoint(POINT point, int flags);

    [DllImport("shcore.dll")]
    internal static extern int GetDpiForMonitor(IntPtr monitor, int dpiType, out uint dpiX, out uint dpiY);

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }
}
