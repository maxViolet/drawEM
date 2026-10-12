namespace DrawEM.App.Application.Effects;

/// <summary>
/// The global effect channel: Idle -> Running(instance) -> Idle. Owns at most one effect instance across the
/// application, whatever its placement; a new start releases the active instance first, even for the same
/// effect. Each instance ends at its natural end, on failure, on Stop, or <see cref="MaxDuration"/> after it
/// starts; time spent inside <see cref="IEffectSurface.Show"/> counts. Call every member on one thread;
/// timer callbacks are marshalled to that thread through the dispatch delegate.
/// </summary>
public sealed class EffectChannelController : IDisposable
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(10);

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

    /// <summary>Releases the active instance, then shows a new one.</summary>
    /// <returns>The started instance, or <c>null</c> when the channel is disposed or the effect cannot be shown.</returns>
    public EffectInstance? Start(StartEffectCommand command)
    {
        if (disposed)
        {
            return null;
        }

        Stop();

        var instance = new EffectInstance(new EffectInstanceId(++lastInstanceId), command, time.GetUtcNow());
        IEffectPlayback playback;
        try
        {
            playback = surface.Show(instance);
        }
        catch (EffectPlaybackException exception)
        {
            failures.Report(instance, exception.Message);
            return null;
        }

        // The run object is the generation token: callbacks act only while it is still active.
        var run = new Run(instance, playback);
        run.OnCompleted = () => End(run);
        run.OnFailed = reason => End(run, reason);
        playback.Completed += run.OnCompleted;
        playback.Failed += run.OnFailed;
        active = run;

        // The deadline counts from StartedAt, so time spent inside Show shortens what remains.
        var remaining = MaxDuration - (time.GetUtcNow() - instance.StartedAt);
        if (remaining <= TimeSpan.Zero)
        {
            End(run);
            return instance;
        }

        run.Deadline = time.CreateTimer(
            _ => dispatch(() => End(run)), null, remaining, Timeout.InfiniteTimeSpan);
        return instance;
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
        run.Playback.Completed -= run.OnCompleted;
        run.Playback.Failed -= run.OnFailed;
        run.Deadline?.Dispose();

        string? releaseFailure = null;
        try
        {
            run.Playback.Dispose();
        }
        catch (EffectPlaybackException exception)
        {
            releaseFailure = exception.Message;
        }

        // Report only after the playback is released, so logging never delays the release.
        foreach (var reason in new[] { failureReason, releaseFailure })
        {
            if (reason is not null)
            {
                failures.Report(run.Instance, reason);
            }
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
