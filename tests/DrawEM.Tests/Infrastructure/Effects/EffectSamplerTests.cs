using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Infrastructure.Effects;
using DrawEM.Tests.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Effects;

public class EffectSamplerTests
{
    private static readonly MonitorBounds Left = new(-1920, 0, 0, 1080);
    private static readonly MonitorBounds Right = new(0, 0, 2560, 1440);
    private static readonly ScreenPoint OnLeft = new(-1000, 500);
    private static readonly ScreenPoint OnRight = new(1000, 500);
    private static readonly EffectId Confetti = new("confetti");
    private static readonly EffectId FocusRing = new("focus-ring");

    [Theory]
    [InlineData("confetti", EffectPlacement.Monitor)]
    [InlineData("focus-ring", EffectPlacement.Cursor)]
    public void Sample_StartsOnTheCursorsMonitor_WithTheEffectsPlacement(string id, EffectPlacement placement)
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        fixture.Cursor.Position = OnRight;

        editor.Sample(new EffectId(id));

        var command = fixture.Channel.ActiveInstance!.Command;
        Assert.Equal(new EffectId(id), command.Effect);
        Assert.Equal(placement, command.Placement);
        Assert.Equal(Right, command.Monitor);
        Assert.Equal(OnRight, command.CursorPoint);
        Assert.True(command.ConfigurationGeneration < 0, "A sample must not look like a configuration's start.");
        Assert.Empty(editor.Failures);
    }

    [Fact]
    public void StopSamples_StopsTheEditorsCurrentSample()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);
        editor.Sample(FocusRing);

        editor.Sampler.StopSamples();

        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(2, fixture.Surface.Disposed);
    }

    [Fact]
    public void StopSamples_AfterAShortcutReplacedTheSample_KeepsTheShortcutEffect()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);
        var shortcut = fixture.StartFromShortcut(FocusRing, EffectPlacement.Cursor);

        editor.Sampler.StopSamples();

        Assert.Same(shortcut, fixture.Channel.ActiveInstance);
    }

    [Fact]
    public void StopSamples_AfterAnotherEditorsSampleReplacedIt_KeepsTheNewerSample()
    {
        var fixture = new Fixture();
        var older = fixture.Editor();
        var newer = fixture.Editor();
        older.Sample(Confetti);
        newer.Sample(FocusRing);
        var running = fixture.Channel.ActiveInstance;

        older.Sampler.StopSamples();
        fixture.Surface.Last!.Fail("bad frame");

        Assert.NotNull(running);
        Assert.Equal(FocusRing, running.Command.Effect);
        Assert.Empty(older.Failures);
        Assert.Equal(["bad frame"], newer.Failures);
    }

    [Fact]
    public void SampleThatCannotShow_ReportsInSettings_AndLeavesNothingOnScreen()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        fixture.StartFromShortcut(FocusRing, EffectPlacement.Cursor);
        fixture.Surface.ShowFailure = "cannot create the window";

        editor.Sample(Confetti);

        Assert.Equal(["cannot create the window"], editor.Failures);
        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(fixture.Surface.Shown, fixture.Surface.Disposed);
        Assert.Equal(["cannot create the window"], fixture.Log);
    }

    [Fact]
    public void SampleThatFailsWhilePlaying_ReportsInSettings_AndReleasesItsWindow()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);

        fixture.Surface.Last!.Fail("bad frame");

        Assert.Equal(["bad frame"], editor.Failures);
        Assert.Null(fixture.Channel.ActiveInstance);
        Assert.Equal(1, fixture.Surface.Disposed);
        Assert.Equal(["bad frame"], fixture.Log);
    }

    [Fact]
    public void ShortcutFailure_IsLoggedButNotShownInSettings()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);
        fixture.StartFromShortcut(FocusRing, EffectPlacement.Cursor);

        fixture.Surface.Last!.Fail("bad frame");

        Assert.Empty(editor.Failures);
        Assert.Equal(["bad frame"], fixture.Log);
    }

    [Fact]
    public void StopSamples_DoesNotShowAReleaseFailureInSettings()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);
        fixture.Surface.Last!.DisposeFailure = "engine error";

        editor.Sampler.StopSamples();

        Assert.Empty(editor.Failures);
        Assert.Equal(["engine error"], fixture.Log);
    }

    [Fact]
    public void ForgetSamples_KeepsTheSampleRunning_ButStopsReportingIt()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);

        editor.Sampler.ForgetSamples();
        editor.Sampler.StopSamples();
        fixture.Surface.Last!.Fail("bad frame");

        Assert.Empty(editor.Failures);
        Assert.Equal(["bad frame"], fixture.Log);
    }

    [Fact]
    public void Sample_WithTheCursorOutsideEveryMonitor_ReportsAndStartsNothing()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        fixture.Cursor.Position = new ScreenPoint(5000, 5000);

        editor.Sample(Confetti);

        Assert.Single(editor.Failures);
        Assert.Equal(0, fixture.Surface.Shown);
    }

    [Fact]
    public void Sample_WithoutACursorPosition_ReportsAndStartsNothing()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        fixture.Cursor.Available = false;

        editor.Sample(Confetti);

        Assert.Single(editor.Failures);
        Assert.Equal(0, fixture.Surface.Shown);
    }

    [Fact]
    public void SampleThatCannotStart_KeepsTheEarlierSampleOwned_SoStopStillStopsIt()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);
        fixture.Cursor.Available = false;

        editor.Sample(FocusRing);
        editor.Sampler.StopSamples();

        Assert.Null(fixture.Channel.ActiveInstance);
    }

    [Theory]
    [InlineData("unknown effect")]
    [InlineData("no cursor position")]
    [InlineData("cursor outside every monitor")]
    public void SampleThatCannotStart_StopsReportingTheEarlierSamplesFailures(string cause)
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(Confetti);
        var effect = FocusRing;
        switch (cause)
        {
            case "unknown effect":
                effect = new EffectId("missing");
                break;
            case "no cursor position":
                fixture.Cursor.Available = false;
                break;
            default:
                fixture.Cursor.Position = new ScreenPoint(5000, 5000);
                break;
        }

        editor.Sample(effect);
        editor.Failures.Clear();
        fixture.Surface.Last!.Fail("bad frame");

        Assert.Empty(editor.Failures);
        Assert.Equal(["bad frame"], fixture.Log);
    }

    [Fact]
    public void Sample_StopsWhenTheCursorEntersAnotherMonitor()
    {
        var fixture = new Fixture();
        var editor = fixture.Editor();
        editor.Sample(FocusRing);

        fixture.Mouse.Move(OnRight);
        fixture.RunQueued();

        Assert.Null(fixture.Channel.ActiveInstance);
    }

    private sealed class Fixture
    {
        public Fixture()
        {
            var log = new RecordingLog(Log);
            Failures = new EffectSampleFailureRouter(log);
            Channel = new EffectChannelController(Surface, Failures, new ManualTimeProvider(), action => action());
            Guard = new EffectMonitorGuard(Mouse, new FakeDisplayChangeSource(), Monitors, Cursor, Channel, Queued.Enqueue);
            Cursor.Position = OnLeft;
        }

        public FakeMouseHookSource Mouse { get; } = new();

        public FakeMonitorBoundsSource Monitors { get; } = new(Left, Right);

        public SettableCursor Cursor { get; } = new();

        public FailingSurface Surface { get; } = new();

        public List<string> Log { get; } = [];

        public Queue<Action> Queued { get; } = new();

        public EffectSampleFailureRouter Failures { get; }

        public EffectChannelController Channel { get; }

        public EffectMonitorGuard Guard { get; }

        /// <summary>One Settings window's sampler, with the failures it showed.</summary>
        public SamplingEditor Editor() => new(new EffectSampler(Cursor, Monitors, Guard, Channel, Failures));

        /// <summary>Starts an effect as a shortcut press does, at the current cursor point.</summary>
        public EffectInstance? StartFromShortcut(EffectId effect, EffectPlacement placement)
        {
            var point = Cursor.Position;
            var epoch = Guard.Enqueue(point);
            Assert.True(Monitors.TryGetBounds(point, out var monitor));
            Guard.Dequeue(epoch, new StartEffectCommand(effect, placement, monitor, point, 0, epoch));
            return Channel.ActiveInstance;
        }

        public void RunQueued()
        {
            while (Queued.TryDequeue(out var action))
            {
                action();
            }
        }
    }

    private sealed class SamplingEditor(EffectSampler sampler)
    {
        public EffectSampler Sampler { get; } = sampler;

        public List<string> Failures { get; } = [];

        public void Sample(EffectId effect) => Sampler.Sample(effect, Failures.Add);
    }

    private sealed class RecordingLog(List<string> reasons) : IEffectFailureReporter
    {
        public void Report(EffectInstance instance, string reason) => reasons.Add(reason);
    }
}
