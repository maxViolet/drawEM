namespace DrawEM.App.Domain.Settings;

/// <summary>
/// Opaque identity of one sound in drawEM's managed library. Only the media adapter knows how an identity
/// maps to a stored file; Domain compares and stores it as text.
/// </summary>
public sealed record ManagedSoundId
{
    /// <exception cref="ArgumentException"><paramref name="value"/> is blank.</exception>
    public ManagedSoundId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}

/// <summary>
/// A sound selected for a slot: its library identity and the original file name shown to the user.
/// </summary>
public sealed record SoundReference
{
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="displayName"/> is blank.</exception>
    public SoundReference(ManagedSoundId id, string displayName)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        Id = id;
        DisplayName = displayName;
    }

    public ManagedSoundId Id { get; }

    public string DisplayName { get; }
}

/// <summary>
/// What a filled slot does when its shortcut is pressed. Sound is the only v3 type; later versions add
/// subtypes without changing <see cref="ActionSlot"/>. A draft may lack a shortcut; a validated
/// <see cref="SettingsSnapshot"/> never does.
/// </summary>
public abstract record SlotAction(Shortcut? Shortcut);

public sealed record SoundAction(SoundReference Sound, Shortcut? Shortcut) : SlotAction(Shortcut)
{
    /// <summary>
    /// Never <c>null</c> when constructed; a <c>with</c> expression can still clear it, so
    /// <see cref="SettingsSnapshot.Validate"/> checks it again.
    /// </summary>
    public SoundReference Sound { get; init; } = Sound ?? throw new ArgumentNullException(nameof(Sound));
}

/// <summary>One of the eight numbered action slots. A slot with no action is empty and has no shortcut.</summary>
public sealed record ActionSlot
{
    public const int Count = 8;

    /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is outside 1–8.</exception>
    public ActionSlot(int number, SlotAction? action = null)
    {
        if (number is < 1 or > Count)
        {
            throw new ArgumentOutOfRangeException(nameof(number), number, $"Slot number must be 1–{Count}.");
        }

        Number = number;
        Action = action;
    }

    public int Number { get; }

    public SlotAction? Action { get; }

    public bool IsEmpty => Action is null;

    public static ActionSlot Empty(int number) => new(number);
}
