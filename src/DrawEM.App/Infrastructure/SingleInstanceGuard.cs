using System.Security.Principal;

namespace DrawEM.App.Infrastructure;

/// <summary>
/// Holds a named mutex while drawEM runs, so one Windows user runs one instance. A second instance would
/// register the global shortcuts again and share the per-user settings and sound library. The name is
/// global and includes the user's SID, so the rule also holds across that user's sessions.
/// </summary>
public sealed class SingleInstanceGuard : IDisposable
{
    private readonly Mutex mutex;
    private bool disposed;

    private SingleInstanceGuard(Mutex mutex)
    {
        this.mutex = mutex;
    }

    /// <summary><c>Global\drawEM-&lt;user SID&gt;</c> for the current Windows user.</summary>
    public static string DefaultName { get; } =
        $@"Global\drawEM-{WindowsIdentity.GetCurrent().User?.Value ?? Environment.UserName}";

    /// <summary>
    /// Returns the guard when no other process holds <paramref name="name"/>, or <c>null</c> when another
    /// instance runs. A mutex left by an instance that crashed or was forced to exit is taken over.
    /// Dispose on the thread that acquired it.
    /// </summary>
    public static SingleInstanceGuard? TryAcquire(string name)
    {
        var mutex = new Mutex(initiallyOwned: false, name);
        try
        {
            if (mutex.WaitOne(TimeSpan.Zero))
            {
                return new SingleInstanceGuard(mutex);
            }
        }
        catch (AbandonedMutexException)
        {
            return new SingleInstanceGuard(mutex);
        }

        mutex.Dispose();
        return null;
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        try
        {
            mutex.ReleaseMutex();
        }
        finally
        {
            mutex.Dispose();
        }
    }
}
