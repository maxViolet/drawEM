using DrawEM.App.Application;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class GlobalShortcutAdapterTests
{
    [Fact]
    public void CtrlAltZ_Pressed_EntersDrawModeOnce_DespiteAutoRepeat()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FailingCursorPositionSource(), action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);

        Assert.Single(states.Where(state => state.IsDrawModeActive));
    }

    [Fact]
    public void CtrlAltZ_Released_ExitsDrawMode()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FailingCursorPositionSource(), action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.ReleaseKey(VirtualKeys.Z);

        Assert.Equal([true, false], states.Select(state => state.IsDrawModeActive));
    }

    [Fact]
    public void CtrlAltX_Pressed_InvokesClear()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));
        controller.End();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FailingCursorPositionSource(), action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.X);

        Assert.Empty(controller.CompletedStrokes);
    }

    [Fact]
    public void CtrlAltZ_Pressed_DefersDrawModeChangeUntilDispatcherRuns()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        var queuedActions = new Queue<Action>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FailingCursorPositionSource(), queuedActions.Enqueue);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Empty(states);
        Assert.Single(queuedActions);

        queuedActions.Dequeue().Invoke();

        Assert.True(Assert.Single(states).IsDrawModeActive);
    }

    [Fact]
    public void DrawMode_SuppressesNonShortcutKeys_ButAllowsDrawingChord()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FailingCursorPositionSource(), action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.False(source.ShouldSuppressKey(VirtualKeys.LeftControl));
        Assert.False(source.ShouldSuppressKey(VirtualKeys.LeftMenu));
        Assert.False(source.ShouldSuppressKey(VirtualKeys.Z));
        Assert.True(source.ShouldSuppressKey(VirtualKeys.A));

        source.ReleaseKey(VirtualKeys.Z);

        Assert.False(source.ShouldSuppressKey(VirtualKeys.A));
    }

    [Fact]
    public void CtrlAltX_SuppressesXAndClearsThenExitsDrawMode()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FailingCursorPositionSource(), action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));

        var xWasSuppressed = source.PressKey(VirtualKeys.X);

        var state = states.Last();
        Assert.True(xWasSuppressed);
        Assert.False(state.IsDrawModeActive);
        Assert.Empty(state.CompletedStrokes);
        Assert.Null(state.ActiveStroke);
    }

    [Fact]
    public void CtrlAltZ_Pressed_StartsActiveStrokeAtCurrentCursorPosition()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        var cursorPosition = new ScreenPoint(120, 240);
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            new DrawingModeInputGate(),
            new FakeCursorPositionSource(cursorPosition),
            action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Equal([cursorPosition], Assert.Single(states).ActiveStroke!.Points);
    }

    [Fact]
    public void CtrlAltZ_Pressed_EntersDrawModeWithoutStartingPoint_WhenCursorPositionUnavailable()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            new DrawingModeInputGate(),
            new FailingCursorPositionSource(),
            action => action());

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        var state = Assert.Single(states);
        Assert.True(state.IsDrawModeActive);
        Assert.Null(state.ActiveStroke);
    }

    private sealed class FakeKeyboardHookSource : IKeyboardHookSource
    {
        public event Action<int>? KeyDown;

        public event Action<int>? KeyUp;

        public event Func<int, bool>? KeySuppressionRequested;

        public bool PressKey(int vkCode)
        {
            KeyDown?.Invoke(vkCode);
            return ShouldSuppressKey(vkCode);
        }

        public void ReleaseKey(int vkCode) => KeyUp?.Invoke(vkCode);

        public bool ShouldSuppressKey(int vkCode) =>
            KeySuppressionRequested?
                .GetInvocationList()
                .Cast<Func<int, bool>>()
                .Any(handler => handler(vkCode))
            ?? false;
    }

    private sealed class FakeCursorPositionSource(ScreenPoint point) : ICursorPositionSource
    {
        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = point;
            return true;
        }
    }

    private sealed class FailingCursorPositionSource : ICursorPositionSource
    {
        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = default;
            return false;
        }
    }
}
