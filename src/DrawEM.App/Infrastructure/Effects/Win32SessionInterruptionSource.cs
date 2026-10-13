using Microsoft.Win32;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Reports Windows session locks and system suspends on the UI thread through the dispatch delegate. Unlock and
/// resume are not reported: an interrupted effect never comes back.
/// </summary>
public sealed class Win32SessionInterruptionSource : ISessionInterruptionSource, IDisposable
{
    private readonly Action<Action> dispatch;

    public Win32SessionInterruptionSource(Action<Action> dispatch)
    {
        this.dispatch = dispatch;
        SystemEvents.SessionSwitch += OnSessionSwitch;
        SystemEvents.PowerModeChanged += OnPowerModeChanged;
    }

    public event Action? Interrupted;

    /// <summary>Whether a session switch interrupts effects.</summary>
    public static bool Interrupts(SessionSwitchReason reason) => reason == SessionSwitchReason.SessionLock;

    /// <summary>Whether a power mode change interrupts effects.</summary>
    public static bool Interrupts(PowerModes mode) => mode == PowerModes.Suspend;

    /// <summary>Stops reporting. Safe to repeat.</summary>
    public void Dispose()
    {
        SystemEvents.SessionSwitch -= OnSessionSwitch;
        SystemEvents.PowerModeChanged -= OnPowerModeChanged;
    }

    private void OnSessionSwitch(object? sender, SessionSwitchEventArgs e)
    {
        if (Interrupts(e.Reason))
        {
            RaiseInterrupted();
        }
    }

    private void OnPowerModeChanged(object? sender, PowerModeChangedEventArgs e)
    {
        if (Interrupts(e.Mode))
        {
            RaiseInterrupted();
        }
    }

    private void RaiseInterrupted() => dispatch(() => Interrupted?.Invoke());
}
