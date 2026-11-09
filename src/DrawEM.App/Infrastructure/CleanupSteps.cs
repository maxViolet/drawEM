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
        var failures = Failures(steps);
        if (failures is [var failure])
        {
            ExceptionDispatchInfo.Throw(failure);
        }

        if (failures.Count > 0)
        {
            throw new AggregateException(failures);
        }
    }

    /// <summary>Runs every step in order, even if one throws, and returns each failure in order.</summary>
    public static IReadOnlyList<Exception> Failures(IEnumerable<Action> steps)
    {
        var failures = new List<Exception>();
        foreach (var step in steps)
        {
            try
            {
                step();
            }
            catch (Exception exception)
            {
                failures.Add(exception);
            }
        }

        return failures;
    }
}
