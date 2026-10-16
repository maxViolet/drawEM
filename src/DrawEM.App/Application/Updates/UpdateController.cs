namespace DrawEM.App.Application.Updates;

/// <summary>
/// Consent-based updates for an installed copy. Checks once at <see cref="Start"/>, then every 24 hours while
/// running, with at most one check in flight; a newer version downloads in the background. Each downloaded
/// version is announced once, and nothing installs until the user confirms a restart. A portable copy makes no
/// check and creates no timer. Call every member on one thread; timer callbacks and check completions are
/// marshalled to that thread through the dispatch delegate.
/// </summary>
public sealed class UpdateController : IDisposable
{
    /// <summary>Time from the start of one scheduled check to the next.</summary>
    public static readonly TimeSpan CheckInterval = TimeSpan.FromHours(24);

    private readonly IUpdateService service;
    private readonly IUpdateFailureReporter failures;
    private readonly Action<string> showUpdateReady;
    private readonly Func<string, bool> confirmRestart;
    private readonly Action exit;
    private readonly TimeProvider time;
    private readonly Action<Action> dispatch;
    private ITimer? timer;
    private CancellationTokenSource? running;
    private string? readyVersion;
    private bool started;
    private bool applying;
    private bool disposed;

    /// <param name="showUpdateReady">Shows the tray item and balloon for a downloaded version.</param>
    /// <param name="confirmRestart">Asks the user to restart now for the version; <c>true</c> means Yes.</param>
    /// <param name="exit">The normal app exit, which releases hooks, sound, and the single-instance guard.</param>
    public UpdateController(
        IUpdateService service,
        IUpdateFailureReporter failures,
        Action<string> showUpdateReady,
        Func<string, bool> confirmRestart,
        Action exit,
        TimeProvider time,
        Action<Action> dispatch)
    {
        this.service = service;
        this.failures = failures;
        this.showUpdateReady = showUpdateReady;
        this.confirmRestart = confirmRestart;
        this.exit = exit;
        this.time = time;
        this.dispatch = dispatch;
    }

    /// <summary>
    /// Offers a pending version at once, or checks the feed; then schedules the 24-hour checks. Does nothing
    /// for a portable copy, after the first call, or after <see cref="Dispose"/>.
    /// </summary>
    public void Start()
    {
        if (disposed || started || !service.IsInstalled)
        {
            return;
        }

        started = true;
        ScheduleNextCheck();

        // A version downloaded before the last exit is offered again without a new download.
        if (service.PendingVersion is { } pending)
        {
            Announce(pending);
            return;
        }

        Check();
    }

    /// <summary>
    /// The tray item or balloon was chosen. Asks the user to confirm; on Yes, starts the updater and exits.
    /// On No, the update stays offered.
    /// </summary>
    public void RequestUpdate()
    {
        if (disposed || applying || readyVersion is not { } version || !confirmRestart(version))
        {
            return;
        }

        // The confirmation dialog can outlive a Dispose that ran from a nested message loop.
        if (disposed)
        {
            return;
        }

        try
        {
            service.ApplyAfterExit();
        }
        catch (Exception exception)
        {
            failures.Report("apply", exception.Message);
            return;
        }

        applying = true;
        exit();
    }

    /// <summary>Stops the timer and cancels a running check; later completions are ignored.</summary>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        timer?.Dispose();
        timer = null;
        running?.Cancel();
    }

    /// <summary>The next tick counts from this one, so a slow or skipped check does not move the cadence.</summary>
    private void ScheduleNextCheck()
    {
        timer?.Dispose();
        timer = time.CreateTimer(_ => dispatch(OnTimer), null, CheckInterval, Timeout.InfiniteTimeSpan);
    }

    private void OnTimer()
    {
        if (disposed)
        {
            return;
        }

        ScheduleNextCheck();
        Check();
    }

    private void Check()
    {
        if (running is not null)
        {
            return;
        }

        var cancellation = new CancellationTokenSource();
        Task<string?> check;
        try
        {
            check = service.CheckAndDownloadAsync(cancellation.Token);
        }
        catch (Exception exception)
        {
            cancellation.Dispose();
            failures.Report("check", exception.Message);
            return;
        }

        // The source is the generation token: a completion acts only while its check is still the running one.
        running = cancellation;
        check.ContinueWith(
            completed => dispatch(() => OnCheckCompleted(cancellation, completed)),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
    }

    private void OnCheckCompleted(CancellationTokenSource cancellation, Task<string?> check)
    {
        if (!ReferenceEquals(running, cancellation))
        {
            return;
        }

        running = null;
        cancellation.Dispose();
        if (disposed || check.IsCanceled)
        {
            return;
        }

        if (check.IsFaulted)
        {
            var exception = check.Exception!.InnerExceptions.Count == 1
                ? check.Exception.InnerException!
                : check.Exception;
            failures.Report("check", exception.Message);
            return;
        }

        if (check.Result is { } version)
        {
            Announce(version);
        }
    }

    private void Announce(string version)
    {
        if (version == readyVersion)
        {
            return;
        }

        readyVersion = version;
        showUpdateReady(version);
    }
}
