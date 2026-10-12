using DrawEM.App.Application.Effects;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Queues effect failures and writes them on a background thread, so a slow disk never holds the UI
/// thread. Each record keeps the time it was reported, not of the write.
/// </summary>
public sealed class LoggingEffectFailureReporter : IEffectFailureReporter, IDisposable
{
    private readonly TimeProvider time;
    private readonly BackgroundLogWriter<EffectFailure> writer;

    /// <param name="append">Writes one record, for example <see cref="EffectFailureLog.Append"/>.</param>
    public LoggingEffectFailureReporter(Action<EffectFailure> append, TimeProvider time)
    {
        this.time = time;
        writer = new BackgroundLogWriter<EffectFailure>(append);
    }

    /// <summary>Queues the record and returns at once. Records reported after <see cref="Dispose"/> are dropped.</summary>
    public void Report(EffectInstance instance, string reason) =>
        writer.Enqueue(new EffectFailure(time.GetLocalNow(), instance.Command.Effect, instance.Id.Value, reason));

    /// <summary>Stops accepting records and waits up to <see cref="BackgroundLogWriter{T}.DrainTimeout"/> for queued ones.</summary>
    public void Dispose() => writer.Dispose();
}
