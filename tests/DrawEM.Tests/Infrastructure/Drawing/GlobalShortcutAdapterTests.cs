using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Drawing;

public class GlobalShortcutAdapterTests
{
    private static readonly MonitorBounds DefaultMonitor = new(-1000, -1000, 1000, 1000);
    private static readonly IMonitorBoundsSource DefaultMonitorSource = new FakeMonitorBoundsSource(DefaultMonitor);
    private static readonly ShortcutBindings DefaultBindings = new(SettingsSnapshot.Default.DrawShortcut, SettingsSnapshot.Default.ClearShortcut, []);

    [Fact]
    public void CtrlAltX_ClearsOnlyMonitorUnderCursor()
    {
        var left = new MonitorBounds(0, 0, 100, 100);
        var right = new MonitorBounds(100, 0, 200, 100);
        var controller = new DrawingSessionController();
        controller.EnterDrawMode(new ScreenPoint(10, 10), left);
        controller.ExitDrawMode();
        controller.EnterDrawMode(new ScreenPoint(110, 10), right);
        controller.ExitDrawMode();
        var source = new FakeKeyboardHookSource();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(),
            new FakeCursorPositionSource(new ScreenPoint(110, 10)), action => action(),
            new FakeMonitorBoundsSource(left, right), DefaultBindings, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.X);

        Assert.Equal(left, Assert.Single(controller.CompletedStrokes).Bounds);
    }

    [Fact]
    public void BoundaryCrossing_RequiresShortcutReleaseBeforeDrawingAgain()
    {
        var left = new MonitorBounds(0, 0, 100, 100);
        var right = new MonitorBounds(100, 0, 200, 100);
        var source = new FakeKeyboardHookSource();
        var cursor = new MutableCursorPositionSource(new ScreenPoint(90, 50));
        var gate = new DrawingModeInputGate();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, gate, cursor, action => action(),
            new FakeMonitorBoundsSource(left, right), DefaultBindings, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        Assert.True(gate.StopAtBoundary(new ScreenPoint(101, 50)));
        controller.ExitDrawMode();
        cursor.Position = new ScreenPoint(110, 50);
        var countAfterCrossing = states.Count;

        source.PressKey(VirtualKeys.Z);
        Assert.Equal(countAfterCrossing, states.Count);
        source.ReleaseKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);
        Assert.Equal(right, states[^1].ActiveStroke!.Bounds);
    }

    [Fact]
    public void CtrlAltZ_Pressed_EntersDrawModeOnce_DespiteAutoRepeat()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource, DefaultBindings, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);
        source.PressKey(VirtualKeys.Z);

        Assert.Single(states, state => state.IsDrawModeActive);
    }

    [Fact]
    public void CtrlAltZ_Released_ExitsDrawMode()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource, DefaultBindings, _ => { });

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
        controller.EnterDrawMode(new ScreenPoint(1, 1), DefaultMonitor);
        controller.ReportPointer(new ScreenPoint(2, 2));
        controller.ExitDrawMode();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource, DefaultBindings, _ => { });

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
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), queuedActions.Enqueue, DefaultMonitorSource, DefaultBindings, _ => { });

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
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource, DefaultBindings, _ => { });

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
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(), new FakeCursorPositionSource(new ScreenPoint(0, 0)), action => action(), DefaultMonitorSource, DefaultBindings, _ => { });

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
            action => action(),
            DefaultMonitorSource, DefaultBindings, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Equal([cursorPosition], Assert.Single(states).ActiveStroke!.Points);
    }

    [Fact]
    public void CtrlAltZ_Pressed_DoesNotEnterDrawMode_WhenCursorPositionUnavailable()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var inputGate = new DrawingModeInputGate();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            inputGate,
            new FailingCursorPositionSource(),
            action => action(),
            DefaultMonitorSource, DefaultBindings, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);

        Assert.Empty(states);
        Assert.False(inputGate.IsActive);
    }

    [Fact]
    public void CtrlAltZ_Pressed_EntersDrawMode_OnceCursorPositionBecomesAvailable()
    {
        var source = new FakeKeyboardHookSource();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        var cursorSource = new RecoveringCursorPositionSource(failuresBeforeSuccess: 2, new ScreenPoint(5, 9));
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(
            source,
            controller,
            new DrawingModeInputGate(),
            cursorSource,
            action => action(),
            DefaultMonitorSource, DefaultBindings, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.Z);
        Assert.Empty(states);

        source.PressKey(VirtualKeys.Z);
        Assert.Empty(states);

        source.PressKey(VirtualKeys.Z);
        var state = Assert.Single(states);
        Assert.True(state.IsDrawModeActive);
        Assert.Equal([new ScreenPoint(5, 9)], state.ActiveStroke!.Points);
    }

    [Theory]
    [InlineData(VirtualKeys.D1, SoundSlot.Slot1)]
    [InlineData(VirtualKeys.D2, SoundSlot.Slot2)]
    [InlineData(VirtualKeys.D3, SoundSlot.Slot3)]
    [InlineData(VirtualKeys.D4, SoundSlot.Slot4)]
    [InlineData(VirtualKeys.D5, SoundSlot.Slot5)]
    [InlineData(VirtualKeys.D6, SoundSlot.Slot6)]
    [InlineData(VirtualKeys.D7, SoundSlot.Slot7)]
    [InlineData(VirtualKeys.D8, SoundSlot.Slot8)]
    public void CtrlAltDigit_FirstKeyDown_QueuesOneCommandForItsSlot(int vkCode, SoundSlot slot)
    {
        var sound = new SoundTestHarness((slot, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(vkCode);
        sound.RunQueuedActions();

        Assert.Equal([new PlaySoundCommand(new SoundId("applause"))], sound.Played);
    }

    [Fact]
    public void CtrlAltDigit_AutoRepeat_QueuesNoMoreCommands_UntilReleasedAndPressedAgain()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.D1);
        sound.Source.PressKey(VirtualKeys.D1);
        sound.Source.PressKey(VirtualKeys.D1);
        sound.RunQueuedActions();
        Assert.Single(sound.Played);

        sound.Source.ReleaseKey(VirtualKeys.D1);
        sound.Source.PressKey(VirtualKeys.D1);
        sound.RunQueuedActions();

        Assert.Equal(2, sound.Played.Count);
    }

    [Fact]
    public void CtrlAltDigit_ChordCompletedWhileDigitHeld_QueuesOneCommand()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot2, "drumroll"));

        sound.Source.PressKey(VirtualKeys.RightControl);
        sound.Source.PressKey(VirtualKeys.D2);
        sound.RunQueuedActions();
        Assert.Empty(sound.Played);

        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.D2);
        sound.RunQueuedActions();

        Assert.Equal([new PlaySoundCommand(new SoundId("drumroll"))], sound.Played);
    }

    [Fact]
    public void CtrlAltDigit_UnassignedSlot_QueuesNothing()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.D3);

        Assert.Empty(sound.Queued);
        Assert.Empty(sound.SoundQueued);
        Assert.Empty(sound.Played);
    }

    [Fact]
    public void CtrlAltDigit_AssignedSlot_SuppressesDigitUntilReleased()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);

        Assert.True(sound.Source.PressKey(VirtualKeys.D1));
        Assert.True(sound.Source.PressKey(VirtualKeys.D1));
        Assert.True(sound.Source.ReleaseKey(VirtualKeys.D1));
        Assert.False(sound.Source.ShouldSuppressKey(VirtualKeys.D1));
    }

    [Fact]
    public void CtrlAltDigit_ChordCompletedWhileDigitHeld_SuppressesRepeats_ButPassesKeyUp()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        Assert.False(sound.Source.PressKey(VirtualKeys.D1));
        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.RunQueuedActions();
        Assert.Single(sound.Played);

        Assert.True(sound.Source.PressKey(VirtualKeys.D1));
        Assert.False(sound.Source.ReleaseKey(VirtualKeys.D1));
        Assert.Single(sound.Played);
    }

    [Fact]
    public void CtrlAltDigit_UnassignedSlotOutsideDrawMode_PassesThrough()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);

        Assert.False(sound.Source.PressKey(VirtualKeys.D3));
        Assert.False(sound.Source.ReleaseKey(VirtualKeys.D3));
    }

    [Fact]
    public void Digit_WithoutChord_PassesThroughAndPlaysNothing()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);

        Assert.False(sound.Source.PressKey(VirtualKeys.D1));
        Assert.False(sound.Source.ReleaseKey(VirtualKeys.D1));
        Assert.Empty(sound.Queued);
        Assert.Empty(sound.SoundQueued);
    }

    [Fact]
    public void CtrlAltDigit_Pressed_DefersPlaybackUntilSoundDispatcherRuns()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.D1);

        Assert.Empty(sound.Played);
        Assert.Empty(sound.Queued);
        Assert.Single(sound.SoundQueued);

        sound.RunSoundActions();

        Assert.Single(sound.Played);
    }

    [Fact]
    public void CtrlAltDigit_PlaysSoundWithoutWaitingForQueuedDrawingCommand()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));
        var states = new List<DrawingState>();
        sound.Controller.StateChanged += states.Add;

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.Z);
        Assert.True(sound.Gate.IsActive);
        Assert.Empty(states);

        Assert.True(sound.Source.PressKey(VirtualKeys.D1));
        sound.Source.PressKey(VirtualKeys.D1);
        sound.RunSoundActions();

        Assert.Equal([new PlaySoundCommand(new SoundId("applause"))], sound.Played);
        Assert.Single(sound.Queued);
        Assert.Empty(states);
        Assert.True(sound.Gate.IsActive);
        Assert.True(sound.Source.ShouldSuppressKey(VirtualKeys.A));

        sound.RunQueuedActions();

        Assert.True(Assert.Single(states).IsDrawModeActive);
        Assert.Single(sound.Played);
    }

    [Fact]
    public void CtrlAltDigit_DoesNotQueryMonitorBounds()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.D1);
        sound.RunQueuedActions();

        Assert.Single(sound.Played);
        Assert.Equal(0, sound.Monitors.Queries);
    }

    [Fact]
    public void CtrlAltDigit_DuringActiveStroke_PlaysSound_AndKeepsStrokeAndSuppression()
    {
        var sound = new SoundTestHarness((SoundSlot.Slot1, "applause"));
        var states = new List<DrawingState>();
        sound.Controller.StateChanged += states.Add;

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.Z);
        sound.RunQueuedActions();
        sound.Controller.ReportPointer(new ScreenPoint(3, 4));
        var statesBeforeSound = states.Count;

        var digitWasSuppressed = sound.Source.PressKey(VirtualKeys.D1);
        sound.RunQueuedActions();

        Assert.Single(sound.Played);
        Assert.Equal(statesBeforeSound, states.Count);
        var state = states[^1];
        Assert.True(state.IsDrawModeActive);
        Assert.Equal([new ScreenPoint(0, 0), new ScreenPoint(3, 4)], state.ActiveStroke!.Points);
        Assert.True(sound.Gate.IsActive);
        Assert.True(digitWasSuppressed);
        Assert.True(sound.Source.ShouldSuppressKey(VirtualKeys.A));
        Assert.False(sound.Source.ShouldSuppressKey(VirtualKeys.Z));

        sound.Source.ReleaseKey(VirtualKeys.D1);
        sound.RunQueuedActions();

        Assert.True(sound.Gate.IsActive);
        Assert.Equal(statesBeforeSound, states.Count);
    }

    [Fact]
    public void CtrlAltX_AltReleasedBeforeX_StillSuppressesXUntilReleased()
    {
        var sound = new SoundTestHarness();

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        Assert.True(sound.Source.PressKey(VirtualKeys.X));
        sound.Source.ReleaseKey(VirtualKeys.LeftMenu);

        Assert.True(sound.Source.PressKey(VirtualKeys.X));
        Assert.True(sound.Source.ReleaseKey(VirtualKeys.X));
        Assert.Single(sound.Queued);
    }

    [Fact]
    public void CtrlAltX_ChordCompletedWhileXHeld_ClearsOnce_AndPassesXKeyUp()
    {
        var sound = new SoundTestHarness();

        Assert.False(sound.Source.PressKey(VirtualKeys.X));
        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        Assert.Single(sound.Queued);

        Assert.True(sound.Source.PressKey(VirtualKeys.X));
        Assert.False(sound.Source.ReleaseKey(VirtualKeys.X));
        Assert.Single(sound.Queued);
    }

    // Remapped bindings: draw Ctrl+Shift+D, clear Alt+Shift+F5, sound Ctrl+Shift+F1.
    private static readonly ShortcutBindings Remapped = new(
        Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Shift, ShortcutKey.Letter('D')),
        Shortcut.Create(ShortcutModifiers.Alt | ShortcutModifiers.Shift, ShortcutKey.Function(5)),
        [(Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Shift, ShortcutKey.Function(1)),
            new PlaySoundCommand(new SoundId("applause")))]);

    private const int D = 0x44;
    private const int F5 = VirtualKeys.F1 + 4;

    [Fact]
    public void RemappedDraw_HoldsUntilKeyRelease_AndIgnoresAutoRepeat()
    {
        var sound = new SoundTestHarness(Remapped);
        var states = new List<DrawingState>();
        sound.Controller.StateChanged += states.Add;

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftShift);
        Assert.False(sound.Source.PressKey(D));
        sound.Source.PressKey(D);
        sound.Source.PressKey(D);
        sound.RunQueuedActions();
        Assert.True(sound.Gate.IsActive);

        sound.Source.ReleaseKey(D);
        sound.RunQueuedActions();

        Assert.Equal([true, false], states.Select(state => state.IsDrawModeActive));
    }

    [Fact]
    public void RemappedDraw_SuppressesOtherKeys_ButPassesItsOwnKeyAndModifiers()
    {
        var sound = new SoundTestHarness(Remapped);

        sound.Source.PressKey(VirtualKeys.RightControl);
        sound.Source.PressKey(VirtualKeys.RightShift);
        sound.Source.PressKey(D);

        Assert.False(sound.Source.ShouldSuppressKey(D));
        Assert.False(sound.Source.ShouldSuppressKey(VirtualKeys.RightControl));
        Assert.False(sound.Source.ShouldSuppressKey(VirtualKeys.RightShift));
        Assert.True(sound.Source.ShouldSuppressKey(VirtualKeys.Z));
        Assert.True(sound.Source.ShouldSuppressKey(VirtualKeys.A));
    }

    [Fact]
    public void RemappedDraw_StaysActiveWhenAnotherModifierJoins()
    {
        var sound = new SoundTestHarness(Remapped);

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftShift);
        sound.Source.PressKey(D);
        sound.Source.PressKey(VirtualKeys.LeftMenu);

        Assert.True(sound.Gate.IsActive);
    }

    [Fact]
    public void RemappedDraw_ExtraModifier_DoesNotStart()
    {
        var sound = new SoundTestHarness(Remapped);

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);
        sound.Source.PressKey(VirtualKeys.LeftShift);

        Assert.False(sound.Source.PressKey(D));
        Assert.False(sound.Gate.IsActive);
        Assert.Empty(sound.Queued);
    }

    [Fact]
    public void Remapped_OldDefaultShortcuts_DoNothing()
    {
        var sound = new SoundTestHarness(Remapped);

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftMenu);

        Assert.False(sound.Source.PressKey(VirtualKeys.Z));
        Assert.False(sound.Source.PressKey(VirtualKeys.X));
        Assert.False(sound.Source.PressKey(VirtualKeys.D1));
        sound.RunQueuedActions();
        Assert.False(sound.Gate.IsActive);
        Assert.Empty(sound.Queued);
        Assert.Empty(sound.Played);
    }

    [Fact]
    public void RemappedClear_ClearsOnlyMonitorUnderCursor_AndSuppressesItsKeyThroughRelease()
    {
        var left = new MonitorBounds(0, 0, 100, 100);
        var right = new MonitorBounds(100, 0, 200, 100);
        var controller = new DrawingSessionController();
        controller.EnterDrawMode(new ScreenPoint(10, 10), left);
        controller.ExitDrawMode();
        controller.EnterDrawMode(new ScreenPoint(110, 10), right);
        controller.ExitDrawMode();
        var source = new FakeKeyboardHookSource();
        _ = new GlobalShortcutAdapter(source, controller, new DrawingModeInputGate(),
            new FakeCursorPositionSource(new ScreenPoint(110, 10)), action => action(),
            new FakeMonitorBoundsSource(left, right), Remapped, _ => { });

        source.PressKey(VirtualKeys.LeftMenu);
        source.PressKey(VirtualKeys.LeftShift);

        Assert.True(source.PressKey(F5));
        Assert.Equal(left, Assert.Single(controller.CompletedStrokes).Bounds);
        Assert.True(source.PressKey(F5));
        Assert.True(source.ReleaseKey(F5));
        Assert.Single(controller.CompletedStrokes);
    }

    [Fact]
    public void RemappedSound_FiresOncePerPress_AndSuppressesItsKeyThroughRelease()
    {
        var sound = new SoundTestHarness(Remapped);

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftShift);
        Assert.True(sound.Source.PressKey(VirtualKeys.F1));
        Assert.True(sound.Source.PressKey(VirtualKeys.F1));
        Assert.True(sound.Source.ReleaseKey(VirtualKeys.F1));
        sound.Source.PressKey(VirtualKeys.F1);
        sound.RunQueuedActions();

        Assert.Equal(2, sound.Played.Count);
        Assert.All(sound.Played, command => Assert.Equal(new SoundId("applause"), command.Sound));
    }

    [Fact]
    public void RemappedSound_DuringRemappedDraw_PlaysAndKeepsDrawing()
    {
        var sound = new SoundTestHarness(Remapped);
        var states = new List<DrawingState>();
        sound.Controller.StateChanged += states.Add;

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.LeftShift);
        sound.Source.PressKey(D);
        sound.RunQueuedActions();
        var statesBeforeSound = states.Count;

        Assert.True(sound.Source.PressKey(VirtualKeys.F1));
        sound.RunQueuedActions();

        Assert.Single(sound.Played);
        Assert.True(sound.Gate.IsActive);
        Assert.Equal(statesBeforeSound, states.Count);
        Assert.True(states[^1].IsDrawModeActive);
    }

    [Fact]
    public void RemappedDraw_BoundaryCrossing_RequiresKeyReleaseBeforeDrawingOnNextMonitor()
    {
        var left = new MonitorBounds(0, 0, 100, 100);
        var right = new MonitorBounds(100, 0, 200, 100);
        var source = new FakeKeyboardHookSource();
        var cursor = new MutableCursorPositionSource(new ScreenPoint(90, 50));
        var gate = new DrawingModeInputGate();
        var controller = new DrawingSessionController();
        var states = new List<DrawingState>();
        controller.StateChanged += states.Add;
        _ = new GlobalShortcutAdapter(source, controller, gate, cursor, action => action(),
            new FakeMonitorBoundsSource(left, right), Remapped, _ => { });

        source.PressKey(VirtualKeys.LeftControl);
        source.PressKey(VirtualKeys.LeftShift);
        source.PressKey(D);
        Assert.Equal(left, states[^1].ActiveStroke!.Bounds);
        Assert.True(gate.StopAtBoundary(new ScreenPoint(101, 50)));
        controller.ExitDrawMode();
        cursor.Position = new ScreenPoint(110, 50);
        var countAfterCrossing = states.Count;

        source.PressKey(D);
        Assert.Equal(countAfterCrossing, states.Count);
        source.ReleaseKey(D);
        source.PressKey(D);
        Assert.Equal(right, states[^1].ActiveStroke!.Bounds);
    }

    [Fact]
    public void AltGr_DoesNotStartRemappedDraw_AndPassesKeyThrough()
    {
        var sound = new SoundTestHarness(Remapped);

        // Windows reports AltGr as Left Ctrl followed by Right Alt.
        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.RightMenu);
        sound.Source.PressKey(VirtualKeys.LeftShift);

        Assert.False(sound.Source.PressKey(D));
        Assert.False(sound.Source.ReleaseKey(D));
        Assert.False(sound.Gate.IsActive);
        Assert.Empty(sound.Queued);
    }

    [Fact]
    public void AltGrWithShift_DoesNotStartCtrlShiftSound_AndPassesKeyThrough()
    {
        var ctrlShiftA = Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Shift, ShortcutKey.Letter('A'));
        var sound = new SoundTestHarness(new ShortcutBindings(
            SettingsSnapshot.Default.DrawShortcut,
            SettingsSnapshot.Default.ClearShortcut,
            [(ctrlShiftA, new PlaySoundCommand(new SoundId("applause")))]));

        // Windows reports AltGr as Left Ctrl followed by Right Alt.
        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.RightMenu);
        sound.Source.PressKey(VirtualKeys.LeftShift);

        Assert.False(sound.Source.PressKey(VirtualKeys.A));
        Assert.False(sound.Source.ReleaseKey(VirtualKeys.A));
        sound.RunQueuedActions();
        Assert.Empty(sound.Played);
    }

    [Fact]
    public void RightAltHeld_BlocksDrawEvenWithLeftAltAndCtrl()
    {
        var sound = new SoundTestHarness();

        sound.Source.PressKey(VirtualKeys.LeftControl);
        sound.Source.PressKey(VirtualKeys.RightMenu);
        sound.Source.PressKey(VirtualKeys.LeftMenu);

        Assert.False(sound.Source.PressKey(VirtualKeys.Z));
        Assert.False(sound.Gate.IsActive);
        Assert.Empty(sound.Queued);
    }

    private sealed class SoundTestHarness
    {
        public SoundTestHarness(params (SoundSlot Slot, string Sound)[] assignments)
            : this(ShortcutBindings.ForCodeAssignments(new SoundConfiguration(assignments.ToDictionary(
                assignment => assignment.Slot,
                assignment => new SoundAssignment(new SoundId(assignment.Sound), $@"C:\sounds\{assignment.Sound}.wav")))))
        {
        }

        public SoundTestHarness(ShortcutBindings bindings)
        {
            _ = new GlobalShortcutAdapter(
                Source,
                Controller,
                Gate,
                new FakeCursorPositionSource(new ScreenPoint(0, 0)),
                Queued.Enqueue,
                Monitors,
                bindings,
                command => SoundQueued.Enqueue(() => Played.Add(command)));
        }

        public FakeKeyboardHookSource Source { get; } = new();

        public DrawingSessionController Controller { get; } = new();

        public DrawingModeInputGate Gate { get; } = new();

        public CountingMonitorBoundsSource Monitors { get; } = new(DefaultMonitor);

        public Queue<Action> Queued { get; } = new();

        public Queue<Action> SoundQueued { get; } = new();

        public List<PlaySoundCommand> Played { get; } = [];

        public void RunQueuedActions()
        {
            while (Queued.TryDequeue(out var action))
            {
                action();
            }

            RunSoundActions();
        }

        public void RunSoundActions()
        {
            while (SoundQueued.TryDequeue(out var action))
            {
                action();
            }
        }
    }

    private sealed class CountingMonitorBoundsSource(MonitorBounds monitor) : IMonitorBoundsSource
    {
        public int Queries { get; private set; }

        public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
        {
            Queries++;
            bounds = monitor;
            return true;
        }
    }

    private sealed class FakeKeyboardHookSource : IKeyboardHookSource
    {
        public event Action<int>? KeyDown;

        public event Action<int>? KeyUp;

        public event Func<int, KeyDirection, bool>? KeySuppressionRequested;

        public bool PressKey(int vkCode)
        {
            KeyDown?.Invoke(vkCode);
            return ShouldSuppressKey(vkCode);
        }

        /// <summary>Asks for suppression before raising KeyUp, as the Win32 hook does.</summary>
        public bool ReleaseKey(int vkCode)
        {
            var suppress = ShouldSuppressKey(vkCode, KeyDirection.Up);
            KeyUp?.Invoke(vkCode);
            return suppress;
        }

        public bool ShouldSuppressKey(int vkCode, KeyDirection direction = KeyDirection.Down) =>
            KeySuppressionRequested?
                .GetInvocationList()
                .Cast<Func<int, KeyDirection, bool>>()
                .Any(handler => handler(vkCode, direction))
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

    private sealed class MutableCursorPositionSource(ScreenPoint position) : ICursorPositionSource
    {
        public ScreenPoint Position { get; set; } = position;

        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = Position;
            return true;
        }
    }

    private sealed class FakeMonitorBoundsSource(params MonitorBounds[] monitors) : IMonitorBoundsSource
    {
        public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
        {
            foreach (var monitor in monitors)
            {
                if (monitor.Contains(point))
                {
                    bounds = monitor;
                    return true;
                }
            }

            bounds = default;
            return false;
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

    private sealed class RecoveringCursorPositionSource(int failuresBeforeSuccess, ScreenPoint point)
        : ICursorPositionSource
    {
        private int remainingFailures = failuresBeforeSuccess;

        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            if (remainingFailures > 0)
            {
                remainingFailures--;
                position = default;
                return false;
            }

            position = point;
            return true;
        }
    }
}
