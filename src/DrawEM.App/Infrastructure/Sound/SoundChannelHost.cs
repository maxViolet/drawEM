using System.Windows.Threading;
using System.Diagnostics;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Owns the sound thread: a dedicated STA dispatcher thread that runs the global sound channel.
/// WPF <see cref="System.Windows.Media.MediaPlayer"/> can be stopped only on the thread that created it;
/// a dedicated thread keeps the ten-second deadline independent of a busy UI thread.
/// </summary>
/// <remarks>
/// <see cref="Play"/> may be called from any thread. <see cref="TryHoldStarts"/>, <see cref="Stop"/>, and
/// <see cref="Dispose"/> may be called from any thread except the sound thread, because they wait for it.
/// </remarks>
public sealed class SoundChannelHost : ISaveSoundChannel, IDisposable
{
    public static readonly TimeSpan ShutdownTimeout = TimeSpan.FromSeconds(2);
    public static readonly TimeSpan DefaultWaitTimeout = TimeSpan.FromSeconds(2);
    private readonly Thread thread;
    private readonly Dispatcher dispatcher;
    private readonly SoundChannelController channel;
    private readonly TimeSpan waitTimeout;

    /// <summary>
    /// Held while a request decides to start and starts its player, and while a Save holds starts, so
    /// ending requests never interleaves with a start.
    /// </summary>
    private readonly object startGate = new();
    private int disposed;

    /// <summary>Advanced by <see cref="ISoundStartHold.EndEarlierRequests"/>. A request starts only under the value it was made with.</summary>
    private int generation;

    /// <param name="createChannel">Builds the channel from a delegate that queues work on the sound thread.</param>
    public SoundChannelHost(Func<Action<Action>, SoundChannelController> createChannel)
        : this(createChannel, DefaultWaitTimeout)
    {
    }

    /// <param name="createChannel">Builds the channel from a delegate that queues work on the sound thread.</param>
    /// <param name="waitTimeout">Longest time <see cref="TryHoldStarts"/> and <see cref="Stop"/> each wait for the sound thread.</param>
    public SoundChannelHost(Func<Action<Action>, SoundChannelController> createChannel, TimeSpan waitTimeout)
    {
        this.waitTimeout = waitTimeout;
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

    /// <summary>
    /// Queues <paramref name="command"/> on the sound thread. Returns at once. The request never starts if
    /// a <see cref="ISoundStartHold.EndEarlierRequests"/> call comes before its player starts.
    /// </summary>
    public void Play(PlaySoundCommand command)
    {
        var request = new Request(this, Volatile.Read(ref generation));
        Post(() => channel.Play(command, request));
    }

    /// <summary>
    /// Waits at most the wait timeout until no request is starting its player, then keeps every request from
    /// starting until the hold is disposed. A request that is starting when this is called finished its start
    /// before the hold is taken.
    /// </summary>
    /// <returns>The hold, or <c>null</c> when a start did not finish in time.</returns>
    /// <exception cref="InvalidOperationException">Called on the sound thread.</exception>
    public ISoundStartHold? TryHoldStarts()
    {
        if (dispatcher.CheckAccess())
        {
            throw new InvalidOperationException("Hold sound starts from outside the sound thread.");
        }

        return Monitor.TryEnter(startGate, waitTimeout) ? new StartHold(this) : null;
    }

    /// <summary>
    /// Stops the active sound on the sound thread and waits until its player is released. Requests queued
    /// before this call run first. Waits at most the wait timeout, so a stuck sound thread cannot hold the
    /// caller.
    /// </summary>
    /// <returns>
    /// <c>true</c> when the stop ran: no earlier request is still running and the active player is released.
    /// <c>false</c> when that was not confirmed in time, or the host is disposed. A timed-out stop stays
    /// queued and still runs before any later request.
    /// </returns>
    /// <exception cref="InvalidOperationException">Called on the sound thread.</exception>
    public bool Stop()
    {
        if (dispatcher.CheckAccess())
        {
            throw new InvalidOperationException("Stop the sound channel from outside the sound thread.");
        }

        if (Volatile.Read(ref disposed) != 0)
        {
            return false;
        }

        var stop = dispatcher.InvokeAsync(channel.Stop, DispatcherPriority.Normal);
        try
        {
            stop.Task.WaitAsync(waitTimeout).GetAwaiter().GetResult();
            return true;
        }
        catch (TimeoutException) when (!stop.Task.IsCompleted)
        {
            return false;
        }
    }

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

    /// <summary>One play request, current while no hold has ended the generation it was made in.</summary>
    private sealed class Request(SoundChannelHost host, int generation) : IPlayRequest
    {
        public bool IsCurrent => Volatile.Read(ref host.generation) == generation;

        public bool TryStart(Action start)
        {
            lock (host.startGate)
            {
                if (!IsCurrent)
                {
                    return false;
                }

                start();
                return true;
            }
        }
    }

    /// <summary>Owns <see cref="startGate"/> on the thread that took it until disposed.</summary>
    private sealed class StartHold(SoundChannelHost host) : ISoundStartHold
    {
        private bool released;

        public void EndEarlierRequests() => Interlocked.Increment(ref host.generation);

        public void Dispose()
        {
            if (!released)
            {
                released = true;
                Monitor.Exit(host.startGate);
            }
        }
    }
}
