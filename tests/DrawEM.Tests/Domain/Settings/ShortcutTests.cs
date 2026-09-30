using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests.Domain.Settings;

public class ShortcutTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;

    [Theory]
    [InlineData(ShortcutModifiers.Control | ShortcutModifiers.Alt)]
    [InlineData(ShortcutModifiers.Control | ShortcutModifiers.Shift)]
    [InlineData(ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
    [InlineData(ShortcutModifiers.Control | ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
    public void TryCreate_AcceptsTwoOrMoreModifiers(ShortcutModifiers modifiers)
    {
        Assert.True(Shortcut.TryCreate(modifiers, ShortcutKey.Letter('Q'), out var shortcut, out var error));
        Assert.Equal(ShortcutError.None, error);
        Assert.Equal(modifiers, shortcut.Modifiers);
    }

    [Theory]
    [InlineData(ShortcutModifiers.None)]
    [InlineData(ShortcutModifiers.Control)]
    [InlineData(ShortcutModifiers.Alt)]
    [InlineData(ShortcutModifiers.Shift)]
    public void TryCreate_RejectsFewerThanTwoModifiers(ShortcutModifiers modifiers)
    {
        Assert.False(Shortcut.TryCreate(modifiers, ShortcutKey.Letter('C'), out var shortcut, out var error));
        Assert.Null(shortcut);
        Assert.Equal(ShortcutError.TooFewModifiers, error);
    }

    [Theory]
    [InlineData(ShortcutModifiers.Windows | ShortcutModifiers.Control | ShortcutModifiers.Alt)]
    [InlineData(ShortcutModifiers.Windows | ShortcutModifiers.Shift)]
    public void TryCreate_RejectsWindowsKeyChords(ShortcutModifiers modifiers)
    {
        Assert.False(Shortcut.TryCreate(modifiers, ShortcutKey.Digit(1), out _, out var error));
        Assert.Equal(ShortcutError.WindowsKey, error);
    }

    [Fact]
    public void TryCreate_RejectsUndefinedModifierBits()
    {
        Assert.False(Shortcut.TryCreate(CtrlAlt | (ShortcutModifiers)64, ShortcutKey.Digit(1), out _, out var error));
        Assert.Equal(ShortcutError.Unrecognized, error);
    }

    [Fact]
    public void TryCreate_RejectsDefaultKey()
    {
        Assert.False(ShortcutKey.IsValid(default));
        Assert.False(Shortcut.TryCreate(CtrlAlt, default, out var shortcut, out var error));
        Assert.Null(shortcut);
        Assert.Equal(ShortcutError.IllegalKey, error);
        Assert.Throws<ArgumentException>(() => Shortcut.Create(CtrlAlt, default));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("z")]
    [InlineData("0")]
    [InlineData("9")]
    [InlineData("F1")]
    [InlineData("f12")]
    public void ShortcutKey_TryParse_AcceptsLettersDigitsAndF1ToF12(string text)
    {
        Assert.True(ShortcutKey.TryParse(text, out var key));
        Assert.Equal(text.ToUpperInvariant(), key.Value.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("F0")]
    [InlineData("F01")]
    [InlineData("F13")]
    [InlineData("Tab")]
    [InlineData("Escape")]
    [InlineData("-")]
    [InlineData("ß")]
    [InlineData("AB")]
    public void ShortcutKey_TryParse_RejectsIllegalKeys(string? text)
    {
        Assert.False(ShortcutKey.TryParse(text, out _));
    }

    [Fact]
    public void ShortcutKey_Factories_RejectIllegalKeys()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ShortcutKey.Letter('1'));
        Assert.Throws<ArgumentOutOfRangeException>(() => ShortcutKey.Digit(10));
        Assert.Throws<ArgumentOutOfRangeException>(() => ShortcutKey.Function(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => ShortcutKey.Function(13));
    }

    [Theory]
    [InlineData("Ctrl+Alt+Z", "Ctrl+Alt+Z")]
    [InlineData("alt+ctrl+z", "Ctrl+Alt+Z")]
    [InlineData("Shift+Alt+F5", "Alt+Shift+F5")]
    [InlineData("Ctrl+Alt+Shift+7", "Ctrl+Alt+Shift+7")]
    public void TryParse_ReadsAnyModifierOrderAndWritesCanonicalText(string text, string canonical)
    {
        Assert.True(Shortcut.TryParse(text, out var shortcut, out _));
        Assert.Equal(canonical, shortcut.ToString());
    }

    [Theory]
    [InlineData("Ctrl+C", ShortcutError.TooFewModifiers)]
    [InlineData("Alt+F4", ShortcutError.TooFewModifiers)]
    [InlineData("Win+Ctrl+Alt+1", ShortcutError.WindowsKey)]
    [InlineData("Ctrl+RightAlt+1", ShortcutError.Unrecognized)]
    [InlineData("Ctrl+Ctrl+1", ShortcutError.Unrecognized)]
    [InlineData("Ctrl+Alt", ShortcutError.Unrecognized)]
    [InlineData("Ctrl+Alt+Tab", ShortcutError.Unrecognized)]
    [InlineData("Ctrl+Alt+1+2", ShortcutError.Unrecognized)]
    [InlineData("", ShortcutError.Unrecognized)]
    public void TryParse_RejectsInvalidShortcuts(string text, ShortcutError expected)
    {
        Assert.False(Shortcut.TryParse(text, out _, out var error));
        Assert.Equal(expected, error);
    }

    [Fact]
    public void Equality_IsByModifiersAndKey()
    {
        Assert.Equal(
            Shortcut.Create(CtrlAlt, ShortcutKey.Letter('z')),
            Shortcut.Create(ShortcutModifiers.Alt | ShortcutModifiers.Control, ShortcutKey.Letter('Z')));
        Assert.NotEqual(
            Shortcut.Create(CtrlAlt, ShortcutKey.Digit(1)),
            Shortcut.Create(CtrlAlt, ShortcutKey.Function(1)));
    }
}
