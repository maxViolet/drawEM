namespace DrawEM.App.Infrastructure;

public sealed class CompositeDisposable(params IDisposable[] disposables) : IDisposable
{
    private readonly IDisposable[] disposables = disposables;
    private bool disposed;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        foreach (var disposable in disposables)
        {
            disposable.Dispose();
        }
    }
}
