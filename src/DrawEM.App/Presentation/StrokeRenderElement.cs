using System.Windows;
using System.Windows.Media;
using DrawEM.App.Domain;

namespace DrawEM.App.Presentation;

public sealed class StrokeRenderElement : FrameworkElement
{
    private DrawingState state = new(Array.Empty<Stroke>(), null, false);
    private Point origin;

    public void UpdateState(DrawingState newState, Point windowOrigin)
    {
        state = newState;
        origin = windowOrigin;
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        foreach (var stroke in state.CompletedStrokes)
        {
            DrawStroke(drawingContext, stroke);
        }

        if (state.ActiveStroke is { } active)
        {
            DrawStroke(drawingContext, active);
        }
    }

    private void DrawStroke(DrawingContext drawingContext, Stroke stroke)
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
            drawingContext.DrawLine(pen, ToWpfPoint(stroke.Points[i - 1]), ToWpfPoint(stroke.Points[i]));
        }
    }

    private Point ToWpfPoint(ScreenPoint point) => new(point.X - origin.X, point.Y - origin.Y);

    private static Brush ToBrush(DrawingColor color) => color switch
    {
        DrawingColor.Orange => Brushes.Orange,
        _ => Brushes.Orange,
    };
}
