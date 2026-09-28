using System.Threading.Channels;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Queues playback failures and writes them on a background task, so a slow disk never holds the
/// sound thread. Each record keeps the time of <see cref="Report"/>, not of the write.
/// </summary>
public sealed class LoggingSoundFailureReporter : ISoundFailureReporter, IDisposable
{
    /// <summary>Longest time <see cref="Dispose"/> waits for queued records at exit.</summary>
    public static readonly TimeSpan DrainTimeout = TimeSpan.FromSeconds(1);

    private readonly Channel<SoundFailure> queue =
        Channel.CreateUnbounded<SoundFailure>(new UnboundedChannelOptions { SingleReader = true });
    private readonly Action<SoundFailure> append;
    private readonly SoundConfiguration configuration;
    private readonly TimeProvider time;
    private readonly Task writer;

    /// <param name="append">Writes one record, for example <see cref="SoundFailureLog.Append"/>.</param>
    public LoggingSoundFailureReporter(
        Action<SoundFailure> append,
        SoundConfiguration configuration,
        TimeProvider time)
    {
        this.append = append;
        this.configuration = configuration;
        this.time = time;
        writer = Task.Run(WriteQueuedAsync);
    }

    /// <summary>Queues the record and returns at once. Records reported after <see cref="Dispose"/> are dropped.</summary>
    public void Report(SoundId sound, string reason) =>
        queue.Writer.TryWrite(new SoundFailure(time.GetLocalNow(), null, sound, configuration.PathOf(sound), reason));

    /// <summary>Stops accepting records and waits up to <see cref="DrainTimeout"/> for queued ones.</summary>
    public void Dispose()
    {
        queue.Writer.TryComplete();
        writer.Wait(DrainTimeout);
    }

    private async Task WriteQueuedAsync()
    {
        await foreach (var failure in queue.Reader.ReadAllAsync())
        {
            try
            {
                append(failure);
            }
            catch (Exception)
            {
                // Logging is best effort; one bad write must not stop later records.
            }
        }
    }
}
