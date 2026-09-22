using System.Collections.ObjectModel;
using DrawEM.App.Domain;

namespace DrawEM.App.Application;

public sealed class DrawingSessionController
{
    private const int StrokeThickness = 4;
    private readonly List<Stroke> completedStrokes = [];
    private List<ScreenPoint>? activePoints;
    private bool drawModeActive;
    private int generation;

    public IReadOnlyList<Stroke> CompletedStrokes => completedStrokes.AsReadOnly();

    public event Action<DrawingState>? StateChanged;
    public void EnterDrawMode()
    {
        drawModeActive = true;
        PublishState();
    }

    public void EnterDrawMode(ScreenPoint startingPoint)
    {
        drawModeActive = true;
        activePoints = [startingPoint];
        PublishState();
    }

    public void ExitDrawMode()
    {
        drawModeActive = false;
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
                Snapshot(activePoints),
                DrawingColor.Orange,
                StrokeThickness));
            activePoints = null;
        }

        PublishState();
    }

    public void ClearAndExitDrawMode()
    {
        drawModeActive = false;
        completedStrokes.Clear();
        activePoints = null;
        generation++;
        PublishState();
    }

    private void PublishState()
    {
        var active = activePoints is null
            ? null
            : new Stroke(Snapshot(activePoints), DrawingColor.Orange, StrokeThickness);

        StateChanged?.Invoke(new DrawingState(Snapshot(completedStrokes), active, drawModeActive, generation));
    }

    private static ReadOnlyCollection<T> Snapshot<T>(List<T> source) => new(source.ToArray());
}
