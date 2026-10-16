using DrawEM.App.Application.Updates;

namespace DrawEM.App.Infrastructure.Updates;

/// <summary>
/// Queues update failures and writes them on a background thread, so a slow disk never holds the UI
/// thread. Each record keeps the time it was reported, not of the write.
/// </summary>
public sealed class LoggingUpdateFailureReporter : IUpdateFailureReporter, IDisposable
{
    private readonly TimeProvider time;
    private readonly BackgroundLogWriter<UpdateFailure> writer;

    /// <param name="append">Writes one record, for example <see cref="UpdateFailureLog.Append"/>.</param>
    public LoggingUpdateFailureReporter(Action<UpdateFailure> append, TimeProvider time)
    {
        this.time = time;
        writer = new BackgroundLogWriter<UpdateFailure>(append);
    }

    /// <summary>Queues the record and returns at once. Records reported after <see cref="Dispose"/> are dropped.</summary>
    public void Report(string operation, string reason) =>
        writer.Enqueue(new UpdateFailure(time.GetLocalNow(), operation, reason));

    /// <summary>Stops accepting records and waits up to <see cref="BackgroundLogWriter{T}.DrainTimeout"/> for queued ones.</summary>
    public void Dispose() => writer.Dispose();
}
