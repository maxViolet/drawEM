using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DrawEM.App.Domain.Drawing;

/// <summary>An opaque RGB color written as <c>#RRGGBB</c>.</summary>
public readonly record struct HexColor(byte Red, byte Green, byte Blue)
{
    /// <summary>Parses exactly <c>#</c> followed by six hexadecimal digits, ignoring case.</summary>
    public static bool TryParse(string? text, [NotNullWhen(true)] out HexColor? color)
    {
        color = null;
        if (text is not { Length: 7 } || text[0] != '#' ||
            !uint.TryParse(text.AsSpan(1), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var rgb))
        {
            return false;
        }

        color = new HexColor((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        return true;
    }

    /// <summary>Uppercase <c>#RRGGBB</c>.</summary>
    public override string ToString() => $"#{Red:X2}{Green:X2}{Blue:X2}";
}

/// <summary>A stroke width in physical pixels, 1–20.</summary>
public readonly record struct StrokeWidth
{
    public const int Min = 1;
    public const int Max = 20;

    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixels"/> is outside 1–20.</exception>
    public StrokeWidth(int pixels)
    {
        if (!IsValid(pixels))
        {
            throw new ArgumentOutOfRangeException(nameof(pixels), pixels, $"Width must be {Min}–{Max} physical pixels.");
        }

        Pixels = pixels;
    }

    public int Pixels { get; }

    public static bool IsValid(int pixels) => pixels is >= Min and <= Max;

    public override string ToString() => Pixels.ToString(CultureInfo.InvariantCulture);
}

public sealed record DrawingStyle(HexColor Color, StrokeWidth Width);
