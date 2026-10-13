using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Effects;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Effects;
using DrawEM.Tests.Infrastructure.Drawing;
using Microsoft.Win32;

namespace DrawEM.Tests.Infrastructure.Effects;

/// <summary>
/// The effect channel through the app lifecycle, wired as in <c>App.xaml.cs</c>: the real shortcut adapter,
/// effect monitor guard, effect channel, drawing controller, and sound channel, with fake hooks, sources,
/// players, and effect surface. Effect starts wait in a UI queue until a test runs them.
/// </summary>
public sealed class EffectLifecycleTests
{
    private const string StopEffects = "Stop effects";
    private const string DisplayChange = "display change";
    private const string SessionLock = "session lock";
    private const string SystemSuspend = "system suspend";
    private const string Exit = "exit";
    private const string StartupFailed = "startup failure";
    private static readonly MonitorBounds Left = new(0, 0, 100, 100);
    private static readonly MonitorBounds Right = new(100, 0, 200, 100);
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private static readonly EffectId Confetti = new("confetti");
    private static readonly EffectId FocusRing = new("focus-ring");

    /// <summary>Applause on Ctrl+Alt+1, Confetti on Ctrl+Alt+3, and Focus ring on Ctrl+Alt+4.</summary>
    private static readonly SettingsSnapshot Settings =
        TestSettings.WithActions([(1, Applause)], [(3, Confetti), (4, FocusRing)]);

    /// <summary>Every event that ends effects.</summary>
    public static TheoryData<string> Events =>
        new() { StopEffects, DisplayChange, SessionLock, SystemSuspend, Exit, StartupFailed };

    /// <summary>The events after which the app keeps running.</summary>
    public static TheoryData<string> Interruptions => new() { StopEffects, DisplayChange, SessionLock, SystemSuspend };

    [Theory]
    [MemberData(nameof(Events))]
    public void Event_RemovesTheRunningEffect_AndAStartQueuedBeforeItNeverAppears(string name)
    {
        var app = new Harness();
        var running = app.StartConfetti();
        app.Release(VirtualKeys.D3);
        app.Press(VirtualKeys.D4);

        app.Raise(name);
        app.RunUi();

        Assert.Equal([running], app.Effects.Shown);
        Assert.Equal([running], app.Effects.Removed);
        Assert.Null(app.EffectChannel.ActiveInstance);
        Assert.Equal(0, app.Time.ActiveTimerCount);
        Assert.Empty(app.Effects.Failures);
    }

    [Theory]
    [MemberData(nameof(Events))]
    public void RepeatedEvent_IsSafe_AndLeavesNoEffectOrDeadline(string name)
    {
        var app = new Harness();
        var running = app.StartConfetti();

        app.Raise(name);
        app.Raise(name);
        app.RunUi();

        Assert.Equal([running], app.Effects.Removed);
        Assert.Null(app.EffectChannel.ActiveInstance);
        Assert.Equal(0, app.Time.ActiveTimerCount);
        Assert.Empty(app.Effects.Failures);
    }

    [Theory]
    [MemberData(nameof(Interruptions))]
    public void Interruption_KeepsDrawingsAndSound(string name)
    {
        var app = new Harness();
        app.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D1);
        app.Release(VirtualKeys.D1);
        app.Controller.EnterDrawMode(new ScreenPoint(10, 10), Left);
        app.Controller.ReportPointer(new ScreenPoint(20, 20));
        app.Controller.ExitDrawMode();
        app.Press(VirtualKeys.D3);
        app.RunUi();
        Assert.NotNull(app.EffectChannel.ActiveInstance);

        app.Raise(name);
        app.RunUi();

        Assert.Null(app.EffectChannel.ActiveInstance);
        Assert.Single(app.States[^1].CompletedStrokes);
        Assert.Equal(new SoundId(Applause.LibraryFileName), app.Sound.ActiveSound);
        Assert.Equal(["play"], Assert.Single(app.Players.Created).Calls);
    }

    [Theory]
    [MemberData(nameof(Interruptions))]
    public void AfterAnInterruption_NothingRestartsByItself_ButAFreshPressStarts(string name)
    {
        var app = new Harness();
        var interrupted = app.StartConfetti();

        app.Raise(name);
        app.RunUi();
        app.Time.Advance(EffectChannelController.MaxDuration);
        app.RunUi();

        Assert.Equal([interrupted], app.Effects.Shown);

        app.Release(VirtualKeys.D3);
        app.Press(VirtualKeys.D3);
        app.RunUi();

        Assert.Equal(2, app.Effects.Shown.Count);
        Assert.NotSame(interrupted, app.EffectChannel.ActiveInstance);
    }

    [Theory]
    [InlineData(SessionSwitchReason.SessionLock, true)]
    [InlineData(SessionSwitchReason.SessionUnlock, false)]
    [InlineData(SessionSwitchReason.SessionLogon, false)]
    [InlineData(SessionSwitchReason.ConsoleConnect, false)]
    [InlineData(SessionSwitchReason.RemoteConnect, false)]
    public void SessionLockInterrupts_UnlockDoesNot(SessionSwitchReason reason, bool interrupts)
    {
        Assert.Equal(interrupts, Win32SessionInterruptionSource.Interrupts(reason));
    }

    [Theory]
    [InlineData(PowerModes.Suspend, true)]
    [InlineData(PowerModes.Resume, false)]
    [InlineData(PowerModes.StatusChange, false)]
    public void SystemSuspendInterrupts_ResumeDoesNot(PowerModes mode, bool interrupts)
    {
        Assert.Equal(interrupts, Win32SessionInterruptionSource.Interrupts(mode));
    }

    [Fact]
    public void RuntimeFailure_RemovesTheEffect_DetachesItsCallbacks_AndIsWrittenToTheLocalLog()
    {
        var directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
        var path = Path.Combine(directory, "logs", "effect.log");
        var surface = new FailingSurface();
        var time = new ManualTimeProvider();
        var log = new LoggingEffectFailureReporter(new EffectFailureLog(path).Append, time);
        var channel = new EffectChannelController(surface, log, time, action => action());

        try
        {
            var instance = channel.Start(new StartEffectCommand(
                Confetti, EffectPlacement.Monitor, Left, new ScreenPoint(10, 10), 0, 0));
            Assert.NotNull(instance);

            surface.Playback!.Fail("The effect could not be drawn: Skia failed.");
            log.Dispose();

            Assert.Null(channel.ActiveInstance);
            Assert.True(surface.Playback.Disposed);
            Assert.Equal(0, surface.Playback.Subscribers);
            Assert.Equal(0, time.ActiveTimerCount);
            var line = Assert.Single(File.ReadAllLines(path));
            Assert.EndsWith(
                $"\teffect=confetti\tinstance={instance.Id.Value}\treason=The effect could not be drawn: Skia failed.",
                line);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private sealed class Harness
    {
        private readonly Queue<Action> ui = new();
        private readonly FakeKeyboardHookSource keyboard = new();
        private readonly FakeDisplayChangeSource displays = new();
        private readonly FakeSessionInterruptionSource sessions = new();
        private readonly EffectMonitorGuard guard;

        public Harness()
        {
            var cursor = new FakeCursorPositionSource(new ScreenPoint(10, 10));
            var monitors = new FakeMonitorBoundsSource(Left, Right);
            var mouse = new FakeMouseHookSource();
            EffectChannel = new EffectChannelController(Effects, Effects, Time, ui.Enqueue);
            guard = new EffectMonitorGuard(mouse, displays, sessions, monitors, cursor, EffectChannel, ui.Enqueue);
            Sound = new SoundChannelController(Players, new NoSoundFailures(), new ManualTimeProvider(), action => action());
            Controller = new DrawingSessionController(() => Settings.Style);
            Controller.StateChanged += States.Add;
            var settings = new ActiveSettings(
                Settings, reference => @"C:\library\" + reference.LibraryFileName);
            _ = new GlobalShortcutAdapter(
                keyboard,
                keyboard,
                Controller,
                new DrawingModeInputGate(),
                cursor,
                ui.Enqueue,
                monitors,
                ShortcutBindings.FromSnapshot(settings.Current, settings.CommandFor),
                Sound.Play,
                guard);
        }

        public RecordingEffectSurface Effects { get; } = new();

        public ManualTimeProvider Time { get; } = new();

        public EffectChannelController EffectChannel { get; }

        public FakePlayerFactory Players { get; } = new();

        public SoundChannelController Sound { get; }

        public DrawingSessionController Controller { get; }

        public List<DrawingState> States { get; } = [];

        public void Press(params int[] keys) => keyboard.Press(keys);

        public void Release(params int[] keys) => keyboard.Release(keys);

        /// <summary>Presses Ctrl+Alt+3 and runs the start; Ctrl, Alt, and 3 stay down.</summary>
        public EffectInstance StartConfetti()
        {
            Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu, VirtualKeys.D3);
            RunUi();
            var running = Assert.Single(Effects.Shown);
            Assert.Equal(Confetti, running.Command.Effect);
            return running;
        }

        /// <summary>Raises <paramref name="name"/> as the Windows adapters and <c>App.xaml.cs</c> deliver it.</summary>
        public void Raise(string name)
        {
            switch (name)
            {
                case StopEffects:
                    // App.xaml.cs routes the tray command to the guard.
                    guard.StopEffects();
                    break;
                case DisplayChange:
                    displays.Change();
                    break;
                case SessionLock:
                case SystemSuspend:
                    sessions.Interrupt();
                    break;
                case Exit:
                    // TrayApplication disposes the channel with the other resources.
                    new CompositeDisposable(EffectChannel).Dispose();
                    break;
                case StartupFailed:
                    StartupFailure.Handle(
                        () => CleanupSteps.RunAll([EffectChannel.Dispose]),
                        failure => throw failure,
                        () => { },
                        () => { });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(name), name, "Unknown lifecycle event.");
            }
        }

        public void RunUi()
        {
            while (ui.TryDequeue(out var action))
            {
                action();
            }
        }
    }

    private sealed class NoSoundFailures : ISoundFailureReporter
    {
        public void Report(PlaySoundCommand command, string reason) =>
            throw new InvalidOperationException($"Unexpected sound failure: {reason}");
    }

    private sealed class FakePlayerFactory : ISoundPlayerFactory
    {
        public List<FakePlayer> Created { get; } = [];

        public ISoundPlayer Create(PlaySoundCommand command)
        {
            var player = new FakePlayer();
            Created.Add(player);
            return player;
        }
    }

    private sealed class FakePlayer : ISoundPlayer
    {
        public event Action? Completed { add { } remove { } }

        public event Action<string>? Failed { add { } remove { } }

        public List<string> Calls { get; } = [];

        public void Play() => Calls.Add("play");

        public void Stop() => Calls.Add("stop");

        public void Dispose() => Calls.Add("dispose");
    }

    /// <summary>Shows one playback whose rendering a test can fail, counting its subscribers.</summary>
    private sealed class FailingSurface : IEffectSurface
    {
        public Playback? Playback { get; private set; }

        public IEffectPlayback Show(EffectInstance instance) => Playback = new Playback();
    }

    private sealed class Playback : IEffectPlayback
    {
        private Action? completed;
        private Action<string>? failed;

        public event Action? Completed
        {
            add => completed += value;
            remove => completed -= value;
        }

        public event Action<string>? Failed
        {
            add => failed += value;
            remove => failed -= value;
        }

        public int Subscribers =>
            (completed?.GetInvocationList().Length ?? 0) + (failed?.GetInvocationList().Length ?? 0);

        public bool Disposed { get; private set; }

        public void Fail(string reason) => failed?.Invoke(reason);

        public void Dispose() => Disposed = true;
    }
}
