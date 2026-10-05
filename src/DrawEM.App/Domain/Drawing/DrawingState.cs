namespace DrawEM.App.Domain.Drawing;

public readonly record struct ScreenPoint(int X, int Y);

/// <summary>
/// A stroke and the style it started with. The style never changes after creation, so a later
/// settings change does not restyle the stroke. A single-point stroke is a dot whose diameter is the width.
/// </summary>
public sealed record Stroke(
    IReadOnlyList<ScreenPoint> Points,
    DrawingStyle Style,
    MonitorBounds? Bounds = null);

// Generation increments on ClearAndExitDrawMode. A renderer that caches per-stroke
// visuals cannot rely on CompletedStrokes.Count alone to detect a reset: a coalesced
// clear followed by a new stroke of the same length leaves the count unchanged.
public sealed record DrawingState(
    IReadOnlyList<Stroke> CompletedStrokes,
    Stroke? ActiveStroke,
    bool IsDrawModeActive,
    int Generation);
