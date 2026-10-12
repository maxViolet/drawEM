using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Effects;
using DrawEM.Tests.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Effects;

public class EffectMonitorGuardTests
{
    private static readonly MonitorBounds Left = new(-1920, 0, 0, 1080);
    private static readonly MonitorBounds Right = new(0, 0, 2560, 1440);
    private static readonly ScreenPoint OnLeft = new(-1000, 500);
    private static readonly ScreenPoint OnRight = new(1000, 500);

    [Theory]
    [InlineData(EffectPlacement.Monitor)]
    [InlineData(EffectPlacement.Cursor)]
    public void MovingWithinTheTargetMonitor_KeepsTheEffect_AndQueuesNoUiWork(EffectPlacement placement)
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft, placement);

        fixture.Mouse.Move(new ScreenPoint(-1900, 10));
        fixture.Mouse.Move(new ScreenPoint(-1, 1079));

        Assert.Empty(fixture.Queued);
        Assert.NotNull(fixture.Channel.ActiveInstance);
        Assert.Equal(OnLeft, fixture.Channel.ActiveInstance.Command.CursorPoint);
    }

    [Theory]
    [InlineData(EffectPlacement.Monitor)]
    [InlineData(EffectPlacement.Cursor)]
    public void EnteringAnotherMonitor_StopsTheEffect_OfEitherPlacement(EffectPlacement placement)
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft, placement);

        fixture.Mouse.Move(OnRight);
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(1, fixture.Surface.Disposed);
    }

    [Fact]
    public void LeavingAndReturningBeforeTheUiRuns_StillStopsTheEffect_AndNeverRestartsIt()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);

        fixture.Mouse.Move(OnRight);
        fixture.Mouse.Move(OnLeft);
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(1, fixture.Surface.Shown);
    }

    [Fact]
    public void TouchingAnOutsideDesktopEdge_DoesNotStopTheEffect()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);

        fixture.Mouse.Move(new ScreenPoint(-1925, 500));
        fixture.Mouse.Move(new ScreenPoint(-1000, 1090));
        fixture.RunQueued();

        Assert.NotNull(fixture.Channel.ActiveInstance);
    }

    [Fact]
    public void WithNoEffectRunningOrQueued_PointerMovesDoNoMonitorLookup()
    {
        var fixture = new Fixture();

        fixture.Mouse.Move(OnLeft);
        fixture.Mouse.Move(OnRight);

        Assert.Equal(0, fixture.Monitors.Queries);
        Assert.Empty(fixture.Queued);
    }

    [Fact]
    public void AfterTheEffectEnds_PointerMovesDoNoMonitorLookup()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);
        fixture.Surface.Last!.Complete();
        var queries = fixture.Monitors.Queries;

        fixture.Mouse.Move(OnRight);
        fixture.Mouse.Move(OnLeft);

        Assert.Equal(queries, fixture.Monitors.Queries);
        Assert.Empty(fixture.Queued);
    }

    [Fact]
    public void PressWithNothingRunningOrQueued_DoesNoMonitorLookupInTheHook()
    {
        var fixture = new Fixture();

        fixture.Guard.Enqueue(OnLeft);

        Assert.Equal(0, fixture.Monitors.Queries);
    }

    [Fact]
    public void PressOnAnotherMonitorWhileAnEffectRuns_StopsIt_EvenIfThatPressIsDropped()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);

        // The hook reported no move to the right monitor; the press there is the first sign of it.
        var epoch = fixture.Guard.Enqueue(OnRight);
        fixture.Guard.Dequeue(epoch, null);
        fixture.Cursor.Position = OnRight;
        fixture.Guard.CheckCursor();
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
    }

    [Fact]
    public void TwoQueuedPressesOnDifferentMonitors_OnlyTheLaterOneStarts()
    {
        var fixture = new Fixture();
        var first = fixture.Guard.Enqueue(OnLeft);
        var second = fixture.Guard.Enqueue(OnRight);

        fixture.Guard.Dequeue(first, fixture.Command(OnLeft, first));
        Assert.Null(fixture.Channel.ActiveInstance);

        fixture.Guard.Dequeue(second, fixture.Command(OnRight, second));
        fixture.RunQueued();

        Assert.Equal(Right, fixture.Channel.ActiveInstance?.Command.Monitor);
    }

    [Fact]
    public void QueuedStart_CursorEntersAnotherMonitorBeforeDispatch_StartsNothing()
    {
        var fixture = new Fixture();
        var epoch = fixture.Guard.Enqueue(OnLeft);

        fixture.Mouse.Move(OnRight);
        fixture.Guard.Dequeue(epoch, fixture.Command(OnLeft, epoch));
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(0, fixture.Surface.Shown);
    }

    [Fact]
    public void QueuedStart_CursorStaysOnItsMonitor_Starts()
    {
        var fixture = new Fixture();
        var epoch = fixture.Guard.Enqueue(OnLeft);

        fixture.Mouse.Move(new ScreenPoint(-10, 10));
        fixture.Guard.Dequeue(epoch, fixture.Command(OnLeft, epoch));

        Assert.NotNull(fixture.Channel.ActiveInstance);
    }

    [Fact]
    public void TransitionBeforeAPress_DoesNotInvalidateThatPress()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);
        fixture.Mouse.Move(OnRight);

        var epoch = fixture.Guard.Enqueue(OnRight);
        fixture.Guard.Dequeue(epoch, fixture.Command(OnRight, epoch));
        fixture.RunQueued();

        Assert.Equal(Right, fixture.Channel.ActiveInstance?.Command.Monitor);
    }

    [Fact]
    public void NoTargetMonitor_StartsNothing_AndStopsObserving()
    {
        var fixture = new Fixture();
        var epoch = fixture.Guard.Enqueue(new ScreenPoint(9000, 9000));

        fixture.Guard.Dequeue(epoch, null);
        var queries = fixture.Monitors.Queries;
        fixture.Mouse.Move(OnRight);

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(queries, fixture.Monitors.Queries);
    }

    [Fact]
    public void FrameCheck_CatchesMovementTheHookDidNotReport()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);
        fixture.Cursor.Position = OnRight;

        fixture.Guard.CheckCursor();
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
    }

    [Fact]
    public void FrameCheck_OnTheTargetMonitor_QueuesNoUiWork()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);
        fixture.Cursor.Position = new ScreenPoint(-5, 5);

        fixture.Guard.CheckCursor();

        Assert.Empty(fixture.Queued);
        Assert.NotNull(fixture.Channel.ActiveInstance);
    }

    [Fact]
    public void FrameCheck_WithNoEffect_ReadsNothing()
    {
        var fixture = new Fixture();
        fixture.Cursor.Position = OnRight;

        fixture.Guard.CheckCursor();

        Assert.Equal(0, fixture.Cursor.Reads);
        Assert.Equal(0, fixture.Monitors.Queries);
    }

    [Fact]
    public void TopologyChange_StopsTheEffect_AndInvalidatesAQueuedStart()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);
        var epoch = fixture.Guard.Enqueue(new ScreenPoint(-20, 20));

        fixture.Displays.Change();
        fixture.Guard.Dequeue(epoch, fixture.Command(new ScreenPoint(-20, 20), epoch));
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(1, fixture.Surface.Shown);
    }

    [Fact]
    public void AfterATopologyChange_TheNextEffectUsesTheNewLayout()
    {
        var fixture = new Fixture();
        fixture.StartAt(OnLeft);
        fixture.Displays.Change();
        fixture.Monitors.Replace(new MonitorBounds(-1920, 0, 0, 1080), new MonitorBounds(0, -1440, 2560, 0));

        fixture.StartAt(new ScreenPoint(-10, 10));
        fixture.Mouse.Move(new ScreenPoint(1000, -500));
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(2, fixture.Surface.Shown);
    }

    private sealed class Fixture
    {
        public Fixture()
        {
            Channel = new EffectChannelController(Surface, Surface, new ManualTimeProvider(), action => action());
            Guard = new EffectMonitorGuard(Mouse, Displays, Monitors, Cursor, Channel, Queued.Enqueue);
        }

        public FakeMouseHookSource Mouse { get; } = new();

        public FakeDisplays Displays { get; } = new();

        public CountingMonitors Monitors { get; } = new(Left, Right);

        public SettableCursor Cursor { get; } = new();

        public RecordingSurface Surface { get; } = new();

        public Queue<Action> Queued { get; } = new();

        public EffectChannelController Channel { get; }

        public EffectMonitorGuard Guard { get; }

        public StartEffectCommand Command(ScreenPoint point, long epoch, EffectPlacement placement = EffectPlacement.Monitor)
        {
            Assert.True(Monitors.TryGetBounds(point, out var monitor));
            var effect = placement == EffectPlacement.Monitor ? "confetti" : "focus-ring";
            return new StartEffectCommand(new EffectId(effect), placement, monitor, point, 0, epoch);
        }

        /// <summary>Queues and dispatches a start, as a shortcut press does, and checks it started.</summary>
        public void StartAt(ScreenPoint point, EffectPlacement placement = EffectPlacement.Monitor)
        {
            Cursor.Position = point;
            var epoch = Guard.Enqueue(point);
            Guard.Dequeue(epoch, Command(point, epoch, placement));
            Assert.NotNull(Channel.ActiveInstance);
        }

        public void RunQueued()
        {
            while (Queued.TryDequeue(out var action))
            {
                action();
            }
        }
    }

    private sealed class FakeDisplays : IDisplayChangeSource
    {
        public event Action? DisplayChanged;

        public void Change() => DisplayChanged?.Invoke();
    }

    private sealed class RecordingSurface : IEffectSurface, IEffectFailureReporter
    {
        public int Shown { get; private set; }

        public int Disposed { get; private set; }

        public Playback? Last { get; private set; }

        public IEffectPlayback Show(EffectInstance instance)
        {
            Shown++;
            Last = new Playback(this);
            return Last;
        }

        public void Report(EffectInstance instance, string reason) =>
            throw new InvalidOperationException($"Unexpected effect failure: {reason}");

        public sealed class Playback(RecordingSurface owner) : IEffectPlayback
        {
            public event Action? Completed;

            public event Action<string>? Failed
            {
                add { }
                remove { }
            }

            public void Complete() => Completed?.Invoke();

            public void Dispose() => owner.Disposed++;
        }
    }
}
