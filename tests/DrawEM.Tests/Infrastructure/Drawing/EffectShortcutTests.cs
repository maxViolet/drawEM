using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Effects;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

public class EffectShortcutTests
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private const int EffectKey = VirtualKeys.D2;
    private const int SoundKey = VirtualKeys.D1;
    private static readonly MonitorBounds Left = new(-1920, 0, 0, 1080);
    private static readonly MonitorBounds Right = new(0, 0, 2560, 1440);
    private static readonly BuiltInEffect Confetti = Find("confetti");
    private static readonly BuiltInEffect FocusRing = Find("focus-ring");
    private static readonly PlaySoundCommand Applause = new(new SoundId("applause.wav"), @"C:\library\applause.wav");

    [Theory]
    [InlineData("confetti")]
    [InlineData("focus-ring")]
    public void EffectShortcut_StartsOncePerPress_HoldDoesNotRepeat_FreshPressRestarts(string id)
    {
        var effect = Find(id);
        var harness = new Harness(effect);

        harness.PressChord(EffectKey);
        harness.Source.PressKey(EffectKey);
        harness.Source.PressKey(EffectKey);
        harness.RunQueued();

        var first = Assert.Single(harness.Started);
        Assert.Equal(effect.Id, first.Effect);
        Assert.Equal(effect.Placement, first.Placement);

        harness.Source.ReleaseKey(EffectKey);
        harness.Source.PressKey(EffectKey);
        harness.RunQueued();

        Assert.Equal([first, first], harness.Started);
    }

    [Fact]
    public void CursorEffect_TargetsPointCapturedAtKeyDown_AndResolvesItsMonitorOutsideTheHook()
    {
        var harness = new Harness(FocusRing);
        harness.Cursor.Position = new ScreenPoint(-100, 500);

        harness.PressChord(EffectKey);
        harness.Cursor.Position = new ScreenPoint(1200, 700);

        Assert.Empty(harness.Started);
        Assert.Equal(0, harness.Monitors.Queries);

        harness.RunQueued();

        var command = Assert.Single(harness.Started);
        Assert.Equal(new ScreenPoint(-100, 500), command.CursorPoint);
        Assert.Equal(Left, command.Monitor);
    }

    [Fact]
    public void EffectShortcut_CursorOutsideEveryMonitor_StartsNothing()
    {
        var harness = new Harness(Confetti);
        harness.Cursor.Position = new ScreenPoint(5000, 5000);

        harness.PressChord(EffectKey);
        harness.RunQueued();

        Assert.Empty(harness.Started);
    }

    [Fact]
    public void EffectShortcut_CursorUnavailable_StartsNothing_ButStillHidesTheKey()
    {
        var harness = new Harness(Confetti);
        harness.Cursor.Available = false;

        Assert.True(harness.PressChord(EffectKey));
        harness.RunQueued();

        Assert.Empty(harness.Queued);
        Assert.Empty(harness.Started);
        Assert.True(harness.Source.ReleaseKey(EffectKey));
    }

    [Fact]
    public void EffectShortcut_HidesKeyDownRepeatsAndKeyUp_ThenThePlainKeyPassesThrough()
    {
        var harness = new Harness(Confetti);

        Assert.True(harness.PressChord(EffectKey));
        Assert.True(harness.Source.PressKey(EffectKey));
        Assert.True(harness.Source.ReleaseKey(EffectKey));
        harness.Source.ReleaseKey(VirtualKeys.LeftMenu);
        harness.Source.ReleaseKey(VirtualKeys.LeftControl);

        Assert.False(harness.Source.PressKey(EffectKey));
        Assert.False(harness.Source.ReleaseKey(EffectKey));
    }

    [Fact]
    public void QueuedEffect_SaveInterruptsBeforeDispatch_NeverStartsUnderTheNewConfiguration()
    {
        var harness = new Harness(FocusRing);

        harness.PressChord(EffectKey);
        harness.Adapter.Interrupt();
        harness.Adapter.Bind(WithEffectInSlot2(Confetti), _ => Applause);
        harness.Source.Release(EffectKey, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
        harness.PressChord(EffectKey);
        harness.RunQueued();

        var command = Assert.Single(harness.Started);
        Assert.Equal(Confetti.Id, command.Effect);
    }

    [Fact]
    public void QueuedEffect_CarriesTheConfigurationGenerationOfItsPress()
    {
        var harness = new Harness(Confetti);
        harness.PressChord(EffectKey);
        harness.RunQueued();
        harness.Adapter.Interrupt();
        harness.Source.Release(EffectKey, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);

        harness.PressChord(EffectKey);
        harness.RunQueued();

        Assert.Equal(2, harness.Started.Count);
        Assert.True(harness.Started[1].ConfigurationGeneration > harness.Started[0].ConfigurationGeneration);
    }

    [Fact]
    public void AltGrChord_DoesNotStartEffect_AndPassesKeyThrough()
    {
        var harness = new Harness(Confetti);

        // Windows reports AltGr as Left Ctrl followed by Right Alt.
        harness.Source.Press(VirtualKeys.LeftControl, VirtualKeys.RightMenu, VirtualKeys.LeftMenu);

        Assert.False(harness.Source.PressKey(EffectKey));
        Assert.False(harness.Source.ReleaseKey(EffectKey));
        harness.RunQueued();
        Assert.Empty(harness.Started);
    }

    [Fact]
    public void EffectShortcut_DuringCapture_IsCapturedNotStarted_AndNeedsAFreshPressAfterwards()
    {
        var harness = new Harness(Confetti);
        var results = new List<ShortcutCaptureResult>();
        harness.Adapter.Begin(results.Add);

        Assert.True(harness.PressChord(EffectKey));
        harness.RunQueued();
        harness.Adapter.End();

        Assert.IsType<ShortcutCaptureResult.Captured>(Assert.Single(results));
        Assert.Empty(harness.Started);

        harness.Source.PressKey(EffectKey);
        harness.RunQueued();
        Assert.Empty(harness.Started);

        harness.Source.Release(EffectKey, VirtualKeys.LeftMenu, VirtualKeys.LeftControl);
        harness.PressChord(EffectKey);
        harness.RunQueued();
        Assert.Single(harness.Started);
    }

    [Fact]
    public void SoundAndEffectShortcuts_EachStartOnlyTheirOwnAction()
    {
        var harness = new Harness(Confetti);

        harness.PressChord(SoundKey);
        harness.RunQueued();
        Assert.Equal([Applause], harness.Played);
        Assert.Empty(harness.Started);

        harness.Source.ReleaseKey(SoundKey);
        harness.Source.PressKey(EffectKey);
        harness.RunQueued();

        Assert.Equal([Applause], harness.Played);
        Assert.Single(harness.Started);
    }

    private static BuiltInEffect Find(string id) =>
        EffectCatalog.TryFind(new EffectId(id), out var effect) ? effect : throw new ArgumentException(id);

    private static SettingsSnapshot WithEffectInSlot2(BuiltInEffect effect) =>
        SettingsSnapshot.Validate(
            SettingsSnapshot.Default.Style,
            SettingsSnapshot.Default.DrawShortcut,
            SettingsSnapshot.Default.ClearShortcut,
            Enumerable.Range(1, ActionSlot.Count).Select(number => number == 2
                ? new ActionSlot(2, new EffectAction(effect.Id, Shortcut.Create(CtrlAlt, ShortcutKey.Digit(2))))
                : ActionSlot.Empty(number))).Snapshot!;

    /// <summary>Ctrl+Alt+1 plays a sound and Ctrl+Alt+2 starts the effect; dispatcher work waits in <see cref="Queued"/>.</summary>
    private sealed class Harness
    {
        public Harness(BuiltInEffect effect)
        {
            Adapter = new GlobalShortcutAdapter(
                Source,
                Source,
                new DrawingSessionController(),
                new DrawingModeInputGate(),
                Cursor,
                Queued.Enqueue,
                Monitors,
                new ShortcutBindings(
                    SettingsSnapshot.Default.DrawShortcut,
                    SettingsSnapshot.Default.ClearShortcut,
                    [(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(1)), Applause)],
                    [(Shortcut.Create(CtrlAlt, ShortcutKey.Digit(2)), effect)]),
                Played.Add,
                Started.Add);
        }

        public GlobalShortcutAdapter Adapter { get; }

        public FakeKeyboardHookSource Source { get; } = new();

        public SettableCursor Cursor { get; } = new();

        public CountingMonitors Monitors { get; } = new(Left, Right);

        public Queue<Action> Queued { get; } = new();

        public List<PlaySoundCommand> Played { get; } = [];

        public List<StartEffectCommand> Started { get; } = [];

        /// <returns>Whether the hook hid the key-down of <paramref name="key"/>.</returns>
        public bool PressChord(int key)
        {
            Source.Press(VirtualKeys.LeftControl, VirtualKeys.LeftMenu);
            return Source.PressKey(key);
        }

        public void RunQueued()
        {
            while (Queued.TryDequeue(out var action))
            {
                action();
            }
        }
    }

    private sealed class SettableCursor : ICursorPositionSource
    {
        public ScreenPoint Position { get; set; } = new(100, 100);

        public bool Available { get; set; } = true;

        public bool TryGetCurrentPosition(out ScreenPoint position)
        {
            position = Position;
            return Available;
        }
    }

    private sealed class CountingMonitors(params MonitorBounds[] monitors) : IMonitorBoundsSource
    {
        private readonly FakeMonitorBoundsSource inner = new(monitors);

        public int Queries { get; private set; }

        public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
        {
            Queries++;
            return inner.TryGetBounds(point, out bounds);
        }
    }
}
