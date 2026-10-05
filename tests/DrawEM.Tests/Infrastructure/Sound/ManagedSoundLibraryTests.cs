using System.Text;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public sealed class ManagedSoundLibraryTests : IDisposable
{
    private readonly string root = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));

    private string LibraryDirectory => Path.Combine(root, "sounds");

    private string SourceDirectory => Path.Combine(root, "source");

    public void Dispose()
    {
        if (!Directory.Exists(root))
        {
            return;
        }

        foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }

        Directory.Delete(root, recursive: true);
    }

    [Fact]
    public void DefaultDirectory_IsSoundsUnderCurrentUserDrawEMProfile()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.Equal(Path.Combine(localAppData, "drawEM", "sounds"), ManagedSoundLibrary.DefaultDirectory);
    }

    [Theory]
    [InlineData("applause.wav", ".wav")]
    [InlineData("Theme.MP3", ".mp3")]
    public void Import_CopiesIntoLibraryAndLeavesSourceUntouched(string fileName, string extension)
    {
        var source = WriteSource(fileName, "sound-bytes");
        var written = File.GetLastWriteTimeUtc(source);

        var sound = Library().Import(source);

        Assert.Equal(fileName, sound.DisplayName);
        Assert.EndsWith(extension, sound.LibraryFileName, StringComparison.Ordinal);
        Assert.Equal([sound.LibraryFileName], LibraryFiles());
        Assert.Equal(Bytes("sound-bytes"), File.ReadAllBytes(CopyPath(sound)));
        Assert.Equal(Bytes("sound-bytes"), File.ReadAllBytes(source));
        Assert.Equal(written, File.GetLastWriteTimeUtc(source));
    }

    [Fact]
    public void Import_SameContent_GivesSameStableReferenceAndOneCopy()
    {
        var first = Library().Import(WriteSource("a.wav", "same"));
        var second = new ManagedSoundLibrary(LibraryDirectory).Import(WriteSource("b.wav", "same"));

        Assert.Equal(first.LibraryFileName, second.LibraryFileName);
        Assert.Equal("b.wav", second.DisplayName);
        Assert.Equal([first.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void Import_DifferentContent_GivesDifferentCopies()
    {
        var library = Library();

        var first = library.Import(WriteSource("a.wav", "one"));
        var second = library.Import(WriteSource("b.wav", "two"));

        Assert.NotEqual(first.LibraryFileName, second.LibraryFileName);
        Assert.Equal(2, LibraryFiles().Length);
    }

    [Fact]
    public void Import_ReusesCopyWhilePlaybackHoldsIt()
    {
        var library = Library();
        var sound = library.Import(WriteSource("a.wav", "same"));
        using var playing = new FileStream(CopyPath(sound), FileMode.Open, FileAccess.Read, FileShare.Read);

        var again = library.Import(WriteSource("b.wav", "same"));

        Assert.Equal(sound.LibraryFileName, again.LibraryFileName);
        Assert.Equal([sound.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void Import_DamagedCopyOfSameLength_IsRewrittenFromSource()
    {
        var library = Library();
        var sound = library.Import(WriteSource("a.wav", "original"));
        File.WriteAllBytes(CopyPath(sound), Bytes("damaged!"));

        var again = library.Import(WriteSource("b.wav", "original"));

        Assert.Equal(sound.LibraryFileName, again.LibraryFileName);
        Assert.Equal(Bytes("original"), File.ReadAllBytes(CopyPath(sound)));
    }

    [Fact]
    public void Import_FileFromInsideLibrary_ThrowsAndCancelKeepsIt()
    {
        var library = Library();
        var sound = library.Import(WriteSource("a.wav", "data"));
        library.CommitSave(SettingsSnapshot.Default, Snapshot((1, sound)), removeUnreferencedCopies: true);

        Assert.Throws<SoundImportException>(() => library.Import(CopyPath(sound)));
        library.DiscardDraft(SettingsSnapshot.Default);

        Assert.True(File.Exists(CopyPath(sound)));
    }

    [Theory]
    [InlineData("notes.txt")]
    [InlineData("clip.ogg")]
    [InlineData("wav")]
    public void Import_UnsupportedType_ThrowsAndCopiesNothing(string fileName)
    {
        var source = WriteSource(fileName, "data");

        Assert.Throws<SoundImportException>(() => Library().Import(source));

        Assert.Empty(LibraryFiles());
        Assert.True(File.Exists(source));
    }

    [Fact]
    public void Import_MissingSource_ThrowsAndLeavesNoTemporaryFile()
    {
        var failure = Assert.Throws<SoundImportException>(
            () => Library().Import(Path.Combine(SourceDirectory, "missing.wav")));

        Assert.Contains("missing.wav", failure.Message);
        Assert.Empty(LibraryFiles());
    }

    [Fact]
    public void Import_EmptySource_ThrowsAndLeavesNoTemporaryFile()
    {
        var source = WriteSource("empty.mp3", "");

        Assert.Throws<SoundImportException>(() => Library().Import(source));

        Assert.Empty(LibraryFiles());
    }

    [Fact]
    public void Import_UnreadableSource_ThrowsAndLeavesSourceUntouched()
    {
        var source = WriteSource("locked.wav", "data");
        using (new FileStream(source, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            Assert.Throws<SoundImportException>(() => Library().Import(source));
        }

        Assert.Empty(LibraryFiles());
        Assert.Equal(Bytes("data"), File.ReadAllBytes(source));
    }

    [Fact]
    public void Import_WhenLibraryCannotBeWritten_Throws()
    {
        Directory.CreateDirectory(root);
        File.WriteAllText(LibraryDirectory, "a file where the library directory belongs");
        var source = WriteSource("a.wav", "data");

        Assert.Throws<SoundImportException>(() => Library().Import(source));

        Assert.True(File.Exists(source));
    }

    [Fact]
    public void DiscardDraft_RemovesUnreferencedImportsAndKeepsSavedCopies()
    {
        var library = Library();
        var saved = library.Import(WriteSource("saved.wav", "saved"));
        var savedSettings = Snapshot((1, saved), (2, saved));
        library.CommitSave(SettingsSnapshot.Default, savedSettings, removeUnreferencedCopies: true);
        var draftOnly = library.Import(WriteSource("new.wav", "new"));
        var reused = library.Import(WriteSource("copy-of-saved.wav", "saved"));

        var failures = library.DiscardDraft(savedSettings);

        Assert.Empty(failures);
        Assert.Equal(saved.LibraryFileName, reused.LibraryFileName);
        Assert.False(File.Exists(CopyPath(draftOnly)));
        Assert.Equal([saved.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void CommitSave_ReplacingOneSharedSlot_KeepsCopyStillReferenced()
    {
        var library = Library();
        var shared = library.Import(WriteSource("shared.wav", "shared"));
        var previous = Snapshot((1, shared), (2, shared));
        library.CommitSave(SettingsSnapshot.Default, previous, removeUnreferencedCopies: true);
        var replacement = library.Import(WriteSource("replacement.mp3", "replacement"));

        var failures = library.CommitSave(previous, Snapshot((1, replacement), (2, shared)), removeUnreferencedCopies: true);

        Assert.Empty(failures);
        Assert.Equal(
            new[] { shared.LibraryFileName, replacement.LibraryFileName }.Order(),
            LibraryFiles().Order());
    }

    [Fact]
    public void CommitSave_RemovesCopyAfterItsLastSavedReferenceDisappears()
    {
        var library = Library();
        var shared = library.Import(WriteSource("shared.wav", "shared"));
        var both = Snapshot((1, shared), (2, shared));
        library.CommitSave(SettingsSnapshot.Default, both, removeUnreferencedCopies: true);
        var one = Snapshot((2, shared));
        library.CommitSave(both, one, removeUnreferencedCopies: true);
        Assert.True(File.Exists(CopyPath(shared)));

        var failures = library.CommitSave(one, SettingsSnapshot.Default, removeUnreferencedCopies: true);

        Assert.Empty(failures);
        Assert.Empty(LibraryFiles());
    }

    [Fact]
    public void CommitSave_RemovesDraftImportsReplacedBeforeSave()
    {
        var library = Library();
        var abandoned = library.Import(WriteSource("first-choice.wav", "first"));
        var chosen = library.Import(WriteSource("second-choice.wav", "second"));

        library.CommitSave(SettingsSnapshot.Default, Snapshot((1, chosen)), removeUnreferencedCopies: true);

        Assert.False(File.Exists(CopyPath(abandoned)));
        Assert.Equal([chosen.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void CommitSave_ReportsCopyThatCannotBeRemovedAndContinues()
    {
        var library = Library();
        var locked = library.Import(WriteSource("locked.wav", "locked"));
        var free = library.Import(WriteSource("free.wav", "free"));
        var previous = Snapshot((1, locked), (2, free));
        library.CommitSave(SettingsSnapshot.Default, previous, removeUnreferencedCopies: true);

        IReadOnlyList<SoundCleanupFailure> failures;
        using (new FileStream(CopyPath(locked), FileMode.Open, FileAccess.Read, FileShare.None))
        {
            failures = library.CommitSave(previous, SettingsSnapshot.Default, removeUnreferencedCopies: true);
        }

        Assert.Equal(locked.LibraryFileName, Assert.Single(failures).LibraryFileName);
        Assert.Equal([locked.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void CommitSave_NeverDeletesFileWithUnmanagedName()
    {
        Directory.CreateDirectory(LibraryDirectory);
        var foreign = Path.Combine(LibraryDirectory, "keep.wav");
        File.WriteAllText(foreign, "not a managed copy");
        var previous = Snapshot((1, new SoundReference("keep.wav", "keep.wav")));

        var failures = Library().CommitSave(previous, SettingsSnapshot.Default, removeUnreferencedCopies: true);

        Assert.Empty(failures);
        Assert.True(File.Exists(foreign));
    }

    [Fact]
    public void CommitSaveWithoutRemoval_RemovesNothing_LaterCancelKeepsThatDraft_AndOrphanCleanupRemovesIt()
    {
        var library = Library();
        var abandoned = library.Import(WriteSource("abandoned.wav", "abandoned"));
        var kept = library.Import(WriteSource("kept.wav", "kept"));
        var saved = Snapshot((1, kept));

        Assert.Empty(library.CommitSave(SettingsSnapshot.Default, saved, removeUnreferencedCopies: false));
        var cancelFailures = library.DiscardDraft(saved);

        Assert.Empty(cancelFailures);
        Assert.True(File.Exists(CopyPath(abandoned)));
        Assert.True(File.Exists(CopyPath(kept)));

        var cleanupFailures = library.RemoveOrphans(saved);

        Assert.Empty(cleanupFailures);
        Assert.False(File.Exists(CopyPath(abandoned)));
        Assert.True(File.Exists(CopyPath(kept)));
    }

    [Fact]
    public void Startup_AfterForcedExit_RemovesDraftImportAndKeepsSavedCopies()
    {
        var store = new JsonSettingsStore(root);
        var before = Library();
        var saved = before.Import(WriteSource("saved.wav", "saved"));
        var savedSettings = Snapshot((1, saved), (4, saved));
        store.Save(savedSettings);
        before.CommitSave(SettingsSnapshot.Default, savedSettings, removeUnreferencedCopies: true);
        var draft = before.Import(WriteSource("draft.mp3", "draft"));
        var leftoverTemp = Path.Combine(LibraryDirectory, "import-" + Guid.NewGuid().ToString("N") + ".tmp");
        File.WriteAllText(leftoverTemp, "partial");
        var foreign = Path.Combine(LibraryDirectory, "readme.txt");
        File.WriteAllText(foreign, "not a managed copy");

        var startup = SettingsStartup.Load(store, new NoReporter());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());

        Assert.Empty(failures);
        Assert.False(File.Exists(CopyPath(draft)));
        Assert.False(File.Exists(leftoverTemp));
        Assert.True(File.Exists(foreign));
        Assert.True(File.Exists(CopyPath(saved)));
    }

    [Fact]
    public void Startup_WithUnreadableSettings_RemovesNothing()
    {
        var store = new JsonSettingsStore(root);
        var library = Library();
        var saved = library.Import(WriteSource("saved.wav", "saved"));
        var savedSettings = Snapshot((1, saved));
        store.Save(savedSettings);
        library.CommitSave(SettingsSnapshot.Default, savedSettings, removeUnreferencedCopies: true);
        var draft = library.Import(WriteSource("draft.wav", "draft"));
        File.WriteAllText(store.SettingsPath, "{ damaged");

        var startup = SettingsStartup.Load(store, new NoReporter());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());

        Assert.False(startup.SavedSettingsLoaded);
        Assert.Empty(failures);
        Assert.True(File.Exists(CopyPath(saved)));
        Assert.True(File.Exists(CopyPath(draft)));
    }

    [Fact]
    public void Startup_WithoutSavedSettings_RemovesNothing()
    {
        var draft = Library().Import(WriteSource("draft.wav", "draft"));

        var startup = SettingsStartup.Load(new JsonSettingsStore(root), new NoReporter());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());

        Assert.Empty(failures);
        Assert.True(File.Exists(CopyPath(draft)));
    }

    [Fact]
    public void RemoveOrphans_KeepsImportsOfTheOpenDraft()
    {
        var library = Library();
        var draft = library.Import(WriteSource("draft.wav", "draft"));

        library.RemoveOrphans(SettingsSnapshot.Default);

        Assert.True(File.Exists(CopyPath(draft)));
    }

    [Fact]
    public void RemoveOrphans_WithoutLibraryDirectory_DoesNothing()
    {
        Assert.Empty(Library().RemoveOrphans(SettingsSnapshot.Default));
        Assert.False(Directory.Exists(LibraryDirectory));
    }

    private ManagedSoundLibrary Library() => new(LibraryDirectory);

    private string WriteSource(string fileName, string content)
    {
        Directory.CreateDirectory(SourceDirectory);
        var path = Path.Combine(SourceDirectory, fileName);
        File.WriteAllBytes(path, Bytes(content));
        return path;
    }

    private string CopyPath(SoundReference sound) => Path.Combine(LibraryDirectory, sound.LibraryFileName);

    private string[] LibraryFiles() =>
        Directory.Exists(LibraryDirectory)
            ? Directory.GetFiles(LibraryDirectory).Select(path => Path.GetFileName(path)).ToArray()
            : [];

    private static byte[] Bytes(string content) => Encoding.UTF8.GetBytes(content);

    private static SettingsSnapshot Snapshot(params (int Slot, SoundReference Sound)[] sounds)
    {
        var slots = Enumerable.Range(1, ActionSlot.Count).Select(ActionSlot.Empty).ToArray();
        foreach (var (slot, sound) in sounds)
        {
            var shortcut = Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Alt, ShortcutKey.Digit(slot));
            slots[slot - 1] = new ActionSlot(slot, new SoundAction(sound, shortcut));
        }

        var defaults = SettingsSnapshot.Default;
        return SettingsSnapshot.Validate(defaults.Style, defaults.DrawShortcut, defaults.ClearShortcut, slots).Snapshot!;
    }

    private sealed class NoReporter : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy)
        {
        }
    }
}
