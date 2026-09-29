namespace DrawEM.App.Infrastructure;

public static class StartupFailure
{
    /// <summary>
    /// Releases what startup created, then shows the startup error and shuts down. A cleanup failure
    /// is reported instead of thrown, so it cannot hide the startup error or keep the process running.
    /// </summary>
    public static void Handle(
        Action cleanup,
        Action<Exception> reportCleanupFailure,
        Action showError,
        Action shutdown)
    {
        try
        {
            cleanup();
        }
        catch (Exception exception)
        {
            reportCleanupFailure(exception);
        }

        try
        {
            showError();
        }
        finally
        {
            shutdown();
        }
    }
}
