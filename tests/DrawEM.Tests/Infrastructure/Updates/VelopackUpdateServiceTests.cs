using DrawEM.App.Application.Updates;
using DrawEM.App.Infrastructure.Updates;
using Velopack.Sources;

namespace DrawEM.Tests.Infrastructure.Updates;

// The test process never runs VelopackApp.Run(), so Velopack refuses to create an UpdateManager. That covers
// the fallback: the failure is logged and the copy behaves as not installed. The installed and portable cases
// are checked on Windows against a local feed (R4-07 plan, step 6).
public class VelopackUpdateServiceTests
{
    [Fact]
    public void WhenTheManagerCannotBeCreated_ReportsIt_AndIsNotInstalled()
    {
        var failures = new List<string>();

        var service = new VelopackUpdateService(new GithubSource(UpdateFeed.RepositoryUrl, null, false), new Reporter(failures));

        Assert.False(service.IsInstalled);
        Assert.Null(service.PendingVersion);
        Assert.StartsWith("startup: ", Assert.Single(failures));
    }

    [Fact]
    public void WhenTheManagerCannotBeCreated_CheckAndApplyThrow()
    {
        var service = new VelopackUpdateService(new GithubSource(UpdateFeed.RepositoryUrl, null, false), new Reporter([]));

        // Thrown synchronously, so the controller reports it like a faulted check.
        Assert.Throws<InvalidOperationException>(() => { _ = service.CheckAndDownloadAsync(CancellationToken.None); });
        Assert.Throws<InvalidOperationException>(service.ApplyAfterExit);
    }

    private sealed class Reporter(List<string> failures) : IUpdateFailureReporter
    {
        public void Report(string operation, string reason) => failures.Add($"{operation}: {reason}");
    }
}
