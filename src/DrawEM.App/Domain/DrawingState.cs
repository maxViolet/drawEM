namespace DrawEM.App.Domain;

public readonly record struct ScreenPoint(int X, int Y);

public enum DrawingColor
{
    Orange,
}

public sealed record Stroke(
    IReadOnlyList<ScreenPoint> Points,
    DrawingColor Color,
    int Thickness);

public sealed record DrawingState(
    IReadOnlyList<Stroke> CompletedStrokes,
    Stroke? ActiveStroke,
    bool IsDrawModeActive);
