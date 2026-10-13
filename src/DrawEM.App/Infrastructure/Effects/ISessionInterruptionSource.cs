namespace DrawEM.App.Infrastructure.Effects;

/// <summary>Reports that the desktop is going away for a while: the session locks or the system suspends.</summary>
public interface ISessionInterruptionSource
{
    /// <summary>
    /// Raised as soon as the session locks or the system starts to suspend, on any thread. Handlers must be
    /// thread-safe and must not block.
    /// </summary>
    event Action? Interrupted;
}
