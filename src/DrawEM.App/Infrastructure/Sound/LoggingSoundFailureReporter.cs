using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Queues playback failures and writes them on a background task, so a slow disk never holds the
/// sound thread. Each record keeps the time of <see cref="Report"/>, not of the write.
/// </summary>
public sealed class LoggingSoundFailureReporter : ISoundFailureReporter, IDisposable
{
    /// <summary>Longest time <see cref="Dispose"/> waits for queued records at exit.</summary>
    public static readonly TimeSpan DrainTimeout = BackgroundLogWriter<SoundFailure>.DrainTimeout;
    private readonly SoundConfiguration configuration;
    private readonly TimeProvider time;
    private readonly BackgroundLogWriter<SoundFailure> writer;

    /// <param name="append">Writes one record, for example <see cref="SoundFailureLog.Append"/>.</param>
    public LoggingSoundFailureReporter(
        Action<SoundFailure> append,
        SoundConfiguration configuration,
        TimeProvider time)
    {
        this.configuration = configuration;
        this.time = time;
        writer = new BackgroundLogWriter<SoundFailure>(append);
    }

    /// <summary>Queues the record and returns at once. Records reported after <see cref="Dispose"/> are dropped.</summary>
    public void Report(SoundId sound, string reason) =>
        writer.Enqueue(new SoundFailure(time.GetLocalNow(), null, sound, configuration.PathOf(sound), reason));

    /// <summary>Stops accepting records and waits up to <see cref="DrainTimeout"/> for queued ones.</summary>
    public void Dispose() => writer.Dispose();
}
