using System.Runtime.ExceptionServices;

namespace DrawEM.App.Infrastructure;

public static class CleanupSteps
{
    /// <summary>
    /// Runs every step in order, even if one throws. Rethrows a single failure as is,
    /// or several as an <see cref="AggregateException"/>, after the last step has run.
    /// </summary>
    public static void RunAll(IEnumerable<Action> steps)
    {
        List<Exception>? failures = null;
        foreach (var step in steps)
        {
            try
            {
                step();
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
