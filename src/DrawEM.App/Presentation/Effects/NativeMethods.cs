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

    /// <summary>EnumDisplaySettings mode index: the display's current settings.</summary>
    internal const int ENUM_CURRENT_SETTINGS = -1;

    /// <summary>MonitorFromPoint flag: return null when no monitor contains the point.</summary>
    internal const uint MONITOR_DEFAULTTONULL = 0;

    /// <summary>The window's DPI: 96 at 100% scaling. Per-monitor-aware windows report their monitor's DPI.</summary>
    [DllImport("user32.dll")]
    internal static extern uint GetDpiForWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern IntPtr MonitorFromPoint(POINT point, uint flags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetMonitorInfo(IntPtr monitor, ref MONITORINFOEX info);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool EnumDisplaySettings(string deviceName, int modeNumber, ref DEVMODE mode);

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        internal int X;
        internal int Y;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MONITORINFOEX
    {
        internal int Size;
        internal RECT Monitor;
        internal RECT WorkArea;
        internal uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string DeviceName;
    }

    /// <summary>DEVMODEW, display fields only.</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string DeviceName;

        internal short SpecVersion;
        internal short DriverVersion;
        internal short Size;
        internal short DriverExtra;
        internal int Fields;
        internal int PositionX;
        internal int PositionY;
        internal int DisplayOrientation;
        internal int DisplayFixedOutput;
        internal short Color;
        internal short Duplex;
        internal short YResolution;
        internal short TTOption;
        internal short Collate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string FormName;

        internal short LogPixels;
        internal int BitsPerPel;
        internal int PelsWidth;
        internal int PelsHeight;
        internal int DisplayFlags;
        internal int DisplayFrequency;
        internal int ICMMethod;
        internal int ICMIntent;
        internal int MediaType;
        internal int DitherType;
        internal int Reserved1;
        internal int Reserved2;
        internal int PanningWidth;
        internal int PanningHeight;
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
