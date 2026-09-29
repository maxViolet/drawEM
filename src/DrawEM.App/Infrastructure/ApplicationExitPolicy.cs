using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.App.Infrastructure;

/// <summary>
/// Used on the UI thread. After the application has attempted resource cleanup, drains its log
/// and terminates the process only when a sound shutdown timeout left playback potentially alive.
/// </summary>
public sealed class ApplicationExitPolicy(AppFailureLog log, Action<int> terminateProcess)
{
    private bool soundTimedOut;
    private bool completed;

    public void Report(string operation, Exception failure)
    {
        soundTimedOut |= failure is SoundChannelShutdownTimeoutException
            || failure is AggregateException aggregate
                && aggregate.Flatten().InnerExceptions.Any(error => error is SoundChannelShutdownTimeoutException);
        log.Append(operation, failure);
    }

    public void Complete()
    {
        if (completed)
        {
            return;
        }

        completed = true;
        log.Dispose();
        if (soundTimedOut)
        {
            // IsBackground means sound cannot keep the CLR alive after foreground threads end.
            // It does not end this still-running UI thread. After attempted cleanup and bounded
            // log drain, the timeout policy deliberately exits now instead of continuing WPF
            // callbacks or waiting for startup UI. This is not needed for ordinary shutdown.
            terminateProcess(1);
        }
    }

    /// <summary>After startup cleanup, a stuck sound thread must not wait for a modal dialog.</summary>
    public void ShowStartupError(Action showError)
    {
        if (soundTimedOut)
        {
            Complete();
        }
        else
        {
            showError();
        }
    }
}
