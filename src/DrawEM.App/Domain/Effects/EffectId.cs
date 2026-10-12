namespace DrawEM.App.Domain.Effects;

/// <summary>
/// Stable name of one built-in effect, such as <c>confetti</c>. Display names and files may change without
/// changing the ID. Carries no placement, resource, or shortcut detail.
/// </summary>
public sealed record EffectId
{
    public EffectId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }
}
