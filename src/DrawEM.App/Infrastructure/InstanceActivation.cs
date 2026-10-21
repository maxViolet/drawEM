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
    private readonly ManualResetEvent stop = new(false);
    private readonly Thread listener;
    private bool disposed;

    // A thread of its own, not ThreadPool.RegisterWaitForSingleObject: the pool's wait thread lets go of the
    // event only some time after Unregister, so after Dispose another Signal could still open it.
    private InstanceActivation(EventWaitHandle signal, Action activated)
    {
        this.signal = signal;
        listener = new Thread(() => Run(activated)) { IsBackground = true, Name = "drawEM activation" };
        listener.Start();
    }

    /// <summary><c>Global\drawEM-activate-&lt;user SID&gt;</c> for the current Windows user.</summary>
    public static string DefaultName { get; } =
        $@"Global\drawEM-activate-{WindowsIdentity.GetCurrent().User?.Value ?? Environment.UserName}";

    /// <summary>
    /// Calls <paramref name="activated"/> on a background thread each time another instance signals
    /// <paramref name="name"/>, until disposed. <paramref name="activated"/> must not block on the thread
    /// that disposes the listener.
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
        stop.Set();
        listener.Join();
        signal.Dispose();
        stop.Dispose();
    }

    private void Run(Action activated)
    {
        WaitHandle[] handles = [stop, signal];
        while (WaitHandle.WaitAny(handles) == 1)
        {
            activated();
        }
    }
}
