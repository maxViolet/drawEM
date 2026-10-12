using DrawEM.App.Application.Effects;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// One Settings window's effect previews. A sample takes the shortcut path: the cursor point is captured at
/// the click, the start goes through the same <see cref="IEffectStartQueue"/> and monitor epoch, and the
/// effect channel replaces whatever runs. Stopping acts only on this sampler's latest instance, so a newer
/// shortcut effect or another window's sample keeps running. Call every member on the UI thread.
/// </summary>
public sealed class EffectSampler : IEffectSampler
{
    /// <summary>A sample belongs to no saved configuration, so a Save never drops it as a queued start.</summary>
    private const long SampleConfigurationGeneration = 0;

    private readonly ICursorPositionSource cursor;
    private readonly IMonitorBoundsSource monitors;
    private readonly IEffectStartQueue starts;
    private readonly EffectChannelController channel;
    private readonly EffectSampleFailureRouter failures;
    /// <summary>This sampler's latest started sample; it may since have ended or been replaced.</summary>
    private EffectInstance? latest;

    public EffectSampler(
        ICursorPositionSource cursor,
        IMonitorBoundsSource monitors,
        IEffectStartQueue starts,
        EffectChannelController channel,
        EffectSampleFailureRouter failures)
    {
        ArgumentNullException.ThrowIfNull(cursor);
        ArgumentNullException.ThrowIfNull(monitors);
        ArgumentNullException.ThrowIfNull(starts);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(failures);
        this.cursor = cursor;
        this.monitors = monitors;
        this.starts = starts;
        this.channel = channel;
        this.failures = failures;
    }

    public void Sample(EffectId effect, Action<string> reportFailure)
    {
        ArgumentNullException.ThrowIfNull(effect);
        ArgumentNullException.ThrowIfNull(reportFailure);
        Forget();
        if (!EffectCatalog.TryFind(effect, out var builtIn))
        {
            reportFailure($"'{effect.Value}' is not a built-in effect.");
            return;
        }

        if (!cursor.TryGetCurrentPosition(out var point))
        {
            reportFailure("The cursor position is not available.");
            return;
        }

        var monitorEpoch = starts.Enqueue(point);
        if (!monitors.TryGetBounds(point, out var monitor))
        {
            starts.Dequeue(monitorEpoch, null);
            reportFailure("The cursor is not on a monitor.");
            return;
        }

        var command = new StartEffectCommand(
            effect, builtIn.Placement, monitor, point, SampleConfigurationGeneration, monitorEpoch);

        // Watched before the start: a window that cannot be shown is reported inside it.
        failures.Watch(command, reportFailure);
        starts.Dequeue(monitorEpoch, command);
        if (channel.ActiveInstance is { } started && ReferenceEquals(started.Command, command))
        {
            latest = started;
        }
        else
        {
            // Nothing started, so nothing more can be reported for this command.
            failures.Forget(command);
        }
    }

    public void StopSamples()
    {
        var instance = latest;
        Forget();
        if (instance is not null)
        {
            channel.Stop(instance.Id);
        }
    }

    public void ForgetSamples() => Forget();

    private void Forget()
    {
        if (latest is not null)
        {
            failures.Forget(latest.Command);
        }

        latest = null;
    }
}

/// <summary>
/// The effect channel's failure reporter. Records every failure in the inner reporter, and also passes the
/// failure of the latest sample to the Settings window that requested it. Called on the UI thread, which owns
/// the effect channel.
/// </summary>
public sealed class EffectSampleFailureRouter : IEffectFailureReporter
{
    private readonly IEffectFailureReporter inner;
    private (StartEffectCommand Command, Action<string> Report)? route;

    public EffectSampleFailureRouter(IEffectFailureReporter inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        this.inner = inner;
    }

    /// <summary>
    /// Routes failures of this exact request to <paramref name="reportFailure"/>, replacing an earlier sample
    /// from any window. Requests are matched by reference, so a shortcut showing the same effect is not routed.
    /// </summary>
    public void Watch(StartEffectCommand command, Action<string> reportFailure)
    {
        route = (command, reportFailure);
    }

    /// <summary>Stops routing <paramref name="command"/>; a newer sample keeps its route.</summary>
    public void Forget(StartEffectCommand command)
    {
        if (route is { } current && ReferenceEquals(command, current.Command))
        {
            route = null;
        }
    }

    public void Report(EffectInstance instance, string reason)
    {
        inner.Report(instance, reason);
        if (route is { } current && ReferenceEquals(instance.Command, current.Command))
        {
            current.Report(reason);
        }
    }
}
