using System.Text;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Settings;

/// <summary>Save, restart, and media cleanup against the real settings file and sound library.</summary>
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
        var store = Store();
        var library = Library();
        var saved = library.Import(WriteSource("saved.wav", "saved"));
        var previous = TestSettings.WithSounds((1, saved));
        var active = new ActiveSettings(SettingsSnapshot.Default, ManagedPath);
        SettingsPersistence.Save(store, library, SettingsSnapshot.Default, previous, Publish(active));
        var replacement = library.Import(WriteSource("replacement.wav", "replacement"));
        var savedFile = File.ReadAllBytes(store.SettingsPath);
        File.SetAttributes(store.SettingsPath, FileAttributes.ReadOnly);
        var activated = false;

        Assert.Throws<SettingsStoreException>(() => SettingsPersistence.Save(
            store, library, previous, TestSettings.WithSounds((1, replacement)), _ => activated = true));

        Assert.False(activated);
        Assert.Same(previous, active.Current.Snapshot);
        Assert.Equal(savedFile, File.ReadAllBytes(store.SettingsPath));
        Assert.Equal(Bytes("saved"), File.ReadAllBytes(CopyPath(saved)));
        Assert.Equal(Bytes("replacement"), File.ReadAllBytes(CopyPath(replacement)));

        library.DiscardDraft(previous);

        Assert.Equal([saved.LibraryFileName], LibraryFiles());
    }

    [Fact]
    public void Restart_AfterSave_ActivatesSavedSnapshotAndRemovesOnlyDraftLeftovers()
    {
        var library = Library();
        var kept = library.Import(WriteSource("kept.wav", "kept"));
        var added = library.Import(WriteSource("added.mp3", "added"));
        var draft = TestSettings.WithSounds((1, kept), (2, added), (6, kept));
        var active = new ActiveSettings(SettingsSnapshot.Default, ManagedPath);
        SettingsPersistence.Save(Store(), library, SettingsSnapshot.Default, draft, Publish(active));
        var leftover = library.Import(WriteSource("unsaved.wav", "unsaved"));

        var startup = SettingsStartup.Load(Store(), new NoSettingsFailures());
        var restarted = new ActiveSettings(startup.Active, ManagedPath);
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());

        Assert.True(startup.SavedSettingsLoaded);
        Assert.Equal(active.Current.Snapshot, restarted.Current.Snapshot);
        Assert.Empty(failures);
        Assert.False(File.Exists(CopyPath(leftover)));
        Assert.Equal(new[] { kept.LibraryFileName, added.LibraryFileName }.Order(), LibraryFiles().Order());
        Assert.Equal(CopyPath(added), restarted.Current.CommandFor(added).Path);
    }

    [Fact]
    public void Restart_WithSavedCopyDeletedOutsideApp_LoadsSnapshotAndCleansUpWithoutFailure()
    {
        var library = Library();
        var missing = library.Import(WriteSource("missing.wav", "missing"));
        var present = library.Import(WriteSource("present.wav", "present"));
        var saved = TestSettings.WithSounds((1, missing), (2, present));
        SettingsPersistence.Save(Store(), library, SettingsSnapshot.Default, saved, _ => true);
        File.Delete(CopyPath(missing));

        var startup = SettingsStartup.Load(Store(), new NoSettingsFailures());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, Library());
        var active = new ActiveSettings(startup.Active, ManagedPath);

        Assert.True(startup.SavedSettingsLoaded);
        Assert.Equal(saved, active.Current.Snapshot);
        Assert.Empty(failures);
        Assert.Equal([present.LibraryFileName], LibraryFiles());
        Assert.Equal(CopyPath(missing), active.Current.CommandFor(missing).Path);
    }

    [Fact]
    public void Save_WhenRetiredCopyIsLocked_SucceedsActivatesAndReportsCopy()
    {
        var store = Store();
        var library = Library();
        var locked = library.Import(WriteSource("locked.wav", "locked"));
        var previous = TestSettings.WithSounds((1, locked));
        var active = new ActiveSettings(SettingsSnapshot.Default, ManagedPath);
        SettingsPersistence.Save(store, library, SettingsSnapshot.Default, previous, Publish(active));
        var replacement = library.Import(WriteSource("replacement.wav", "replacement"));
        var draft = TestSettings.WithSounds((1, replacement));

        IReadOnlyList<SoundCleanupFailure> failures;
        using (new FileStream(CopyPath(locked), FileMode.Open, FileAccess.Read, FileShare.None))
        {
            failures = SettingsPersistence.Save(store, library, previous, draft, Publish(active));
        }

        Assert.Equal(locked.LibraryFileName, Assert.Single(failures).LibraryFileName);
        Assert.Same(draft, active.Current.Snapshot);
        Assert.Equal(draft, Assert.IsType<SettingsLoadResult.Loaded>(store.Load()).Snapshot);
        Assert.True(File.Exists(CopyPath(replacement)));
    }

    /// <summary>Activates like a Save whose sound stop was confirmed.</summary>
    private static Func<SettingsSnapshot, bool> Publish(ActiveSettings active) => snapshot =>
    {
        active.Publish(snapshot);
        return true;
    };

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

    private sealed class NoSettingsFailures : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy)
        {
        }
    }
}
