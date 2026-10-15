using Microsoft.Win32;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Reports Windows display-setting notifications and DPI changes observed by an active effect window at once,
/// on the thread that observed them, so queued effect starts are dropped before the UI thread runs any of them.
/// </summary>
public sealed class Win32DisplayChangeSource : IDisplayChangeSource, IDisposable
{
    public Win32DisplayChangeSource()
    {
        SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
    }

    public event Action? DisplayChanged;

    /// <summary>Called by the active effect window after its monitor's DPI changes.</summary>
    public void NotifyDpiChanged() => RaiseDisplayChanged();

    /// <summary>Handles a display settings notification as Windows reports it.</summary>
    public void Report() => RaiseDisplayChanged();

    public void Dispose() => SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;

    private void OnDisplaySettingsChanged(object? sender, EventArgs e) => Report();

    private void RaiseDisplayChanged() => DisplayChanged?.Invoke();
}
