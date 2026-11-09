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
        ShortcutKeyKind.Letter => InRange(key.Value, 'A', 'Z'),
        ShortcutKeyKind.Digit => InRange(key.Value, 0, 9),
        ShortcutKeyKind.Function => InRange(key.Value, 1, 12),
        _ => false,
    };

    /// <summary>Parses <c>A</c>–<c>Z</c>, <c>0</c>–<c>9</c>, or <c>F1</c>–<c>F12</c>, ignoring case.</summary>
    public static bool TryParse(string? text, [NotNullWhen(true)] out ShortcutKey? key)
    {
        key = string.IsNullOrEmpty(text) ? null
            : text.Length == 1 ? ParseCharacter(text[0])
            : ParseFunction(text);
        return key is not null;
    }

    private static ShortcutKey? ParseCharacter(char character)
    {
        var c = char.ToUpperInvariant(character);
        if (InRange(c, 'A', 'Z'))
        {
            return Letter(c);
        }

        return InRange(c, '0', '9') ? Digit(c - '0') : null;
    }

    /// <summary><c>F1</c>–<c>F12</c> with no leading zero, for text of two or more characters.</summary>
    private static ShortcutKey? ParseFunction(string text) =>
        HasFunctionPrefix(text) ? ParseFunctionNumber(text.AsSpan(1)) : null;

    private static bool HasFunctionPrefix(string text) =>
        char.ToUpperInvariant(text[0]) == 'F' && text.Length <= 3 && text[1] != '0';

    private static ShortcutKey? ParseFunctionNumber(ReadOnlySpan<char> digits) =>
        int.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out var number) && InRange(number, 1, 12)
            ? Function(number)
            : null;

    private static bool InRange(int value, int min, int max) => value >= min && value <= max;

    public override string ToString() => Kind switch
    {
        ShortcutKeyKind.Letter => ((char)Value).ToString(),
        ShortcutKeyKind.Digit => Value.ToString(CultureInfo.InvariantCulture),
        _ => "F" + Value.ToString(CultureInfo.InvariantCulture),
    };
}
