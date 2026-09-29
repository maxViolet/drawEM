using System.Windows.Threading;
using System.Diagnostics;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Owns the sound thread: a dedicated STA dispatcher thread that runs the global sound channel.
/// WPF <see cref="System.Windows.Media.MediaPlayer"/> can be stopped only on the thread that created it;
/// a dedicated thread keeps the ten-second deadline independent of a busy UI thread.
/// </summary>
/// <remarks>
/// <see cref="Play"/> may be called from any thread. <see cref="Dispose"/> may be called from any thread
/// except the sound thread, because it waits for that thread to end.
/// </remarks>
public sealed class SoundChannelHost : IDisposable
{
    public static readonly TimeSpan ShutdownTimeout = TimeSpan.FromSeconds(2);
    private readonly Thread thread;
    private readonly Dispatcher dispatcher;
    private readonly SoundChannelController channel;
    private int disposed;

    /// <param name="createChannel">Builds the channel from a delegate that queues work on the sound thread.</param>
    public SoundChannelHost(Func<Action<Action>, SoundChannelController> createChannel)
    {
        Dispatcher? started = null;
        using (var ready = new ManualResetEventSlim())
        {
            thread = new Thread(() =>
            {
                started = Dispatcher.CurrentDispatcher;
                ready.Set();
                Dispatcher.Run();
            })
            {
                IsBackground = true,
                Name = "drawEM sound",
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            ready.Wait();
        }

        dispatcher = started!;

        try
        {
            channel = createChannel(Post);
        }
        catch (Exception startupFailure)
        {
            disposed = 1;
            try
            {
                EndThread(Stopwatch.StartNew());
            }
            catch (Exception shutdownFailure)
            {
                throw new AggregateException("Sound channel startup and shutdown failed.", startupFailure, shutdownFailure);
            }
            throw;
        }
    }

    /// <summary>Queues <paramref name="command"/> on the sound thread. Returns at once.</summary>
    public void Play(PlaySoundCommand command) => Post(() => channel.Play(command));

    /// <summary>
    /// Stops playback and releases the player on the sound thread, then ends the thread. The thread
    /// ends even if releasing the player throws. Cleanup and thread termination share a two-second
    /// budget; a timeout leaves cleanup queued and requires process exit to stop a stuck player.
    /// </summary>
    /// <exception cref="InvalidOperationException">Called on the sound thread.</exception>
    public void Dispose()
    {
        if (dispatcher.CheckAccess())
        {
            throw new InvalidOperationException("Dispose the sound channel host from outside the sound thread.");
        }

        if (Interlocked.Exchange(ref disposed, 1) != 0)
        {
            return;
        }

        var elapsed = Stopwatch.StartNew();
        try
        {
            var cleanup = dispatcher.InvokeAsync(channel.Dispose, DispatcherPriority.Normal);
            try
            {
                cleanup.Task.WaitAsync(Remaining(elapsed)).GetAwaiter().GetResult();
            }
            catch (TimeoutException) when (!cleanup.Task.IsCompleted)
            {
                throw new SoundChannelShutdownTimeoutException();
            }
        }
        finally
        {
            EndThread(elapsed);
        }
    }

    private void EndThread(Stopwatch elapsed)
    {
        // This request does not wait for a stuck callback. Background priority lets queued cleanup
        // run first if that callback returns after the caller has timed out.
        dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
        if (!thread.Join(Remaining(elapsed)))
        {
            throw new SoundChannelShutdownTimeoutException();
        }
    }

    private static TimeSpan Remaining(Stopwatch elapsed)
    {
        var remaining = ShutdownTimeout - elapsed.Elapsed;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    private void Post(Action action)
    {
        if (Volatile.Read(ref disposed) == 0)
        {
            dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }
    }
}
