using System.Text;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Settings;

/// <summary>
/// Save, restart, and media cleanup through <see cref="SettingsSaveOperation"/> against the real settings
/// file and sound library. Sound and shortcuts are idle stand-ins whose stop is always confirmed.
/// </summary>
public sealed class RuntimeMediaTests : IDisposable
{
    private readonly string root = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));

    private string SettingsDirectory => Path.Combine(root, "profile");

    private string LibraryDirectory => Path.Combine(SettingsDirectory, "sounds");

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
    public void Save_WhenSettingsCannotBeSaved_KeepsPreviousActiveFileAndMedia()
    {
        var rig = new SaveRig(this);
        var saved = rig.Library.Import(WriteSource("saved.wav", "saved"));
        var previous = TestSettings.WithSounds((1, saved));
        rig.Save(previous);
        var replacement = rig.Library.Import(WriteSource("replacement.wav", "replacement"));
        var savedFile = File.ReadAllBytes(rig.Store.SettingsPath);
        File.SetAttributes(rig.Store.SettingsPath, FileAttributes.ReadOnly);

        var result = rig.Save(TestSettings.WithSounds((1, replacement)));

        Assert.IsType<SettingsSaveResult.NotSaved>(result);
        Assert.Equal(previous, rig.Settings.Current);
        Assert.Equal(savedFile, File.ReadAllBytes(rig.Store.SettingsPath));
        Assert.Equal(Bytes("saved"), File.ReadAllBytes(CopyPath(saved)));
        Assert.Equal(Bytes("replacement"), File.ReadAllBytes(CopyPath(replacement)));

        rig.Library.DiscardDraft(rig.Settings.Current);

        Assert.Equal([saved.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void Restart_AfterSave_ActivatesSavedSnapshotAndRemovesOnlyDraftLeftovers()
    {
        var rig = new SaveRig(this);
        var kept = rig.Library.Import(WriteSource("kept.wav", "kept"));
        var added = rig.Library.Import(WriteSource("added.mp3", "added"));
        rig.Save(TestSettings.WithSounds((1, kept), (2, added), (6, kept)));
        var leftover = rig.Library.Import(WriteSource("unsaved.wav", "unsaved"));

        var startup = SettingsStartup.Load(Store(), new NoSettingsFailures());
        var restarted = new ActiveSettings(startup.Active, ManagedPath);
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());

        Assert.True(startup.SavedSettingsLoaded);
        Assert.Equal(rig.Settings.Current, restarted.Current);
        Assert.Empty(failures);
        Assert.False(File.Exists(CopyPath(leftover)));
        Assert.Equal(new[] { kept.LibraryFileName, added.LibraryFileName }.Order(), LibraryFiles().Order());
        Assert.Equal(CopyPath(added), restarted.CommandFor(added).Path);
    }

    [Fact]
    public void Restart_WithSavedCopyDeletedOutsideApp_LoadsSnapshotAndCleansUpWithoutFailure()
    {
        var rig = new SaveRig(this);
        var missing = rig.Library.Import(WriteSource("missing.wav", "missing"));
        var present = rig.Library.Import(WriteSource("present.wav", "present"));
        var saved = TestSettings.WithSounds((1, missing), (2, present));
        rig.Save(saved);
        File.Delete(CopyPath(missing));

        var startup = SettingsStartup.Load(Store(), new NoSettingsFailures());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());
        var active = new ActiveSettings(startup.Active, ManagedPath);

        Assert.True(startup.SavedSettingsLoaded);
        Assert.Equal(saved, active.Current);
        Assert.Empty(failures);
        Assert.Equal([present.LibraryFileName], LibraryFiles());
        Assert.Equal(CopyPath(missing), active.CommandFor(missing).Path);
    }

    [Fact]
    public void Save_WhenRetiredCopyIsLocked_SucceedsActivatesAndReportsCopy()
    {
        var rig = new SaveRig(this);
        var locked = rig.Library.Import(WriteSource("locked.wav", "locked"));
        rig.Save(TestSettings.WithSounds((1, locked)));
        var replacement = rig.Library.Import(WriteSource("replacement.wav", "replacement"));
        var draft = TestSettings.WithSounds((1, replacement));

        SettingsSaveResult result;
        using (new FileStream(CopyPath(locked), FileMode.Open, FileAccess.Read, FileShare.None))
        {
            result = rig.Save(draft);
        }

        Assert.IsType<SettingsSaveResult.Saved>(result);
        Assert.Equal(locked.LibraryFileName, Assert.Single(rig.Notifications.CleanupFailures).LibraryFileName);
        Assert.Equal(draft, rig.Settings.Current);
        Assert.Equal(draft, Assert.IsType<SettingsLoadResult.Loaded>(rig.Store.Load()).Snapshot);
        Assert.True(File.Exists(CopyPath(replacement)));
    }

    private JsonSettingsStore Store() => new(SettingsDirectory);

    private ManagedSoundLibrary Library() => new(LibraryDirectory);

    private string ManagedPath(SoundReference sound) => CopyPath(sound);

    private string CopyPath(SoundReference sound) => Path.Combine(LibraryDirectory, sound.LibraryFileName);

    private string WriteSource(string fileName, string content)
    {
        Directory.CreateDirectory(SourceDirectory);
        var path = Path.Combine(SourceDirectory, fileName);
        File.WriteAllBytes(path, Bytes(content));
        return path;
    }

    private string[] LibraryFiles() =>
        Directory.Exists(LibraryDirectory)
            ? Directory.GetFiles(LibraryDirectory).Select(path => Path.GetFileName(path)).ToArray()
            : [];

    private static byte[] Bytes(string content) => Encoding.UTF8.GetBytes(content);

    /// <summary>A Save over the real store and library, starting from <see cref="SettingsSnapshot.Default"/>.</summary>
    private sealed class SaveRig
    {
        private readonly SettingsSaveOperation operation;

        public SaveRig(RuntimeMediaTests test)
        {
            Store = test.Store();
            Library = test.Library();
            Settings = new ActiveSettings(SettingsSnapshot.Default, test.ManagedPath);
            operation = new SettingsSaveOperation(
                Store, Library, Settings, new IdleSoundChannel(), new IdleShortcuts(), Notifications);
        }

        public JsonSettingsStore Store { get; }

        public ManagedSoundLibrary Library { get; }

        public ActiveSettings Settings { get; }

        public RecordingNotifications Notifications { get; } = new();

        public SettingsSaveResult Save(SettingsSnapshot draft) =>
            operation.Save(draft.Style, draft.DrawShortcut, draft.ClearShortcut, draft.Slots);
    }

    private sealed class IdleSoundChannel : ISaveSoundChannel, ISoundStartHold
    {
        public ISoundStartHold? TryHoldStarts() => this;

        public bool Stop() => true;

        public void EndEarlierRequests()
        {
        }

        public void Dispose()
        {
        }
    }

    private sealed class IdleShortcuts : IActiveShortcuts
    {
        public void Interrupt()
        {
        }

        public void Bind(SettingsSnapshot snapshot, Func<SoundReference, PlaySoundCommand> commandFor)
        {
        }
    }

    private sealed class RecordingNotifications : ISettingsSaveNotifications
    {
        public List<SoundCleanupFailure> CleanupFailures { get; } = [];

        public void NotSaved(string reason)
        {
        }

        public void CleanupFailed(SoundCleanupFailure failure) => CleanupFailures.Add(failure);

        public void StopUnconfirmed() => throw new InvalidOperationException("The idle stop is always confirmed.");
    }

    private sealed class NoSettingsFailures : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy)
        {
        }
    }
}
