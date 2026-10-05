using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Queues sound failures and writes them on a background task, so a slow disk never holds the
/// sound thread. Each record keeps the time it was reported, not of the write.
/// </summary>
public sealed class LoggingSoundFailureReporter : ISoundFailureReporter, IDisposable
{
    /// <summary>Longest time <see cref="Dispose"/> waits for queued records at exit.</summary>
    public static readonly TimeSpan DrainTimeout = BackgroundLogWriter<SoundFailure>.DrainTimeout;

    /// <summary>The reason <see cref="ReportUnconfirmedStop"/> records.</summary>
    public const string UnconfirmedStopReason =
        "Sound did not stop in time during a settings save. Unreferenced sound copies are kept until the next start.";

    private readonly TimeProvider time;
    private readonly BackgroundLogWriter<SoundFailure> writer;

    /// <param name="append">Writes one record, for example <see cref="SoundFailureLog.Append"/>.</param>
    public LoggingSoundFailureReporter(Action<SoundFailure> append, TimeProvider time)
    {
        this.time = time;
        writer = new BackgroundLogWriter<SoundFailure>(append);
    }

    /// <summary>Queues the record and returns at once. Records reported after <see cref="Dispose"/> are dropped.</summary>
    public void Report(PlaySoundCommand command, string reason) =>
        writer.Enqueue(new SoundFailure(time.GetLocalNow(), null, command.Sound, command.Path, reason));

    /// <summary>Queues a managed copy that startup cleanup could not remove, like <see cref="Report"/>.</summary>
    /// <param name="path">The copy, or the library directory when it could not be listed.</param>
    public void ReportCleanup(string path, string reason) =>
        writer.Enqueue(new SoundFailure(time.GetLocalNow(), null, null, path, reason));

    /// <summary>
    /// Queues that a Save could not confirm the sound channel stopped, so it removed no sound copy, like
    /// <see cref="Report"/>.
    /// </summary>
    public void ReportUnconfirmedStop() =>
        writer.Enqueue(new SoundFailure(time.GetLocalNow(), null, null, null, UnconfirmedStopReason));

    /// <summary>Stops accepting records and waits up to <see cref="DrainTimeout"/> for queued ones.</summary>
    public void Dispose() => writer.Dispose();
}
