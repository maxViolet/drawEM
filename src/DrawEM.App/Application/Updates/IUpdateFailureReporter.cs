namespace DrawEM.App.Application.Updates;

/// <summary>
/// Records a failed update operation, such as <c>check</c> or <c>apply</c>. Called on the controller thread.
/// Must not throw and must not block on I/O.
/// </summary>
public interface IUpdateFailureReporter
{
    void Report(string operation, string reason);
}
