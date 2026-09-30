using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests.Domain.Settings;

public class SettingsSnapshotTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private static readonly DrawingStyle Style = new(new HexColor(1, 2, 3), new StrokeWidth(5));
    private static readonly Shortcut Draw = Keys("Ctrl+Alt+Z");
    private static readonly Shortcut Clear = Keys("Ctrl+Alt+X");

    [Fact]
    public void Default_IsOrangeRedFourPixelsDefaultShortcutsAndEightEmptySlots()
    {
        var defaults = SettingsSnapshot.Default;

        Assert.Equal("#FF4500", defaults.Style.Color.ToString());
        Assert.Equal(4, defaults.Style.Width.Pixels);
        Assert.Equal("Ctrl+Alt+Z", defaults.DrawShortcut.ToString());
        Assert.Equal("Ctrl+Alt+X", defaults.ClearShortcut.ToString());
        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8], defaults.Slots.Select(slot => slot.Number));
        Assert.All(defaults.Slots, slot => Assert.True(slot.IsEmpty));
    }

    [Fact]
    public void Validate_AcceptsFilledAndEmptySlots()
    {
        var slots = EmptySlots();
        slots[0] = Sound(1, "Ctrl+Alt+1");
        slots[7] = Sound(8, "Alt+Shift+F8");

        var result = SettingsSnapshot.Validate(Style, Draw, Clear, slots);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Equal(Style, result.Snapshot!.Style);
        Assert.IsType<SoundAction>(result.Snapshot.Slots[0].Action);
        Assert.True(result.Snapshot.Slots[1].IsEmpty);
    }

    [Fact]
    public void Validate_CopiesSlots_SoLaterChangesToTheInputDoNotLeakIn()
    {
        var slots = EmptySlots();
        var snapshot = SettingsSnapshot.Validate(Style, Draw, Clear, slots).Snapshot!;

        slots[0] = Sound(1, "Ctrl+Alt+1");

        Assert.True(snapshot.Slots[0].IsEmpty);
        Assert.Throws<NotSupportedException>(() => ((IList<ActionSlot>)snapshot.Slots)[0] = Sound(1, "Ctrl+Alt+1"));
    }

    [Fact]
    public void Validate_RequiresDrawAndClearShortcuts()
    {
        var result = SettingsSnapshot.Validate(Style, null, null, EmptySlots());

        Assert.False(result.IsValid);
        Assert.Equal(
            [
                new SettingsError(SettingsErrorCode.MissingShortcut, SettingsCommand.Draw),
                new SettingsError(SettingsErrorCode.MissingShortcut, SettingsCommand.Clear),
            ],
            result.Errors);
    }

    [Fact]
    public void Validate_RequiresShortcutForEveryFilledSoundSlot()
    {
        var slots = EmptySlots();
        slots[2] = new ActionSlot(3, new SoundAction(new SoundReference("a.wav", "a.wav"), null));

        var result = SettingsSnapshot.Validate(Style, Draw, Clear, slots);

        Assert.Equal([new SettingsError(SettingsErrorCode.MissingShortcut, SettingsCommand.ForSlot(3))], result.Errors);
        Assert.Equal("Slot 3 requires a shortcut.", result.Errors[0].ToString());
    }

    [Fact]
    public void Validate_RejectsDuplicateActiveShortcutsAndNamesBothOwners()
    {
        var slots = EmptySlots();
        slots[0] = Sound(1, "Ctrl+Alt+Z");
        slots[1] = Sound(2, "Ctrl+Alt+2");
        slots[4] = Sound(5, "Alt+Ctrl+2");

        var result = SettingsSnapshot.Validate(Style, Draw, Draw, slots);

        Assert.Equal(
            [
                new SettingsError(SettingsErrorCode.DuplicateShortcut, SettingsCommand.Clear, SettingsCommand.Draw),
                new SettingsError(SettingsErrorCode.DuplicateShortcut, SettingsCommand.ForSlot(1), SettingsCommand.Draw),
                new SettingsError(SettingsErrorCode.DuplicateShortcut, SettingsCommand.ForSlot(5), SettingsCommand.ForSlot(2)),
            ],
            result.Errors);
        Assert.Equal("Slot 5 uses the same shortcut as Slot 2.", result.Errors[2].ToString());
    }

    [Fact]
    public void Validate_RejectsDefaultWidth()
    {
        var style = new DrawingStyle(new HexColor(1, 2, 3), default);

        var result = SettingsSnapshot.Validate(style, Draw, Clear, EmptySlots());

        Assert.False(result.IsValid);
        Assert.Equal([new SettingsError(SettingsErrorCode.InvalidWidth)], result.Errors);
    }

    [Fact]
    public void Validate_RejectsFilledSlotWithoutSound()
    {
        var slots = EmptySlots();
        var filled = new SoundAction(new SoundReference("a.wav", "a.wav"), Keys("Ctrl+Alt+1"));
        slots[0] = new ActionSlot(1, filled with { Sound = null! });

        var result = SettingsSnapshot.Validate(Style, Draw, Clear, slots);

        Assert.False(result.IsValid);
        Assert.Equal([new SettingsError(SettingsErrorCode.MissingSound, SettingsCommand.ForSlot(1))], result.Errors);
        Assert.Equal("Slot 1 requires a sound.", result.Errors[0].ToString());
    }

    [Fact]
    public void SoundAction_RejectsNullSound()
    {
        Assert.Throws<ArgumentNullException>(() => new SoundAction(null!, Keys("Ctrl+Alt+1")));
    }

    [Fact]
    public void Validate_AllowsSeveralSlotsToShareOneSound()
    {
        var slots = EmptySlots();
        slots[0] = Sound(1, "Ctrl+Alt+1", "shared.mp3");
        slots[1] = Sound(2, "Ctrl+Alt+2", "shared.mp3");

        Assert.True(SettingsSnapshot.Validate(Style, Draw, Clear, slots).IsValid);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 8 })]
    [InlineData(new[] { 2, 1, 3, 4, 5, 6, 7, 8 })]
    [InlineData(new[] { 1, 1, 3, 4, 5, 6, 7, 8 })]
    public void Validate_RequiresExactlySlotsOneToEightInOrder(int[] numbers)
    {
        var result = SettingsSnapshot.Validate(Style, Draw, Clear, numbers.Select(ActionSlot.Empty));

        Assert.Equal([new SettingsError(SettingsErrorCode.SlotLayout)], result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(9)]
    public void ActionSlot_RejectsNumbersOutsideOneToEight(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ActionSlot.Empty(number));
    }

    [Theory]
    [InlineData("#FF4500", 0xFF, 0x45, 0x00)]
    [InlineData("#00ff7f", 0x00, 0xFF, 0x7F)]
    public void HexColor_TryParse_AcceptsSixDigitHex(string text, byte red, byte green, byte blue)
    {
        Assert.True(HexColor.TryParse(text, out var color));
        Assert.Equal(new HexColor(red, green, blue), color);
        Assert.Equal(text.ToUpperInvariant(), color.Value.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("FF4500")]
    [InlineData("#FF450")]
    [InlineData("#FF45000")]
    [InlineData("#FF4500FF")]
    [InlineData("#GG4500")]
    [InlineData("#+F4500")]
    [InlineData("# F4500")]
    [InlineData("orangered")]
    public void HexColor_TryParse_RejectsInvalidColors(string? text)
    {
        Assert.False(HexColor.TryParse(text, out _));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(20)]
    public void StrokeWidth_AcceptsOneToTwenty(int pixels)
    {
        Assert.Equal(pixels, new StrokeWidth(pixels).Pixels);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(21)]
    [InlineData(-4)]
    public void StrokeWidth_RejectsOutsideOneToTwenty(int pixels)
    {
        Assert.False(StrokeWidth.IsValid(pixels));
        Assert.Throws<ArgumentOutOfRangeException>(() => new StrokeWidth(pixels));
    }

    [Theory]
    [InlineData(@"..\a.wav")]
    [InlineData("C:a.wav")]
    [InlineData("sub/a.wav")]
    [InlineData("..")]
    [InlineData(" ")]
    public void SoundReference_RejectsPathsAndBlankNames(string libraryFileName)
    {
        Assert.ThrowsAny<ArgumentException>(() => new SoundReference(libraryFileName, "a.wav"));
    }

    private static Shortcut Keys(string text) =>
        Shortcut.TryParse(text, out var shortcut, out _) ? shortcut : throw new ArgumentException(text);

    private static ActionSlot Sound(int number, string shortcut, string file = "a.wav") =>
        new(number, new SoundAction(new SoundReference(file, file), Keys(shortcut)));

    private static ActionSlot[] EmptySlots() => Enumerable.Range(1, 8).Select(ActionSlot.Empty).ToArray();
}
