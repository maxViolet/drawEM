namespace DrawEM.App.Infrastructure.Updates;

/// <summary>One failed update operation, such as <c>check</c> or <c>apply</c>, and why it failed.</summary>
public sealed record UpdateFailure(DateTimeOffset Time, string Operation, string Reason);
