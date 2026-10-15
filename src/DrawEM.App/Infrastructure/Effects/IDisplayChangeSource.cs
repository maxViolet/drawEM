namespace DrawEM.App.Infrastructure.Effects;

/// <summary>Reports display topology changes: a monitor added or removed, moved, resized, or rescaled.</summary>
public interface IDisplayChangeSource
{
    /// <summary>
    /// Raised as soon as the topology changed, on any thread. Handlers must be thread-safe and must not block.
    /// </summary>
    event Action? DisplayChanged;
}
