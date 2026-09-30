using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DrawEM.App.Domain.Settings;

public enum ShortcutKeyKind
{
    Letter,
    Digit,
    Function,
}

/// <summary>
/// The one non-modifier key of a shortcut: a letter A–Z, a digit 0–9, or F1–F12. Other keys are not
/// representable. Carries no virtual-key code; the platform adapter maps keys to and from this type.
/// </summary>
public readonly record struct ShortcutKey
{
    private ShortcutKey(ShortcutKeyKind kind, int value)
    {
        Kind = kind;
        Value = value;
    }

    public ShortcutKeyKind Kind { get; }

    /// <summary>The letter's character code, the digit 0–9, or the function key number 1–12.</summary>
    public int Value { get; }

    /// <exception cref="ArgumentOutOfRangeException"><paramref name="letter"/> is not A–Z (either case).</exception>
    public static ShortcutKey Letter(char letter)
    {
        var upper = char.ToUpperInvariant(letter);
        if (upper is < 'A' or > 'Z')
        {
            throw new ArgumentOutOfRangeException(nameof(letter), letter, "Shortcut letter must be A–Z.");
        }

        return new ShortcutKey(ShortcutKeyKind.Letter, upper);
    }

    /// <exception cref="ArgumentOutOfRangeException"><paramref name="digit"/> is not 0–9.</exception>
    public static ShortcutKey Digit(int digit)
    {
        if (digit is < 0 or > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(digit), digit, "Shortcut digit must be 0–9.");
        }

        return new ShortcutKey(ShortcutKeyKind.Digit, digit);
    }

    /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is not 1–12.</exception>
    public static ShortcutKey Function(int number)
    {
        if (number is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(number), number, "Shortcut function key must be F1–F12.");
        }

        return new ShortcutKey(ShortcutKeyKind.Function, number);
    }

    /// <summary>
    /// <c>false</c> for <c>default(ShortcutKey)</c>, the only value the factories cannot produce.
    /// </summary>
    public static bool IsValid(ShortcutKey key) => key.Kind switch
    {
        ShortcutKeyKind.Letter => key.Value is >= 'A' and <= 'Z',
        ShortcutKeyKind.Digit => key.Value is >= 0 and <= 9,
        ShortcutKeyKind.Function => key.Value is >= 1 and <= 12,
        _ => false,
    };

    /// <summary>Parses <c>A</c>–<c>Z</c>, <c>0</c>–<c>9</c>, or <c>F1</c>–<c>F12</c>, ignoring case.</summary>
    public static bool TryParse(string? text, [NotNullWhen(true)] out ShortcutKey? key)
    {
        key = null;
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        if (text.Length == 1)
        {
            var c = char.ToUpperInvariant(text[0]);
            if (c is >= 'A' and <= 'Z')
            {
                key = Letter(c);
            }
            else if (c is >= '0' and <= '9')
            {
                key = Digit(c - '0');
            }

            return key is not null;
        }

        if (text[0] is 'F' or 'f' && text.Length <= 3 && text[1] != '0' &&
            int.TryParse(text.AsSpan(1), NumberStyles.None, CultureInfo.InvariantCulture, out var number) &&
            number is >= 1 and <= 12)
        {
            key = Function(number);
        }

        return key is not null;
    }

    public override string ToString() => Kind switch
    {
        ShortcutKeyKind.Letter => ((char)Value).ToString(),
        ShortcutKeyKind.Digit => Value.ToString(CultureInfo.InvariantCulture),
        _ => "F" + Value.ToString(CultureInfo.InvariantCulture),
    };
}
