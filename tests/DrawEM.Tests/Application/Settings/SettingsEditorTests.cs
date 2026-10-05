using System.IO;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Application.Settings;

public sealed class SettingsEditorTests : IDisposable
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private const ShortcutModifiers CtrlShift = ShortcutModifiers.Control | ShortcutModifiers.Shift;
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private readonly string root = Path.Combine(Path.GetTempPath(), "drawEM-editor-" + Guid.NewGuid().ToString("N"));
    private readonly ManagedSoundLibrary library;
    private readonly FakeSettingsSave saver = new();
    private readonly FakeSampler sampler = new();
    private readonly List<SoundCleanupFailure> cleanupFailures = [];
    private SettingsSnapshot active = TestSettings.WithSounds((1, Applause));

    public SettingsEditorTests()
    {
        Directory.CreateDirectory(root);
        library = new ManagedSoundLibrary(Path.Combine(root, "library"));
    }

    public void Dispose() => Directory.Delete(root, recursive: true);

    [Fact]
    public void NewDraft_CopiesTheActiveSnapshot()
    {
        var editor = Editor();

        Assert.Equal(active.Style.Color, editor.Color);
        Assert.Equal(active.Style.Width.Pixels, editor.Width);
        Assert.Equal(active.DrawShortcut, editor.DrawShortcut);
        Assert.Equal(active.ClearShortcut, editor.ClearShortcut);
        Assert.Equal(active.Slots, editor.Slots);
        Assert.All(editor.Slots.Skip(1), slot => Assert.True(slot.IsEmpty));
        Assert.Empty(editor.Validate());
    }

    [Fact]
    public void DraftEdits_DoNotChangeTheActiveSnapshotOrSave()
    {
        var before = active;
        var editor = Editor();

        editor.SetColor(new HexColor(0, 0, 0xFF));
        editor.SetWidth(12);
        editor.SetShortcut(SettingsCommand.Draw, Shortcut.Create(CtrlShift, ShortcutKey.Letter('D')));
        editor.SelectSound(2, SourceFile("drumroll.mp3", [1, 2, 3]));
        editor.RemoveSound(1);
        editor.RestoreDefaults();
        editor.Sample(1, _ => { });

        Assert.Same(before, active);
        Assert.Empty(saver.Saved);
    }

    [Fact]
    public void RestoreDefaults_ResetsOnlyTheDraft()
    {
        var editor = Editor();
        editor.SetColor(new HexColor(1, 2, 3));
        editor.SetWidth(20);
        editor.SetShortcut(SettingsCommand.Clear, Shortcut.Create(CtrlShift, ShortcutKey.Letter('C')));

        editor.RestoreDefaults();

        Assert.Equal(SettingsSnapshot.Default.Style.Color, editor.Color);
        Assert.Equal(4, editor.Width);
        Assert.Equal(SettingsSnapshot.Default.DrawShortcut, editor.DrawShortcut);
        Assert.Equal(SettingsSnapshot.Default.ClearShortcut, editor.ClearShortcut);
        Assert.All(editor.Slots, slot => Assert.Equal(ActionSlot.Empty(slot.Number), slot));
        Assert.Equal(TestSettings.WithSounds((1, Applause)), active);
        Assert.Empty(saver.Saved);
    }

    [Fact]
    public void SelectSound_InEmptySlot_ProposesTheSlotsDefaultShortcut()
    {
        var editor = Editor();

        editor.SelectSound(5, SourceFile("horn.wav", [5]));

        Assert.Equal("horn.wav", SoundIn(editor, 5)!.DisplayName);
        Assert.Equal(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(5)), editor.Slots[4].Action?.Shortcut);
        Assert.Empty(editor.Validate());
    }

    [Fact]
    public void SelectSound_InFilledSlot_KeepsItsShortcut()
    {
        var editor = Editor();
        var custom = Shortcut.Create(CtrlShift, ShortcutKey.Function(7));
        editor.SetShortcut(SettingsCommand.ForSlot(1), custom);

        editor.SelectSound(1, SourceFile("horn.wav", [5]));

        Assert.Equal("horn.wav", SoundIn(editor, 1)!.DisplayName);
        Assert.Equal(custom, editor.Slots[0].Action?.Shortcut);
    }

    [Fact]
    public void ProposedShortcut_CanBeReplacedWithAValidAlternativeAndSaved()
    {
        var editor = Editor();
        editor.SelectSound(3, SourceFile("horn.wav", [5]));
        var alternative = Shortcut.Create(ShortcutModifiers.Alt | ShortcutModifiers.Shift, ShortcutKey.Letter('H'));

        editor.SetShortcut(SettingsCommand.ForSlot(3), alternative);
        var result = editor.Save();

        var saved = Assert.IsType<SettingsSaveResult.Saved>(result).Snapshot;
        Assert.Equal(alternative, saved.Slots[2].Action!.Shortcut);
        Assert.Equal("horn.wav", Assert.IsType<SoundAction>(saved.Slots[2].Action).Sound.DisplayName);
    }

    [Fact]
    public void ProposedShortcut_ThatConflicts_IsReportedAndNotReassigned()
    {
        active = SettingsSnapshot.Validate(
            SettingsSnapshot.Default.Style,
            Shortcut.Create(CtrlAlt, ShortcutKey.Digit(2)),
            SettingsSnapshot.Default.ClearShortcut,
            SettingsSnapshot.Default.Slots).Snapshot!;
        var editor = Editor();

        editor.SelectSound(2, SourceFile("horn.wav", [5]));

        var error = Assert.Single(editor.Validate());
        Assert.Equal(
            new SettingsError(SettingsErrorCode.DuplicateShortcut, SettingsCommand.ForSlot(2), SettingsCommand.Draw),
            error);
        Assert.Equal(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(2)), editor.DrawShortcut);
        Assert.IsType<SettingsSaveResult.Invalid>(editor.Save());
    }

    [Fact]
    public void SelectSound_ThatFailsToCopy_LeavesTheSlotUnchanged()
    {
        var editor = Editor();

        Assert.Throws<SoundImportException>(() => editor.SelectSound(2, Path.Combine(root, "notes.txt")));
        Assert.Throws<SoundImportException>(() => editor.SelectSound(2, Path.Combine(root, "missing.wav")));

        Assert.True(editor.Slots[1].IsEmpty);
        Assert.Null(editor.Slots[1].Action?.Shortcut);
    }

    [Fact]
    public void SameFileInTwoSlots_SharesOneManagedCopy()
    {
        var editor = Editor();
        var source = SourceFile("horn.wav", [7, 7, 7]);

        editor.SelectSound(2, source);
        editor.SelectSound(3, source);

        Assert.Equal(SoundIn(editor, 2), SoundIn(editor, 3));
        Assert.Single(Directory.GetFiles(library.LibraryDirectory));
        Assert.True(File.Exists(source));
    }

    [Fact]
    public void RemoveSound_EmptiesTheSlotAndItsShortcut()
    {
        var editor = Editor();

        editor.RemoveSound(1);

        Assert.Equal(ActionSlot.Empty(1), editor.Slots[0]);
        Assert.Throws<InvalidOperationException>(() =>
            editor.SetShortcut(SettingsCommand.ForSlot(1), Shortcut.Create(CtrlAlt, ShortcutKey.Digit(1))));
    }

    [Fact]
    public void Validate_ReportsInvalidWidth()
    {
        var editor = Editor();

        editor.SetWidth(21);

        Assert.Equal([new SettingsError(SettingsErrorCode.InvalidWidth)], editor.Validate());
    }

    [Fact]
    public void Save_PassesTheWholeDraft()
    {
        var editor = Editor();
        editor.SetColor(new HexColor(0, 0x80, 0));
        editor.SetWidth(9);
        editor.SelectSound(8, SourceFile("horn.wav", [5]));

        var saved = Assert.IsType<SettingsSaveResult.Saved>(editor.Save()).Snapshot;

        Assert.Equal(new DrawingStyle(new HexColor(0, 0x80, 0), new StrokeWidth(9)), saved.Style);
        Assert.Equal(Applause, Assert.IsType<SoundAction>(saved.Slots[0].Action).Sound);
        Assert.Equal(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(8)), saved.Slots[7].Action!.Shortcut);
        Assert.Equal([saved], saver.Saved);
    }

    [Fact]
    public void Cancel_RemovesDraftImportsButKeepsCopiesTheActiveSettingsReference()
    {
        var kept = Editor();
        kept.SelectSound(2, SourceFile("kept.wav", [1]));
        var keptSound = SoundIn(kept, 2)!;
        active = TestSettings.WithSounds((1, Applause), (2, keptSound));
        Assert.IsType<SettingsSaveResult.Saved>(kept.Save());
        var editor = Editor();
        editor.SelectSound(3, SourceFile("kept-again.wav", [1]));
        editor.SelectSound(4, SourceFile("discarded.wav", [2]));
        var discarded = SoundIn(editor, 4)!;

        editor.Cancel();

        Assert.True(File.Exists(library.PathFor(keptSound.LibraryFileName)));
        Assert.False(File.Exists(library.PathFor(discarded.LibraryFileName)));
        Assert.Empty(cleanupFailures);
    }

    [Fact]
    public void Cancel_StopsASampleBeforeRemovingTheDraftCopy()
    {
        var editor = Editor();
        editor.SelectSound(2, SourceFile("horn.wav", [1]));
        var sound = SoundIn(editor, 2)!;
        editor.Sample(2, _ => { });

        editor.Cancel();

        Assert.Equal(1, sampler.Stops);
        Assert.False(File.Exists(library.PathFor(sound.LibraryFileName)));
    }

    [Fact]
    public void Cancel_WhenTheSampleStopIsUnconfirmed_LeavesDraftCopiesForStartupCleanup()
    {
        sampler.StopConfirmed = false;
        var editor = Editor();
        editor.SelectSound(2, SourceFile("horn.wav", [1]));
        var sound = SoundIn(editor, 2)!;

        editor.Cancel();

        Assert.True(File.Exists(library.PathFor(sound.LibraryFileName)));
        Assert.Empty(cleanupFailures);
    }

    [Fact]
    public void Cancel_AfterSave_RemovesNothing()
    {
        var editor = Editor();
        editor.SelectSound(2, SourceFile("horn.wav", [1]));
        var sound = SoundIn(editor, 2)!;
        Assert.IsType<SettingsSaveResult.Saved>(editor.Save());

        editor.Cancel();

        Assert.True(File.Exists(library.PathFor(sound.LibraryFileName)));
    }

    [Fact]
    public void Save_EndsTheDraftsSamplesWithoutAnotherStop()
    {
        var editor = Editor();
        editor.Sample(1, _ => { });

        Assert.IsType<SettingsSaveResult.Saved>(editor.Save());
        editor.Cancel();

        Assert.Equal(1, sampler.Forgets);
        Assert.Equal(0, sampler.Stops);
    }

    [Fact]
    public void FailedSave_KeepsTheDraftsSamples()
    {
        saver.Result = new SettingsSaveResult.NotSaved("disk full");
        var editor = Editor();
        editor.Sample(1, _ => { });

        editor.Save();

        Assert.Equal(0, sampler.Forgets);
    }

    [Fact]
    public void FailedSave_KeepsTheDraftOpen_SoCancelStillDiscardsItsImports()
    {
        saver.Result = new SettingsSaveResult.NotSaved("disk full");
        var editor = Editor();
        editor.SelectSound(2, SourceFile("horn.wav", [1]));
        var sound = SoundIn(editor, 2)!;

        Assert.Equal(new SettingsSaveResult.NotSaved("disk full"), editor.Save());
        Assert.Equal("horn.wav", SoundIn(editor, 2)!.DisplayName);
        editor.Cancel();

        Assert.False(File.Exists(library.PathFor(sound.LibraryFileName)));
    }

    [Fact]
    public void Reopen_LoadsTheSettingsActiveAtThatTime()
    {
        var first = Editor();
        first.SetWidth(15);
        first.Cancel();
        Assert.Equal(4, Editor().Width);

        var second = Editor();
        second.SetWidth(15);
        active = Assert.IsType<SettingsSaveResult.Saved>(second.Save()).Snapshot;

        Assert.Equal(15, Editor().Width);
    }

    [Fact]
    public void Sample_PlaysTheDraftSelectedSound()
    {
        var editor = Editor();
        editor.SelectSound(1, SourceFile("horn.wav", [9]));
        var draftSound = SoundIn(editor, 1)!;
        var failures = new List<string>();

        editor.Sample(1, failures.Add);
        editor.Sample(2, failures.Add);

        var (sound, report) = Assert.Single(sampler.Samples);
        Assert.Equal(draftSound, sound);
        report("broken");
        Assert.Equal(["broken"], failures);
        Assert.Equal(Applause, Assert.IsType<SoundAction>(active.Slots[0].Action).Sound);
    }

    private SettingsEditor Editor() =>
        new(() => active, library, saver, sampler, cleanupFailures.Add);

    private static SoundReference? SoundIn(SettingsEditor editor, int slot) =>
        (editor.Slots[slot - 1].Action as SoundAction)?.Sound;

    private string SourceFile(string name, byte[] content)
    {
        var path = Path.Combine(root, name);
        File.WriteAllBytes(path, content);
        return path;
    }
}
