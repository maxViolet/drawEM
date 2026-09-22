using DrawEM.App.Application;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class GlobalMouseInputAdapterTests
{
    [Fact]
    public void PointerMovement_DuringDrawMode_BecomesCompletedStroke()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        _ = new GlobalMouseInputAdapter(source, controller);

        controller.EnterDrawMode();
        source.Move(new ScreenPoint(10, 20));
        source.Move(new ScreenPoint(15, 25));
        controller.ExitDrawMode();

        var stroke = Assert.Single(controller.CompletedStrokes);
        Assert.Equal([new ScreenPoint(10, 20), new ScreenPoint(15, 25)], stroke.Points);
    }

    [Fact]
    public void PointerButtons_AreSuppressedOnlyDuringDrawMode()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        _ = new GlobalMouseInputAdapter(source, controller);

        Assert.False(source.ShouldSuppressPointerButton());

        controller.EnterDrawMode();

        Assert.True(source.ShouldSuppressPointerButton());

        controller.ExitDrawMode();

        Assert.False(source.ShouldSuppressPointerButton());
    }

    [Fact]
    public void PointerWheel_IsSuppressedOnlyDuringDrawMode()
    {
        var source = new FakeMouseHookSource();
        var controller = new DrawingSessionController();
        _ = new GlobalMouseInputAdapter(source, controller);

        Assert.False(source.ShouldSuppressPointerWheel());

        controller.EnterDrawMode();

        Assert.True(source.ShouldSuppressPointerWheel());

        controller.ExitDrawMode();

        Assert.False(source.ShouldSuppressPointerWheel());
    }

    private sealed class FakeMouseHookSource : IMouseHookSource
    {
        public event Action<ScreenPoint>? PointerMoved;

        public event Func<bool>? PointerButtonActivity;

        public event Func<bool>? PointerWheelActivity;

        public void Move(ScreenPoint point) => PointerMoved?.Invoke(point);

        public bool ShouldSuppressPointerButton() =>
            PointerButtonActivity?
                .GetInvocationList()
                .Cast<Func<bool>>()
                .Any(handler => handler())
            ?? false;

        public bool ShouldSuppressPointerWheel() =>
            PointerWheelActivity?
                .GetInvocationList()
                .Cast<Func<bool>>()
                .Any(handler => handler())
            ?? false;
    }
}
