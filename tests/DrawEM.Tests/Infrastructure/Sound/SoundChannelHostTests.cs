using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class SoundChannelHostTests
{
    private static readonly TimeSpan Wait = TimeSpan.FromSeconds(5);
    private static readonly SoundId Applause = new("applause");

    [Fact]
    public void StartupFailure_WithStuckSoundThread_PreservesOriginalErrorAndShutdownTimeout()
    {
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        Thread? soundThread = null;
        var original = new InvalidOperationException("Channel setup failed.");
        try
        {
            var failure = Assert.Throws<AggregateException>(() => new SoundChannelHost(dispatch =>
            {
                dispatch(() =>
                {
                    soundThread = Thread.CurrentThread;
                    entered.Set();
                    release.Wait();
                });
                Assert.True(entered.Wait(Wait));
                throw original;
            }));

            Assert.Contains(original, failure.InnerExceptions);
            Assert.Contains(failure.InnerExceptions, error => error is SoundChannelShutdownTimeoutException);
        }
        finally
        {
            release.Set();
            Assert.True(soundThread!.Join(Wait));
        }
    }

    [Fact]
    public async Task Dispose_WhenSoundThreadIsStuck_ReturnsTimeoutWithinTwoSeconds()
    {
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        Thread? soundThread = null;
        var players = new ThreadRecordingFactory
        {
            OnCreate = () =>
            {
                soundThread = Thread.CurrentThread;
                entered.Set();
                release.Wait();
            },
        };
        var host = new SoundChannelHost(dispatch => NewChannel(players, new ManualTimeProvider(), dispatch));
        host.Play(Command(Applause));
        Task<Exception>? disposal = null;
        try
        {
            Assert.True(entered.Wait(Wait));
            disposal = Task.Factory.StartNew(() => Record.Exception(host.Dispose), TaskCreationOptions.LongRunning);
            var failure = await disposal.WaitAsync(TimeSpan.FromMilliseconds(2500));
            Assert.IsAssignableFrom<TimeoutException>(failure);
        }
        finally
        {
            release.Set();
            if (disposal is not null)
            {
                await disposal.WaitAsync(Wait);
            }
            else
            {
                host.Dispose();
            }
            Assert.True(soundThread!.Join(Wait));
        }
    }

    [Fact]
    public void Play_RunsChannelOnDedicatedStaThread()
    {
        var players = new ThreadRecordingFactory();
        using var host = new SoundChannelHost(dispatch => NewChannel(players, new ManualTimeProvider(), dispatch));

        host.Play(Command(Applause));

        Assert.True(players.Played.Wait(Wait));
        Assert.NotEqual(Environment.CurrentManagedThreadId, players.Player!.PlayThreadId);
        Assert.Equal(ApartmentState.STA, players.Player.PlayApartment);
    }

    [Fact]
    public void Stop_WithPlayingSound_ReportsStoppedOnceItsPlayerIsReleased()
    {
        var players = new ThreadRecordingFactory();
        using var host = new SoundChannelHost(dispatch => NewChannel(players, new ManualTimeProvider(), dispatch));
        host.Play(Command(Applause));
        Assert.True(players.Played.Wait(Wait));

        var stopped = host.Stop();

        Assert.True(stopped);
        Assert.True(players.Player!.IsDisposed);
        Assert.Equal(players.Player.PlayThreadId, players.Player.StopThreadId);
    }

    [Fact]
    public void Stop_WithBlockedSoundThread_ReportsNotConfirmed_AndEarlierRequestsNeverPlay()
    {
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        var players = new ThreadRecordingFactory
        {
            OnCreate = () =>
            {
                entered.Set();
                release.Wait();
            },
        };
        var host = new SoundChannelHost(
            dispatch => NewChannel(players, new ManualTimeProvider(), dispatch), TimeSpan.FromMilliseconds(50));
        host.Play(Command(Applause));
        Assert.True(entered.Wait(Wait));
        host.Play(Command(Applause));

        bool stopped;
        try
        {
            stopped = host.Stop();
        }
        finally
        {
            release.Set();
        }

        host.Dispose();
        Assert.False(stopped);
        Assert.True(Assert.Single(players.Created).IsDisposed);
        Assert.False(players.Played.IsSet);
    }

    [Fact]
    public void Deadline_StopsPlayerOnSoundThreadWhileCallerThreadIsBusy()
    {
        var players = new ThreadRecordingFactory();
        var time = new ManualTimeProvider();
        using var host = new SoundChannelHost(dispatch => NewChannel(players, time, dispatch));
        host.Play(Command(Applause));
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
        host.Play(Command(Applause));
        Assert.True(players.Played.Wait(Wait));

        host.Dispose();
        host.Dispose();
        host.Play(Command(Applause));

        Assert.True(players.Player!.IsDisposed);
    }

    [Fact]
    public void ChannelThatCannotBeCreated_EndsSoundThread()
    {
        Thread? soundThread = null;

        Assert.Throws<InvalidOperationException>(() => new SoundChannelHost(dispatch =>
        {
            using var ran = new ManualResetEventSlim();
            dispatch(() =>
            {
                soundThread = Thread.CurrentThread;
                ran.Set();
            });
            Assert.True(ran.Wait(Wait));
            throw new InvalidOperationException("Channel setup failed.");
        }));

        Assert.True(soundThread!.Join(Wait));
    }

    [Fact]
    public void ChannelDisposeThatThrows_StillEndsSoundThread()
    {
        var players = new ThreadRecordingFactory { StopFailure = "Stop failed." };
        Thread? soundThread = null;
        var host = new SoundChannelHost(dispatch =>
        {
            dispatch(() => soundThread = Thread.CurrentThread);
            return new SoundChannelController(players, new ThrowingReporter(), new ManualTimeProvider(), dispatch);
        });
        host.Play(Command(Applause));
        Assert.True(players.Played.Wait(Wait));

        Assert.Throws<InvalidOperationException>(host.Dispose);

        Assert.True(soundThread!.Join(Wait));
    }

    [Fact]
    public void Dispose_FromSoundThread_IsRejected()
    {
        SoundChannelHost? host = null;
        Exception? rejected = null;
        var players = new ThreadRecordingFactory { OnCreate = () => rejected = Record.Exception(() => host!.Dispose()) };
        host = new SoundChannelHost(dispatch => NewChannel(players, new ManualTimeProvider(), dispatch));

        host.Play(Command(Applause));

        Assert.True(players.Played.Wait(Wait));
        Assert.IsType<InvalidOperationException>(rejected);
        host.Dispose();
        Assert.True(players.Player!.IsDisposed);
    }

    private static SoundChannelController NewChannel(
        ISoundPlayerFactory players, TimeProvider time, Action<Action> dispatch) =>
        new(players, new IgnoringReporter(), time, dispatch);

    private static PlaySoundCommand Command(SoundId sound) => new(sound, @"C:\library\" + sound.Value + ".wav");

    private sealed class IgnoringReporter : ISoundFailureReporter
    {
        public void Report(PlaySoundCommand command, string reason)
        {
        }
    }

    private sealed class ThrowingReporter : ISoundFailureReporter
    {
        public void Report(PlaySoundCommand command, string reason) => throw new InvalidOperationException("Reporter broke.");
    }

    private sealed class ThreadRecordingFactory : ISoundPlayerFactory
    {
        public ManualResetEventSlim Played { get; } = new();

        public ThreadRecordingPlayer? Player { get; private set; }

        public List<ThreadRecordingPlayer> Created { get; } = [];

        public string? StopFailure { get; init; }

        public Action? OnCreate { get; init; }

        public ISoundPlayer Create(PlaySoundCommand command)
        {
            OnCreate?.Invoke();
            Player = new ThreadRecordingPlayer(Played, StopFailure);
            Created.Add(Player);
            return Player;
        }

        public sealed class ThreadRecordingPlayer(ManualResetEventSlim played, string? stopFailure) : ISoundPlayer
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
                if (stopFailure is not null)
                {
                    throw new SoundPlaybackException(stopFailure);
                }
            }

            public void Dispose() => IsDisposed = true;
        }
    }
}
