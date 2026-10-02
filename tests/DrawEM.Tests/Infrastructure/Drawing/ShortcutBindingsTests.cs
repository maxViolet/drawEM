using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Sound;

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
            return new PlaySoundCommand(new SoundId(sound.LibraryFileName));
        });

        Assert.Equal(new KeyChord(ShortcutModifiers.Control | ShortcutModifiers.Shift, 'D'), bindings.Draw);
        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.F1 + 4), bindings.Clear);
        Assert.Equal(
            [new SoundBinding(new KeyChord(CtrlAlt, VirtualKeys.D0), new PlaySoundCommand(new SoundId("a1.wav")))],
            bindings.Sounds);
        Assert.Equal([applause], resolved);
    }

    [Fact]
    public void ForCodeAssignments_BindsCtrlAltDigitForAssignedSlots()
    {
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot2] = new(new SoundId("drumroll"), @"C:\sounds\drumroll.wav"),
        });

        var bindings = ShortcutBindings.ForCodeAssignments(configuration);

        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.Z), bindings.Draw);
        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.X), bindings.Clear);
        Assert.Equal(
            [new SoundBinding(new KeyChord(CtrlAlt, VirtualKeys.D2), new PlaySoundCommand(new SoundId("drumroll")))],
            bindings.Sounds);
    }

    [Fact]
    public void Constructor_DuplicateShortcut_Throws()
    {
        var draw = SettingsSnapshot.Default.DrawShortcut;

        Assert.Throws<ArgumentException>(() => new ShortcutBindings(
            draw, SettingsSnapshot.Default.ClearShortcut, [(draw, new PlaySoundCommand(new SoundId("applause")))]));
    }
}
