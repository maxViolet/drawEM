namespace DrawEM.Tests;

/// <summary>
/// Time provider whose clock moves only through <see cref="Advance"/>. Due one-shot timers fire
/// synchronously inside <see cref="Advance"/>. Timers and timestamps follow elapsed time, as real ones do,
/// so <see cref="ChangeWallClock"/> moves only <see cref="GetUtcNow"/>.
/// </summary>
internal sealed class ManualTimeProvider : TimeProvider
{
    private readonly List<ManualTimer> timers = [];
    private DateTimeOffset now = new(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);
    private TimeSpan elapsed;

    public int ActiveTimerCount => timers.Count;

    public override DateTimeOffset GetUtcNow() => now;

    public override long TimestampFrequency => TimeSpan.TicksPerSecond;

    public override long GetTimestamp() => elapsed.Ticks;

    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        var timer = new ManualTimer(this, callback, state);
        timer.Change(dueTime, period);
        return timer;
    }

    public void Advance(TimeSpan delta)
    {
        now += delta;
        elapsed += delta;
        foreach (var timer in timers.Where(timer => timer.DueAt <= elapsed).ToArray())
        {
            timers.Remove(timer);
            timer.Fire();
        }
    }

    /// <summary>Sets the wall clock forward or back, as a user or time sync does; no time elapses.</summary>
    public void ChangeWallClock(TimeSpan delta) => now += delta;

    private sealed class ManualTimer(ManualTimeProvider owner, TimerCallback callback, object? state) : ITimer
    {
        public TimeSpan DueAt { get; private set; }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            if (period != Timeout.InfiniteTimeSpan)
            {
                throw new NotSupportedException("Only one-shot timers are supported.");
            }

            owner.timers.Remove(this);
            if (dueTime != Timeout.InfiniteTimeSpan)
            {
                DueAt = owner.elapsed + dueTime;
                owner.timers.Add(this);
            }

            return true;
        }

        public void Fire() => callback(state);

        public void Dispose() => owner.timers.Remove(this);

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
