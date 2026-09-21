using System.Runtime.InteropServices;

namespace DrawEM.App.Presentation;

internal static class NativeMethods
{
    internal const int WM_NCHITTEST = 0x0084;
    internal const int HTTRANSPARENT = -1;

    [DllImport("user32.dll")]
    internal static extern bool GetCursorPos(out POINT point);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

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
