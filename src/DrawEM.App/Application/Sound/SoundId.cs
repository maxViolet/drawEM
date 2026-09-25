namespace DrawEM.App.Application.Sound;

/// <summary>
/// Stable name of one configured sound. Carries no file path, device, or shortcut detail.
/// </summary>
public sealed record SoundId
{
    public SoundId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }
}
