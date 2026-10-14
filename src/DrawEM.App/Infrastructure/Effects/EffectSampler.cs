using DrawEM.App.Application.Effects;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
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
    private static long lastSampleConfigurationGeneration;

    /// <summary>
    /// A sample belongs to one Settings window rather than a saved configuration. Each sampler gets a unique
    /// negative generation, so another window sampling the same slot replaces it instead of toggling it.
    /// </summary>
    private readonly long sampleConfigurationGeneration = Interlocked.Decrement(ref lastSampleConfigurationGeneration);

    private readonly ICursorPositionSource cursor;
    private readonly IMonitorBoundsSource monitors;
    private readonly IEffectStartQueue starts;
    private readonly EffectChannelController channel;
    private readonly EffectSampleFailureRouter failures;
    /// <summary>This sampler's latest started sample; it may since have ended or been replaced.</summary>
    private EffectInstance? latest;
    private Action<bool>? runningChanged;

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

    public void Sample(
        int slot,
        EffectAction effect,
        Action<string> reportFailure,
        Action<bool> runningChanged)
    {
        ArgumentNullException.ThrowIfNull(effect);
        ArgumentNullException.ThrowIfNull(reportFailure);
        ArgumentNullException.ThrowIfNull(runningChanged);

        if (effect.Loop
            && latest is { } current
            && current.Command.Slot == slot
            && ReferenceEquals(channel.ActiveInstance, current))
        {
            StopSamples();
            return;
        }

        // A newer request ends the earlier sample's reports even when it cannot start. The earlier sample stays
        // owned until this one replaces it, so Cancel can still stop it.
        if (latest is not null)
        {
            failures.Forget(latest.Command);
        }

        if (!EffectCatalog.TryFind(effect.Effect, out var builtIn))
        {
            reportFailure($"'{effect.Effect.Value}' is not a built-in effect.");
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
            slot,
            effect.Effect,
            builtIn.Placement,
            monitor,
            point,
            effect.Loop,
            effect.MaxLength,
            sampleConfigurationGeneration,
            monitorEpoch);

        // Watched before the start: a window that cannot be shown is reported inside it.
        failures.Watch(command, reportFailure);
        starts.Dequeue(monitorEpoch, command);
        if (channel.ActiveInstance is { } started && ReferenceEquals(started.Command, command))
        {
            Forget();
            latest = started;
            if (effect.Loop)
            {
                this.runningChanged = runningChanged;
                channel.ActiveInstanceChanged += OnActiveInstanceChanged;
                runningChanged(true);
            }
        }
        else
        {
            // Nothing started, so nothing more can be reported for this command.
            failures.Forget(command);
            if (latest is not null && !ReferenceEquals(channel.ActiveInstance, latest))
            {
                Forget();
            }
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

    private void OnActiveInstanceChanged(EffectInstance? instance)
    {
        if (latest is { } owned && !ReferenceEquals(instance, owned))
        {
            Forget();
        }
    }

    private void Forget()
    {
        if (latest is not null)
        {
            failures.Forget(latest.Command);
        }

        latest = null;
        channel.ActiveInstanceChanged -= OnActiveInstanceChanged;
        var report = runningChanged;
        runningChanged = null;
        report?.Invoke(false);
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
