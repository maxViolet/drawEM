using DrawEM.App.Application.Updates;
using Velopack;
using Velopack.Sources;

namespace DrawEM.App.Infrastructure.Updates;

/// <summary>
/// The update feed through Velopack. Checks and downloads run on the thread pool, so no Velopack work runs
/// on the UI, hook, or render thread. A copy that Velopack did not install reports
/// <see cref="IsInstalled"/> false, and the controller then makes no check.
/// </summary>
public sealed class VelopackUpdateService : IUpdateService
{
    private readonly UpdateManager? manager;
    private VelopackAsset? downloaded;

    /// <summary>A manager that cannot be created is reported, and the copy is then treated as not installed.</summary>
    public VelopackUpdateService(IUpdateSource source, IUpdateFailureReporter failures)
    {
        try
        {
            manager = new UpdateManager(source);
        }
        catch (Exception exception)
        {
            failures.Report("startup", exception.Message);
        }
    }

    public bool IsInstalled => manager?.IsInstalled ?? false;

    /// <summary>A package downloaded before the last exit and not yet applied; it is applied without a new download.</summary>
    public string? PendingVersion
    {
        get
        {
            if (manager?.UpdatePendingRestart is not { } pending)
            {
                return null;
            }

            Volatile.Write(ref downloaded, pending);
            return pending.Version.ToString();
        }
    }

    public Task<string?> CheckAndDownloadAsync(CancellationToken cancellationToken)
    {
        var updates = manager ?? throw new InvalidOperationException("The update manager could not be created.");
        return Task.Run(async () =>
        {
            var update = await updates.CheckForUpdatesAsync().ConfigureAwait(false);
            if (update is null)
            {
                return null;
            }

            cancellationToken.ThrowIfCancellationRequested();
            await updates.DownloadUpdatesAsync(update, null, cancellationToken).ConfigureAwait(false);
            Volatile.Write(ref downloaded, update.TargetFullRelease);
            return update.TargetFullRelease.Version.ToString();
        }, cancellationToken);
    }

    /// <summary>
    /// Starts the updater, which waits up to 60 seconds for this process to exit. Unlike
    /// <c>ApplyUpdatesAndRestart</c>, it does not exit the process, so the normal exit cleanup runs first.
    /// </summary>
    public void ApplyAfterExit()
    {
        var updates = manager ?? throw new InvalidOperationException("The update manager could not be created.");
        var asset = Volatile.Read(ref downloaded) ?? throw new InvalidOperationException("No update is downloaded.");
        updates.WaitExitThenApplyUpdates(asset, silent: false, restart: true);
    }
}
