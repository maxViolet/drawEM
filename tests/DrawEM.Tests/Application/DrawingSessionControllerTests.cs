using DrawEM.App.Application;
using DrawEM.App.Domain;

namespace DrawEM.Tests.Application;

public class DrawingSessionControllerTests
{
    [Fact]
    public void CompletedStroke_ContainsStartAndMovedPoints()
    {
        var session = new DrawingSessionController();

        session.Start(new ScreenPoint(100, 200));
        session.Move(new ScreenPoint(120, 215));
        session.End();

        var stroke = Assert.Single(session.CompletedStrokes);
        Assert.Equal(DrawingColor.OrangeRed, stroke.Color);
        Assert.Equal(4, stroke.Thickness);
        Assert.Equal(
            [new ScreenPoint(100, 200), new ScreenPoint(120, 215)],
            stroke.Points);
    }

    [Fact]
    public void TwoStrokes_AreStoredSeparately()
    {
        var session = new DrawingSessionController();

        session.Start(new ScreenPoint(0, 0));
        session.Move(new ScreenPoint(10, 10));
        session.End();

        session.Start(new ScreenPoint(50, 50));
        session.Move(new ScreenPoint(60, 60));
        session.End();

        Assert.Equal(2, session.CompletedStrokes.Count);
        Assert.Equal(
            [new ScreenPoint(0, 0), new ScreenPoint(10, 10)],
            session.CompletedStrokes[0].Points);
        Assert.Equal(
            [new ScreenPoint(50, 50), new ScreenPoint(60, 60)],
            session.CompletedStrokes[1].Points);
    }

    [Fact]
    public void ClearAndExitDrawMode_RemovesCompletedAndActiveStrokes()
    {
        var session = new DrawingSessionController();

        session.EnterDrawMode();
        session.Start(new ScreenPoint(0, 0));
        session.Move(new ScreenPoint(10, 10));
        session.End();

        session.Start(new ScreenPoint(20, 20));
        session.Move(new ScreenPoint(30, 30));

        session.ClearAndExitDrawMode();

        Assert.Empty(session.CompletedStrokes);

        session.ReportPointer(new ScreenPoint(40, 40));
        Assert.Empty(session.CompletedStrokes);

        session.End();
        Assert.Empty(session.CompletedStrokes);
    }

    [Fact]
    public void ReportPointer_WhileDrawModeActive_StartsThenExtendsActiveStroke()
    {
        var session = new DrawingSessionController();
        var states = new List<DrawingState>();
        session.StateChanged += states.Add;

        session.EnterDrawMode();
        session.ReportPointer(new ScreenPoint(1, 1));
        session.ReportPointer(new ScreenPoint(2, 2));

        var lastState = states[^1];
        Assert.True(lastState.IsDrawModeActive);
        Assert.Empty(lastState.CompletedStrokes);
        Assert.Equal(
            [new ScreenPoint(1, 1), new ScreenPoint(2, 2)],
            lastState.ActiveStroke!.Points);
    }

    [Fact]
    public void ReportPointer_WhileDrawModeInactive_IsIgnored()
    {
        var session = new DrawingSessionController();

        session.ReportPointer(new ScreenPoint(1, 1));

        Assert.Empty(session.CompletedStrokes);
    }

    [Fact]
    public void ExitDrawMode_CompletesActiveStrokeAndPublishesInactiveState()
    {
        var session = new DrawingSessionController();
        var states = new List<DrawingState>();
        session.StateChanged += states.Add;

        session.EnterDrawMode();
        session.ReportPointer(new ScreenPoint(1, 1));
        session.ReportPointer(new ScreenPoint(2, 2));
        session.ExitDrawMode();

        var lastState = states[^1];
        Assert.False(lastState.IsDrawModeActive);
        Assert.Null(lastState.ActiveStroke);
        var stroke = Assert.Single(lastState.CompletedStrokes);
        Assert.Equal([new ScreenPoint(1, 1), new ScreenPoint(2, 2)], stroke.Points);
    }
}
