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

    private sealed class FakeTrayHost(List<string> calls) : ITrayHost
    {
        public event Action? ExitRequested;

        public void Show() => calls.Add("tray shown");

        public void Dispose() => calls.Add("tray disposed");

        public void RequestExit() => ExitRequested?.Invoke();
    }

    private sealed class FakeDisposable(List<string> calls, string name) : IDisposable
    {
        public void Dispose() => calls.Add(name);
    }

    private sealed class FakeOverlay(List<string> calls) : IOverlayLifetime
    {
        public void Close() => calls.Add("overlay");
    }

    private sealed class FakeApplicationLifetime(List<string> calls) : IApplicationLifetime
    {
        public void Shutdown() => calls.Add("shutdown");
    }
}
