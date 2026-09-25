namespace DrawEM.Tests;

/// <summary>
/// Time provider whose clock moves only through <see cref="Advance"/>. Due one-shot timers fire
/// synchronously inside <see cref="Advance"/>.
/// </summary>
internal sealed class ManualTimeProvider : TimeProvider
{
    private readonly List<ManualTimer> timers = [];
    private DateTimeOffset now = new(2026, 9, 25, 12, 0, 0, TimeSpan.Zero);

    public int ActiveTimerCount => timers.Count;

    public override DateTimeOffset GetUtcNow() => now;

    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        var timer = new ManualTimer(this, callback, state);
        timer.Change(dueTime, period);
        return timer;
    }

    public void Advance(TimeSpan delta)
    {
        now += delta;
        foreach (var timer in timers.Where(timer => timer.DueAt <= now).ToArray())
        {
            timers.Remove(timer);
            timer.Fire();
        }
    }

    private sealed class ManualTimer(ManualTimeProvider owner, TimerCallback callback, object? state) : ITimer
    {
        public DateTimeOffset DueAt { get; private set; }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            if (period != Timeout.InfiniteTimeSpan)
            {
                throw new NotSupportedException("Only one-shot timers are supported.");
            }

            owner.timers.Remove(this);
            if (dueTime != Timeout.InfiniteTimeSpan)
            {
                DueAt = owner.now + dueTime;
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
