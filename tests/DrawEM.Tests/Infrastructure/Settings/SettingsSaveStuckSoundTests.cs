using System.Text;
using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;
using DrawEM.App.Infrastructure;
using DrawEM.Tests.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Settings;

/// <summary>
/// Save while a play request from the previous configuration holds the real sound thread past the wait
/// timeout, against the real settings file, sound library, sound log, sound host, and shortcut adapter.
/// Ctrl+Alt+1 plays Old before the Save; the Save rebinds it to New.
/// </summary>
public sealed class SettingsSaveStuckSoundTests : IDisposable
{
    private static readonly TimeSpan Wait = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan WaitTimeout = TimeSpan.FromMilliseconds(50);
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
    public void Save_WhileOldRequestsOpenTheirPlayer_OldRequestsNeverPlay_AndEveryCopyIsKept()
    {
        var run = SaveWhileOldRequestOpensItsPlayer();

        Assert.Equal(new SettingsSaveResult.Saved(run.Saved), run.Result);
        Assert.Equal(run.Saved, run.ActiveAfterSave);
        Assert.True(File.Exists(CopyPath(run.Old)));
        Assert.True(File.Exists(CopyPath(run.New)));

        // The blocked old request releases its player unplayed; the queued one never opens a player.
        Assert.Equal(
            [
                "create " + CopyPath(run.Old),
                "stop " + CopyPath(run.Old),
                "dispose " + CopyPath(run.Old),
                "create " + CopyPath(run.New),
                "play " + CopyPath(run.New),
                "stop " + CopyPath(run.New),
                "dispose " + CopyPath(run.New),
            ],
            run.PlayerEvents);
        var record = Assert.Single(run.SoundLog);
        Assert.Equal(LoggingSoundFailureReporter.UnconfirmedStopReason, record.Reason);
        Assert.Empty(run.SaveFailures);
    }

    [Fact]
    public void Restart_AfterSaveWithUnconfirmedStop_RemovesLeftoverCopyAndKeepsReferencedOnes()
    {
        var run = SaveWhileOldRequestOpensItsPlayer();

        var startup = SettingsStartup.Load(new JsonSettingsStore(SettingsDirectory), new NoSettingsFailures());
        var failures = SettingsStartup.RemoveOrphanSounds(startup, new ManagedSoundLibrary(LibraryDirectory));

        Assert.Equal(run.Saved, startup.Active);
        Assert.Empty(failures);
        Assert.False(File.Exists(CopyPath(run.Old)));
        Assert.True(File.Exists(CopyPath(run.New)));
    }

    [Fact]
    public void Save_WhileOldRequestIsStartingItsPlayer_SavesNothing_AndRetryAfterTheStartSucceeds()
    {
        var rig = new Rig(this, blockInPlay: true);
        SettingsSaveResult result;
        try
        {
            // The old request has passed its check and is inside its player's start.
            rig.Keyboard.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
            Assert.True(rig.Players.Blocked.Wait(Wait));

            result = rig.Save();

            Assert.Equal(new SettingsSaveResult.NotSaved(SettingsSaveOperation.SoundStartingReason), result);
            Assert.Equal([NotSavedMessage], rig.SaveFailures);
            Assert.Equal(rig.Previous, Assert.IsType<SettingsLoadResult.Loaded>(rig.Store.Load()).Snapshot);
            Assert.Same(rig.Previous, rig.Settings.Current);
        }
        finally
        {
            rig.Players.Release.Set();
        }

        // The start began before the refused Save; a Save after it stops that sound and retires Old.
        Assert.True(rig.Players.OldPlayStarted.Wait(Wait));
        rig.Keyboard.Release(VirtualKeys.D1, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
        var retry = rig.Save();
        var run = rig.Finish(retry);

        Assert.Equal(new SettingsSaveResult.Saved(rig.Saved), retry);
        Assert.Equal(rig.Saved, run.ActiveAfterSave);
        Assert.Equal(
            [
                "create " + CopyPath(rig.Old),
                "play " + CopyPath(rig.Old),
                "stop " + CopyPath(rig.Old),
                "dispose " + CopyPath(rig.Old),
            ],
            run.PlayerEvents);
        Assert.False(File.Exists(CopyPath(rig.Old)));
        Assert.True(File.Exists(CopyPath(rig.New)));
        Assert.Empty(run.SoundLog);
        Assert.Equal([NotSavedMessage], run.SaveFailures);
    }

    /// <summary>
    /// The old request blocks the sound thread while it opens its player, and a second press queues another
    /// old request. Save rebinds Ctrl+Alt+1 while the keys are still held and its stop times out. Then the
    /// keys are pressed again without a full release, released, pressed fresh, and the blocked request is
    /// let go.
    /// </summary>
    private StuckSaveRun SaveWhileOldRequestOpensItsPlayer()
    {
        var rig = new Rig(this, blockInPlay: false);
        SettingsSaveResult result;
        try
        {
            rig.Keyboard.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
            Assert.True(rig.Players.Blocked.Wait(Wait));
            rig.Keyboard.Release(VirtualKeys.D1);
            rig.Keyboard.Press(VirtualKeys.D1);

            result = rig.Save();
            rig.CaptureActive();

            // Keys held across the Save start nothing until every key is released.
            rig.Keyboard.Release(VirtualKeys.D1);
            rig.Keyboard.Press(VirtualKeys.D1);
            rig.Keyboard.Release(VirtualKeys.D1, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
            rig.Keyboard.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
        }
        finally
        {
            rig.Players.Release.Set();
        }

        Assert.True(rig.Players.NewPlayStarted.Wait(Wait));
        return rig.Finish(result);
    }

    private static string NotSavedMessage => SettingsFailureDialog.NotSavedMessage(SettingsSaveOperation.SoundStartingReason);

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
        IReadOnlyList<SoundFailure> SoundLog,
        IReadOnlyList<string> SaveFailures);

    /// <summary>Old saved on Ctrl+Alt+1, New imported as the draft's Ctrl+Alt+1, and the app wired as in App.</summary>
    private sealed class Rig
    {
        private readonly List<SoundFailure> soundLog = [];
        private readonly LoggingSoundFailureReporter reporter;
        private readonly SoundChannelHost host;
        private readonly SettingsSaveOperation operation;
        private SettingsSnapshot? activeAfterSave;

        public Rig(SettingsSaveStuckSoundTests test, bool blockInPlay)
        {
            Store = new JsonSettingsStore(test.SettingsDirectory);
            var library = new ManagedSoundLibrary(test.LibraryDirectory);
            Old = library.Import(test.WriteSource("old.wav", "old"));
            Previous = TestSettings.WithSounds((1, Old));
            Store.Save(Previous);
            library.CommitSave(SettingsSnapshot.Default, Previous, removeUnreferencedCopies: true);
            New = library.Import(test.WriteSource("new.wav", "new"));
            Saved = TestSettings.WithSounds((1, New));

            Settings = new ActiveSettings(Previous, test.CopyPath);
            var time = new ManualTimeProvider();
            reporter = new LoggingSoundFailureReporter(record => { lock (soundLog) { soundLog.Add(record); } }, time);
            Players = new BlockingPlayerFactory(test.CopyPath(Old), blockInPlay);
            host = new SoundChannelHost(dispatch => new SoundChannelController(Players, reporter, time, dispatch), WaitTimeout);
            var controller = new DrawingSessionController(() => Settings.Current.Style);
            var shortcuts = new GlobalShortcutAdapter(
                Keyboard,
                Keyboard,
                controller,
                new DrawingModeInputGate(),
                new FakeCursorPositionSource(new ScreenPoint(10, 10)),
                action => action(),
                new FakeMonitorBoundsSource(new MonitorBounds(0, 0, 100, 100)),
                ShortcutBindings.FromSnapshot(Settings.Current, Settings.CommandFor),
                host.Play);
            var notifications = new SettingsSaveNotifications(new SettingsFailureDialog(SaveFailures.Add), reporter, library);
            operation = new SettingsSaveOperation(Store, library, Settings, host, shortcuts, notifications);
        }

        public JsonSettingsStore Store { get; }

        public SoundReference Old { get; }

        public SoundReference New { get; }

        public SettingsSnapshot Previous { get; }

        public SettingsSnapshot Saved { get; }

        public ActiveSettings Settings { get; }

        public BlockingPlayerFactory Players { get; }

        public FakeKeyboardHookSource Keyboard { get; } = new();

        public List<string> SaveFailures { get; } = [];

        public SettingsSaveResult Save() => operation.Save(Saved.Style, Saved.DrawShortcut, Saved.ClearShortcut, Saved.Slots);

        public void CaptureActive() => activeAfterSave = Settings.Current;

        /// <summary>Ends the sound thread and drains the sound log, then reports what happened.</summary>
        public StuckSaveRun Finish(SettingsSaveResult result)
        {
            var active = activeAfterSave ?? Settings.Current;
            host.Dispose();
            reporter.Dispose();
            lock (soundLog)
            {
                return new StuckSaveRun(Old, New, Saved, result, active, Players.Events, [.. soundLog], [.. SaveFailures]);
            }
        }
    }

    private sealed class NoSettingsFailures : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy) => throw new InvalidOperationException(reason);
    }

    /// <summary>
    /// Records player calls by path on the sound thread. The first player for <see cref="BlockedPath"/> blocks
    /// that thread until <see cref="Release"/> is set: while it opens, or inside its start when
    /// <c>blockInPlay</c> is set.
    /// </summary>
    private sealed class BlockingPlayerFactory(string blockedPath, bool blockInPlay) : ISoundPlayerFactory
    {
        private readonly List<string> events = [];

        public string BlockedPath { get; } = blockedPath;

        public bool BlockInPlay { get; } = blockInPlay;

        public ManualResetEventSlim Blocked { get; } = new();

        public ManualResetEventSlim Release { get; } = new();

        public ManualResetEventSlim OldPlayStarted { get; } = new();

        public ManualResetEventSlim NewPlayStarted { get; } = new();

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
            if (!BlockInPlay && command.Path == BlockedPath)
            {
                BlockUntilReleased();
            }

            return new Player(this, command.Path);
        }

        private void BlockUntilReleased()
        {
            Blocked.Set();
            Release.Wait();
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
                    factory.NewPlayStarted.Set();
                    return;
                }

                if (factory.BlockInPlay)
                {
                    factory.BlockUntilReleased();
                }

                factory.OldPlayStarted.Set();
            }

            public void Stop() => factory.Record("stop " + path);

            public void Dispose() => factory.Record("dispose " + path);
        }
    }
}
