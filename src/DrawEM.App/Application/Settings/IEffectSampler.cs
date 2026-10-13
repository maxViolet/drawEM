using DrawEM.App.Domain.Effects;

namespace DrawEM.App.Application.Settings;

/// <summary>
/// Previews a draft effect from one Settings window on the global effect channel, as a shortcut would start
/// it, without changing active shortcuts. Each Settings window owns its own sampler. Call every member on the
/// UI thread.
/// </summary>
public interface IEffectSampler
{
    /// <summary>
    /// Starts <paramref name="effect"/> at its normal size and placement on the monitor containing the cursor,
    /// replacing whatever effect runs.
    /// </summary>
    /// <param name="reportFailure">
    /// Receives the reason when this sample cannot start or fails while it plays. Not called once a newer
    /// sample was requested or the samples were stopped or forgotten.
    /// </param>
    void Sample(EffectId effect, Action<string> reportFailure);

    /// <summary>
    /// Stops this sampler's latest sample if it still runs, and stops reporting its failures. A newer effect,
    /// from a shortcut or another Settings window, keeps running.
    /// </summary>
    void StopSamples();

    /// <summary>Stops reporting sample failures and forgets earlier samples without stopping them.</summary>
    void ForgetSamples();
}
