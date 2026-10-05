using System.Runtime.ExceptionServices;

namespace DrawEM.Tests;

internal static class StaThread
{
    /// <summary>Runs <paramref name="action"/> on a new STA thread, as WPF requires, and rethrows its failure.</summary>
    public static void Run(Action action)
    {
        ExceptionDispatchInfo? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                failure = ExceptionDispatchInfo.Capture(exception);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        failure?.Throw();
    }
}
