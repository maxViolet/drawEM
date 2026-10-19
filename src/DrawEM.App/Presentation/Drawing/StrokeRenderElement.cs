using System.Windows;
using System.Windows.Media;
using DrawEM.App.Domain.Drawing;
using Brush = System.Windows.Media.Brush;
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
    private readonly Dictionary<(HexColor Color, double Thickness), Pen> penCache = [];
    private readonly Dictionary<HexColor, Brush> brushCache = [];
    private DrawingVisual? activeDotVisual;
    private DrawingVisual? activeTailVisual;
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
                // The active figures already contain every canonical path piece. Keep
                // them in place: the temporary tail is the completed stroke's final
                // piece, so reclassification only drops tracking references and never
                // redraws points accumulated during the stroke.
                activeSegmentVisuals.Clear();
                activeDotVisual = null;
                activeTailVisual = null;
                activePointCount = 0;
                activeFirstPoint = null;
                reclassifiedActiveStroke = true;
                continue;
            }

            DrawFullStroke(stroke, transform);
        }

        completedRendered = state.CompletedStrokes.Count;

        if (hadNewCompletedStrokes)
        {
            // A tracked stroke's pieces have just become permanent. Otherwise any
            // partial visuals are stale duplicates of the full stroke drawn above.
            if (!reclassifiedActiveStroke)
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

        if (active.Style.Smoothing == StrokeSmoothing.Bezier)
        {
            DrawActiveBezier(active, transform);
        }
        else
        {
            var fromIndex = Math.Max(activePointCount, 1);
            if (active.Points.Count > fromIndex)
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
                DrawDot(drawingContext, stroke, transform);
            }
            else if (stroke.Style.Smoothing == StrokeSmoothing.None)
            {
                var pen = GetPen(stroke, transform);
                for (var i = 1; i < stroke.Points.Count; i++)
                    drawingContext.DrawLine(pen, transform.ToLocalPoint(stroke.Points[i - 1]), transform.ToLocalPoint(stroke.Points[i]));
            }
            else
            {
                drawingContext.DrawGeometry(null, GetPen(stroke, transform), BuildBezierGeometry(stroke.Points, transform));
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
            DrawDot(drawingContext, active, transform);
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
            var pen = GetPen(active, transform);
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

    private void DrawActiveBezier(Stroke active, PhysicalToLocalTransform transform)
    {
        if (activeTailVisual is not null)
        {
            children.Remove(activeTailVisual);
            activeTailVisual = null;
        }

        var count = active.Points.Count;
        var firstNewPiece = activePointCount == 0 ? 0 : activePointCount - 1;
        var lastNewPiece = count - 2;
        if (firstNewPiece <= lastNewPiece)
        {
            AddActivePieces(active, transform, firstNewPiece, lastNewPiece);
        }

        var tail = new DrawingVisual();
        using (var context = tail.RenderOpen())
        {
            var clipped = PushMonitorClip(context, active, transform);
            context.DrawLine(
                CreateBezierTailPen(active, transform),
                transform.ToLocalPoint(Midpoint(active.Points[count - 2], active.Points[count - 1])),
                transform.ToLocalPoint(active.Points[count - 1]));
            if (clipped) context.Pop();
        }
        children.Add(tail);
        activeTailVisual = tail;
    }

    private void AddActivePieces(
        Stroke active, PhysicalToLocalTransform transform, int firstPiece, int lastPiece)
    {
        var visual = new DrawingVisual();
        using (var context = visual.RenderOpen())
        {
            var clipped = PushMonitorClip(context, active, transform);
            var brush = GetBrush(active.Style.Color);
            var width = LocalWidth(active, transform);
            // Each piece gets its round start cap at its join and a flat end cap;
            // the following piece supplies the join cap without double-compositing.
            var startPen = new Pen(brush, width)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Flat,
                LineJoin = PenLineJoin.Round,
            };
            startPen.Freeze();
            for (var piece = firstPiece; piece <= lastPiece; piece++)
            {
                var geometry = new StreamGeometry();
                using (var path = geometry.Open())
                {
                    if (piece == 0)
                    {
                        path.BeginFigure(transform.ToLocalPoint(active.Points[0]), false, false);
                        path.LineTo(transform.ToLocalPoint(Midpoint(active.Points[0], active.Points[1])), true, false);
                    }
                    else
                    {
                        var start = Midpoint(active.Points[piece - 1], active.Points[piece]);
                        path.BeginFigure(transform.ToLocalPoint(start), false, false);
                        path.QuadraticBezierTo(
                            transform.ToLocalPoint(active.Points[piece]),
                            transform.ToLocalPoint(Midpoint(active.Points[piece], active.Points[piece + 1])),
                            true,
                            false);
                    }
                }
                context.DrawGeometry(null, startPen, geometry);
            }
            if (clipped) context.Pop();
        }
        children.Add(visual);
        activeSegmentVisuals.Add(visual);
    }

    private static StreamGeometry BuildBezierGeometry(
        IReadOnlyList<ScreenPoint> points, PhysicalToLocalTransform transform)
    {
        var geometry = new StreamGeometry();
        using (var context = geometry.Open())
        {
            var pieces = StrokeSmoothingGeometry.CreatePieces(points);
            context.BeginFigure(transform.ToLocalPoint(pieces[0] switch
            {
                StrokePathPiece.Line line => line.Start,
                StrokePathPiece.Quadratic quadratic => quadratic.Start,
                _ => throw new InvalidOperationException("Unknown stroke path piece."),
            }), false, false);
            foreach (var piece in pieces)
            {
                switch (piece)
                {
                    case StrokePathPiece.Line line:
                        context.LineTo(transform.ToLocalPoint(line.End), true, false);
                        break;
                    case StrokePathPiece.Quadratic quadratic:
                        context.QuadraticBezierTo(
                            transform.ToLocalPoint(quadratic.Control),
                            transform.ToLocalPoint(quadratic.End),
                            true,
                            false);
                        break;
                }
            }
        }
        return geometry;
    }

    private static ScreenPoint Midpoint(ScreenPoint first, ScreenPoint second) =>
        StrokeSmoothingGeometry.Midpoint(first, second);

    private Pen CreateBezierTailPen(Stroke stroke, PhysicalToLocalTransform transform)
    {
        var pen = new Pen(GetBrush(stroke.Style.Color), LocalWidth(stroke, transform))
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
            LineJoin = PenLineJoin.Round,
        };
        pen.Freeze();
        return pen;
    }

    private void RemoveActiveVisuals()
    {
        if (activeDotVisual is not null)
        {
            children.Remove(activeDotVisual);
            activeDotVisual = null;
        }

        if (activeTailVisual is not null)
        {
            children.Remove(activeTailVisual);
            activeTailVisual = null;
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
        activeTailVisual = null;
        activeSegmentVisuals.Clear();
        activeDotVisual = null;
        completedRendered = 0;
        activePointCount = 0;
        activeFirstPoint = null;

        // Drop pens and brushes of cleared styles, so changing colors over time does not grow the caches.
        penCache.Clear();
        brushCache.Clear();
    }

    /// <summary>A pen whose width is the stroke's physical width converted to local units.</summary>
    private Pen GetPen(Stroke stroke, PhysicalToLocalTransform transform)
    {
        var key = (Color: stroke.Style.Color, Thickness: LocalWidth(stroke, transform));
        if (penCache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var pen = new Pen(GetBrush(key.Color), key.Thickness)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
            LineJoin = PenLineJoin.Round,
        };
        pen.Freeze();
        penCache[key] = pen;
        return pen;
    }

    private Brush GetBrush(HexColor color)
    {
        if (brushCache.TryGetValue(color, out var cached))
        {
            return cached;
        }

        var brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.Red, color.Green, color.Blue));
        brush.Freeze();
        brushCache[color] = brush;
        return brush;
    }

    /// <summary>A single-point stroke is a dot whose diameter equals the stroke's physical width.</summary>
    private void DrawDot(DrawingContext context, Stroke stroke, PhysicalToLocalTransform transform)
    {
        var radius = LocalWidth(stroke, transform) / DiameterToRadius;
        context.DrawEllipse(
            GetBrush(stroke.Style.Color), null, transform.ToLocalPoint(stroke.Points[0]), radius, radius);
    }

    /// <summary>The stroke's physical width in local units: a pen width or a dot diameter.</summary>
    private static double LocalWidth(Stroke stroke, PhysicalToLocalTransform transform) =>
        transform.ToLocalLength(stroke.Style.Width.Pixels);

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
}
