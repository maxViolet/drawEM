using System.Text;
using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;
using DrawEM.Tests.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Settings;

/// <summary>
/// Save while a play request from the previous configuration holds the real sound thread past the stop
/// timeout, against the real settings file, sound library, sound log, and shortcut adapter.
/// </summary>
public sealed class SettingsSaveStuckSoundTests : IDisposable
{
    private static readonly TimeSpan Wait = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan StopTimeout = TimeSpan.FromMilliseconds(50);
    private readonly string root = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));

    private string SettingsDirectory => Path.Combine(root, "profile");

    private string LibraryDirectory => Path.Combine(SettingsDirectory, "sounds");

    public void Dispose()
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Save_WhileOldRequestsHoldSoundThread_OldRequestsNeverPlay_AndEveryCopyIsKept()
    {
        var run = SaveWhileSoundThreadIsStuck();

        Assert.Equal(new SettingsSaveResult.Saved(run.Saved), run.Result);
        Assert.Equal(run.Saved, run.ActiveAfterSave);
        Assert.True(File.Exists(CopyPath(run.Old)));
        Assert.True(File.Exists(CopyPath(run.New)));

        // The blocked old request releases its player unplayed; the queued one never opens a player.
        Assert.Equal(
            [
                "create " + CopyPath(run.Old),
                "dispose " + CopyPath(run.Old),
                "create " + CopyPath(run.New),
                "play " + CopyPath(run.New),
                "stop " + CopyPath(run.New),
                "dispose " + CopyPath(run.New),
            ],
            run.PlayerEvents);
        var record = Assert.Single(run.SoundLog);
        Assert.Equal(LoggingSoundFailureReporter.UnconfirmedStopReason, record.Reason);
    }

    [Fact]
    public void Restart_AfterSaveWithUnconfirmedStop_RemovesLeftoverCopyAndKeepsReferencedOnes()
    {
        var run = SaveWhileSoundThreadIsStuck();

        var startup = SettingsStartup.Load(new JsonSettingsStore(SettingsDirectory), new NoSettingsFailures());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, new ManagedSoundLibrary(LibraryDirectory));

        Assert.Equal(run.Saved, startup.Active);
        Assert.Empty(failures);
        Assert.False(File.Exists(CopyPath(run.Old)));
        Assert.True(File.Exists(CopyPath(run.New)));
    }

    /// <summary>
    /// Ctrl+Alt+1 plays Old and its request blocks the sound thread while it opens; a second press of 1
    /// queues another Old request. Save rebinds Ctrl+Alt+1 to New while the keys are still held and the
    /// stop times out. Then the keys are pressed again without a full release, released, pressed fresh,
    /// and the blocked request is let go.
    /// </summary>
    private StuckSaveRun SaveWhileSoundThreadIsStuck()
    {
        var store = new JsonSettingsStore(SettingsDirectory);
        var library = new ManagedSoundLibrary(LibraryDirectory);
        var old = library.Import(WriteSource("old.wav", "old"));
        var previous = TestSettings.WithSounds((1, old));
        SettingsPersistence.Save(store, library, SettingsSnapshot.Default, previous, _ => true);
        var @new = library.Import(WriteSource("new.wav", "new"));
        var saved = TestSettings.WithSounds((1, @new));

        var settings = new ActiveSettings(previous, CopyPath);
        var time = new ManualTimeProvider();
        var soundLog = new List<SoundFailure>();
        var reporter = new LoggingSoundFailureReporter(record => { lock (soundLog) { soundLog.Add(record); } }, time);
        var players = new BlockingPlayerFactory(CopyPath(old));
        var host = new SoundChannelHost(
            dispatch => new SoundChannelController(players, reporter, time, dispatch), StopTimeout);
        var keyboard = new FakeKeyboard();
        var controller = new DrawingSessionController(() => settings.Current.Snapshot.Style);
        var active = settings.Current;
        var shortcuts = new GlobalShortcutAdapter(
            keyboard,
            keyboard,
            controller,
            new DrawingModeInputGate(),
            new FakeCursorPositionSource(new ScreenPoint(10, 10)),
            action => action(),
            new FakeMonitorBoundsSource(new MonitorBounds(0, 0, 100, 100)),
            ShortcutBindings.FromSnapshot(active.Snapshot, active.CommandFor),
            host.Play);
        var operation = new SettingsSaveOperation(
            store, library, settings, host.Stop, shortcuts, controller, new NoSaveFailures(),
            failure => reporter.ReportCleanup(failure.LibraryFileName ?? LibraryDirectory, failure.Reason),
            reporter.ReportUnconfirmedStop);

        SettingsSaveResult result;
        ActiveConfiguration activeAfterSave;
        try
        {
            keyboard.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
            Assert.True(players.Blocked.Wait(Wait));
            keyboard.Release(VirtualKeys.D1);
            keyboard.Press(VirtualKeys.D1);

            result = operation.Save(saved.Style, saved.DrawShortcut, saved.ClearShortcut, saved.Slots);
            activeAfterSave = settings.Current;

            // Keys held across the Save start nothing until every key is released.
            keyboard.Release(VirtualKeys.D1);
            keyboard.Press(VirtualKeys.D1);
            keyboard.Release(VirtualKeys.D1, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
            keyboard.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
        }
        finally
        {
            players.Release.Set();
        }

        Assert.True(players.LaterPlayStarted.Wait(Wait));
        host.Dispose();
        reporter.Dispose();
        lock (soundLog)
        {
            return new StuckSaveRun(old, @new, saved, result, activeAfterSave.Snapshot, players.Events, [.. soundLog]);
        }
    }

    private string CopyPath(SoundReference sound) => Path.Combine(LibraryDirectory, sound.LibraryFileName);

    private string WriteSource(string fileName, string content)
    {
        var path = Path.Combine(root, "source", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, Encoding.UTF8.GetBytes(content));
        return path;
    }

    private sealed record StuckSaveRun(
        SoundReference Old,
        SoundReference New,
        SettingsSnapshot Saved,
        SettingsSaveResult Result,
        SettingsSnapshot ActiveAfterSave,
        IReadOnlyList<string> PlayerEvents,
        IReadOnlyList<SoundFailure> SoundLog);

    private sealed class FakeKeyboard : IKeyboardHookSource, INeutralKeyEmitter
    {
        private Func<int, KeyDirection, bool>? handleKey;

        public void SetKeyHandler(Func<int, KeyDirection, bool> handler) => handleKey = handler;

        public void Press(params int[] keys)
        {
            foreach (var key in keys)
            {
                handleKey!(key, KeyDirection.Down);
            }
        }

        public void Release(params int[] keys)
        {
            foreach (var key in keys)
            {
                handleKey!(key, KeyDirection.Up);
            }
        }

        public void EmitNeutralKey()
        {
        }
    }

    private sealed class NoSaveFailures : ISettingsSaveFailureReporter
    {
        public void SettingsNotSaved(string reason) => throw new InvalidOperationException(reason);
    }

    private sealed class NoSettingsFailures : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy) => throw new InvalidOperationException(reason);
    }

    /// <summary>
    /// Records player calls by path on the sound thread. Creating the player for <see cref="BlockedPath"/>
    /// blocks that thread until <see cref="Release"/> is set.
    /// </summary>
    private sealed class BlockingPlayerFactory(string blockedPath) : ISoundPlayerFactory
    {
        private readonly List<string> events = [];

        public string BlockedPath { get; } = blockedPath;

        public ManualResetEventSlim Blocked { get; } = new();

        public ManualResetEventSlim Release { get; } = new();

        public ManualResetEventSlim LaterPlayStarted { get; } = new();

        public IReadOnlyList<string> Events
        {
            get
            {
                lock (events)
                {
                    return [.. events];
                }
            }
        }

        public ISoundPlayer Create(PlaySoundCommand command)
        {
            Record("create " + command.Path);
            if (command.Path == BlockedPath)
            {
                Blocked.Set();
                Release.Wait();
            }

            return new Player(this, command.Path);
        }

        private void Record(string entry)
        {
            lock (events)
            {
                events.Add(entry);
            }
        }

        private sealed class Player(BlockingPlayerFactory factory, string path) : ISoundPlayer
        {
            public event Action? Completed { add { } remove { } }

            public event Action<string>? Failed { add { } remove { } }

            public void Play()
            {
                factory.Record("play " + path);
                if (path != factory.BlockedPath)
                {
                    factory.LaterPlayStarted.Set();
                }
            }

            public void Stop() => factory.Record("stop " + path);

            public void Dispose() => factory.Record("dispose " + path);
        }
    }
}
