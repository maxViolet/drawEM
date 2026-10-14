using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure;
using DrawEM.App.Presentation.Settings;
using DrawEM.Tests.Application.Settings;
using DrawEM.Tests.Infrastructure.Drawing;

namespace DrawEM.Tests.Presentation.Settings;

public sealed class SettingsViewModelTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private const ShortcutModifiers CtrlShift = ShortcutModifiers.Control | ShortcutModifiers.Shift;
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private static readonly EffectId Confetti = new("confetti");
    private static readonly EffectId FocusRing = new("focus-ring");
    private readonly FakeLibrary library = new();
    private readonly FakeSettingsSave saver = new();
    private readonly FakeSampler sampler = new();
    private readonly FakeEffectSampler effectSampler = new();
    private readonly FakeCapture capture = new();
    private readonly FakePicker picker = new();
    private SettingsSnapshot active = TestSettings.WithSounds((1, Applause));

    [Fact]
    public void Opens_ShowingTheActiveSettings()
    {
        var model = Model();

        Assert.Equal("#FF4500", model.HexText);
        Assert.Equal("4", model.WidthText);
        Assert.Equal("Ctrl+Alt+Z", model.Draw.Text);
        Assert.Equal("Ctrl+Alt+X", model.Clear.Text);
        Assert.Equal(8, model.Slots.Count);
        Assert.Equal(["01", "02", "03", "04", "05", "06", "07", "08"], model.Slots.Select(slot => slot.Label));
        Assert.Equal(SlotKind.Sound, model.Slots[0].Kind);
        Assert.Equal("Applause.wav", model.Slots[0].FileName);
        Assert.Equal("Ctrl+Alt+1", model.Slots[0].Shortcut.Text);
        Assert.True(model.Slots[0].HasSound);
        Assert.Equal("No sound selected", model.Slots[1].FileName);
        Assert.Equal("No shortcut", model.Slots[1].Shortcut.Text);
        Assert.False(model.Slots[1].Shortcut.IsEnabled);
        Assert.False(model.Slots[1].HasAction);
        Assert.False(model.Slots[1].HasSound);
        Assert.True(model.CanSave);
    }

    [Theory]
    [InlineData("#00ff00", "#00FF00", true)]
    [InlineData("00FF00", "#FF4500", false)]
    [InlineData("#00FF0", "#FF4500", false)]
    [InlineData("#GG0000", "#FF4500", false)]
    public void HexText_UpdatesTheColorOnlyWhenValid(string text, string color, bool valid)
    {
        var model = Model();

        model.HexText = text;

        Assert.Equal(color, model.Color);
        Assert.Equal(valid, model.HexError is null);
        Assert.Equal(valid, model.CanSave);
    }

    [Fact]
    public void ColorPicker_SetsHexText()
    {
        var model = Model();

        model.SetColor(new HexColor(0x12, 0xAB, 0xEF));

        Assert.Equal("#12ABEF", model.HexText);
        Assert.Equal("#12ABEF", model.Color);
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("20", true)]
    [InlineData("0", false)]
    [InlineData("21", false)]
    [InlineData("4.5", false)]
    [InlineData("", false)]
    [InlineData("-3", false)]
    public void WidthText_OutsideOneToTwenty_BlocksSave(string text, bool valid)
    {
        var model = Model();

        model.WidthText = text;

        Assert.Equal(valid, model.WidthError is null);
        Assert.Equal(valid, model.CanSave);
    }

    [Theory]
    [InlineData("4", 1, "5")]
    [InlineData("4", -1, "3")]
    [InlineData("20", 1, "20")]
    [InlineData("1", -1, "1")]
    [InlineData("25", -1, "20")]
    [InlineData("0", 1, "1")]
    [InlineData("abc", 1, "1")]
    [InlineData("", -1, "1")]
    public void StepWidth_ChangesByOnePixel_WithinOneToTwenty(string text, int step, string stepped)
    {
        var model = Model();
        model.WidthText = text;

        model.StepWidth(step);

        Assert.Equal(stepped, model.WidthText);
        Assert.Null(model.WidthError);
        Assert.True(model.CanSave);
    }

    [Theory]
    [InlineData("1", true, false)]
    [InlineData("4", true, true)]
    [InlineData("20", false, true)]
    [InlineData("abc", true, true)]
    [InlineData("", true, true)]
    [InlineData("25", true, true)]
    [InlineData("0", true, true)]
    public void WidthSteppers_AreDisabledAtTheLimits(string text, bool canIncrease, bool canDecrease)
    {
        var model = Model();

        model.WidthText = text;

        Assert.Equal(canIncrease, model.CanIncreaseWidth);
        Assert.Equal(canDecrease, model.CanDecreaseWidth);
    }

    [Fact]
    public void FocusedCaptureField_RecordsAChordIntoTheDraftOnly()
    {
        var model = Model();

        model.Draw.BeginCapture();
        Assert.True(model.Draw.IsCapturing);
        Assert.Equal(ShortcutFieldViewModel.CapturePrompt, model.Draw.Text);
        capture.Report(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlShift, ShortcutKey.Letter('D'))));

        Assert.False(model.Draw.IsCapturing);
        Assert.Equal("Ctrl+Shift+D", model.Draw.Text);
        Assert.Equal(SettingsSnapshot.Default.DrawShortcut, active.DrawShortcut);
        Assert.Empty(saver.Saved);
    }

    [Theory]
    [InlineData(ShortcutCaptureRejection.TooFewModifiers, SettingsViewModel.TooFewModifiersMessage)]
    [InlineData(ShortcutCaptureRejection.RightAlt, SettingsViewModel.RightAltMessage)]
    public void RejectedChord_ShowsWhyAndKeepsCapturing(ShortcutCaptureRejection reason, string message)
    {
        var model = Model();
        model.Clear.BeginCapture();

        capture.Report(new ShortcutCaptureResult.Rejected(reason));

        Assert.Equal(message, model.Clear.Error);
        Assert.True(model.Clear.IsCapturing);
        Assert.Equal(SettingsSnapshot.Default.ClearShortcut, model.Clear.Shortcut);
        capture.Report(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlAlt, ShortcutKey.Letter('C'))));
        Assert.Null(model.Clear.Error);
        Assert.Equal("Ctrl+Alt+C", model.Clear.Text);
    }

    [Fact]
    public void EscapeOrFocusLoss_EndsCaptureWithoutChangingTheDraft()
    {
        var model = Model();
        model.Draw.BeginCapture();
        capture.Report(new ShortcutCaptureResult.Cancelled());
        Assert.False(model.Draw.IsCapturing);

        model.Draw.BeginCapture();
        model.Draw.EndCapture();

        Assert.False(model.Draw.IsCapturing);
        Assert.Equal(1, capture.Ended);
        Assert.Equal("Ctrl+Alt+Z", model.Draw.Text);
    }

    [Fact]
    public void ResultForAnEarlierCapture_IsIgnored()
    {
        var model = Model();
        model.Draw.BeginCapture();
        var drawReport = capture.Current!;
        model.Clear.BeginCapture();

        drawReport(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlShift, ShortcutKey.Letter('Q'))));

        Assert.Equal("Ctrl+Alt+Z", model.Draw.Text);
        Assert.True(model.Clear.IsCapturing);
        Assert.False(model.Draw.IsCapturing);
    }

    [Fact]
    public void DuplicateShortcut_IsShownOnBothFieldsAndBlocksSave()
    {
        var model = Model();
        model.Clear.BeginCapture();

        capture.Report(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(1))));

        Assert.Equal("Same shortcut as Slot 1.", model.Clear.Error);
        Assert.Equal("Same shortcut as Clear.", model.Slots[0].Shortcut.Error);
        Assert.Contains("Slot 1 uses the same shortcut as Clear.", model.Errors);
        Assert.Equal("Ctrl+Alt+Z", model.Draw.Text);
        Assert.False(model.CanSave);
        Assert.False(model.Save());
        Assert.Empty(saver.Saved);
    }

    [Fact]
    public void ChooseFile_FillsAnEmptySlotAndProposesItsShortcut()
    {
        var model = Model();
        picker.Next = @"C:\sounds\Horn.wav";

        model.Slots[3].ChooseFile();

        Assert.Equal(["C:\\sounds\\Horn.wav"], library.Imported);
        Assert.Equal("Horn.wav", model.Slots[3].FileName);
        Assert.Equal("Ctrl+Alt+4", model.Slots[3].Shortcut.Text);
        Assert.True(model.Slots[3].Shortcut.IsEnabled);
        Assert.True(model.CanSave);
    }

    [Fact]
    public void ChooseFile_Cancelled_ChangesNothing()
    {
        var model = Model();

        model.Slots[3].ChooseFile();

        Assert.Empty(library.Imported);
        Assert.False(model.Slots[3].HasAction);
    }

    [Fact]
    public void ChooseFile_CopyFailure_IsShownOnTheSlot()
    {
        var model = Model();
        picker.Next = @"C:\sounds\locked.wav";
        library.Failure = "'locked.wav' could not be copied: access denied";

        model.Slots[2].ChooseFile();

        Assert.Equal(library.Failure, model.Slots[2].Message);
        Assert.False(model.Slots[2].HasAction);
        Assert.True(model.CanSave);
    }

    [Fact]
    public void ClearSlot_ClearsASoundSlot()
    {
        var model = Model();

        model.Slots[0].ClearSlot();

        Assert.False(model.Slots[0].HasAction);
        Assert.Equal(SlotKind.Sound, model.Slots[0].Kind);
        Assert.Equal("No shortcut", model.Slots[0].Shortcut.Text);
        Assert.True(model.CanSave);
    }

    [Fact]
    public void EffectOptions_ListEachBuiltInEffectUnderItsPlacement()
    {
        Assert.Equal(
            [("Monitor", "Confetti", Confetti), ("Cursor", "Focus ring", FocusRing)],
            EffectOption.All.Select(option => (option.Group, option.Name, option.Id)));
    }

    [Fact]
    public void Opens_ShowingAnEffectSlot()
    {
        active = TestSettings.WithEffects((2, FocusRing));
        var model = Model();

        Assert.Equal(SlotKind.Effect, model.Slots[1].Kind);
        Assert.True(model.Slots[1].IsEffect);
        Assert.False(model.Slots[1].IsSound);
        Assert.Equal(FocusRing, model.Slots[1].Effect?.Id);
        Assert.Equal("Ctrl+Alt+2", model.Slots[1].Shortcut.Text);
        Assert.True(model.Slots[1].HasAction);
        Assert.False(model.Slots[1].HasSound);
    }

    [Fact]
    public void EmptySlot_ChangedToEffect_ChangesTheDraftOnlyWhenAnEffectIsChosen_AndSavesIt()
    {
        var model = Model();

        model.Slots[4].ChangeKind(SlotKind.Effect);

        Assert.True(model.Slots[4].IsEffect);
        Assert.Null(model.Slots[4].Effect);
        Assert.False(model.Slots[4].HasAction);
        Assert.False(model.Slots[4].Shortcut.IsEnabled);
        Assert.Empty(picker.Picks);

        model.Slots[4].Effect = EffectOption.All.Single(option => option.Id == Confetti);

        Assert.Equal(Confetti, model.Slots[4].Effect?.Id);
        Assert.Equal("Ctrl+Alt+5", model.Slots[4].Shortcut.Text);
        Assert.True(active.Slots[4].IsEmpty);
        Assert.Empty(saver.Saved);
        Assert.True(model.Save());
        Assert.Equal(
            new EffectAction(Confetti, Shortcut.Create(CtrlAlt, ShortcutKey.Digit(5))),
            Assert.Single(saver.Saved).Slots[4].Action);
    }

    [Fact]
    public void SoundSlot_ChangedToEffect_ChoosesTheFirstEffectAndKeepsTheShortcut()
    {
        var model = Model();
        model.Slots[0].Shortcut.BeginCapture();
        capture.Report(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlShift, ShortcutKey.Letter('K'))));

        model.Slots[0].ChangeKind(SlotKind.Effect);

        Assert.Equal(SlotKind.Effect, model.Slots[0].Kind);
        Assert.Equal(EffectOption.All[0], model.Slots[0].Effect);
        Assert.Equal("Ctrl+Shift+K", model.Slots[0].Shortcut.Text);
        Assert.False(model.Slots[0].HasSound);
        Assert.Equal(TestSettings.WithSounds((1, Applause)), active);
        Assert.Empty(saver.Saved);
    }

    [Fact]
    public void ChangingTypeDuringShortcutCapture_EndsTheCapture()
    {
        var model = Model();
        model.Slots[0].Shortcut.BeginCapture();
        var staleReport = capture.Current!;

        model.Slots[0].ChangeKind(SlotKind.Effect);
        staleReport(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlShift, ShortcutKey.Letter('K'))));

        Assert.False(model.Slots[0].Shortcut.IsCapturing);
        Assert.Equal(1, capture.Ended);
        Assert.Equal("Ctrl+Alt+1", model.Slots[0].Shortcut.Text);
    }

    [Fact]
    public void EffectSlot_ChangedToSound_UsesThePickedFileAndKeepsTheShortcut()
    {
        active = TestSettings.WithEffects((3, Confetti));
        var model = Model();
        picker.Next = @"C:\sounds\Horn.wav";

        model.Slots[2].ChangeKind(SlotKind.Sound);

        Assert.Equal(SlotKind.Sound, model.Slots[2].Kind);
        Assert.Equal("Horn.wav", model.Slots[2].FileName);
        Assert.Equal("Ctrl+Alt+3", model.Slots[2].Shortcut.Text);
        Assert.True(model.Slots[2].HasSound);
    }

    [Fact]
    public void EffectSlot_ChangedToSound_WithThePickerCancelled_KeepsTheEffect()
    {
        active = TestSettings.WithEffects((3, Confetti));
        var model = Model();

        model.Slots[2].ChangeKind(SlotKind.Sound);

        Assert.Single(picker.Picks);
        Assert.Empty(library.Imported);
        Assert.Equal(SlotKind.Effect, model.Slots[2].Kind);
        Assert.Equal(Confetti, model.Slots[2].Effect?.Id);
    }

    [Fact]
    public void EffectSlot_ChangedToSound_WhenTheCopyFails_KeepsTheEffectAndShowsWhy()
    {
        active = TestSettings.WithEffects((3, Confetti));
        var model = Model();
        picker.Next = @"C:\sounds\locked.wav";
        library.Failure = "'locked.wav' could not be copied: access denied";

        model.Slots[2].ChangeKind(SlotKind.Sound);

        Assert.Equal(library.Failure, model.Slots[2].Message);
        Assert.Equal(SlotKind.Effect, model.Slots[2].Kind);
        Assert.Equal(Confetti, model.Slots[2].Effect?.Id);
        Assert.Equal("Ctrl+Alt+3", model.Slots[2].Shortcut.Text);
    }

    [Fact]
    public void EffectSlot_ChangedToAnotherEffect_KeepsTheShortcut()
    {
        active = TestSettings.WithEffects((3, Confetti));
        var model = Model();

        model.Slots[2].Effect = EffectOption.All.Single(option => option.Id == FocusRing);

        Assert.Equal(FocusRing, model.Slots[2].Effect?.Id);
        Assert.Equal("Ctrl+Alt+3", model.Slots[2].Shortcut.Text);
    }

    [Fact]
    public void EffectWithAConflictingProposedShortcut_IsShownOnTheSlotAndBlocksSave()
    {
        var model = Model();
        model.Draw.BeginCapture();
        capture.Report(new ShortcutCaptureResult.Captured(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(6))));
        model.Slots[5].ChangeKind(SlotKind.Effect);

        model.Slots[5].Effect = EffectOption.All[1];

        Assert.Equal("Same shortcut as Hold to draw.", model.Slots[5].Shortcut.Error);
        Assert.Equal("Same shortcut as Slot 6.", model.Draw.Error);
        Assert.False(model.CanSave);
        Assert.False(model.Save());
        Assert.Empty(saver.Saved);
    }

    [Fact]
    public void ClearSlot_ClearsAnEffectSlotAndKeepsShowingTheEffectList()
    {
        active = TestSettings.WithEffects((7, FocusRing));
        var model = Model();

        model.Slots[6].ClearSlot();

        Assert.False(model.Slots[6].HasAction);
        Assert.True(model.Slots[6].IsEffect);
        Assert.Null(model.Slots[6].Effect);
        Assert.Equal("No shortcut", model.Slots[6].Shortcut.Text);
        Assert.False(model.Slots[6].Shortcut.IsEnabled);
        Assert.True(model.CanSave);
    }

    [Fact]
    public void Close_AfterChangingASavedSoundToAnEffect_KeepsTheSavedSound()
    {
        var model = Model();
        model.Slots[0].ChangeKind(SlotKind.Effect);

        model.Close();

        Assert.Equal([TestSettings.WithSounds((1, Applause))], library.Discarded);
        Assert.Equal(Applause, Assert.IsType<SoundAction>(active.Slots[0].Action).Sound);
        Assert.Empty(saver.Saved);
    }

    [Fact]
    public void Sample_FailureIsShownOnItsSlot()
    {
        var model = Model();

        model.Slots[0].Sample();
        sampler.Samples[0].Report("file not found");

        Assert.Equal(Applause, sampler.Samples[0].Sound);
        Assert.Equal("Sample failed: file not found", model.Slots[0].Message);
    }

    [Fact]
    public void Sample_OnAnEffectSlot_StartsTheDraftEffect_AndShowsItsFailure()
    {
        active = TestSettings.WithEffects((2, Confetti));
        var model = Model();
        model.Slots[1].Effect = EffectOption.All.Single(option => option.Id == FocusRing);

        model.Slots[1].Sample();
        effectSampler.Samples[0].Report("cannot render");

        Assert.Equal(FocusRing, effectSampler.Samples[0].Effect);
        Assert.Empty(sampler.Samples);
        Assert.Equal("Sample failed: cannot render", model.Slots[1].Message);
        Assert.Equal(Confetti, Assert.IsType<EffectAction>(active.Slots[1].Action).Effect);
    }

    [Fact]
    public void Close_StopsTheEditorsEffectSample()
    {
        active = TestSettings.WithEffects((2, Confetti));
        var model = Model();
        model.Slots[1].Sample();

        model.Close();

        Assert.Equal(1, effectSampler.Stops);
    }

    [Fact]
    public void RestoreDefaults_ChangesOnlyTheDraft()
    {
        var model = Model();
        model.HexText = "#000000";
        model.WidthText = "abc";
        model.Slots[3].ChangeKind(SlotKind.Effect);

        model.RestoreDefaults();

        Assert.Equal("#FF4500", model.HexText);
        Assert.Equal("4", model.WidthText);
        Assert.All(model.Slots, slot => Assert.False(slot.HasAction));
        Assert.All(model.Slots, slot => Assert.Equal(SlotKind.Sound, slot.Kind));
        Assert.True(model.CanSave);
        Assert.Equal(TestSettings.WithSounds((1, Applause)), active);
        Assert.Empty(saver.Saved);
        Assert.True(model.Save());
        Assert.Equal(SettingsSnapshot.Default, Assert.Single(saver.Saved));
    }

    [Fact]
    public void FailedSave_ShowsTheReasonAndKeepsTheDraft()
    {
        saver.Result = new SettingsSaveResult.NotSaved("Access to the path is denied.");
        var model = Model();
        model.WidthText = "7";

        Assert.False(model.Save());

        Assert.Equal("Access to the path is denied.", model.Status);
        Assert.Equal("7", model.WidthText);
        Assert.Empty(library.Discarded);
    }

    [Fact]
    public void Close_WithoutSave_DiscardsTheDraft()
    {
        var model = Model();
        model.Draw.BeginCapture();

        model.Close();

        Assert.Equal([active], library.Discarded);
        Assert.False(model.Draw.IsCapturing);
        Assert.Equal(1, capture.Ended);
    }

    [Fact]
    public void Close_AfterSave_DiscardsNothing()
    {
        var model = Model();
        model.WidthText = "6";

        Assert.True(model.Save());
        model.Close();

        Assert.Equal(6, Assert.Single(saver.Saved).Style.Width.Pixels);
        Assert.Empty(library.Discarded);
    }

    [Fact]
    public void CaptureThroughTheGlobalHook_RecordsAnActiveBindingWithoutPlayingIt()
    {
        var keys = new FakeKeyboardHookSource();
        var queued = new Queue<Action>();
        var played = new List<PlaySoundCommand>();
        var states = new List<DrawingState>();
        var controller = new DrawingSessionController();
        controller.StateChanged += states.Add;
        var applause = new PlaySoundCommand(new SoundId(Applause.LibraryFileName), @"C:\library\applause.wav");
        var hook = new GlobalShortcutAdapter(
            keys,
            keys,
            controller,
            new DrawingModeInputGate(),
            new FakeCursorPositionSource(new ScreenPoint(0, 0)),
            queued.Enqueue,
            new FakeMonitorBoundsSource(new MonitorBounds(-100, -100, 100, 100)),
            ShortcutBindings.FromSnapshot(active, _ => applause),
            played.Add,
            new FakeEffectStartQueue());
        var model = new SettingsViewModel(
            new SettingsEditor(() => active, library, saver, sampler, effectSampler, _ => { }), hook, picker);

        model.Clear.BeginCapture();
        keys.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu);
        Assert.True(keys.PressKey(VirtualKeys.D1));
        keys.Release(VirtualKeys.D1, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
        while (queued.TryDequeue(out var action))
        {
            action();
        }

        Assert.Equal("Ctrl+Alt+1", model.Clear.Text);
        Assert.Equal("Same shortcut as Slot 1.", model.Clear.Error);
        Assert.Empty(played);
        Assert.Empty(states);
        Assert.False(model.CanSave);
    }

    private SettingsViewModel Model() =>
        new(new SettingsEditor(() => active, library, saver, sampler, effectSampler, _ => { }), capture, picker);
}
