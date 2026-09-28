using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class TrayApplicationTests
{
    [Fact]
    public void Start_ExposesTrayExit_WhichUnregistersShortcutsClosesOverlayAndShutsDown()
    {
        var calls = new List<string>();
        var tray = new FakeTrayHost(calls);
        var application = new TrayApplication(
            tray,
            new FakeDisposable(calls, "shortcuts"),
            new FakeOverlay(calls),
            new FakeApplicationLifetime(calls));

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
            new FakeApplicationLifetime(calls));

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
            new FakeApplicationLifetime(calls));

        application.Dispose();

        Assert.Equal(["shortcuts", "overlay", "tray disposed"], calls);
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
            scheduledActions.Enqueue);

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
            new FakeApplicationLifetime(calls));

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
            new FakeApplicationLifetime(calls));

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
