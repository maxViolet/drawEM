namespace DrawEM.App.Application.Effects;

/// <summary>
/// The global effect channel: Idle -> Running(instance) -> Idle. Owns at most one effect instance across the
/// application, whatever its placement. A looping start for the active slot and configuration toggles it off;
/// every other start releases the active instance first. Each instance ends at its natural end, on failure,
/// on Stop, or at its command's optional maximum length. Call every member on one thread; timer callbacks are
/// marshalled to that thread through the dispatch delegate.
/// </summary>
public sealed class EffectChannelController : IDisposable
{

    private readonly IEffectSurface surface;
    private readonly IEffectFailureReporter failures;
    private readonly TimeProvider time;
    private readonly Action<Action> dispatch;
    private Run? active;
    private long lastInstanceId;
    private bool disposed;

    public EffectChannelController(
        IEffectSurface surface,
        IEffectFailureReporter failures,
        TimeProvider time,
        Action<Action> dispatch)
    {
        this.surface = surface;
        this.failures = failures;
        this.time = time;
        this.dispatch = dispatch;
    }

    /// <summary>The instance that owns the channel, or <c>null</c> when the channel is idle.</summary>
    public EffectInstance? ActiveInstance => active?.Instance;

    /// <summary>Raised after the channel starts or finishes an instance.</summary>
    internal event Action<EffectInstance?>? ActiveInstanceChanged;

    /// <summary>Stops a matching looping slot, or releases the active instance and shows a new one.</summary>
    /// <returns>The started instance, or <c>null</c> when toggled off, disposed, or unable to show.</returns>
    public EffectInstance? Start(StartEffectCommand command)
    {
        if (disposed)
        {
            return null;
        }

        var togglesOff = command.Loop && IsActiveLoopOf(command);
        Stop();
        if (togglesOff)
        {
            return null;
        }

        // StartedAt is the wall-clock record; the deadline uses elapsed time, which a clock change cannot move.
        var showing = time.GetTimestamp();
        if (Show(command) is not { } run)
        {
            return null;
        }

        Activate(run);
        ScheduleDeadline(run, showing);
        return run.Instance;
    }

    private bool IsActiveLoopOf(StartEffectCommand command) =>
        active is { } current
        && current.Instance.Command.Loop
        && current.Instance.Command.Slot == command.Slot
        && current.Instance.Command.ConfigurationGeneration == command.ConfigurationGeneration;

    /// <returns>The shown run, or <c>null</c> after reporting why the surface could not show it.</returns>
    private Run? Show(StartEffectCommand command)
    {
        var instance = new EffectInstance(new EffectInstanceId(++lastInstanceId), command, time.GetUtcNow());
        try
        {
            // The run object is the generation token: callbacks act only while it is still active.
            return new Run(instance, surface.Show(instance));
        }
        catch (EffectPlaybackException exception)
        {
            failures.Report(instance, exception.Message);
            return null;
        }
    }

    private void Activate(Run run)
    {
        if (!run.Instance.Command.Loop)
        {
            run.OnCompleted = () => End(run);
            run.Playback.Completed += run.OnCompleted;
        }

        run.OnFailed = reason => End(run, reason);
        run.Playback.Failed += run.OnFailed;
        active = run;
        ActiveInstanceChanged?.Invoke(run.Instance);
    }

    /// <summary>The max length of a one-shot effect; a loop plays until stopped.</summary>
    private static TimeSpan? Limit(StartEffectCommand command) => command.Loop ? null : command.MaxLength;

    private void ScheduleDeadline(Run run, long showing)
    {
        if (Limit(run.Instance.Command) is not { } maxLength)
        {
            return;
        }

        // The deadline counts from the start, so time spent inside Show shortens what remains.
        var remaining = maxLength - time.GetElapsedTime(showing);
        if (remaining <= TimeSpan.Zero)
        {
            End(run);
            return;
        }

        run.Deadline = time.CreateTimer(
            _ => dispatch(() => End(run)), null, remaining, Timeout.InfiniteTimeSpan);
    }

    /// <summary>Releases the active instance, if any. The channel stays usable.</summary>
    public void Stop()
    {
        if (active is { } run)
        {
            End(run);
        }
    }

    /// <summary>Releases the active instance only if it is <paramref name="id"/>; a replacement keeps running.</summary>
    public void Stop(EffectInstanceId id)
    {
        if (active is { } run && run.Instance.Id == id)
        {
            End(run);
        }
    }

    /// <summary>
    /// Releases the active instance if it was started in a monitor epoch before <paramref name="monitorEpoch"/>:
    /// the cursor has entered another monitor, or the display topology changed, since it started.
    /// </summary>
    public void StopStartedBefore(long monitorEpoch)
    {
        if (active is { } run && run.Instance.Command.MonitorEpoch < monitorEpoch)
        {
            End(run);
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        Stop();
    }

    private void End(Run run, string? failureReason = null)
    {
        if (run != active)
        {
            return;
        }

        active = null;
        var releaseFailure = Release(run);

        // Report only after the playback is released, so logging never delays the release.
        Report(run.Instance, failureReason);
        Report(run.Instance, releaseFailure);
        ActiveInstanceChanged?.Invoke(null);
    }

    /// <returns>Why the playback could not be released, or <c>null</c>.</returns>
    private static string? Release(Run run)
    {
        if (run.OnCompleted is not null)
        {
            run.Playback.Completed -= run.OnCompleted;
        }

        run.Playback.Failed -= run.OnFailed;
        run.Deadline?.Dispose();
        try
        {
            run.Playback.Dispose();
            return null;
        }
        catch (EffectPlaybackException exception)
        {
            return exception.Message;
        }
    }

    private void Report(EffectInstance instance, string? reason)
    {
        if (reason is not null)
        {
            failures.Report(instance, reason);
        }
    }

    private sealed class Run(EffectInstance instance, IEffectPlayback playback)
    {
        public EffectInstance Instance { get; } = instance;

        public IEffectPlayback Playback { get; } = playback;

        public ITimer? Deadline { get; set; }

        public Action? OnCompleted { get; set; }

        public Action<string>? OnFailed { get; set; }
    }
}
