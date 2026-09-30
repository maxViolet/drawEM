using DrawEM.App.Application.Input;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class VirtualKeysTests
{
    [Theory]
    [InlineData(0x41, "A")]
    [InlineData(0x5A, "Z")]
    [InlineData(0x30, "0")]
    [InlineData(0x39, "9")]
    [InlineData(0x70, "F1")]
    [InlineData(0x7B, "F12")]
    public void ToInputKey_MapsLettersMainRowDigitsAndF1ToF12ToCandidates(int vkCode, string key)
    {
        Assert.True(ShortcutKey.TryParse(key, out var expected));
        Assert.Equal(InputKey.Candidate(expected.Value), VirtualKeys.ToInputKey(vkCode));
    }

    [Theory]
    [InlineData(0xA2, ModifierKey.LeftControl)]
    [InlineData(0xA3, ModifierKey.RightControl)]
    [InlineData(0xA4, ModifierKey.LeftAlt)]
    [InlineData(0xA5, ModifierKey.RightAlt)]
    [InlineData(0xA0, ModifierKey.LeftShift)]
    [InlineData(0xA1, ModifierKey.RightShift)]
    [InlineData(0x5B, ModifierKey.LeftWindows)]
    [InlineData(0x5C, ModifierKey.RightWindows)]
    public void ToInputKey_MapsSideSpecificModifiers(int vkCode, ModifierKey modifier)
    {
        Assert.Equal(InputKey.Modifier(modifier), VirtualKeys.ToInputKey(vkCode));
    }

    [Theory]
    [InlineData(0x11)] // VK_CONTROL
    [InlineData(0x12)] // VK_MENU
    [InlineData(0x10)] // VK_SHIFT
    [InlineData(0x61)] // VK_NUMPAD1
    [InlineData(0x7C)] // VK_F13
    [InlineData(0x09)] // VK_TAB
    [InlineData(0xE8)] // unassigned, reserved for the v3 neutral key
    public void ToInputKey_KeepsOtherKeysOpaque(int vkCode)
    {
        Assert.Equal(InputKey.Other(vkCode), VirtualKeys.ToInputKey(vkCode));
    }
}
