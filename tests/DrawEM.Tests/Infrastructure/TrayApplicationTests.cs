using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Sound;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

namespace DrawEM.Tests.Infrastructure;

public class TrayApplicationTests
{
    [Fact]
    public void SoundTimeout_CleansRemainingResourcesAndDrainsLogBeforeForcedExit()
    {
        var calls = new List<string>();
        var records = new ConcurrentQueue<string>();
        var log = new AppFailureLog(records.Enqueue);
        var policy = new ApplicationExitPolicy(log, code =>
        {
            Assert.Contains(records, line => line.Contains("sound thread did not stop"));
            Assert.Equal(1, code);
            calls.Add("forced exit");
        });
        var application = new TrayApplication(
            new FakeTrayHost(calls),
            new CompositeDisposable(new TimedOutSound(calls), new FakeDisposable(calls, "sound log")),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            failure => policy.Report("exit", failure));

        application.Exit();
        policy.Complete();

        Assert.Equal(["sound", "sound log", "overlay", "tray disposed", "shutdown", "forced exit"], calls);
    }

    private sealed class TimedOutSound(List<string> calls) : IDisposable
    {
        public void Dispose()
        {
            calls.Add("sound");
            throw new SoundChannelShutdownTimeoutException();
        }
    }

    [Fact]
    public void Start_ExposesTrayExit_WhichUnregistersShortcutsClosesOverlayAndShutsDown()
    {
        var calls = new List<string>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);

        application.Start();
        tray.RequestExit();

        Assert.Equal(["tray shown", "shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void Exit_IsIdempotent()
    {
        var calls = new List<string>();
        var application = new TrayApplication(
            new FakeTrayHost(calls),
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);

        application.Exit();
        application.Exit();

        Assert.Equal(["shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void Dispose_ReleasesResourcesWithoutRequestingApplicationShutdown()
    {
        var calls = new List<string>();
        var application = new TrayApplication(
            new FakeTrayHost(calls),
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);

        application.Dispose();

        Assert.Equal(["shortcuts", "overlay", "tray disposed"], calls);
    }

    [Fact]
    public void TraySettings_OpensSettingsUntilExit()
    {
        var calls = new List<string>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);
        application.SettingsRequested += () => calls.Add("settings");
        application.Start();

        tray.RequestSettings();
        application.Exit();
        tray.RequestSettings();

        Assert.Equal(["tray shown", "settings", "shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void TrayStopEffects_StopsEffectsUntilExit_AndReleasesNothingElse()
    {
        var calls = new List<string>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);
        application.StopEffectsRequested += () => calls.Add("stop effects");
        application.Start();

        tray.RequestStopEffects();
        tray.RequestStopEffects();

        Assert.Equal(["tray shown", "stop effects", "stop effects"], calls);

        application.Exit();
        tray.RequestStopEffects();

        Assert.Equal(
            ["tray shown", "stop effects", "stop effects", "shortcuts", "overlay", "tray disposed", "shutdown"],
            calls);
    }

    [Fact]
    public void TrayUpdate_IsForwardedUntilExit()
    {
        var calls = new List<string>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);
        application.UpdateRequested += () => calls.Add("update");

        tray.RequestUpdate();
        application.Exit();
        tray.RequestUpdate();

        Assert.Equal(["update", "shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void TrayUpdate_IsNotForwardedAfterDispose()
    {
        var calls = new List<string>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);
        application.UpdateRequested += () => calls.Add("update");

        application.Dispose();
        tray.RequestUpdate();

        Assert.Equal(["shortcuts", "overlay", "tray disposed"], calls);
    }

    [Fact]
    public void ShowUpdateReady_ReachesTheTrayUntilExit()
    {
        var calls = new List<string>();
        var application = new TrayApplication(
            new FakeTrayHost(calls),
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);

        application.ShowUpdateReady("1.2.3");
        application.Exit();
        application.ShowUpdateReady("1.2.4");

        Assert.Equal(["update ready 1.2.3", "shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void TrayExit_StopsEffectsAtOnce_BeforeTheScheduledExitRuns()
    {
        var calls = new List<string>();
        var scheduledActions = new Queue<Action>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            scheduledActions.Enqueue,
            ExceptionDispatchInfo.Throw);
        application.StopEffectsRequested += () => calls.Add("stop effects");

        tray.RequestExit();

        Assert.Equal(["stop effects"], calls);

        scheduledActions.Dequeue()();

        Assert.Equal(["stop effects", "shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void TrayExit_IsScheduledAfterTheTrayCallbackReturns()
    {
        var calls = new List<string>();
        var scheduledActions = new Queue<Action>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            scheduledActions.Enqueue,
            ExceptionDispatchInfo.Throw);

        tray.RequestExit();

        Assert.Empty(calls);
        Assert.Single(scheduledActions);

        scheduledActions.Dequeue().Invoke();

        Assert.Equal(["shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void Exit_WhenResourceReleaseThrows_StillClosesOverlayAndShutsDown()
    {
        var calls = new List<string>();
        var application = new TrayApplication(
            new FakeTrayHost(calls),
            new ThrowingDisposable(calls),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);

        Assert.Throws<InvalidOperationException>(application.Exit);

        Assert.Equal(["shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void Exit_WhenOverlayAndTrayCloseThrow_StillShutsDown()
    {
        var calls = new List<string>();
        var application = new TrayApplication(
            new FakeTrayHost(calls, throwOnDispose: true),
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls, throwOnClose: true),
            new FakeApplicationLifetime(calls),
            action => action(),
            ExceptionDispatchInfo.Throw);

        var thrown = Assert.Throws<AggregateException>(application.Exit);

        Assert.Equal(2, thrown.InnerExceptions.Count);
        Assert.Equal(["shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void TrayExit_WhenResourceReleaseThrows_ReportsFailureAndStillShutsDown()
    {
        var calls = new List<string>();
        var reported = new List<Exception>();
        var scheduledActions = new Queue<Action>();
        var tray = new FakeTrayHost(calls);
        _ = new TrayApplication(
            tray,
            new ThrowingDisposable(calls),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            scheduledActions.Enqueue,
            reported.Add);

        tray.RequestExit();
        var exit = scheduledActions.Dequeue();
        var exception = Record.Exception(exit);

        Assert.Null(exception);
        Assert.IsType<InvalidOperationException>(Assert.Single(reported));
        Assert.Equal(["shortcuts", "overlay", "tray disposed", "shutdown"], calls);
    }

    [Fact]
    public void Dispose_WhenResourceReleaseThrows_ReportsFailureWithoutThrowing()
    {
        var calls = new List<string>();
        var reported = new List<Exception>();
        var application = new TrayApplication(
            new FakeTrayHost(calls),
            new ThrowingDisposable(calls),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls),
            action => action(),
            reported.Add);

        var exception = Record.Exception(application.Dispose);

        Assert.Null(exception);
        Assert.Single(reported);
        Assert.Equal(["shortcuts", "overlay", "tray disposed"], calls);
    }

    private sealed class ThrowingDisposable(List<string> calls) : IDisposable
    {
        public void Dispose()
        {
            calls.Add("shortcuts");
            throw new InvalidOperationException("Sound stop failed.");
        }
    }
}
