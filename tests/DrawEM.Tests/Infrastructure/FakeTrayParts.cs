using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

/// <summary>Records tray host calls in order and raises tray commands on request.</summary>
internal sealed class FakeTrayHost(List<string> calls, bool throwOnDispose = false) : ITrayHost
{
    public event Action? ExitRequested;

    public event Action? SettingsRequested;

    public event Action? StopEffectsRequested;

    public event Action? UpdateRequested;

    public void Show() => calls.Add("tray shown");

    public void ShowUpdateReady(string version) => calls.Add("update ready " + version);

    public void Dispose()
    {
        calls.Add("tray disposed");
        if (throwOnDispose)
        {
            throw new InvalidOperationException("Tray icon removal failed.");
        }
    }

    public void RequestExit() => ExitRequested?.Invoke();

    public void RequestSettings() => SettingsRequested?.Invoke();

    public void RequestStopEffects() => StopEffectsRequested?.Invoke();

    public void RequestUpdate() => UpdateRequested?.Invoke();
}

internal sealed class FakeDisposable(List<string> calls, string name) : IDisposable
{
    public void Dispose() => calls.Add(name);
}

internal sealed class FakeOverlay(List<string> calls, bool throwOnClose = false) : IOverlayLifetime
{
    public void Close()
    {
        calls.Add("overlay");
        if (throwOnClose)
        {
            throw new InvalidOperationException("Overlay close failed.");
        }
    }
}

internal sealed class FakeApplicationLifetime(List<string> calls) : IApplicationLifetime
{
    public void Shutdown() => calls.Add("shutdown");
}
