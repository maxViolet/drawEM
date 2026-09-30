using System.Diagnostics.CodeAnalysis;

namespace DrawEM.App.Domain.Settings;

/// <summary>
/// Modifier keys held for a shortcut. <see cref="Alt"/> means Left Alt: Right Alt is reserved for AltGr
/// and has no value here. <see cref="Windows"/> exists only so a captured Win chord can be rejected.
/// </summary>
[Flags]
public enum ShortcutModifiers
{
    None = 0,
    Control = 1,
    Alt = 2,
    Shift = 4,
    Windows = 8,
}

public enum ShortcutError
{
    None,

    /// <summary>Fewer than two of Ctrl, Alt, and Shift.</summary>
    TooFewModifiers,

    /// <summary>The chord contains a Windows key.</summary>
    WindowsKey,

    /// <summary>The key is not a letter, digit, or F1–F12, for example <c>default(ShortcutKey)</c>.</summary>
    IllegalKey,

    /// <summary>The text does not name a legal key or modifier.</summary>
    Unrecognized,
}

/// <summary>
/// A valid shortcut: at least two of Ctrl, Alt (Left Alt), and Shift, plus exactly one letter, digit, or
/// F1–F12 key. Only <see cref="TryCreate"/> and <see cref="TryParse"/> create one.
/// </summary>
public sealed record Shortcut
{
    private const ShortcutModifiers Allowed = ShortcutModifiers.Control | ShortcutModifiers.Alt | ShortcutModifiers.Shift;

    private Shortcut(ShortcutModifiers modifiers, ShortcutKey key)
    {
        Modifiers = modifiers;
        Key = key;
    }

    public ShortcutModifiers Modifiers { get; }

    public ShortcutKey Key { get; }

    public static bool TryCreate(
        ShortcutModifiers modifiers,
        ShortcutKey key,
        [NotNullWhen(true)] out Shortcut? shortcut,
        out ShortcutError error)
    {
        shortcut = null;
        error = Check(modifiers);
        if (error == ShortcutError.None && !ShortcutKey.IsValid(key))
        {
            error = ShortcutError.IllegalKey;
        }

        if (error != ShortcutError.None)
        {
            return false;
        }

        shortcut = new Shortcut(modifiers, key);
        return true;
    }

    /// <exception cref="ArgumentException">The combination is not a valid shortcut.</exception>
    public static Shortcut Create(ShortcutModifiers modifiers, ShortcutKey key) =>
        TryCreate(modifiers, key, out var shortcut, out var error)
            ? shortcut
            : throw new ArgumentException($"Invalid shortcut: {error}.", nameof(modifiers));

    /// <summary>
    /// Parses the <see cref="ToString"/> form, for example <c>Ctrl+Alt+Z</c>. Tokens ignore case and order,
    /// but each modifier may appear once and the key must be last.
    /// </summary>
    public static bool TryParse(string? text, [NotNullWhen(true)] out Shortcut? shortcut, out ShortcutError error)
    {
        shortcut = null;
        error = ShortcutError.Unrecognized;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var tokens = text.Split('+');
        var modifiers = ShortcutModifiers.None;
        foreach (var token in tokens[..^1])
        {
            var modifier = token.ToUpperInvariant() switch
            {
                "CTRL" => ShortcutModifiers.Control,
                "ALT" => ShortcutModifiers.Alt,
                "SHIFT" => ShortcutModifiers.Shift,
                "WIN" => ShortcutModifiers.Windows,
                _ => ShortcutModifiers.None,
            };

            if (modifier == ShortcutModifiers.None || modifiers.HasFlag(modifier))
            {
                return false;
            }

            modifiers |= modifier;
        }

        if (!ShortcutKey.TryParse(tokens[^1], out var key))
        {
            return false;
        }

        return TryCreate(modifiers, key.Value, out shortcut, out error);
    }

    /// <summary>Canonical text: modifiers in the order Ctrl, Alt, Shift, then the key.</summary>
    public override string ToString()
    {
        var parts = new List<string>(4);
        if (Modifiers.HasFlag(ShortcutModifiers.Control))
        {
            parts.Add("Ctrl");
        }

        if (Modifiers.HasFlag(ShortcutModifiers.Alt))
        {
            parts.Add("Alt");
        }

        if (Modifiers.HasFlag(ShortcutModifiers.Shift))
        {
            parts.Add("Shift");
        }

        parts.Add(Key.ToString());
        return string.Join('+', parts);
    }

    private static ShortcutError Check(ShortcutModifiers modifiers)
    {
        if (modifiers.HasFlag(ShortcutModifiers.Windows))
        {
            return ShortcutError.WindowsKey;
        }

        if ((modifiers & ~Allowed) != 0)
        {
            return ShortcutError.Unrecognized;
        }

        var count = (modifiers.HasFlag(ShortcutModifiers.Control) ? 1 : 0) +
            (modifiers.HasFlag(ShortcutModifiers.Alt) ? 1 : 0) +
            (modifiers.HasFlag(ShortcutModifiers.Shift) ? 1 : 0);
        return count >= 2 ? ShortcutError.None : ShortcutError.TooFewModifiers;
    }
}
