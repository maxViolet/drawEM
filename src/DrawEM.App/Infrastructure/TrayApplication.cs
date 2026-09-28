namespace DrawEM.App.Infrastructure;

public interface ITrayHost : IDisposable
{
    event Action? ExitRequested;

    void Show();
}

public interface IOverlayLifetime
{
    void Close();
}

public interface IApplicationLifetime
{
    void Shutdown();
}

public sealed class TrayApplication : IDisposable
{
    private readonly ITrayHost trayHost;
    private readonly IDisposable shortcutRegistration;
    private readonly IOverlayLifetime overlay;
    private readonly IApplicationLifetime application;
    private readonly Action<Action> scheduleExit;
    private bool exited;

    public TrayApplication(
        ITrayHost trayHost,
        IDisposable shortcutRegistration,
        IOverlayLifetime overlay,
        IApplicationLifetime application)
        : this(trayHost, shortcutRegistration, overlay, application, action => action())
    {
    }

    public TrayApplication(
        ITrayHost trayHost,
        IDisposable shortcutRegistration,
        IOverlayLifetime overlay,
        IApplicationLifetime application,
        Action<Action> scheduleExit)
    {
        this.trayHost = trayHost;
        this.shortcutRegistration = shortcutRegistration;
        this.overlay = overlay;
        this.application = application;
        this.scheduleExit = scheduleExit;
        trayHost.ExitRequested += OnExitRequested;
    }

    public void Start() => trayHost.Show();

    public void Exit() => Stop(shutDownApplication: true);

    public void Dispose() => Stop(shutDownApplication: false);

    private void OnExitRequested() => scheduleExit(Exit);

    private void Stop(bool shutDownApplication)
    {
        if (exited)
        {
            return;
        }

        exited = true;
        trayHost.ExitRequested -= OnExitRequested;
        try
        {
            shortcutRegistration.Dispose();
        }
        finally
        {
            // A failed release must not leave the overlay open or the process running.
            overlay.Close();
            trayHost.Dispose();
            if (shutDownApplication)
            {
                application.Shutdown();
            }
        }
    }
}
