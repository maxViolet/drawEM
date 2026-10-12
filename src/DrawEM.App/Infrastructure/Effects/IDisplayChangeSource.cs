namespace DrawEM.App.Infrastructure.Effects;

/// <summary>Reports display topology changes: a monitor added or removed, moved, resized, or rescaled.</summary>
public interface IDisplayChangeSource
{
    /// <summary>Raised on the UI thread after the topology changed.</summary>
    event Action? DisplayChanged;
}
