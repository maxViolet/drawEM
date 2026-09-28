using System.Runtime.ExceptionServices;

namespace DrawEM.App.Infrastructure;

public sealed class CompositeDisposable(params IDisposable[] disposables) : IDisposable
{
    private readonly IDisposable[] disposables = disposables;
    private bool disposed;

    /// <summary>
    /// Disposes every resource in order, even if one throws. Rethrows a single failure as is,
    /// or several as an <see cref="AggregateException"/>, after the last resource is disposed.
    /// </summary>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        List<Exception>? failures = null;
        foreach (var disposable in disposables)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception exception)
            {
                (failures ??= []).Add(exception);
            }
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
}
