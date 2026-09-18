using System.Windows;
using System.Windows.Media;
using DrawEM.App.Domain;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;

namespace DrawEM.App.Presentation;

public sealed class StrokeRenderElement : FrameworkElement
{
    private readonly DrawingVisual visual = new();

    public StrokeRenderElement()
    {
        AddVisualChild(visual);
    }

    protected override int VisualChildrenCount => 1;

    protected override Visual GetVisualChild(int index) => visual;

    public void UpdateState(DrawingState state, PhysicalToLocalTransform transform)
    {
        using var drawingContext = visual.RenderOpen();

        foreach (var stroke in state.CompletedStrokes)
        {
            DrawStroke(drawingContext, stroke, transform);
        }

        if (state.ActiveStroke is { } active)
        {
            DrawStroke(drawingContext, active, transform);
        }
    }

    private static void DrawStroke(DrawingContext drawingContext, Stroke stroke, PhysicalToLocalTransform transform)
    {
        if (stroke.Points.Count < 2)
        {
            return;
        }

        var pen = new Pen(ToBrush(stroke.Color), stroke.Thickness)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
            LineJoin = PenLineJoin.Round,
        };

        for (var i = 1; i < stroke.Points.Count; i++)
        {
            drawingContext.DrawLine(
                pen,
                transform.ToLocalPoint(stroke.Points[i - 1]),
                transform.ToLocalPoint(stroke.Points[i]));
        }
    }

    private static Brush ToBrush(DrawingColor color) => color switch
    {
        DrawingColor.Orange => Brushes.Orange,
        _ => Brushes.Orange,
    };
}
