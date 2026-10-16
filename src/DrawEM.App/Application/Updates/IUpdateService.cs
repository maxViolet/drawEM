namespace DrawEM.App.Application.Updates;

/// <summary>
/// The installed app's update feed. Versions are plain strings, so no updater types leave Infrastructure.
/// </summary>
public interface IUpdateService
{
    /// <summary>
    /// Whether this copy was installed by the updater. A portable copy is not, and it never checks for updates.
    /// </summary>
    bool IsInstalled { get; }

    /// <summary>A version downloaded earlier and not yet applied, or <c>null</c>.</summary>
    string? PendingVersion { get; }

    /// <summary>
    /// Checks the feed and downloads a newer version, off the calling thread.
    /// </summary>
    /// <returns>The downloaded version, ready to apply, or <c>null</c> when the app is up to date.</returns>
    Task<string?> CheckAndDownloadAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Starts the updater, which waits for this process to exit, applies the downloaded version, and starts it.
    /// The caller exits the app normally afterwards.
    /// </summary>
    void ApplyAfterExit();
}
