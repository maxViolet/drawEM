using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure;

public class StartupFailureTests
{
    [Fact]
    public void OrdinaryCleanupFailure_ShowsStartupErrorAndDoesNotForceExit()
    {
        var calls = new List<string>();
        var policy = new ApplicationExitPolicy(new AppFailureLog(_ => { }), _ => calls.Add("forced exit"));

        StartupFailure.Handle(
            () => throw new InvalidOperationException("Hook release failed."),
            failure => policy.Report("startup cleanup", failure),
            () => policy.ShowStartupError(() => calls.Add("dialog")),
            () => calls.Add("shutdown"));
        policy.Complete();

        Assert.Equal(["dialog", "shutdown"], calls);
    }

    [Fact]
    public void SoundShutdownTimeout_ExitsAfterCleanupWithoutWaitingForStartupDialog()
    {
        var calls = new List<string>();
        var policy = new ApplicationExitPolicy(new AppFailureLog(_ => { }), _ => calls.Add("forced exit"));

        StartupFailure.Handle(
            () =>
            {
                calls.Add("cleanup");
                throw new SoundChannelShutdownTimeoutException();
            },
            failure => policy.Report("startup cleanup", failure),
            () => policy.ShowStartupError(() => calls.Add("dialog")),
            () => calls.Add("shutdown"));
        policy.Complete();

        Assert.Equal(["cleanup", "forced exit", "shutdown"], calls);
    }

    [Fact]
    public void Handle_WhenCleanupThrows_ReportsItThenShowsErrorAndShutsDown()
    {
        var calls = new List<string>();
        var failure = new InvalidOperationException("Hook release failed.");
        Exception? reported = null;

        StartupFailure.Handle(
            () =>
            {
                calls.Add("cleanup");
                throw failure;
            },
            exception =>
            {
                calls.Add("report");
                reported = exception;
            },
            () => calls.Add("show error"),
            () => calls.Add("shutdown"));

        Assert.Same(failure, reported);
        Assert.Equal(["cleanup", "report", "show error", "shutdown"], calls);
    }

    [Fact]
    public void Handle_WhenCleanupSucceeds_ReportsNothing()
    {
        var calls = new List<string>();

        StartupFailure.Handle(
            () => calls.Add("cleanup"),
            _ => calls.Add("report"),
            () => calls.Add("show error"),
            () => calls.Add("shutdown"));

        Assert.Equal(["cleanup", "show error", "shutdown"], calls);
    }

    [Fact]
    public void Handle_WhenShowingErrorThrows_StillShutsDown()
    {
        var calls = new List<string>();

        Assert.Throws<InvalidOperationException>(() => StartupFailure.Handle(
            () => calls.Add("cleanup"),
            _ => calls.Add("report"),
            () => throw new InvalidOperationException("No message box."),
            () => calls.Add("shutdown")));

        Assert.Equal(["cleanup", "shutdown"], calls);
    }
}
