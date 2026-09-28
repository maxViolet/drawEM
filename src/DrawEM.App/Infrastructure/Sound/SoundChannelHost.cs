using System.Windows.Threading;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Runs the global sound channel on its own STA dispatcher thread. WPF <see cref="System.Windows.Media.MediaPlayer"/>
/// can be stopped only on the thread that created it; a dedicated thread keeps the ten-second deadline
/// independent of a busy UI thread.
/// </summary>
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
        using var ready = new ManualResetEventSlim();
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
        dispatcher = started!;

        channel = createChannel(Post);
    }

    /// <summary>Queues <paramref name="command"/> on the sound thread. Returns at once.</summary>
    public void Play(PlaySoundCommand command) => Post(() => channel.Play(command));

    /// <summary>Stops playback, releases the player, and ends the sound thread.</summary>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        dispatcher.Invoke(channel.Dispose, DispatcherPriority.Normal);
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
