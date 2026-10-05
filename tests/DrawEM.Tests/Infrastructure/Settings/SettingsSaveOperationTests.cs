using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure;
using DrawEM.Tests.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Settings;

/// <summary>
/// Save wired as in <c>App.xaml.cs</c>: the real shortcut and mouse adapters, input gate, drawing controller,
/// and sound channel, with fake hooks, store, library, and players. Drawing commands wait in a UI queue and
/// play requests in a sound queue until a test runs them.
/// </summary>
public sealed class SettingsSaveOperationTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private const int D = 'D';
    private static readonly MonitorBounds Left = new(0, 0, 100, 100);
    private static readonly MonitorBounds Right = new(100, 0, 200, 100);
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private static readonly SoundReference Drumroll = new("drumroll.mp3", "Drumroll.mp3");
    private static readonly HexColor Blue = new(0x00, 0x00, 0xFF);

    /// <summary>Draw Ctrl+Alt+Z, clear Ctrl+Alt+X, Applause on Ctrl+Alt+1, Drumroll on Ctrl+Alt+2.</summary>
    private static readonly SettingsSnapshot Initial = TestSettings.WithSounds((1, Applause), (2, Drumroll));

    /// <summary>Draw moves to Ctrl+Alt+D, Ctrl+Alt+1 plays Drumroll, slot 2 is empty.</summary>
    private static readonly SettingsSnapshot Remapped = Snapshot(
        SettingsSnapshot.Default.Style, Shortcut.Create(CtrlAlt, ShortcutKey.Letter('D')), (1, Drumroll));

    [Fact]
    public void Save_DuringActiveStrokeAndSound_StopsSoundAndEndsDrawing_BeforeNewSnapshotIsActive()
    {
        var app = new SaveHarness();
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
        app.RunSound();
        app.Release(VirtualKeys.D1);
        app.Press(VirtualKeys.Z);
        app.RunUi();
        app.Mouse.Move(new ScreenPoint(20, 20));
        app.RunUi();
        var player = Assert.Single(app.Players.Created);
        Assert.Equal(new SoundId(Applause.LibraryFileName), app.Channel.ActiveSound);
        Assert.NotNull(app.States[^1].ActiveStroke);
        Assert.True(app.Gate.IsActive);
        app.Players.OnStop = () => app.Events.Add(
            $"sound stopped; gate {(app.Gate.IsActive ? "open" : "closed")}; {Name(app.Settings.Current)} active");
        app.Controller.StateChanged += state => app.Events.Add(
            $"drawing {(state.IsDrawModeActive ? "on" : "off")}, {state.CompletedStrokes.Count} strokes, " +
            $"active stroke {(state.ActiveStroke is null ? "none" : "kept")}; gate {(app.Gate.IsActive ? "open" : "closed")}; " +
            $"{Name(app.Settings.Current)} active");

        var result = app.Save(Remapped);

        Assert.Equal(new SettingsSaveResult.Saved(Remapped), result);
        Assert.Equal(
            [
                "sound stopped; gate open; initial active",
                "drawing off, 0 strokes, active stroke none; gate closed; initial active",
            ],
            app.Events);
        Assert.Equal(Remapped, app.Settings.Current);
        Assert.Equal([Remapped], app.Store.Saved);
        Assert.Equal(["play", "stop", "dispose"], player.Calls);
        Assert.Null(app.Channel.ActiveSound);
        Assert.False(app.Gate.IsActive);

        app.Mouse.Move(new ScreenPoint(30, 30));
        app.RunUi();
        Assert.Empty(app.Controller.CompletedStrokes);
        Assert.Null(app.States[^1].ActiveStroke);
        Assert.Empty(app.SoundFailures);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Save_PersistsBeforeActivating_AndRemovesCopiesOnlyAfterConfirmedStop(bool stopConfirmed)
    {
        var app = new SaveHarness();
        app.Sound.StopConfirmed = stopConfirmed;
        SettingsSnapshot? activeWhenWritten = null;
        SettingsSnapshot? activeWhenCommitted = null;
        app.Store.OnSave = () => activeWhenWritten = app.Settings.Current;
        app.Library.OnCommit = () => activeWhenCommitted = app.Settings.Current;

        var result = app.Save(Remapped);

        Assert.IsType<SettingsSaveResult.Saved>(result);
        Assert.Same(Initial, activeWhenWritten);
        Assert.Equal(Remapped, activeWhenCommitted);
        Assert.Equal([(Initial, Remapped, stopConfirmed)], app.Library.Commits);
        Assert.Equal(stopConfirmed ? 0 : 1, app.Notifications.UnconfirmedStops);
    }

    [Fact]
    public void Save_ClearsStrokesOnEveryMonitor()
    {
        var app = new SaveHarness();
        DrawCompletedStroke(app.Controller, new ScreenPoint(10, 10), Left);
        DrawCompletedStroke(app.Controller, new ScreenPoint(150, 10), Right);
        Assert.Equal(2, app.Controller.CompletedStrokes.Count);

        app.Save(Initial);

        Assert.Empty(app.Controller.CompletedStrokes);
        Assert.Empty(app.States[^1].CompletedStrokes);
        Assert.False(app.States[^1].IsDrawModeActive);
    }

    [Fact]
    public void KeysHeldAcrossSave_DoNothing_UntilAllReleased_ThenOnlyNewShortcutsWork()
    {
        var app = new SaveHarness();
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.Z);

        app.Save(Remapped);
        app.RunUi();
        app.RunSound();

        Assert.DoesNotContain(app.States, state => state.IsDrawModeActive);

        // New draw and sound shortcuts formed while keys from before the Save are still down.
        app.Press(D);
        app.Press(VirtualKeys.D1);
        app.Release(D, VirtualKeys.D1, VirtualKeys.Z);
        app.Press(D);
        app.Release(D);
        app.Press(VirtualKeys.Z);
        app.RunUi();
        app.RunSound();
        Assert.False(app.Gate.IsActive);
        Assert.DoesNotContain(app.States, state => state.IsDrawModeActive);
        Assert.Empty(app.Players.Created);

        app.Release(VirtualKeys.Z, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.Z);
        app.Release(VirtualKeys.Z);
        app.Press(VirtualKeys.D2);
        app.Release(VirtualKeys.D2);
        app.RunUi();
        app.RunSound();
        Assert.DoesNotContain(app.States, state => state.IsDrawModeActive);
        Assert.Empty(app.Players.Created);

        app.Press(VirtualKeys.D1);
        app.Release(VirtualKeys.D1);
        app.Press(D);
        app.RunUi();
        app.RunSound();
        Assert.Equal(new SoundId(Drumroll.LibraryFileName), Assert.Single(app.Players.Created).Sound);
        Assert.True(app.Gate.IsActive);
        Assert.True(app.States[^1].IsDrawModeActive);
    }

    [Fact]
    public void ColorOnlyChange_HeldDrawKeyNeedsFreshPress_AndNextStrokeUsesNewColor()
    {
        var app = new SaveHarness();
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.Z);
        app.RunUi();
        Assert.Equal(Initial.Style, app.States[^1].ActiveStroke!.Style);
        var blue = Snapshot(Initial.Style with { Color = Blue }, Initial.DrawShortcut, (1, Applause), (2, Drumroll));

        app.Save(blue);
        app.Press(VirtualKeys.Z);
        app.Mouse.Move(new ScreenPoint(30, 30));
        app.Release(VirtualKeys.Z);
        app.Press(VirtualKeys.Z);
        app.RunUi();

        Assert.False(app.Gate.IsActive);
        Assert.False(app.States[^1].IsDrawModeActive);
        Assert.Empty(app.Controller.CompletedStrokes);

        app.Release(VirtualKeys.Z, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.Z);
        app.RunUi();

        Assert.True(app.Gate.IsActive);
        Assert.Equal(blue.Style, app.States[^1].ActiveStroke!.Style);
    }

    [Fact]
    public void FailedSave_ReportsOnce_AndKeepsSoundDrawingShortcutsAndMedia()
    {
        var app = new SaveHarness();
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
        app.RunSound();
        app.Release(VirtualKeys.D1);
        app.Press(VirtualKeys.Z);
        app.RunUi();
        var player = Assert.Single(app.Players.Created);
        var drawing = app.States[^1];
        app.Store.Failure = "The disk is full.";

        var result = app.Save(Remapped);

        Assert.Equal(new SettingsSaveResult.NotSaved("The disk is full."), result);
        Assert.Equal(["The disk is full."], app.Notifications.NotSavedReasons);
        Assert.Same(Initial, app.Settings.Current);
        Assert.Empty(app.Library.Commits);
        Assert.Equal(["play"], player.Calls);
        Assert.Equal(new SoundId(Applause.LibraryFileName), app.Channel.ActiveSound);
        Assert.True(app.Gate.IsActive);
        Assert.Same(drawing, app.States[^1]);

        // The previous shortcuts keep working without a fresh press.
        app.Release(VirtualKeys.Z);
        app.Press(VirtualKeys.Z);
        app.Press(VirtualKeys.D2);
        app.RunUi();
        app.RunSound();
        Assert.True(app.Gate.IsActive);
        Assert.True(app.States[^1].IsDrawModeActive);
        Assert.Equal(new SoundId(Drumroll.LibraryFileName), app.Players.Created[^1].Sound);
        Assert.Equal(["The disk is full."], app.Notifications.NotSavedReasons);
    }

    [Fact]
    public void InvalidDraft_ReturnsErrors_AndChangesNothing()
    {
        var app = new SaveHarness();
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
        app.RunSound();
        app.Release(VirtualKeys.D1);
        app.Press(VirtualKeys.Z);
        app.RunUi();
        var player = Assert.Single(app.Players.Created);
        var drawing = app.States[^1];

        var result = app.Operation.Save(Initial.Style, Initial.DrawShortcut, Initial.DrawShortcut, Initial.Slots);

        var invalid = Assert.IsType<SettingsSaveResult.Invalid>(result);
        Assert.Equal(
            [new SettingsError(SettingsErrorCode.DuplicateShortcut, SettingsCommand.Clear, SettingsCommand.Draw)],
            invalid.Errors);
        Assert.Empty(app.Store.Saved);
        Assert.Empty(app.Library.Commits);
        Assert.Empty(app.Notifications.NotSavedReasons);
        Assert.Same(Initial, app.Settings.Current);
        Assert.Equal(["play"], player.Calls);
        Assert.True(app.Gate.IsActive);
        Assert.Same(drawing, app.States[^1]);
    }

    private static void DrawCompletedStroke(DrawingSessionController controller, ScreenPoint start, MonitorBounds monitor)
    {
        controller.EnterDrawMode(start, monitor);
        controller.ReportPointer(start with { X = start.X + 5 });
        controller.ExitDrawMode();
    }

    private static string Name(SettingsSnapshot snapshot) => snapshot == Initial ? "initial" : "saved";

    private static SettingsSnapshot Snapshot(DrawingStyle style, Shortcut draw, params (int Slot, SoundReference Sound)[] sounds) =>
        SettingsSnapshot.Validate(
            style,
            draw,
            SettingsSnapshot.Default.ClearShortcut,
            Enumerable.Range(1, ActionSlot.Count).Select(number =>
                sounds.FirstOrDefault(entry => entry.Slot == number) is { Sound: { } sound }
                    ? new ActionSlot(number, new SoundAction(sound, Shortcut.Create(CtrlAlt, ShortcutKey.Digit(number))))
                    : ActionSlot.Empty(number))).Snapshot!;

    private sealed class SaveHarness
    {
        private readonly Queue<Action> ui = new();

        public SaveHarness()
        {
            Settings = new ActiveSettings(Initial, reference => @"C:\library\" + reference.LibraryFileName);
            Players = new FakePlayerFactory();
            Channel = new SoundChannelController(Players, new RecordingSoundFailures(SoundFailures), new ManualTimeProvider(), action => action());
            Sound = new FakeSoundChannel(Channel);
            Controller = new DrawingSessionController(() => Settings.Current.Style);
            Controller.StateChanged += States.Add;
            var shortcuts = new GlobalShortcutAdapter(
                Keyboard,
                Keyboard,
                Controller,
                Gate,
                new FakeCursorPositionSource(new ScreenPoint(10, 10)),
                ui.Enqueue,
                new FakeMonitorBoundsSource(Left, Right),
                ShortcutBindings.FromSnapshot(Settings.Current, Settings.CommandFor),
                Sound.Play);
            _ = new GlobalMouseInputAdapter(Mouse, Controller, Gate, ui.Enqueue, new FakeMonitorBoundsSource(Left, Right));
            Operation = new SettingsSaveOperation(Store, Library, Settings, Sound, shortcuts, Notifications);
        }

        public ActiveSettings Settings { get; }

        public FakeStore Store { get; } = new();

        public FakeLibrary Library { get; } = new();

        public FakeSoundChannel Sound { get; }

        public FakePlayerFactory Players { get; }

        public SoundChannelController Channel { get; }

        public DrawingSessionController Controller { get; }

        public DrawingModeInputGate Gate { get; } = new();

        public FakeKeyboardHookSource Keyboard { get; } = new();

        public FakeMouse Mouse { get; } = new();

        public RecordingNotifications Notifications { get; } = new();

        public SettingsSaveOperation Operation { get; }

        public List<DrawingState> States { get; } = [];

        public List<string> Events { get; } = [];

        public List<string> SoundFailures { get; } = [];

        public SettingsSaveResult Save(SettingsSnapshot draft) =>
            Operation.Save(draft.Style, draft.DrawShortcut, draft.ClearShortcut, draft.Slots);

        public void Press(params int[] keys) => Keyboard.Press(keys);

        public void Release(params int[] keys) => Keyboard.Release(keys);

        public void RunUi()
        {
            while (ui.TryDequeue(out var action))
            {
                action();
            }
        }

        public void RunSound() => Sound.Run();
    }

    /// <summary>
    /// Plays wait in a queue until <see cref="Run"/>; ending earlier requests discards the queued ones. The
    /// start race is covered against the real host in SettingsSaveStuckSoundTests and SoundChannelHostTests.
    /// </summary>
    private sealed class FakeSoundChannel(SoundChannelController channel) : ISaveSoundChannel, ISoundStartHold
    {
        private readonly Queue<PlaySoundCommand> queue = new();

        public bool StopConfirmed { get; set; } = true;

        public void Play(PlaySoundCommand command) => queue.Enqueue(command);

        public void Run()
        {
            while (queue.TryDequeue(out var command))
            {
                channel.Play(command);
            }
        }

        public ISoundStartHold? TryHoldStarts() => this;

        public void EndEarlierRequests() => queue.Clear();

        public void Dispose()
        {
        }

        public bool Stop()
        {
            channel.Stop();
            return StopConfirmed;
        }
    }

    private sealed class FakeMouse : IMouseHookSource
    {
        public event Action<ScreenPoint>? PointerMoved;

        public event Func<bool>? PointerButtonActivity { add { } remove { } }

        public event Func<bool>? PointerWheelActivity { add { } remove { } }

        public void Move(ScreenPoint point) => PointerMoved?.Invoke(point);
    }

    private sealed class FakeStore : ISettingsStore
    {
        public string? Failure { get; set; }

        public Action? OnSave { get; set; }

        public List<SettingsSnapshot> Saved { get; } = [];

        public SettingsLoadResult Load() => new SettingsLoadResult.Missing();

        public void Save(SettingsSnapshot snapshot)
        {
            OnSave?.Invoke();
            if (Failure is not null)
            {
                throw new SettingsStoreException(Failure);
            }

            Saved.Add(snapshot);
        }
    }

    private sealed class FakeLibrary : ISoundLibrary
    {
        public List<(SettingsSnapshot Previous, SettingsSnapshot Saved, bool RemoveUnreferencedCopies)> Commits { get; } = [];

        public Action? OnCommit { get; set; }

        public SoundReference Import(string sourceFile) => throw new NotSupportedException();

        public IReadOnlyList<SoundCleanupFailure> DiscardDraft(SettingsSnapshot saved) => throw new NotSupportedException();

        public IReadOnlyList<SoundCleanupFailure> CommitSave(
            SettingsSnapshot previous, SettingsSnapshot saved, bool removeUnreferencedCopies)
        {
            OnCommit?.Invoke();
            Commits.Add((previous, saved, removeUnreferencedCopies));
            return [];
        }

        public IReadOnlyList<SoundCleanupFailure> RemoveOrphans(SettingsSnapshot saved) => throw new NotSupportedException();
    }

    private sealed class RecordingNotifications : ISettingsSaveNotifications
    {
        public List<string> NotSavedReasons { get; } = [];

        public int UnconfirmedStops { get; private set; }

        public void NotSaved(string reason) => NotSavedReasons.Add(reason);

        public void CleanupFailed(SoundCleanupFailure failure) => throw new InvalidOperationException(failure.Reason);

        public void StopUnconfirmed() => UnconfirmedStops++;
    }

    private sealed class RecordingSoundFailures(List<string> reasons) : ISoundFailureReporter
    {
        public void Report(PlaySoundCommand command, string reason) => reasons.Add(reason);
    }

    /// <summary>Records players without opening a media engine.</summary>
    private sealed class FakePlayerFactory : ISoundPlayerFactory
    {
        public List<FakePlayer> Created { get; } = [];

        public Action? OnStop { get; set; }

        public ISoundPlayer Create(PlaySoundCommand command)
        {
            var player = new FakePlayer(command.Sound, () => OnStop?.Invoke());
            Created.Add(player);
            return player;
        }
    }

    private sealed class FakePlayer(SoundId sound, Action onStop) : ISoundPlayer
    {
        public event Action? Completed { add { } remove { } }

        public event Action<string>? Failed { add { } remove { } }

        public SoundId Sound { get; } = sound;

        public List<string> Calls { get; } = [];

        public void Play() => Calls.Add("play");

        public void Stop()
        {
            Calls.Add("stop");
            onStop();
        }

        public void Dispose() => Calls.Add("dispose");
    }
}
