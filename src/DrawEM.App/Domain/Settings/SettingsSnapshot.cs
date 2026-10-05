using System.Collections.ObjectModel;

namespace DrawEM.App.Domain.Settings;

/// <summary>A command that owns a shortcut: drawing, clear, or one numbered action slot.</summary>
public readonly record struct SettingsCommand(SettingsCommandKind Kind, int Slot = 0)
{
    public static SettingsCommand Draw { get; } = new(SettingsCommandKind.Draw);

    public static SettingsCommand Clear { get; } = new(SettingsCommandKind.Clear);

    public static SettingsCommand ForSlot(int slot) => new(SettingsCommandKind.Slot, slot);

    public override string ToString() => Kind switch
    {
        SettingsCommandKind.Draw => "Draw",
        SettingsCommandKind.Clear => "Clear",
        _ => $"Slot {Slot}",
    };
}

public enum SettingsCommandKind
{
    Draw,
    Clear,
    Slot,
}

public enum SettingsErrorCode
{
    /// <summary>Draw, clear, or a filled slot has no shortcut.</summary>
    MissingShortcut,

    /// <summary>Two active commands use the same shortcut. <see cref="SettingsError.Other"/> names the other.</summary>
    DuplicateShortcut,

    /// <summary>The slots are not exactly slots 1–8 in order.</summary>
    SlotLayout,

    /// <summary>The stroke width is outside 1–20, for example <c>default(StrokeWidth)</c>.</summary>
    InvalidWidth,

    /// <summary>A filled slot has no sound.</summary>
    MissingSound,

    /// <summary>A shortcut fails <see cref="Shortcut.TryCreate"/> rules.</summary>
    InvalidShortcut,
}

public sealed record SettingsError(SettingsErrorCode Code, SettingsCommand? Command = null, SettingsCommand? Other = null)
{
    public override string ToString() => Describe(command => command.ToString());

    /// <summary>The message, with each command named by <paramref name="name"/>.</summary>
    public string Describe(Func<SettingsCommand, string> name)
    {
        string Name(SettingsCommand? command) => command is { } value ? name(value) : "";

        return Code switch
        {
            SettingsErrorCode.MissingShortcut => $"{Name(Command)} requires a shortcut.",
            SettingsErrorCode.DuplicateShortcut => $"{Name(Command)} uses the same shortcut as {Name(Other)}.",
            SettingsErrorCode.InvalidWidth => $"Width must be {StrokeWidth.Min}–{StrokeWidth.Max} physical pixels.",
            SettingsErrorCode.MissingSound => $"{Name(Command)} requires a sound.",
            SettingsErrorCode.InvalidShortcut => $"{Name(Command)} has an invalid shortcut.",
            _ => $"Settings must contain exactly slots 1–{ActionSlot.Count} in order.",
        };
    }
}

/// <summary>Either a snapshot or the reasons it is invalid.</summary>
public sealed record SettingsValidation(SettingsSnapshot? Snapshot, IReadOnlyList<SettingsError> Errors)
{
    public bool IsValid => Snapshot is not null;
}

/// <summary>
/// One immutable, validated configuration: drawing style, required draw and clear shortcuts, and exactly
/// eight numbered action slots. Every active shortcut is present and unique. Create with
/// <see cref="Validate"/>.
/// </summary>
public sealed record SettingsSnapshot
{
    private SettingsSnapshot(DrawingStyle style, Shortcut draw, Shortcut clear, ReadOnlyCollection<ActionSlot> slots)
    {
        Style = style;
        DrawShortcut = draw;
        ClearShortcut = clear;
        Slots = slots;
    }

    /// <summary>
    /// <c>#FF4500</c>, 4 physical pixels, <c>Ctrl+Alt+Z</c>, <c>Ctrl+Alt+X</c>, and eight empty slots.
    /// </summary>
    public static SettingsSnapshot Default { get; } = Validate(
        new DrawingStyle(new HexColor(0xFF, 0x45, 0x00), new StrokeWidth(4)),
        Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Alt, ShortcutKey.Letter('Z')),
        Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Alt, ShortcutKey.Letter('X')),
        Enumerable.Range(1, ActionSlot.Count).Select(ActionSlot.Empty)).Snapshot!;

    public DrawingStyle Style { get; }

    public Shortcut DrawShortcut { get; }

    public Shortcut ClearShortcut { get; }

    /// <summary>Slots 1–8, in order.</summary>
    public IReadOnlyList<ActionSlot> Slots { get; }

    public static SettingsValidation Validate(
        DrawingStyle style,
        Shortcut? draw,
        Shortcut? clear,
        IEnumerable<ActionSlot> slots)
    {
        ArgumentNullException.ThrowIfNull(style);
        var slotList = slots.ToArray();
        var errors = new List<SettingsError>();

        if (slotList.Length != ActionSlot.Count || slotList.Where((slot, index) => slot.Number != index + 1).Any())
        {
            errors.Add(new SettingsError(SettingsErrorCode.SlotLayout));
        }

        if (!StrokeWidth.IsValid(style.Width.Pixels))
        {
            errors.Add(new SettingsError(SettingsErrorCode.InvalidWidth));
        }

        foreach (var slot in slotList.Where(slot => slot.Action is SoundAction { Sound: null }))
        {
            errors.Add(new SettingsError(SettingsErrorCode.MissingSound, SettingsCommand.ForSlot(slot.Number)));
        }

        var active = new List<(SettingsCommand Command, Shortcut? Shortcut)>
        {
            (SettingsCommand.Draw, draw),
            (SettingsCommand.Clear, clear),
        };
        active.AddRange(slotList
            .Where(slot => !slot.IsEmpty)
            .Select(slot => (SettingsCommand.ForSlot(slot.Number), slot.Action!.Shortcut)));

        var owners = new Dictionary<Shortcut, SettingsCommand>();
        foreach (var (command, shortcut) in active)
        {
            if (shortcut is null)
            {
                errors.Add(new SettingsError(SettingsErrorCode.MissingShortcut, command));
            }
            else if (!Shortcut.TryCreate(shortcut.Modifiers, shortcut.Key, out _, out _))
            {
                errors.Add(new SettingsError(SettingsErrorCode.InvalidShortcut, command));
            }
            else if (owners.TryGetValue(shortcut, out var owner))
            {
                errors.Add(new SettingsError(SettingsErrorCode.DuplicateShortcut, command, owner));
            }
            else
            {
                owners.Add(shortcut, command);
            }
        }

        return errors.Count > 0
            ? new SettingsValidation(null, errors.AsReadOnly())
            : new SettingsValidation(new SettingsSnapshot(style, draw!, clear!, Array.AsReadOnly(slotList)), []);
    }

    public bool Equals(SettingsSnapshot? other) =>
        other is not null &&
        Style == other.Style &&
        DrawShortcut == other.DrawShortcut &&
        ClearShortcut == other.ClearShortcut &&
        Slots.SequenceEqual(other.Slots);

    public override int GetHashCode() => HashCode.Combine(Style, DrawShortcut, ClearShortcut, Slots.Count);
}
