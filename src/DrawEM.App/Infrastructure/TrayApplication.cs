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
    private readonly Action<Exception> reportCleanupFailure;
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
        : this(trayHost, shortcutRegistration, overlay, application, scheduleExit, ExceptionDispatchInfo.Throw)
    {
    }

    /// <param name="reportCleanupFailure">
    /// Receives cleanup failures after every exit step has run, instead of the caller.
    /// </param>
    public TrayApplication(
        ITrayHost trayHost,
        IDisposable shortcutRegistration,
        IOverlayLifetime overlay,
        IApplicationLifetime application,
        Action<Action> scheduleExit,
        Action<Exception> reportCleanupFailure)
    {
        this.trayHost = trayHost;
        this.shortcutRegistration = shortcutRegistration;
        this.overlay = overlay;
        this.application = application;
        this.scheduleExit = scheduleExit;
        this.reportCleanupFailure = reportCleanupFailure;
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
        List<Action> steps = [shortcutRegistration.Dispose, overlay.Close, trayHost.Dispose];
        if (shutDownApplication)
        {
            steps.Add(application.Shutdown);
        }

        try
        {
            CleanupSteps.RunAll(steps);
        }
        catch (Exception exception)
        {
            reportCleanupFailure(exception);
        }
    }
}
