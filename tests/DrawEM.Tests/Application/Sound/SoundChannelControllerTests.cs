using DrawEM.App.Application.Sound;

namespace DrawEM.Tests.Application.Sound;

public class SoundChannelControllerTests
{
    private static readonly SoundId Applause = new("applause");
    private static readonly SoundId Drumroll = new("drumroll");

    [Fact]
    public void Play_StartsPlayerForSoundAndBecomesActive()
    {
        var fixture = new Fixture();

        fixture.Controller.Play(new PlaySoundCommand(Applause));

        var player = Assert.Single(fixture.Players);
        Assert.Equal(Applause, player.Sound);
        Assert.Equal(["play"], player.Calls);
        Assert.Equal(Applause, fixture.Controller.ActiveSound);
    }

    [Fact]
    public void PlayingAnotherSound_StopsAndDisposesPreviousBeforeStarting()
    {
        var fixture = new Fixture();

        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Controller.Play(new PlaySoundCommand(Drumroll));

        Assert.Equal(
            ["create applause", "play applause", "stop applause", "dispose applause", "create drumroll", "play drumroll"],
            fixture.Log);
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
    }

    [Fact]
    public void PlayingSameSoundAgain_RestartsWithNewPlayerAndDeadline()
    {
        var fixture = new Fixture();

        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Time.Advance(TimeSpan.FromSeconds(6));
        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Time.Advance(TimeSpan.FromSeconds(6));

        Assert.Equal(2, fixture.Players.Count);
        Assert.True(fixture.Players[0].IsDisposed);
        Assert.False(fixture.Players[1].IsDisposed);
        Assert.Equal(Applause, fixture.Controller.ActiveSound);

        fixture.Time.Advance(TimeSpan.FromSeconds(4));

        Assert.True(fixture.Players[1].IsDisposed);
        Assert.Null(fixture.Controller.ActiveSound);
    }

    [Fact]
    public void NaturalCompletion_StopsAndDisposesPlayerAndCancelsDeadline()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));

        fixture.Players[0].Complete();

        Assert.Equal(["play", "stop", "dispose"], fixture.Players[0].Calls);
        Assert.Null(fixture.Controller.ActiveSound);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void Deadline_StopsPlaybackTenSecondsAfterStart()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));

        fixture.Time.Advance(TimeSpan.FromSeconds(10) - TimeSpan.FromMilliseconds(1));
        Assert.Equal(["play"], fixture.Players[0].Calls);

        fixture.Time.Advance(TimeSpan.FromMilliseconds(1));
        Assert.Equal(["play", "stop", "dispose"], fixture.Players[0].Calls);
        Assert.Null(fixture.Controller.ActiveSound);
    }

    [Fact]
    public void StaleCompletionAndFailure_DoNotStopReplacement()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Controller.Play(new PlaySoundCommand(Drumroll));

        fixture.Players[0].Complete();
        fixture.Players[0].Fail("Late decode error.");

        Assert.Equal(["play"], fixture.Players[1].Calls);
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
        Assert.Empty(fixture.Failures);
    }

    [Fact]
    public void StaleDeadline_QueuedBeforeReplacement_DoesNotStopReplacement()
    {
        var queued = new Queue<Action>();
        var fixture = new Fixture(queued.Enqueue);
        fixture.Controller.Play(new PlaySoundCommand(Applause));

        fixture.Time.Advance(TimeSpan.FromSeconds(10));
        fixture.Controller.Play(new PlaySoundCommand(Drumroll));
        while (queued.Count > 0)
        {
            queued.Dequeue().Invoke();
        }

        Assert.Equal(["play"], fixture.Players[1].Calls);
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
    }

    [Fact]
    public void PlayerFailure_IsReportedAndReleasesPlayer()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));

        fixture.Players[0].Fail("No output device.");

        Assert.Equal([(Applause, "No output device.")], fixture.Failures);
        Assert.Equal(["play", "stop", "dispose"], fixture.Players[0].Calls);
        Assert.Null(fixture.Controller.ActiveSound);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void PlayerThatCannotBeCreated_IsReportedAndNextSoundStillPlays()
    {
        var fixture = new Fixture();
        fixture.FailCreateFor(Applause, "File not found.");

        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Controller.Play(new PlaySoundCommand(Drumroll));

        Assert.Equal([(Applause, "File not found.")], fixture.Failures);
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
    }

    [Fact]
    public void PlayerThatThrowsOnPlay_IsReportedAndDisposed()
    {
        var fixture = new Fixture();
        fixture.FailPlayFor(Applause, "Invalid media.");

        fixture.Controller.Play(new PlaySoundCommand(Applause));

        Assert.Equal([(Applause, "Invalid media.")], fixture.Failures);
        Assert.True(fixture.Players[0].IsDisposed);
        Assert.Null(fixture.Controller.ActiveSound);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void PlayerThatThrowsOnStop_IsStillDisposedAndReported()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Players[0].StopFailure = "Stop failed.";

        fixture.Controller.Play(new PlaySoundCommand(Drumroll));

        Assert.True(fixture.Players[0].IsDisposed);
        Assert.Equal([(Applause, "Stop failed.")], fixture.Failures);
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
    }

    [Fact]
    public void PlayerThatThrowsOnDispose_IsReportedAndChannelStaysUsable()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Players[0].DisposeFailure = "Close failed.";

        fixture.Players[0].Complete();
        fixture.Controller.Play(new PlaySoundCommand(Drumroll));

        Assert.Equal([(Applause, "Close failed.")], fixture.Failures);
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
    }

    [Fact]
    public void PlayerFailure_WhenStopAlsoThrows_ReportsBothInOrder()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Players[0].StopFailure = "Stop failed.";

        fixture.Players[0].Fail("No output device.");

        Assert.Equal([(Applause, "No output device."), (Applause, "Stop failed.")], fixture.Failures);
        Assert.True(fixture.Players[0].IsDisposed);
        Assert.Null(fixture.Controller.ActiveSound);
    }

    [Fact]
    public void Failures_AreReportedOnlyAfterPlayerIsReleased()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));
        fixture.Players[0].StopFailure = "Stop failed.";

        fixture.Players[0].Fail("No output device.");

        Assert.Equal(
            ["create applause", "play applause", "stop applause", "dispose applause", "report applause", "report applause"],
            fixture.Log);
    }

    [Fact]
    public void Dispose_StopsPlaybackCancelsDeadlineAndIgnoresLaterRequests()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));

        fixture.Controller.Dispose();
        fixture.Controller.Dispose();
        fixture.Controller.Play(new PlaySoundCommand(Drumroll));

        var player = Assert.Single(fixture.Players);
        Assert.Equal(["play", "stop", "dispose"], player.Calls);
        Assert.Null(fixture.Controller.ActiveSound);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void Stop_EndsActivePlaybackAndCancelsDeadline_AndChannelStaysUsable()
    {
        var fixture = new Fixture();
        fixture.Controller.Play(new PlaySoundCommand(Applause));

        fixture.Controller.Stop();

        Assert.Equal(["play", "stop", "dispose"], fixture.Players[0].Calls);
        Assert.Null(fixture.Controller.ActiveSound);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);

        fixture.Controller.Play(new PlaySoundCommand(Drumroll));
        Assert.Equal(Drumroll, fixture.Controller.ActiveSound);
    }

    private sealed class Fixture : ISoundPlayerFactory, ISoundFailureReporter
    {
        private readonly Dictionary<SoundId, string> createFailures = [];
        private readonly Dictionary<SoundId, string> playFailures = [];

        public Fixture(Action<Action>? dispatch = null)
        {
            Controller = new SoundChannelController(this, this, Time, dispatch ?? (action => action()));
        }

        public SoundChannelController Controller { get; }

        public ManualTimeProvider Time { get; } = new();

        public List<FakePlayer> Players { get; } = [];

        public List<string> Log { get; } = [];

        public List<(SoundId Sound, string Reason)> Failures { get; } = [];

        public void FailCreateFor(SoundId sound, string reason) => createFailures[sound] = reason;

        public void FailPlayFor(SoundId sound, string reason) => playFailures[sound] = reason;

        public ISoundPlayer Create(SoundId sound)
        {
            Log.Add("create " + sound.Value);
            if (createFailures.TryGetValue(sound, out var reason))
            {
                throw new SoundPlaybackException(reason);
            }

            var player = new FakePlayer(sound, Log, playFailures.GetValueOrDefault(sound));
            Players.Add(player);
            return player;
        }

        public void Report(SoundId sound, string reason)
        {
            Log.Add("report " + sound.Value);
            Failures.Add((sound, reason));
        }
    }

    private sealed class FakePlayer(SoundId sound, List<string> log, string? playFailure) : ISoundPlayer
    {
        public event Action? Completed;

        public event Action<string>? Failed;

        public SoundId Sound { get; } = sound;

        public List<string> Calls { get; } = [];

        public bool IsDisposed { get; private set; }

        public string? StopFailure { get; set; }

        public string? DisposeFailure { get; set; }

        public void Play()
        {
            Record("play");
            ThrowIf(playFailure);
        }

        public void Stop()
        {
            Record("stop");
            ThrowIf(StopFailure);
        }

        public void Dispose()
        {
            Record("dispose");
            IsDisposed = true;
            ThrowIf(DisposeFailure);
        }

        private static void ThrowIf(string? failure)
        {
            if (failure is not null)
            {
                throw new SoundPlaybackException(failure);
            }
        }

        public void Complete() => Completed?.Invoke();

        public void Fail(string reason) => Failed?.Invoke(reason);

        private void Record(string call)
        {
            Calls.Add(call);
            log.Add(call + " " + Sound.Value);
        }
    }
}
