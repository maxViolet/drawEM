using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

public class ShortcutBindingsTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;

    [Theory]
    [InlineData('A', VirtualKeys.A)]
    [InlineData('z', VirtualKeys.Z)]
    public void VirtualKeyOf_Letter_IsItsUppercaseCode(char letter, int expected) =>
        Assert.Equal(expected, KeyChord.VirtualKeyOf(ShortcutKey.Letter(letter)));

    [Theory]
    [InlineData(0, VirtualKeys.D0)]
    [InlineData(1, VirtualKeys.D1)]
    [InlineData(9, 0x39)]
    public void VirtualKeyOf_Digit_IsMainRowKey(int digit, int expected) =>
        Assert.Equal(expected, KeyChord.VirtualKeyOf(ShortcutKey.Digit(digit)));

    [Theory]
    [InlineData(1, VirtualKeys.F1)]
    [InlineData(12, 0x7B)]
    public void VirtualKeyOf_Function_IsFunctionKey(int number, int expected) =>
        Assert.Equal(expected, KeyChord.VirtualKeyOf(ShortcutKey.Function(number)));

    [Fact]
    public void VirtualKeyOf_DefaultKey_Throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => KeyChord.VirtualKeyOf(default));

    [Fact]
    public void TryGetShortcutKey_EveryLegalKey_RoundTripsVirtualKeyOf()
    {
        var keys = Enumerable.Range('A', 26).Select(letter => ShortcutKey.Letter((char)letter))
            .Concat(Enumerable.Range(0, 10).Select(ShortcutKey.Digit))
            .Concat(Enumerable.Range(1, 12).Select(ShortcutKey.Function));

        foreach (var key in keys)
        {
            Assert.True(KeyChord.TryGetShortcutKey(KeyChord.VirtualKeyOf(key), out var mapped));
            Assert.Equal(key, mapped);
        }
    }

    [Theory]
    [InlineData(VirtualKeys.Escape)]
    [InlineData(0x09)] // Tab
    [InlineData(VirtualKeys.LeftControl)]
    [InlineData(VirtualKeys.LeftWindows)]
    [InlineData(VirtualKeys.Neutral)]
    [InlineData(0x60)] // Numpad 0
    [InlineData(0x7C)] // F13
    public void TryGetShortcutKey_OtherKey_IsNotACandidate(int vkCode) =>
        Assert.False(KeyChord.TryGetShortcutKey(vkCode, out _));

    [Fact]
    public void FromSnapshot_BindsDrawClearAndFilledSlotsOnly()
    {
        var applause = new SoundReference("a1.wav", "applause.wav");
        var snapshot = SettingsSnapshot.Validate(
            SettingsSnapshot.Default.Style,
            Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Shift, ShortcutKey.Letter('D')),
            Shortcut.Create(CtrlAlt, ShortcutKey.Function(5)),
            Enumerable.Range(1, ActionSlot.Count).Select(number => number == 3
                ? new ActionSlot(3, new SoundAction(applause, Shortcut.Create(CtrlAlt, ShortcutKey.Digit(0))))
                : ActionSlot.Empty(number))).Snapshot!;
        var resolved = new List<SoundReference>();

        var bindings = ShortcutBindings.FromSnapshot(snapshot, sound =>
        {
            resolved.Add(sound);
            return Command(sound.LibraryFileName);
        });

        Assert.Equal(new KeyChord(ShortcutModifiers.Control | ShortcutModifiers.Shift, 'D'), bindings.Draw);
        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.F1 + 4), bindings.Clear);
        Assert.Equal(
            [new SoundBinding(new KeyChord(CtrlAlt, VirtualKeys.D0), Command("a1.wav"))],
            bindings.Sounds);
        Assert.Equal([applause], resolved);
    }

    [Fact]
    public void Constructor_DuplicateShortcut_Throws()
    {
        var draw = SettingsSnapshot.Default.DrawShortcut;

        Assert.Throws<ArgumentException>(() => new ShortcutBindings(
            draw, SettingsSnapshot.Default.ClearShortcut, [(draw, Command("applause"))]));
    }

    private static PlaySoundCommand Command(string sound) => new(new SoundId(sound), @"C:\library\" + sound);
}
