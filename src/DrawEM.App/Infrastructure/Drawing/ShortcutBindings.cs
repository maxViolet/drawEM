using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;

namespace DrawEM.App.Infrastructure.Drawing;

/// <summary>A shortcut in hook terms: its modifiers and the virtual-key code of its key.</summary>
public readonly record struct KeyChord(ShortcutModifiers Modifiers, int VirtualKey)
{
    public static KeyChord From(Shortcut shortcut) => new(shortcut.Modifiers, VirtualKeyOf(shortcut.Key));

    /// <exception cref="ArgumentOutOfRangeException"><paramref name="key"/> is <c>default(ShortcutKey)</c>.</exception>
    public static int VirtualKeyOf(ShortcutKey key) => key.Kind switch
    {
        _ when !ShortcutKey.IsValid(key) => throw new ArgumentOutOfRangeException(nameof(key), key, "Invalid shortcut key."),
        ShortcutKeyKind.Letter => key.Value,
        ShortcutKeyKind.Digit => VirtualKeys.D0 + key.Value,
        _ => VirtualKeys.F1 + key.Value - 1,
    };

    /// <summary>The reverse of <see cref="VirtualKeyOf"/>: a letter, a main-row digit, or F1–F12.</summary>
    public static bool TryGetShortcutKey(int vkCode, out ShortcutKey key)
    {
        switch (vkCode)
        {
            case >= VirtualKeys.A and <= VirtualKeys.Z:
                key = ShortcutKey.Letter((char)vkCode);
                return true;
            case >= VirtualKeys.D0 and <= VirtualKeys.D0 + 9:
                key = ShortcutKey.Digit(vkCode - VirtualKeys.D0);
                return true;
            case >= VirtualKeys.F1 and <= VirtualKeys.F12:
                key = ShortcutKey.Function(vkCode - VirtualKeys.F1 + 1);
                return true;
            default:
                key = default;
                return false;
        }
    }
}

/// <summary>A sound shortcut with its prepared play command.</summary>
public sealed record SoundBinding(KeyChord Chord, PlaySoundCommand Command);

/// <summary>
/// One immutable set of active shortcuts for the keyboard hook: draw, clear, and the filled sound slots.
/// Every command is prepared when the bindings are built, so the hook callback only compares keys.
/// </summary>
public sealed class ShortcutBindings
{
    /// <exception cref="ArgumentException">Two commands use the same shortcut.</exception>
    public ShortcutBindings(Shortcut draw, Shortcut clear, IEnumerable<(Shortcut Shortcut, PlaySoundCommand Command)> sounds)
    {
        ArgumentNullException.ThrowIfNull(draw);
        ArgumentNullException.ThrowIfNull(clear);
        ArgumentNullException.ThrowIfNull(sounds);
        Draw = KeyChord.From(draw);
        Clear = KeyChord.From(clear);
        Sounds = Array.AsReadOnly(sounds.Select(sound => new SoundBinding(KeyChord.From(sound.Shortcut), sound.Command)).ToArray());

        var chords = new HashSet<KeyChord> { Draw };
        foreach (var chord in Sounds.Select(sound => sound.Chord).Prepend(Clear))
        {
            if (!chords.Add(chord))
            {
                throw new ArgumentException($"Two commands use the shortcut {chord}.", nameof(sounds));
            }
        }
    }

    public KeyChord Draw { get; }

    public KeyChord Clear { get; }

    public IReadOnlyList<SoundBinding> Sounds { get; }

    /// <summary>Binds draw, clear, and every filled slot of <paramref name="snapshot"/>.</summary>
    /// <param name="resolveSound">Prepares the play command for a slot's sound. Runs here, never in the hook.</param>
    public static ShortcutBindings FromSnapshot(SettingsSnapshot snapshot, Func<SoundReference, PlaySoundCommand> resolveSound)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(resolveSound);
        var sounds = snapshot.Slots
            .Select(slot => slot.Action)
            .OfType<SoundAction>()
            .Select(action => (action.Shortcut!, resolveSound(action.Sound)));
        return new ShortcutBindings(snapshot.DrawShortcut, snapshot.ClearShortcut, sounds);
    }
}
