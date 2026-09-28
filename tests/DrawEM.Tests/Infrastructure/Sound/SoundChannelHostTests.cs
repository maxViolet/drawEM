using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class SoundChannelHostTests
{
    private static readonly TimeSpan Wait = TimeSpan.FromSeconds(5);
    private static readonly SoundId Applause = new("applause");

    [Fact]
    public void Play_RunsChannelOnDedicatedStaThread()
    {
        var players = new ThreadRecordingFactory();
        using var host = new SoundChannelHost(dispatch => NewChannel(players, new ManualTimeProvider(), dispatch));

        host.Play(new PlaySoundCommand(Applause));

        Assert.True(players.Played.Wait(Wait));
        Assert.NotEqual(Environment.CurrentManagedThreadId, players.Player!.PlayThreadId);
        Assert.Equal(ApartmentState.STA, players.Player.PlayApartment);
    }

    [Fact]
    public void Deadline_StopsPlayerOnSoundThreadWhileCallerThreadIsBusy()
    {
        var players = new ThreadRecordingFactory();
        var time = new ManualTimeProvider();
        using var host = new SoundChannelHost(dispatch => NewChannel(players, time, dispatch));
        host.Play(new PlaySoundCommand(Applause));
        Assert.True(players.Played.Wait(Wait));

        // The deadline callback fires on this thread, which never yields to a dispatcher.
        time.Advance(SoundChannelController.MaxDuration);

        Assert.True(players.Player!.Stopped.Wait(Wait));
        Assert.Equal(players.Player.PlayThreadId, players.Player.StopThreadId);
    }

    [Fact]
    public void Dispose_ReleasesActivePlayerAndIsIdempotent()
    {
        var players = new ThreadRecordingFactory();
        var host = new SoundChannelHost(dispatch => NewChannel(players, new ManualTimeProvider(), dispatch));
        host.Play(new PlaySoundCommand(Applause));
        Assert.True(players.Played.Wait(Wait));

        host.Dispose();
        host.Dispose();
        host.Play(new PlaySoundCommand(Applause));

        Assert.True(players.Player!.IsDisposed);
    }

    private static SoundChannelController NewChannel(
        ISoundPlayerFactory players, TimeProvider time, Action<Action> dispatch) =>
        new(players, new IgnoringReporter(), time, dispatch);

    private sealed class IgnoringReporter : ISoundFailureReporter
    {
        public void Report(SoundId sound, string reason)
        {
        }
    }

    private sealed class ThreadRecordingFactory : ISoundPlayerFactory
    {
        public ManualResetEventSlim Played { get; } = new();

        public ThreadRecordingPlayer? Player { get; private set; }

        public ISoundPlayer Create(SoundId sound)
        {
            Player = new ThreadRecordingPlayer(Played);
            return Player;
        }

        public sealed class ThreadRecordingPlayer(ManualResetEventSlim played) : ISoundPlayer
        {
            public event Action? Completed { add { } remove { } }

            public event Action<string>? Failed { add { } remove { } }

            public int PlayThreadId { get; private set; }

            public ApartmentState PlayApartment { get; private set; }

            public int StopThreadId { get; private set; }

            public ManualResetEventSlim Stopped { get; } = new();

            public bool IsDisposed { get; private set; }

            public void Play()
            {
                PlayThreadId = Environment.CurrentManagedThreadId;
                PlayApartment = Thread.CurrentThread.GetApartmentState();
                played.Set();
            }

            public void Stop()
            {
                StopThreadId = Environment.CurrentManagedThreadId;
                Stopped.Set();
            }

            public void Dispose() => IsDisposed = true;
        }
    }
}
