using Microsoft.Win32;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Listens for <see cref="SystemEvents.DisplaySettingsChanged"/> (WM_DISPLAYCHANGE) and raises
/// <see cref="DisplayChanged"/> on the UI thread through the dispatch delegate, whatever thread Windows used.
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

    public void Dispose() => SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;

    private void OnDisplaySettingsChanged(object? sender, EventArgs e) => dispatch(() => DisplayChanged?.Invoke());
}
