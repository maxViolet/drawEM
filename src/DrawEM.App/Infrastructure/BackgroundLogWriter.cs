using System.Threading.Channels;

namespace DrawEM.App.Infrastructure;

/// <summary>One background writer with a bounded drain. Unwritten records may be lost at exit.</summary>
internal sealed class BackgroundLogWriter<T> : IDisposable
{
    public static readonly TimeSpan DrainTimeout = TimeSpan.FromSeconds(1);
    private readonly Channel<T> queue = Channel.CreateUnbounded<T>(
        new UnboundedChannelOptions { SingleReader = true });
    private readonly Action<T> append;
    private readonly Task writer;
    private int disposed;

    public BackgroundLogWriter(Action<T> append)
    {
        this.append = append;
        writer = Task.Run(WriteQueuedAsync);
    }

    public void Enqueue(T record) => queue.Writer.TryWrite(record);

    public void Dispose()
    {
        if (Interlocked.Exchange(ref disposed, 1) != 0)
        {
            return;
        }

        queue.Writer.TryComplete();
        writer.Wait(DrainTimeout);
    }

    private async Task WriteQueuedAsync()
    {
        await foreach (var record in queue.Reader.ReadAllAsync())
        {
            try
            {
                append(record);
            }
            catch (Exception)
            {
                // Best effort: a bad write must not stop later records or shutdown.
            }
        }
    }
}
