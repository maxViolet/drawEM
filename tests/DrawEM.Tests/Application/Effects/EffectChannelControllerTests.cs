using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;

namespace DrawEM.Tests.Application.Effects;

public class EffectChannelControllerTests
{
    private static readonly EffectId Confetti = new("confetti");
    private static readonly EffectId FocusRing = new("focus-ring");
    private static readonly MonitorBounds Primary = new(0, 0, 1920, 1080);

    [Fact]
    public void Start_ShowsInstanceOnSurfaceAndBecomesActive()
    {
        var fixture = new Fixture();
        var command = new StartEffectCommand(Confetti, EffectPlacement.Monitor, Primary, new ScreenPoint(10, 20), 7);

        var instance = fixture.Controller.Start(command);

        var playback = Assert.Single(fixture.Playbacks);
        Assert.NotNull(instance);
        Assert.Same(instance, playback.Instance);
        Assert.Equal(command, instance.Command);
        Assert.Equal(fixture.Time.GetUtcNow(), instance.StartedAt);
        Assert.Same(instance, fixture.Controller.ActiveInstance);
    }

    [Fact]
    public void Start_GivesEachInvocationAUniqueId()
    {
        var fixture = new Fixture();

        var first = fixture.Controller.Start(Command(Confetti));
        var second = fixture.Controller.Start(Command(Confetti));

        Assert.NotEqual(first!.Id, second!.Id);
    }

    [Theory]
    [InlineData(EffectPlacement.Monitor, EffectPlacement.Cursor)]
    [InlineData(EffectPlacement.Cursor, EffectPlacement.Monitor)]
    [InlineData(EffectPlacement.Monitor, EffectPlacement.Monitor)]
    [InlineData(EffectPlacement.Cursor, EffectPlacement.Cursor)]
    public void StartingAnotherEffect_ReleasesPreviousBeforeShowingNext(EffectPlacement first, EffectPlacement second)
    {
        var fixture = new Fixture();

        fixture.Controller.Start(Command(Confetti, first));
        fixture.Controller.Start(Command(FocusRing, second));

        Assert.Equal(["show confetti", "dispose confetti", "show focus-ring"], fixture.Log);
        Assert.Equal(second, fixture.Playbacks[1].Instance.Command.Placement);
        Assert.Equal(FocusRing, fixture.Controller.ActiveInstance?.Command.Effect);
    }

    [Fact]
    public void StartingSameEffectAgain_RestartsWithNewInstanceAndDeadline()
    {
        var fixture = new Fixture();

        fixture.Controller.Start(Command(Confetti));
        fixture.Time.Advance(TimeSpan.FromSeconds(6));
        var second = fixture.Controller.Start(Command(Confetti));
        fixture.Time.Advance(TimeSpan.FromSeconds(6));

        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Equal(0, fixture.Playbacks[1].DisposeCount);
        Assert.Same(second, fixture.Controller.ActiveInstance);

        fixture.Time.Advance(TimeSpan.FromSeconds(4));

        Assert.Equal(1, fixture.Playbacks[1].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
    }

    [Fact]
    public void Completion_ReleasesPlaybackAndCancelsDeadline()
    {
        var fixture = new Fixture();
        fixture.Controller.Start(Command(Confetti));

        fixture.Playbacks[0].Complete();

        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
        Assert.Empty(fixture.Failures);
    }

    [Fact]
    public void Deadline_ReleasesPlaybackTenSecondsAfterStart()
    {
        var fixture = new Fixture();
        fixture.Controller.Start(Command(Confetti));

        fixture.Time.Advance(EffectChannelController.MaxDuration - TimeSpan.FromMilliseconds(1));
        Assert.Equal(0, fixture.Playbacks[0].DisposeCount);

        fixture.Time.Advance(TimeSpan.FromMilliseconds(1));

        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(TimeSpan.FromSeconds(10), EffectChannelController.MaxDuration);
    }

    [Fact]
    public void Deadline_CountsTimeSpentInsideShow()
    {
        var fixture = new Fixture();
        fixture.OnShow = () => fixture.Time.Advance(TimeSpan.FromSeconds(3));
        var instance = fixture.Controller.Start(Command(Confetti));

        fixture.Time.Advance(TimeSpan.FromSeconds(7) - TimeSpan.FromMilliseconds(1));
        Assert.Same(instance, fixture.Controller.ActiveInstance);

        fixture.Time.Advance(TimeSpan.FromMilliseconds(1));

        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
    }

    [Fact]
    public void ShowOutlastingDeadline_ReleasesPlaybackImmediately()
    {
        var fixture = new Fixture();
        fixture.OnShow = () => fixture.Time.Advance(EffectChannelController.MaxDuration);

        fixture.Controller.Start(Command(Confetti));

        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
        Assert.Empty(fixture.Failures);
    }

    [Fact]
    public void Deadline_IsDispatchedToControllerThread()
    {
        var queued = new List<Action>();
        var fixture = new Fixture(queued.Add);
        fixture.Controller.Start(Command(Confetti));

        fixture.Time.Advance(EffectChannelController.MaxDuration);

        Assert.Equal(0, fixture.Playbacks[0].DisposeCount);
        Assert.Single(queued)();
        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
    }

    [Fact]
    public void StaleDeadline_QueuedBeforeReplacement_DoesNotStopNewerEffect()
    {
        var queued = new List<Action>();
        var fixture = new Fixture(queued.Add);
        fixture.Controller.Start(Command(Confetti, EffectPlacement.Monitor));
        fixture.Time.Advance(EffectChannelController.MaxDuration);

        var newer = fixture.Controller.Start(Command(FocusRing, EffectPlacement.Cursor));
        Assert.Single(queued)();

        Assert.Same(newer, fixture.Controller.ActiveInstance);
        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Equal(0, fixture.Playbacks[1].DisposeCount);
    }

    [Fact]
    public void StaleCompletionAndFailure_FromReplacedPlayback_DoNotStopNewerEffect()
    {
        var fixture = new Fixture();
        fixture.Controller.Start(Command(Confetti));
        var old = fixture.Playbacks[0];
        var newer = fixture.Controller.Start(Command(FocusRing));

        old.Complete();
        old.Fail("late failure");

        Assert.Same(newer, fixture.Controller.ActiveInstance);
        Assert.Equal(1, old.DisposeCount);
        Assert.Equal(0, fixture.Playbacks[1].DisposeCount);
        Assert.Empty(fixture.Failures);
    }

    [Fact]
    public void StaleCallbacks_RaisedThroughCapturedHandlers_DoNotStopNewerEffect()
    {
        var fixture = new Fixture();
        fixture.Controller.Start(Command(Confetti));
        var old = fixture.Playbacks[0];
        var completed = old.CompletedHandler!;
        var failed = old.FailedHandler!;
        var newer = fixture.Controller.Start(Command(FocusRing));

        completed();
        failed("late failure");

        Assert.Same(newer, fixture.Controller.ActiveInstance);
        Assert.Equal(1, old.DisposeCount);
        Assert.Equal(0, fixture.Playbacks[1].DisposeCount);
        Assert.Empty(fixture.Failures);
    }

    [Fact]
    public void StopById_OfReplacedInstance_DoesNotStopNewerEffect()
    {
        var fixture = new Fixture();
        var old = fixture.Controller.Start(Command(Confetti))!;
        var newer = fixture.Controller.Start(Command(FocusRing))!;

        fixture.Controller.Stop(old.Id);

        Assert.Same(newer, fixture.Controller.ActiveInstance);
        Assert.Equal(0, fixture.Playbacks[1].DisposeCount);

        fixture.Controller.Stop(newer.Id);

        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(1, fixture.Playbacks[1].DisposeCount);
    }

    [Fact]
    public void Failure_ReleasesPlaybackThenReports()
    {
        var fixture = new Fixture();
        var instance = fixture.Controller.Start(Command(Confetti));

        fixture.Playbacks[0].Fail("bad frame");

        Assert.Equal(["show confetti", "dispose confetti", "report confetti"], fixture.Log);
        Assert.Equal([(instance!, "bad frame")], fixture.Failures);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void FailureThenReleaseFailure_ReportsBothInOrderAfterRelease()
    {
        var fixture = new Fixture();
        var instance = fixture.Controller.Start(Command(Confetti));
        fixture.Playbacks[0].DisposeFailure = "window gone";

        fixture.Playbacks[0].Fail("bad frame");

        Assert.Equal(
            ["show confetti", "dispose confetti", "report confetti", "report confetti"],
            fixture.Log);
        Assert.Equal([(instance!, "bad frame"), (instance!, "window gone")], fixture.Failures);
        Assert.Null(fixture.Controller.ActiveInstance);
    }

    [Fact]
    public void ShowFailure_ReportsAndLeavesChannelIdleAndUsable()
    {
        var fixture = new Fixture();
        fixture.FailShowFor(Confetti, "missing resource");

        var failed = fixture.Controller.Start(Command(Confetti));

        Assert.Null(failed);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
        var failure = Assert.Single(fixture.Failures);
        Assert.Equal(Confetti, failure.Instance.Command.Effect);
        Assert.Equal("missing resource", failure.Reason);

        Assert.NotNull(fixture.Controller.Start(Command(FocusRing)));
    }

    [Fact]
    public void ReleaseFailure_IsReportedAndChannelStaysIdle()
    {
        var fixture = new Fixture();
        var instance = fixture.Controller.Start(Command(Confetti));
        fixture.Playbacks[0].DisposeFailure = "window gone";

        fixture.Controller.Stop();

        Assert.Equal([(instance!, "window gone")], fixture.Failures);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.NotNull(fixture.Controller.Start(Command(FocusRing)));
    }

    [Fact]
    public void Stop_IsSafeToRepeatAndReleasesOnce()
    {
        var fixture = new Fixture();
        fixture.Controller.Stop();
        fixture.Controller.Start(Command(Confetti));

        fixture.Controller.Stop();
        fixture.Controller.Stop();
        fixture.Playbacks[0].Complete();
        fixture.Time.Advance(EffectChannelController.MaxDuration);

        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void Dispose_ReleasesActiveOnceAndIgnoresLaterStarts()
    {
        var fixture = new Fixture();
        fixture.Controller.Start(Command(Confetti));

        fixture.Controller.Dispose();
        fixture.Controller.Dispose();
        var late = fixture.Controller.Start(Command(FocusRing));

        Assert.Null(late);
        Assert.Single(fixture.Playbacks);
        Assert.Equal(1, fixture.Playbacks[0].DisposeCount);
        Assert.Null(fixture.Controller.ActiveInstance);
    }

    private static StartEffectCommand Command(EffectId effect, EffectPlacement placement = EffectPlacement.Monitor) =>
        new(effect, placement, Primary, new ScreenPoint(100, 100), 1);

    private sealed class Fixture : IEffectSurface, IEffectFailureReporter
    {
        private readonly Dictionary<EffectId, string> showFailures = [];

        public Fixture(Action<Action>? dispatch = null)
        {
            Controller = new EffectChannelController(this, this, Time, dispatch ?? (action => action()));
        }

        public EffectChannelController Controller { get; }

        public ManualTimeProvider Time { get; } = new();

        public List<FakePlayback> Playbacks { get; } = [];

        public List<string> Log { get; } = [];

        public List<(EffectInstance Instance, string Reason)> Failures { get; } = [];

        public Action? OnShow { get; set; }

        public void FailShowFor(EffectId effect, string reason) => showFailures[effect] = reason;

        public IEffectPlayback Show(EffectInstance instance)
        {
            var effect = instance.Command.Effect;
            Log.Add("show " + effect.Value);
            OnShow?.Invoke();
            if (showFailures.TryGetValue(effect, out var reason))
            {
                throw new EffectPlaybackException(reason);
            }

            var playback = new FakePlayback(instance, Log);
            Playbacks.Add(playback);
            return playback;
        }

        public void Report(EffectInstance instance, string reason)
        {
            Log.Add("report " + instance.Command.Effect.Value);
            Failures.Add((instance, reason));
        }
    }

    private sealed class FakePlayback(EffectInstance instance, List<string> log) : IEffectPlayback
    {
        public event Action? Completed
        {
            add => CompletedHandler += value;
            remove => CompletedHandler -= value;
        }

        public event Action<string>? Failed
        {
            add => FailedHandler += value;
            remove => FailedHandler -= value;
        }

        public Action? CompletedHandler { get; private set; }

        public Action<string>? FailedHandler { get; private set; }

        public EffectInstance Instance { get; } = instance;

        public int DisposeCount { get; private set; }

        public string? DisposeFailure { get; set; }

        public void Complete() => CompletedHandler?.Invoke();

        public void Fail(string reason) => FailedHandler?.Invoke(reason);

        public void Dispose()
        {
            log.Add("dispose " + Instance.Command.Effect.Value);
            DisposeCount++;
            if (DisposeFailure is not null)
            {
                throw new EffectPlaybackException(DisposeFailure);
            }
        }
    }
}
