namespace DrawEM.App.Domain;

public readonly record struct ScreenPoint(int X, int Y);

public enum DrawingColor
{
    OrangeRed,
}

public sealed record Stroke(
    IReadOnlyList<ScreenPoint> Points,
    DrawingColor Color,
    int Thickness);

// Generation increments on ClearAndExitDrawMode. A renderer that caches per-stroke
// visuals cannot rely on CompletedStrokes.Count alone to detect a reset: a coalesced
// clear followed by a new stroke of the same length leaves the count unchanged.
public sealed record DrawingState(
    IReadOnlyList<Stroke> CompletedStrokes,
    Stroke? ActiveStroke,
    bool IsDrawModeActive,
    int Generation);
