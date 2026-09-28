using System.Runtime.ExceptionServices;

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

        // A failed step must not skip the later ones, so the process still shuts down.
        List<Exception>? failures = null;
        Attempt(shortcutRegistration.Dispose, ref failures);
        Attempt(overlay.Close, ref failures);
        Attempt(trayHost.Dispose, ref failures);
        if (shutDownApplication)
        {
            Attempt(application.Shutdown, ref failures);
        }

        if (failures is [var failure])
        {
            ExceptionDispatchInfo.Throw(failure);
        }

        if (failures is not null)
        {
            throw new AggregateException(failures);
        }
    }

    private static void Attempt(Action step, ref List<Exception>? failures)
    {
        try
        {
            step();
        }
        catch (Exception exception)
        {
            (failures ??= []).Add(exception);
        }
    }
}
