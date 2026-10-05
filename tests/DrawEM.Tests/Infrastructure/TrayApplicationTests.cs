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

    private sealed class FakeTrayHost(List<string> calls, bool throwOnDispose = false) : ITrayHost
    {
        public event Action? ExitRequested;

        public event Action? SettingsRequested;

        public void Show() => calls.Add("tray shown");

        public void Dispose()
        {
            calls.Add("tray disposed");
            if (throwOnDispose)
            {
                throw new InvalidOperationException("Tray icon removal failed.");
            }
        }

        public void RequestExit() => ExitRequested?.Invoke();

        public void RequestSettings() => SettingsRequested?.Invoke();
    }

    private sealed class FakeDisposable(List<string> calls, string name) : IDisposable
    {
        public void Dispose() => calls.Add(name);
    }

    private sealed class FakeOverlay(List<string> calls, bool throwOnClose = false) : IOverlayLifetime
    {
        public void Close()
        {
            calls.Add("overlay");
            if (throwOnClose)
            {
                throw new InvalidOperationException("Overlay close failed.");
            }
        }
    }

    private sealed class FakeApplicationLifetime(List<string> calls) : IApplicationLifetime
    {
        public void Shutdown() => calls.Add("shutdown");
    }
}
