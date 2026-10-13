namespace DrawEM.App.Infrastructure.Effects;

/// <summary>Reports that the desktop is going away for a while: the session locks or the system suspends.</summary>
public interface ISessionInterruptionSource
{
    /// <summary>Raised on the UI thread after the session locked or the system started to suspend.</summary>
    event Action? Interrupted;
}
