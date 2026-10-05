using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Presentation.Settings;
using DrawEM.Tests.Infrastructure.Drawing;
using DrawEM.Tests.Application.Settings;

namespace DrawEM.Tests.Presentation.Settings;

public sealed class SettingsViewModelTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private const ShortcutModifiers CtrlShift = ShortcutModifiers.Control | ShortcutModifiers.Shift;
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private readonly FakeLibrary library = new();
    private readonly FakeSettingsSave saver = new();
    private readonly FakeSampler sampler = new();
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
        Assert.Equal("Applause.wav", model.Slots[0].FileName);
        Assert.Equal("Ctrl+Alt+1", model.Slots[0].Shortcut.Text);
        Assert.Equal("No sound selected", model.Slots[1].FileName);
        Assert.Equal("No shortcut", model.Slots[1].Shortcut.Text);
        Assert.False(model.Slots[1].Shortcut.IsEnabled);
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
        Assert.False(model.Slots[3].HasSound);
    }

    [Fact]
    public void ChooseFile_CopyFailure_IsShownOnTheSlot()
    {
        var model = Model();
        picker.Next = @"C:\sounds\locked.wav";
        library.Failure = "'locked.wav' could not be copied: access denied";

        model.Slots[2].ChooseFile();

        Assert.Equal(library.Failure, model.Slots[2].Message);
        Assert.False(model.Slots[2].HasSound);
        Assert.True(model.CanSave);
    }

    [Fact]
    public void RemoveSound_ClearsTheSlot()
    {
        var model = Model();

        model.Slots[0].RemoveSound();

        Assert.False(model.Slots[0].HasSound);
        Assert.Equal("No shortcut", model.Slots[0].Shortcut.Text);
        Assert.True(model.CanSave);
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
    public void RestoreDefaults_ChangesOnlyTheDraft()
    {
        var model = Model();
        model.HexText = "#000000";
        model.WidthText = "abc";

        model.RestoreDefaults();

        Assert.Equal("#FF4500", model.HexText);
        Assert.Equal("4", model.WidthText);
        Assert.All(model.Slots, slot => Assert.False(slot.HasSound));
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
            played.Add);
        var model = new SettingsViewModel(
            new SettingsEditor(() => active, library, saver, sampler, _ => { }), hook, picker);

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
        new(new SettingsEditor(() => active, library, saver, sampler, _ => { }), capture, picker);
}
