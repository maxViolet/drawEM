using System.Windows.Threading;
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
    private readonly Thread thread;
    private readonly Dispatcher dispatcher;
    private readonly SoundChannelController channel;
    private bool disposed;

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
        catch
        {
            disposed = true;
            EndThread();
            throw;
        }
    }

    /// <summary>Queues <paramref name="command"/> on the sound thread. Returns at once.</summary>
    public void Play(PlaySoundCommand command) => Post(() => channel.Play(command));

    /// <summary>
    /// Stops playback and releases the player on the sound thread, then ends the thread. The thread
    /// ends even if releasing the player throws; that exception is rethrown afterwards.
    /// </summary>
    /// <exception cref="InvalidOperationException">Called on the sound thread.</exception>
    public void Dispose()
    {
        if (dispatcher.CheckAccess())
        {
            throw new InvalidOperationException("Dispose the sound channel host from outside the sound thread.");
        }

        if (disposed)
        {
            return;
        }

        disposed = true;
        try
        {
            dispatcher.Invoke(channel.Dispose, DispatcherPriority.Normal);
        }
        finally
        {
            EndThread();
        }
    }

    private void EndThread()
    {
        dispatcher.InvokeShutdown();
        thread.Join();
    }

    private void Post(Action action)
    {
        if (!disposed)
        {
            dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }
    }
}
