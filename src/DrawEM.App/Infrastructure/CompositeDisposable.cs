namespace DrawEM.App.Infrastructure;

public sealed class CompositeDisposable(params IDisposable[] disposables) : IDisposable
{
    private readonly IDisposable[] disposables = disposables;
    private bool disposed;

    /// <summary>Disposes every resource in order, even if one throws. See <see cref="CleanupSteps.RunAll"/>.</summary>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        CleanupSteps.RunAll(disposables.Select(disposable => (Action)disposable.Dispose));
    }
}
