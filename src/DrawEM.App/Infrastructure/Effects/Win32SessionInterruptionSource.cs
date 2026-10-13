using Microsoft.Win32;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Reports Windows session locks and system suspends at once, on the thread Windows reports them on, so queued
/// effect starts are dropped before the UI thread runs any of them. Unlock and resume are not reported: an
/// interrupted effect never comes back.
/// </summary>
public sealed class Win32SessionInterruptionSource : ISessionInterruptionSource, IDisposable
{
    public Win32SessionInterruptionSource()
    {
        SystemEvents.SessionSwitch += OnSessionSwitch;
        SystemEvents.PowerModeChanged += OnPowerModeChanged;
    }

    public event Action? Interrupted;

    /// <summary>Handles a session switch as Windows reports it. Only a lock interrupts effects.</summary>
    public void Report(SessionSwitchReason reason)
    {
        if (reason == SessionSwitchReason.SessionLock)
        {
            RaiseInterrupted();
        }
    }

    /// <summary>Handles a power mode change as Windows reports it. Only a suspend interrupts effects.</summary>
    public void Report(PowerModes mode)
    {
        if (mode == PowerModes.Suspend)
        {
            RaiseInterrupted();
        }
    }

    /// <summary>Stops reporting. Safe to repeat.</summary>
    public void Dispose()
    {
        SystemEvents.SessionSwitch -= OnSessionSwitch;
        SystemEvents.PowerModeChanged -= OnPowerModeChanged;
    }

    private void OnSessionSwitch(object? sender, SessionSwitchEventArgs e) => Report(e.Reason);

    private void OnPowerModeChanged(object? sender, PowerModeChangedEventArgs e) => Report(e.Mode);

    private void RaiseInterrupted() => Interrupted?.Invoke();
}
