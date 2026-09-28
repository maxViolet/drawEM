using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class StartupFailureTests
{
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
