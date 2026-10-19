using System.Security.Principal;

namespace DrawEM.App.Infrastructure;

/// <summary>
/// Lets a second launch hand control to the running drawEM instead of showing an error. The running
/// instance listens on a named event; a second instance sets it and exits. The installer starts drawEM
/// with no window, so the next click on its shortcut is such a second launch.
/// </summary>
public sealed class InstanceActivation : IDisposable
{
    private readonly EventWaitHandle signal;
    private readonly RegisteredWaitHandle registration;
    private bool disposed;

    private InstanceActivation(EventWaitHandle signal, Action activated)
    {
        this.signal = signal;
        registration = ThreadPool.RegisterWaitForSingleObject(
            signal, (_, _) => activated(), null, Timeout.Infinite, executeOnlyOnce: false);
    }

    /// <summary><c>Global\drawEM-activate-&lt;user SID&gt;</c> for the current Windows user.</summary>
    public static string DefaultName { get; } =
        $@"Global\drawEM-activate-{WindowsIdentity.GetCurrent().User?.Value ?? Environment.UserName}";

    /// <summary>
    /// Calls <paramref name="activated"/> on a thread-pool thread each time another instance signals
    /// <paramref name="name"/>, until disposed.
    /// </summary>
    public static InstanceActivation Listen(string name, Action activated) =>
        new(new EventWaitHandle(false, EventResetMode.AutoReset, name), activated);

    /// <summary>
    /// Signals the instance that listens on <paramref name="name"/>. Returns <c>false</c> when none listens,
    /// for example while it still starts.
    /// </summary>
    public static bool Signal(string name)
    {
        if (!EventWaitHandle.TryOpenExisting(name, out var signal))
        {
            return false;
        }

        using (signal)
        {
            // This launch holds the foreground right; the running instance needs it to bring its window up.
            NativeMethods.AllowSetForegroundWindow(NativeMethods.ASFW_ANY);
            return signal.Set();
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        registration.Unregister(null);
        signal.Dispose();
    }
}
