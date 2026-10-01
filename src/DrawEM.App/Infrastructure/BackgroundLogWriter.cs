using System.Collections.Concurrent;

namespace DrawEM.App.Infrastructure;

/// <summary>
/// One dedicated background writer thread with a bounded drain. Unwritten records may be lost at exit.
/// The writer does not use the thread pool: a busy pool cannot delay the drain, and a stuck write
/// cannot hold a pool thread.
/// </summary>
internal sealed class BackgroundLogWriter<T> : IDisposable
{
    public static readonly TimeSpan DrainTimeout = TimeSpan.FromSeconds(1);
    private readonly BlockingCollection<T> queue = new(new ConcurrentQueue<T>());
    private readonly Action<T> append;
    private readonly Thread writer;
    private int disposed;

    public BackgroundLogWriter(Action<T> append)
    {
        this.append = append;
        writer = new Thread(WriteQueued)
        {
            IsBackground = true,
            Name = "drawEM log",
        };
        writer.Start();
    }

    public void Enqueue(T record)
    {
        try
        {
            queue.TryAdd(record);
        }
        catch (InvalidOperationException)
        {
            // Records that arrive after disposal are dropped.
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref disposed, 1) != 0)
        {
            return;
        }

        queue.CompleteAdding();
        writer.Join(DrainTimeout);
    }

    private void WriteQueued()
    {
        foreach (var record in queue.GetConsumingEnumerable())
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
