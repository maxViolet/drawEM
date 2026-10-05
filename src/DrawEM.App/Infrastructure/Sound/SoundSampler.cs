using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Plays a draft sound from the Settings window on the global sound channel, like a shortcut does, and
/// routes the failure of the latest sample back to the window. Call every member on the UI thread.
/// </summary>
public sealed class SoundSampler : ISoundSampler
{
    private readonly Action<PlaySoundCommand> play;
    private readonly Func<bool> stop;
    private readonly Func<SoundReference, PlaySoundCommand> commandFor;
    private readonly SampleFailureRouter failures;

    /// <summary>A sample was requested since the last <see cref="StopSamples"/>, so it may still play.</summary>
    private bool sampled;

    /// <param name="play">Queues a request on the sound thread, for example SoundChannelHost.Play.</param>
    /// <param name="stop">Stops the sound channel within a bounded wait, for example SoundChannelHost.Stop.</param>
    /// <param name="commandFor">Builds a new play command for a sound, for example ActiveSettings.CommandFor.</param>
    /// <param name="failures">The failure reporter the sound channel was built with.</param>
    public SoundSampler(
        Action<PlaySoundCommand> play,
        Func<bool> stop,
        Func<SoundReference, PlaySoundCommand> commandFor,
        SampleFailureRouter failures)
    {
        ArgumentNullException.ThrowIfNull(play);
        ArgumentNullException.ThrowIfNull(stop);
        ArgumentNullException.ThrowIfNull(commandFor);
        ArgumentNullException.ThrowIfNull(failures);
        this.play = play;
        this.stop = stop;
        this.commandFor = commandFor;
        this.failures = failures;
    }

    public void Sample(SoundReference sound, Action<string> reportFailure)
    {
        ArgumentNullException.ThrowIfNull(sound);
        ArgumentNullException.ThrowIfNull(reportFailure);
        var command = commandFor(sound);
        failures.Watch(command, reportFailure);
        sampled = true;
        play(command);
    }

    public bool StopSamples()
    {
        failures.Forget();
        if (!sampled)
        {
            return true;
        }

        sampled = false;
        return stop();
    }
}

/// <summary>
/// The sound channel's failure reporter. Records every failure in the inner reporter, and also passes the
/// failure of the latest sample to the callback that requested it, on the UI thread.
/// </summary>
/// <remarks>
/// <see cref="Report"/> is called on the sound thread; <see cref="Watch"/> and <see cref="Forget"/> on the UI
/// thread.
/// </remarks>
public sealed class SampleFailureRouter : ISoundFailureReporter
{
    private readonly ISoundFailureReporter inner;
    private readonly Action<Action> dispatch;
    private readonly object gate = new();
    private PlaySoundCommand? watched;
    private Action<string>? callback;

    /// <param name="dispatch">Queues work on the UI thread.</param>
    public SampleFailureRouter(ISoundFailureReporter inner, Action<Action> dispatch)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(dispatch);
        this.inner = inner;
        this.dispatch = dispatch;
    }

    /// <summary>
    /// Routes failures of this exact request to <paramref name="reportFailure"/>, replacing an earlier sample.
    /// Requests are matched by reference, so a shortcut playing the same file is not routed.
    /// </summary>
    public void Watch(PlaySoundCommand command, Action<string> reportFailure)
    {
        lock (gate)
        {
            watched = command;
            callback = reportFailure;
        }
    }

    /// <summary>Stops routing: no failure reaches a sample's callback after this, even one already queued.</summary>
    public void Forget()
    {
        lock (gate)
        {
            watched = null;
            callback = null;
        }
    }

    public void Report(PlaySoundCommand command, string reason)
    {
        inner.Report(command, reason);
        if (IsWatched(command))
        {
            // Checked again on the UI thread: a newer sample may have replaced this one in between.
            dispatch(() =>
            {
                Action<string>? report;
                lock (gate)
                {
                    report = ReferenceEquals(command, watched) ? callback : null;
                }

                report?.Invoke(reason);
            });
        }
    }

    private bool IsWatched(PlaySoundCommand command)
    {
        lock (gate)
        {
            return ReferenceEquals(command, watched);
        }
    }
}
