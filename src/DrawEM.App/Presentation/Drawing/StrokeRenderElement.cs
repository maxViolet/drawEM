using System.Windows;
using System.Windows.Media;
using DrawEM.App.Domain.Drawing;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;

namespace DrawEM.App.Presentation.Drawing;

/// <summary>
/// Renders drawing state incrementally: completed strokes and already-drawn active
/// segments are never reopened, so cost per update is proportional to new points only,
/// not to total accumulated points. See docs/v1/steps/F03-render-lag-investigation-plan.md.
/// </summary>
public sealed class StrokeRenderElement : FrameworkElement
{
    /// <summary>
    /// Initial value of renderedGeneration. DrawingState.Generation starts at 0, so the
    /// first UpdateState always sees a new generation and does a full reset.
    /// </summary>
    private const int NoGenerationRendered = -1;

    /// <summary>Divides a diameter to get a radius.</summary>
    private const double DiameterToRadius = 2d;

    private readonly VisualCollection children;
    private readonly List<DrawingVisual> activeSegmentVisuals = [];
    private readonly Dictionary<(DrawingColor Color, int Thickness), Pen> penCache = [];
    private DrawingVisual? activeDotVisual;
    private int completedRendered;
    private int activePointCount;
    private ScreenPoint? activeFirstPoint;
    private int renderedGeneration = NoGenerationRendered;

    public StrokeRenderElement()
    {
        children = new VisualCollection(this);
    }

    protected override int VisualChildrenCount => children.Count;

    protected override Visual GetVisualChild(int index) => children[index];

    public void UpdateState(DrawingState state, PhysicalToLocalTransform transform)
    {
        // Generation is the authoritative reset signal, not CompletedStrokes.Count: a
        // coalesced clear followed by a new stroke of the same length leaves the count
        // unchanged, which would otherwise hide the reset entirely.
        if (state.Generation != renderedGeneration || state.CompletedStrokes.Count < completedRendered)
        {
            ResetAll();
            renderedGeneration = state.Generation;
        }

        var hadNewCompletedStrokes = state.CompletedStrokes.Count > completedRendered;
        var reclassifiedActiveStroke = false;

        for (var i = completedRendered; i < state.CompletedStrokes.Count; i++)
        {
            var stroke = state.CompletedStrokes[i];
            if (i == completedRendered && IsCurrentlyTrackedActiveStroke(stroke))
            {
                // Already drawn incrementally while it was the active stroke; visuals
                // stay as-is and become permanent, only the tracking below retires.
                reclassifiedActiveStroke = true;
                continue;
            }

            DrawFullStroke(stroke, transform);
        }

        completedRendered = state.CompletedStrokes.Count;

        if (hadNewCompletedStrokes)
        {
            // Exactly one active stroke exists at a time, and it must have ended
            // (End/ClearAndExitDrawMode) for CompletedStrokes to grow. Whatever this
            // renderer was tracking as "active" is stale regardless of outcome: if it
            // matched, its visuals are now part of the completed picture (keep them,
            // just retire the tracking); if it did not match (e.g. a move was skipped
            // before the stroke ended), the partial visuals are a stale duplicate of
            // what DrawFullStroke just drew and must be removed, not just forgotten.
            if (reclassifiedActiveStroke)
            {
                activeSegmentVisuals.Clear();
                activeDotVisual = null;
                activePointCount = 0;
                activeFirstPoint = null;
            }
            else
            {
                RemoveActiveVisuals();
            }
        }

        if (state.ActiveStroke is not { Points.Count: > 0 } active)
        {
            return;
        }

        var isContinuation = activePointCount > 0 && activeFirstPoint == active.Points[0];
        if (!isContinuation)
        {
            RemoveActiveVisuals();
        }

        if (activePointCount == 0 && active.Points.Count == 1)
        {
            DrawActiveDot(active, transform);
            activePointCount = 1;
            activeFirstPoint = active.Points[0];
            return;
        }

        if (activePointCount <= 1 && active.Points.Count >= 2 && activeDotVisual is not null)
        {
            children.Remove(activeDotVisual);
            activeDotVisual = null;
        }

        var fromIndex = Math.Max(activePointCount, 1);
        if (active.Points.Count > fromIndex)
        {
            DrawActiveSegments(active, fromIndex, transform);
        }

        activeFirstPoint ??= active.Points[0];
        activePointCount = active.Points.Count;
    }

    private bool IsCurrentlyTrackedActiveStroke(Stroke stroke) =>
        activePointCount > 0 &&
        activePointCount == stroke.Points.Count &&
        activeFirstPoint == stroke.Points[0];

    private void DrawFullStroke(Stroke stroke, PhysicalToLocalTransform transform)
    {
        if (stroke.Points.Count == 0)
        {
            return;
        }

        var visual = new DrawingVisual();
        using (var drawingContext = visual.RenderOpen())
        {
            var clipped = PushMonitorClip(drawingContext, stroke, transform);
            if (stroke.Points.Count == 1)
            {
                drawingContext.DrawEllipse(
                    ToBrush(stroke.Color),
                    null,
                    transform.ToLocalPoint(stroke.Points[0]),
                    DotRadius(stroke.Thickness),
                    DotRadius(stroke.Thickness));
            }
            else
            {
                var pen = GetPen(stroke.Color, stroke.Thickness);
                for (var i = 1; i < stroke.Points.Count; i++)
                {
                    drawingContext.DrawLine(
                        pen,
                        transform.ToLocalPoint(stroke.Points[i - 1]),
                        transform.ToLocalPoint(stroke.Points[i]));
                }
            }
            if (clipped) drawingContext.Pop();
        }

        children.Add(visual);
    }

    private void DrawActiveDot(Stroke active, PhysicalToLocalTransform transform)
    {
        var visual = new DrawingVisual();
        using (var drawingContext = visual.RenderOpen())
        {
            var clipped = PushMonitorClip(drawingContext, active, transform);
            drawingContext.DrawEllipse(
                ToBrush(active.Color),
                null,
                transform.ToLocalPoint(active.Points[0]),
                DotRadius(active.Thickness),
                DotRadius(active.Thickness));
            if (clipped) drawingContext.Pop();
        }

        children.Add(visual);
        activeDotVisual = visual;
    }

    private void DrawActiveSegments(Stroke active, int fromIndex, PhysicalToLocalTransform transform)
    {
        var visual = new DrawingVisual();
        using (var drawingContext = visual.RenderOpen())
        {
            var clipped = PushMonitorClip(drawingContext, active, transform);
            var pen = GetPen(active.Color, active.Thickness);
            for (var i = fromIndex; i < active.Points.Count; i++)
            {
                drawingContext.DrawLine(
                    pen,
                    transform.ToLocalPoint(active.Points[i - 1]),
                    transform.ToLocalPoint(active.Points[i]));
            }
            if (clipped) drawingContext.Pop();
        }

        children.Add(visual);
        activeSegmentVisuals.Add(visual);
    }

    private void RemoveActiveVisuals()
    {
        if (activeDotVisual is not null)
        {
            children.Remove(activeDotVisual);
            activeDotVisual = null;
        }

        foreach (var segmentVisual in activeSegmentVisuals)
        {
            children.Remove(segmentVisual);
        }

        activeSegmentVisuals.Clear();
        activePointCount = 0;
        activeFirstPoint = null;
    }

    private void ResetAll()
    {
        children.Clear();
        activeSegmentVisuals.Clear();
        activeDotVisual = null;
        completedRendered = 0;
        activePointCount = 0;
        activeFirstPoint = null;
    }

    private Pen GetPen(DrawingColor color, int thickness)
    {
        var key = (color, thickness);
        if (penCache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var pen = new Pen(ToBrush(color), thickness)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
            LineJoin = PenLineJoin.Round,
        };
        pen.Freeze();
        penCache[key] = pen;
        return pen;
    }

    /// <summary>A single-point stroke is a dot whose diameter equals the stroke thickness.</summary>
    private static double DotRadius(int thickness) => thickness / DiameterToRadius;

    private static bool PushMonitorClip(
        DrawingContext context, Stroke stroke, PhysicalToLocalTransform transform)
    {
        if (stroke.Bounds is not { } bounds)
        {
            return false;
        }

        var topLeft = transform.ToLocalPoint(new ScreenPoint(bounds.Left, bounds.Top));
        var bottomRight = transform.ToLocalPoint(new ScreenPoint(bounds.Right, bounds.Bottom));
        context.PushClip(new RectangleGeometry(new Rect(topLeft, bottomRight)));
        return true;
    }

    private static Brush ToBrush(DrawingColor color) => color switch
    {
        DrawingColor.OrangeRed => Brushes.OrangeRed,
        _ => Brushes.OrangeRed,
    };
}
