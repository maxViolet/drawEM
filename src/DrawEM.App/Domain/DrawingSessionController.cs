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

public sealed class DrawingSessionController
{
    private const int StrokeThickness = 4;
    private readonly List<Stroke> completedStrokes = [];
    private List<ScreenPoint>? activePoints;
    private bool drawModeActive;

    public IReadOnlyList<Stroke> CompletedStrokes => completedStrokes;

    public event Action<DrawingState>? StateChanged;
    public event Action<bool>? InputCaptureRequested;

    public void EnterDrawMode()
    {
        drawModeActive = true;
        InputCaptureRequested?.Invoke(true);
        PublishState();
    }

    public void ExitDrawMode()
    {
        drawModeActive = false;
        InputCaptureRequested?.Invoke(false);
        End();
    }

    public void ReportPointer(ScreenPoint point)
    {
        if (!drawModeActive)
        {
            return;
        }

        if (activePoints is null)
        {
            Start(point);
        }
        else
        {
            Move(point);
        }
    }

    public void Start(ScreenPoint point)
    {
        activePoints = [point];
        PublishState();
    }

    public void Move(ScreenPoint point)
    {
        activePoints?.Add(point);
        PublishState();
    }

    public void End()
    {
        if (activePoints is not null)
        {
            completedStrokes.Add(new Stroke(
                activePoints.ToArray(),
                DrawingColor.Orange,
                StrokeThickness));
            activePoints = null;
        }

        PublishState();
    }

    public void Clear()
    {
        completedStrokes.Clear();
        activePoints = null;
        PublishState();
    }

    private void PublishState()
    {
        var active = activePoints is null
            ? null
            : new Stroke(activePoints.ToArray(), DrawingColor.Orange, StrokeThickness);

        StateChanged?.Invoke(new DrawingState(completedStrokes.ToArray(), active, drawModeActive));
    }
}
