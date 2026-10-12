using Microsoft.Win32;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Reports Windows display-setting notifications and DPI changes observed by an active effect window on the
/// UI thread through the dispatch delegate.
/// </summary>
public sealed class Win32DisplayChangeSource : IDisplayChangeSource, IDisposable
{
    private readonly Action<Action> dispatch;

    public Win32DisplayChangeSource(Action<Action> dispatch)
    {
        this.dispatch = dispatch;
        SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
    }

    public event Action? DisplayChanged;

    /// <summary>Called by the active effect window after its monitor's DPI changes.</summary>
    public void NotifyDpiChanged() => RaiseDisplayChanged();

    public void Dispose() => SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;

    private void OnDisplaySettingsChanged(object? sender, EventArgs e) => RaiseDisplayChanged();

    private void RaiseDisplayChanged() => dispatch(() => DisplayChanged?.Invoke());
}
