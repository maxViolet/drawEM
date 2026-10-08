using System.Runtime.InteropServices;

namespace DrawEM.App.Infrastructure.Effects;

internal static class NativeMethods
{
    /// <summary>Hook type for a system-wide low-level keyboard hook (SetWindowsHookEx).</summary>
    internal const int WH_KEYBOARD_LL = 13;

    /// <summary>Hook type for a system-wide low-level mouse hook (SetWindowsHookEx).</summary>
    internal const int WH_MOUSE_LL = 14;

    /// <summary>dwThreadId value for SetWindowsHookEx; low-level hooks require all threads.</summary>
    internal const uint AllThreads = 0;

    /// <summary>nCode value meaning the hook callback carries a real message.</summary>
    internal const int HC_ACTION = 0;

    /// <summary>A non-system key was pressed.</summary>
    internal const int WM_KEYDOWN = 0x0100;

    /// <summary>A key was pressed while Alt is held.</summary>
    internal const int WM_SYSKEYDOWN = 0x0104;

    /// <summary>The cursor moved.</summary>
    internal const int WM_MOUSEMOVE = 0x0200;

    /// <summary>A key registered with RegisterHotKey was pressed; posted to the registering thread.</summary>
    internal const int WM_HOTKEY = 0x0312;

    /// <summary>RegisterHotKey modifier: either Alt key.</summary>
    internal const uint MOD_ALT = 0x0001;

    /// <summary>RegisterHotKey modifier: either Ctrl key.</summary>
    internal const uint MOD_CONTROL = 0x0002;

    /// <summary>RegisterHotKey modifier: auto-repeat does not post another WM_HOTKEY.</summary>
    internal const uint MOD_NOREPEAT = 0x4000;

    /// <summary>INPUT.type value for mouse input.</summary>
    internal const uint INPUT_MOUSE = 0;

    /// <summary>INPUT.type value for keyboard input.</summary>
    internal const uint INPUT_KEYBOARD = 1;

    /// <summary>KEYBDINPUT.dwFlags value for a key release.</summary>
    internal const uint KEYEVENTF_KEYUP = 0x0002;

    /// <summary>MOUSEINPUT.dwFlags: movement occurred.</summary>
    internal const uint MOUSEEVENTF_MOVE = 0x0001;

    /// <summary>MOUSEINPUT.dwFlags: coordinates map to the whole virtual desktop.</summary>
    internal const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;

    /// <summary>MOUSEINPUT.dwFlags: dx and dy are normalized absolute coordinates (0 to 65535).</summary>
    internal const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

    /// <summary>GetSystemMetrics index: left edge of the virtual desktop.</summary>
    internal const int SM_XVIRTUALSCREEN = 76;

    /// <summary>GetSystemMetrics index: top edge of the virtual desktop.</summary>
    internal const int SM_YVIRTUALSCREEN = 77;

    /// <summary>GetSystemMetrics index: width of the virtual desktop.</summary>
    internal const int SM_CXVIRTUALSCREEN = 78;

    /// <summary>GetSystemMetrics index: height of the virtual desktop.</summary>
    internal const int SM_CYVIRTUALSCREEN = 79;

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
        public int x;
        public int y;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    /// <summary>One event for SendInput; the union is sized by MOUSEINPUT, its largest member.</summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        public uint type;
        public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;

        [FieldOffset(0)]
        public KEYBDINPUT ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr SetWindowsHookEx(int idHook, LowLevelHookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr GetModuleHandle(string? lpModuleName);

    [DllImport("user32.dll")]
    internal static extern int GetSystemMetrics(int index);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint modifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}
