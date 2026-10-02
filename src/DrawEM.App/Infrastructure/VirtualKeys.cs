namespace DrawEM.App.Infrastructure;

/// <summary>Win32 virtual-key codes used by the global shortcuts.</summary>
public static class VirtualKeys
{
    /// <summary>A key.</summary>
    public const int A = 0x41;

    /// <summary>Left Shift key.</summary>
    public const int LeftShift = 0xA0;

    /// <summary>Right Shift key.</summary>
    public const int RightShift = 0xA1;

    /// <summary>Left Ctrl key.</summary>
    public const int LeftControl = 0xA2;

    /// <summary>Right Ctrl key.</summary>
    public const int RightControl = 0xA3;

    /// <summary>Left Alt key.</summary>
    public const int LeftMenu = 0xA4;

    /// <summary>Right Alt key (AltGr on some layouts).</summary>
    public const int RightMenu = 0xA5;

    /// <summary>Z key.</summary>
    public const int Z = 0x5A;

    /// <summary>X key.</summary>
    public const int X = 0x58;

    /// <summary>0 key on the main row; 1–9 follow in order.</summary>
    public const int D0 = 0x30;

    /// <summary>1 key on the main row (sound slot 1).</summary>
    public const int D1 = 0x31;

    /// <summary>2 key on the main row (sound slot 2).</summary>
    public const int D2 = 0x32;

    /// <summary>3 key on the main row (sound slot 3).</summary>
    public const int D3 = 0x33;

    /// <summary>4 key on the main row (sound slot 4).</summary>
    public const int D4 = 0x34;

    /// <summary>5 key on the main row (sound slot 5).</summary>
    public const int D5 = 0x35;

    /// <summary>6 key on the main row (sound slot 6).</summary>
    public const int D6 = 0x36;

    /// <summary>7 key on the main row (sound slot 7).</summary>
    public const int D7 = 0x37;

    /// <summary>8 key on the main row (sound slot 8).</summary>
    public const int D8 = 0x38;

    /// <summary>F1 key; F2–F12 follow in order.</summary>
    public const int F1 = 0x70;

    /// <summary>F4 key.</summary>
    public const int F4 = 0x73;

    /// <summary>F12 key.</summary>
    public const int F12 = 0x7B;

    /// <summary>Esc key.</summary>
    public const int Escape = 0x1B;

    /// <summary>Left Windows key.</summary>
    public const int LeftWindows = 0x5B;

    /// <summary>Right Windows key.</summary>
    public const int RightWindows = 0x5C;

    /// <summary>
    /// Unassigned virtual key 0xE8. Sent between a suppressed shortcut key and the release of its modifiers,
    /// so Windows does not read a lone Alt+Shift or Ctrl+Shift press as a layout switch.
    /// </summary>
    public const int Neutral = 0xE8;
}
