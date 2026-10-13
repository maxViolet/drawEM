using DrawEM.App.Application.Effects;
using DrawEM.App.Infrastructure.Effects;

namespace DrawEM.Tests.Infrastructure.Effects;

/// <summary>
/// Records each effect it shows and each one it removes, without a window. Failures are recorded too, so a
/// test can assert there were none.
/// </summary>
internal sealed class RecordingEffectSurface : IEffectSurface, IEffectFailureReporter
{
    public List<EffectInstance> Shown { get; } = [];

    public List<EffectInstance> Removed { get; } = [];

    public List<string> Failures { get; } = [];

    /// <summary>Runs inside each removal, before it is recorded.</summary>
    public Action<EffectInstance>? OnRemove { get; set; }

    /// <summary>An effect channel over a new surface, with timers on a manual clock and callbacks run inline.</summary>
    public static EffectChannelController Channel()
    {
        var surface = new RecordingEffectSurface();
        return new EffectChannelController(surface, surface, new ManualTimeProvider(), action => action());
    }

    public IEffectPlayback Show(EffectInstance instance)
    {
        Shown.Add(instance);
        return new Playback(this, instance);
    }

    public void Report(EffectInstance instance, string reason) => Failures.Add(reason);

    private sealed class Playback(RecordingEffectSurface owner, EffectInstance instance) : IEffectPlayback
    {
        public event Action? Completed
        {
            add { }
            remove { }
        }

        public event Action<string>? Failed
        {
            add { }
            remove { }
        }

        public void Dispose()
        {
            owner.OnRemove?.Invoke(instance);
            owner.Removed.Add(instance);
        }
    }
}

/// <summary>Raises a display change on request.</summary>
internal sealed class FakeDisplayChangeSource : IDisplayChangeSource
{
    public event Action? DisplayChanged;

    public void Change() => DisplayChanged?.Invoke();
}

/// <summary>Counts shown and released playbacks; can fail on show, while playing, or on release.</summary>
internal sealed class FailingSurface : IEffectSurface
{
    public string? ShowFailure { get; set; }

    public int Shown { get; private set; }

    public int Disposed { get; private set; }

    public Playback? Last { get; private set; }

    public IEffectPlayback Show(EffectInstance instance)
    {
        if (ShowFailure is { } reason)
        {
            throw new EffectPlaybackException(reason);
        }

        Shown++;
        Last = new Playback(this);
        return Last;
    }

    public sealed class Playback(FailingSurface owner) : IEffectPlayback
    {
        public event Action? Completed;

        public event Action<string>? Failed;

        /// <summary>Whether anyone still listens for the natural end.</summary>
        public bool HasCompletionHandler => Completed is not null;

        /// <summary>Whether anyone still listens for a failure.</summary>
        public bool HasFailureHandler => Failed is not null;

        public string? DisposeFailure { get; set; }

        public void Fail(string reason) => Failed?.Invoke(reason);

        public void Dispose()
        {
            owner.Disposed++;
            if (DisposeFailure is { } reason)
            {
                throw new EffectPlaybackException(reason);
            }
        }
    }
}
