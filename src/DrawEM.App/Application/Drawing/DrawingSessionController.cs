using System.Collections.ObjectModel;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Drawing;

public sealed class DrawingSessionController
{
    private readonly Func<DrawingStyle> currentStyle;
    private readonly List<Stroke> completedStrokes = [];
    private List<ScreenPoint>? activePoints;
    private DrawingStyle? activeStyle;
    private MonitorBounds? activeBounds;
    private bool drawModeActive;
    private int generation;

    /// <summary>Draws every stroke with the style of <see cref="SettingsSnapshot.Default"/>.</summary>
    public DrawingSessionController()
        : this(() => SettingsSnapshot.Default.Style)
    {
    }

    /// <param name="currentStyle">
    /// The active settings' style. It is read once when a stroke starts, and the stroke keeps that style.
    /// </param>
    public DrawingSessionController(Func<DrawingStyle> currentStyle)
    {
        ArgumentNullException.ThrowIfNull(currentStyle);
        this.currentStyle = currentStyle;
    }

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
        BeginStroke(startingPoint);
        PublishState();
    }

    public void EnterDrawMode(ScreenPoint startingPoint, MonitorBounds bounds)
    {
        if (!bounds.Contains(startingPoint))
        {
            return;
        }

        activeBounds = bounds;
        EnterDrawMode(startingPoint);
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

        if (activeBounds is { } bounds && !bounds.Contains(point))
        {
            ExitDrawMode();
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
        BeginStroke(point);
        PublishState();
    }

    public void Move(ScreenPoint point)
    {
        activePoints?.Add(point);
        PublishState();
    }

    public void End()
    {
        CompleteActiveStroke();
        PublishState();
    }

    private void BeginStroke(ScreenPoint point)
    {
        activePoints = [point];
        activeStyle = currentStyle();
    }

    private void CompleteActiveStroke()
    {
        if (activePoints is not null)
        {
            completedStrokes.Add(new Stroke(Snapshot(activePoints), activeStyle!, activeBounds));
            activePoints = null;
            activeStyle = null;
        }
        activeBounds = null;
    }

    public void ClearAndExitDrawMode()
    {
        drawModeActive = false;
        completedStrokes.Clear();
        activePoints = null;
        activeStyle = null;
        activeBounds = null;
        generation++;
        PublishState();
    }

    public void ClearMonitorAndExitDrawMode(MonitorBounds bounds)
    {
        drawModeActive = false;
        CompleteActiveStroke();
        completedStrokes.RemoveAll(stroke => stroke.Bounds == bounds);
        generation++;
        PublishState();
    }

    private void PublishState()
    {
        var active = activePoints is null
            ? null
            : new Stroke(Snapshot(activePoints), activeStyle!, activeBounds);

        StateChanged?.Invoke(new DrawingState(Snapshot(completedStrokes), active, drawModeActive, generation));
    }

    private static ReadOnlyCollection<T> Snapshot<T>(List<T> source) => new(source.ToArray());
}
