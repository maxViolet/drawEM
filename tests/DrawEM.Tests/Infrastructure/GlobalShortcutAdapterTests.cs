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
        var enterCount = 0;
        controller.InputCaptureRequested += captured =>
        {
            if (captured)
            {
                enterCount++;
            }
        };
        _ = new GlobalShortcutAdapter(source, controller);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);

        Assert.Equal(1, enterCount);
    }

    [Fact]
    public void CtrlAltZ_Released_ExitsDrawMode()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var captureStates = new List<bool>();
        controller.InputCaptureRequested += captureStates.Add;
        _ = new GlobalShortcutAdapter(source, controller);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.ReleaseKey(VirtualKeys.Z);

        Assert.Equal([true, false], captureStates);
    }

    [Fact]
    public void CtrlAltX_Pressed_InvokesClear()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        controller.Start(new ScreenPoint(1, 1));
        controller.Move(new ScreenPoint(2, 2));
        controller.End();
        _ = new GlobalShortcutAdapter(source, controller);

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
        _ = new GlobalShortcutAdapter(source, controller, queuedActions.Enqueue);

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Empty(states);
        Assert.Single(queuedActions);

        queuedActions.Dequeue().Invoke();

        Assert.True(Assert.Single(states).IsDrawModeActive);
    }

    private sealed class FakeKeyboardHookSource : IKeyboardHookSource
    {
        public event Action<int>? KeyDown;

        public event Action<int>? KeyUp;

        public void PressKey(int vkCode) => KeyDown?.Invoke(vkCode);

        public void ReleaseKey(int vkCode) => KeyUp?.Invoke(vkCode);
    }
}
