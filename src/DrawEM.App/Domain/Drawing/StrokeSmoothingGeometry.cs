namespace DrawEM.App.Domain.Drawing;

/// <summary>A straight or quadratic piece of a smoothed stroke path.</summary>
public abstract record StrokePathPiece
{
    private StrokePathPiece() { }

    public sealed record Line(ScreenPoint Start, ScreenPoint End) : StrokePathPiece;
    public sealed record Quadratic(ScreenPoint Start, ScreenPoint Control, ScreenPoint End) : StrokePathPiece;
}

/// <summary>Builds the midpoint quadratic path used by both completed and active stroke rendering.</summary>
public static class StrokeSmoothingGeometry
{
    public static IReadOnlyList<StrokePathPiece> CreatePieces(IReadOnlyList<ScreenPoint> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        if (points.Count < 2)
        {
            return [];
        }

        var pieces = new StrokePathPiece[points.Count];
        var previousMidpoint = Midpoint(points[0], points[1]);
        pieces[0] = new StrokePathPiece.Line(points[0], previousMidpoint);

        for (var i = 1; i < points.Count - 1; i++)
        {
            var nextMidpoint = Midpoint(points[i], points[i + 1]);
            pieces[i] = new StrokePathPiece.Quadratic(previousMidpoint, points[i], nextMidpoint);
            previousMidpoint = nextMidpoint;
        }

        pieces[^1] = new StrokePathPiece.Line(previousMidpoint, points[^1]);
        return pieces;
    }

    public static ScreenPoint Midpoint(ScreenPoint first, ScreenPoint second) =>
        new((int)(((long)first.X + second.X) / 2), (int)(((long)first.Y + second.Y) / 2));
}
