using System.IO;
using DrawEM.App.Application.Updates;
using Velopack.Sources;

namespace DrawEM.App.Infrastructure.Updates;

/// <summary>Chooses where updates come from: public GitHub releases, or a local test feed.</summary>
public static class UpdateFeed
{
    /// <summary>Names a local folder that replaces GitHub as the feed, for testing an upgrade before publishing.</summary>
    public const string OverrideVariable = "DRAWEM_UPDATE_FEED";

    public const string RepositoryUrl = "https://github.com/maxViolet/drawEM";

    /// <summary>
    /// An existing directory in <paramref name="feedOverride"/> becomes the feed; any other non-empty value is
    /// ignored. Both cases are logged. Without an override, the feed is the repository's published releases,
    /// read without a token, so drafts and pre-releases are never offered.
    /// </summary>
    public static IUpdateSource Create(string? feedOverride, IUpdateFailureReporter log)
    {
        if (string.IsNullOrEmpty(feedOverride))
        {
            return new GithubSource(RepositoryUrl, null, false);
        }

        if (Directory.Exists(feedOverride))
        {
            log.Report("feed", $"{OverrideVariable} replaces GitHub with the local feed '{feedOverride}'.");
            return new SimpleFileSource(new DirectoryInfo(feedOverride));
        }

        log.Report("feed", $"{OverrideVariable} '{feedOverride}' is not an existing directory; using GitHub.");
        return new GithubSource(RepositoryUrl, null, false);
    }
}
