using DrawEM.App.Application.Input;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Infrastructure;

/// <summary>Win32 virtual-key codes used by the global shortcuts.</summary>
public static class VirtualKeys
{
    /// <summary>
    /// Translates a low-level hook virtual-key code. Main-row digits, letters, and F1–F12 become shortcut
    /// candidates; side-specific modifiers become modifiers. Everything else, including numpad digits and
    /// the side-neutral <c>VK_CONTROL</c>/<c>VK_MENU</c>/<c>VK_SHIFT</c>, stays an opaque key.
    /// </summary>
    public static InputKey ToInputKey(int vkCode) => vkCode switch
    {
        >= D0 and <= 0x39 => InputKey.Candidate(ShortcutKey.Digit(vkCode - D0)),
        >= A and <= Z => InputKey.Candidate(ShortcutKey.Letter((char)vkCode)),
        >= F1 and <= F1 + 11 => InputKey.Candidate(ShortcutKey.Function(vkCode - F1 + 1)),
        LeftControl => InputKey.Modifier(ModifierKey.LeftControl),
        RightControl => InputKey.Modifier(ModifierKey.RightControl),
        LeftMenu => InputKey.Modifier(ModifierKey.LeftAlt),
        RightMenu => InputKey.Modifier(ModifierKey.RightAlt),
        LeftShift => InputKey.Modifier(ModifierKey.LeftShift),
        RightShift => InputKey.Modifier(ModifierKey.RightShift),
        LeftWindows => InputKey.Modifier(ModifierKey.LeftWindows),
        RightWindows => InputKey.Modifier(ModifierKey.RightWindows),
        _ => InputKey.Other(vkCode),
    };

    /// <summary>0 key on the main row.</summary>
    public const int D0 = 0x30;

    /// <summary>F1 key; F2–F12 follow consecutively.</summary>
    public const int F1 = 0x70;

    /// <summary>Left Shift key.</summary>
    public const int LeftShift = 0xA0;

    /// <summary>Right Shift key.</summary>
    public const int RightShift = 0xA1;

    /// <summary>Left Windows key.</summary>
    public const int LeftWindows = 0x5B;

    /// <summary>Right Windows key.</summary>
    public const int RightWindows = 0x5C;

    /// <summary>A key.</summary>
    public const int A = 0x41;

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
}
