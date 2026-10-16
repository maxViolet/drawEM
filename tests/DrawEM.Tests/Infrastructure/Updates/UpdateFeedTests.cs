using DrawEM.App.Application.Updates;
using DrawEM.App.Infrastructure.Updates;
using Velopack.Sources;

namespace DrawEM.Tests.Infrastructure.Updates;

public class UpdateFeedTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithoutOverride_UsesGitHubAndLogsNothing(string? feedOverride)
    {
        var log = new RecordingReporter();

        var source = UpdateFeed.Create(feedOverride, log);

        Assert.IsType<GithubSource>(source);
        Assert.Empty(log.Lines);
    }

    [Fact]
    public void ExistingDirectory_ReplacesGitHub_AndIsLogged()
    {
        var directory = Directory.CreateTempSubdirectory("drawem-feed-").FullName;
        var log = new RecordingReporter();

        try
        {
            var source = UpdateFeed.Create(directory, log);

            Assert.Equal(directory, Assert.IsType<SimpleFileSource>(source).BaseDirectory.FullName);
            Assert.Contains(directory, Assert.Single(log.Lines));
        }
        finally
        {
            Directory.Delete(directory);
        }
    }

    [Fact]
    public void OtherValue_IsIgnored_AndLogged()
    {
        var missing = Path.Combine(Path.GetTempPath(), "drawem-feed-" + Guid.NewGuid().ToString("N"));
        var log = new RecordingReporter();

        var source = UpdateFeed.Create(missing, log);

        Assert.IsType<GithubSource>(source);
        Assert.Contains("not an existing directory", Assert.Single(log.Lines));
    }

    private sealed class RecordingReporter : IUpdateFailureReporter
    {
        public List<string> Lines { get; } = [];

        public void Report(string operation, string reason) => Lines.Add($"{operation}: {reason}");
    }
}
