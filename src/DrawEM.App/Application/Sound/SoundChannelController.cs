namespace DrawEM.App.Application.Sound;

/// <summary>
/// The global sound channel. Owns at most one playback attempt; a new request stops and releases the
/// active attempt first, even for the same sound. Each attempt ends at its natural end or
/// <see cref="MaxDuration"/> after it starts. Call every member on one thread; timer callbacks are
/// marshalled to that thread through the dispatch delegate.
/// </summary>
public sealed class SoundChannelController : IDisposable
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(10);

    private readonly ISoundPlayerFactory players;
    private readonly ISoundFailureReporter failures;
    private readonly TimeProvider time;
    private readonly Action<Action> dispatch;
    private Attempt? active;
    private bool disposed;

    public SoundChannelController(
        ISoundPlayerFactory players,
        ISoundFailureReporter failures,
        TimeProvider time,
        Action<Action> dispatch)
    {
        this.players = players;
        this.failures = failures;
        this.time = time;
        this.dispatch = dispatch;
    }

    /// <summary>The sound that owns the channel, or <c>null</c> when the channel is idle.</summary>
    public SoundId? ActiveSound => active?.Sound;

    public void Play(PlaySoundCommand command)
    {
        if (disposed)
        {
            return;
        }

        StopActive();

        var sound = command.Sound;
        ISoundPlayer player;
        try
        {
            player = players.Create(sound);
        }
        catch (SoundPlaybackException exception)
        {
            failures.Report(sound, exception.Message);
            return;
        }

        // The attempt object is the generation token: callbacks act only while it is still active.
        var attempt = new Attempt(sound, player);
        attempt.OnCompleted = () => End(attempt);
        attempt.OnFailed = reason => End(attempt, reason);
        player.Completed += attempt.OnCompleted;
        player.Failed += attempt.OnFailed;
        active = attempt;

        attempt.Deadline = time.CreateTimer(
            _ => dispatch(() => End(attempt)), null, MaxDuration, Timeout.InfiniteTimeSpan);

        try
        {
            player.Play();
        }
        catch (SoundPlaybackException exception)
        {
            End(attempt, exception.Message);
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        StopActive();
    }

    private void StopActive()
    {
        if (active is { } attempt)
        {
            End(attempt);
        }
    }

    private void End(Attempt attempt, string? failureReason = null)
    {
        if (attempt != active)
        {
            return;
        }

        active = null;
        attempt.Player.Completed -= attempt.OnCompleted;
        attempt.Player.Failed -= attempt.OnFailed;
        attempt.Deadline?.Dispose();

        // Dispose even when Stop fails, so a broken player never keeps the device open.
        var stopFailure = Release(attempt.Player.Stop);
        var disposeFailure = Release(attempt.Player.Dispose);

        // Report only after the player is released, so logging never delays the release.
        foreach (var reason in new[] { failureReason, stopFailure, disposeFailure })
        {
            if (reason is not null)
            {
                failures.Report(attempt.Sound, reason);
            }
        }
    }

    private static string? Release(Action step)
    {
        try
        {
            step();
            return null;
        }
        catch (SoundPlaybackException exception)
        {
            return exception.Message;
        }
    }

    private sealed class Attempt(SoundId sound, ISoundPlayer player)
    {
        public SoundId Sound { get; } = sound;

        public ISoundPlayer Player { get; } = player;

        public ITimer? Deadline { get; set; }

        public Action? OnCompleted { get; set; }

        public Action<string>? OnFailed { get; set; }
    }
}
