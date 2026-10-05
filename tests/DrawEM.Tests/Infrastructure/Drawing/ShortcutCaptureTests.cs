using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

/// <summary>
/// Shortcut capture and neutral-key injection, driven through <see cref="KeyboardHookEvents"/> so injected
/// neutral keys come back into the hook tagged, as they do from SendInput.
/// </summary>
public class ShortcutCaptureTests
{
    private const int Tab = 0x09;
    private const int C = 0x43;
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private const ShortcutModifiers CtrlShift = ShortcutModifiers.Control | ShortcutModifiers.Shift;
    private const ShortcutModifiers AltShift = ShortcutModifiers.Alt | ShortcutModifiers.Shift;
    private static readonly PlaySoundCommand Applause = new(new SoundId("applause"), @"C:\library\applause");
    private static readonly PlaySoundCommand Drumroll = new(new SoundId("drumroll"), @"C:\library\drumroll");

    [Theory]
    [InlineData(VirtualKeys.Z)]
    [InlineData(VirtualKeys.X)]
    [InlineData(VirtualKeys.D1)]
    public void BoundChord_IsCapturedWithoutDrawingClearingOrPlaying(int vkCode)
    {
        var hook = new Harness();
        hook.Begin();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.True(hook.Press(vkCode));
        Assert.True(hook.Press(vkCode));
        hook.RunQueued();

        Assert.Equal(Captured(CtrlAlt, vkCode), Assert.Single(hook.Results));
        Assert.Empty(hook.States);
        Assert.Empty(hook.Played);
        Assert.False(hook.Gate.IsActive);
    }

    [Fact]
    public void AfterCapture_DispatchResumesOnlyAfterAllKeysReleaseAndAFreshPress()
    {
        var hook = new Harness();
        hook.Begin();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D1);

        Assert.True(hook.Release(VirtualKeys.D1));
        Assert.False(hook.Press(VirtualKeys.D1));
        Assert.False(hook.Release(VirtualKeys.D1));
        Assert.False(hook.Press(VirtualKeys.Z));
        hook.Release(VirtualKeys.Z);
        hook.Release(VirtualKeys.LeftMenu);
        hook.Release(VirtualKeys.LeftControl);
        hook.RunQueued();
        Assert.Empty(hook.Played);
        Assert.Empty(hook.States);

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.True(hook.Press(VirtualKeys.D1));
        hook.RunQueued();

        Assert.Equal([Applause], hook.Played);
        Assert.Single(hook.Results);
    }

    [Fact]
    public void KeyPressedWhileDispatchPaused_MustAlsoBeReleased()
    {
        var hook = new Harness();
        hook.Begin();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D2);

        hook.Release(VirtualKeys.D2);
        hook.Release(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Release(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.D1);
        hook.RunQueued();

        Assert.Empty(hook.Played);
    }

    [Fact]
    public void HeldKeysAtBegin_KeepTheirSuppression_AndBlockCaptureUntilReleased()
    {
        var hook = new Harness();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.True(hook.Press(VirtualKeys.D1));
        Assert.False(hook.Press(VirtualKeys.A));
        hook.RunQueued();
        Assert.Equal([Applause], hook.Played);

        hook.Begin();

        Assert.True(hook.Press(VirtualKeys.D1));
        Assert.True(hook.Release(VirtualKeys.D1));
        Assert.False(hook.Release(VirtualKeys.A));
        Assert.False(hook.Press(VirtualKeys.D2));
        Assert.False(hook.Release(VirtualKeys.D2));
        hook.Release(VirtualKeys.LeftMenu);
        hook.Release(VirtualKeys.LeftControl);
        hook.RunQueued();
        Assert.Empty(hook.Results);

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftShift);
        Assert.True(hook.Press(VirtualKeys.A));
        hook.RunQueued();

        Assert.Equal(Captured(CtrlShift, VirtualKeys.A), Assert.Single(hook.Results));
        Assert.Equal([Applause], hook.Played);
    }

    [Fact]
    public void NewKeyHeldWithEntryKey_CanFormTheChordOnceEntryKeyIsReleased()
    {
        var hook = new Harness();
        hook.Press(Tab);
        hook.Begin();

        hook.Press(VirtualKeys.LeftControl);
        hook.Release(Tab);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D5);
        hook.RunQueued();

        Assert.Equal(Captured(CtrlAlt, VirtualKeys.D5), Assert.Single(hook.Results));
    }

    [Fact]
    public void CtrlC_IsRejectedAndHidden_ThenCaptureRearmsAfterAllItsKeysRelease()
    {
        var hook = new Harness();
        hook.Begin();

        hook.Press(VirtualKeys.LeftControl);
        Assert.True(hook.Press(C));
        hook.Release(C);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.False(hook.Press(VirtualKeys.D1));
        Assert.False(hook.Release(VirtualKeys.D1));
        hook.RunQueued();
        Assert.Equal([Rejected(ShortcutCaptureRejection.TooFewModifiers)], hook.Results);

        hook.Release(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftControl);
        Assert.True(hook.Press(VirtualKeys.D1));
        hook.RunQueued();

        Assert.Equal(
            [Rejected(ShortcutCaptureRejection.TooFewModifiers), Captured(CtrlAlt, VirtualKeys.D1)],
            hook.Results);
        Assert.Empty(hook.Played);
    }

    [Fact]
    public void CtrlRightAlt1_IsRejectedAndPassesThrough_ThenCaptureRearms()
    {
        var hook = new Harness();
        hook.Begin();

        Assert.False(hook.Press(VirtualKeys.LeftControl));
        Assert.False(hook.Press(VirtualKeys.RightMenu));
        Assert.False(hook.Press(VirtualKeys.D1));
        Assert.False(hook.Release(VirtualKeys.D1));
        hook.Release(VirtualKeys.RightMenu);
        hook.Release(VirtualKeys.LeftControl);
        hook.RunQueued();
        Assert.Equal([Rejected(ShortcutCaptureRejection.RightAlt)], hook.Results);

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D1);
        hook.RunQueued();

        Assert.Equal(Captured(CtrlAlt, VirtualKeys.D1), hook.Results[^1]);
        Assert.Empty(hook.Played);
    }

    [Fact]
    public void Escape_IsConsumedAndCancelsCapture()
    {
        var hook = new Harness();
        hook.Begin();
        hook.Press(VirtualKeys.LeftControl);

        Assert.True(hook.Press(VirtualKeys.Escape));
        Assert.True(hook.Press(VirtualKeys.Escape));
        Assert.True(hook.Release(VirtualKeys.Escape));
        hook.Release(VirtualKeys.LeftControl);
        hook.RunQueued();
        Assert.Equal([new ShortcutCaptureResult.Cancelled()], hook.Results);

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D1);
        hook.RunQueued();

        Assert.Single(hook.Results);
        Assert.Equal([Applause], hook.Played);
    }

    [Fact]
    public void End_LeavesCaptureWithoutReporting_AndPausesDispatchWhileKeysAreHeld()
    {
        var hook = new Harness();
        hook.Begin();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);

        hook.End();
        Assert.False(hook.Press(VirtualKeys.D1));
        hook.Release(VirtualKeys.D1);
        hook.Release(VirtualKeys.LeftMenu);
        hook.Release(VirtualKeys.LeftControl);
        hook.RunQueued();
        Assert.Empty(hook.Played);

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D1);
        hook.RunQueued();

        Assert.Empty(hook.Results);
        Assert.Equal([Applause], hook.Played);
    }

    [Fact]
    public void End_WithNoKeysHeld_ResumesDispatchAtOnce()
    {
        var hook = new Harness();
        hook.Begin();
        hook.End();
        hook.End();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D1);
        hook.RunQueued();

        Assert.Equal([Applause], hook.Played);
    }

    [Fact]
    public void Begin_WhileDrawing_ExitsDrawMode()
    {
        var hook = new Harness();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.Z);
        hook.RunQueued();

        hook.Begin();
        hook.RunQueued();

        Assert.False(hook.Gate.IsActive);
        Assert.Equal([true, false], hook.States.Select(state => state.IsDrawModeActive));
        Assert.False(hook.Press(VirtualKeys.A));
    }

    [Fact]
    public void Begin_WhileDrawing_KeepsHidingKeysThatDrawModeHid()
    {
        var hook = new Harness();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.Z);
        Assert.True(hook.Press(VirtualKeys.A));

        hook.Begin();

        Assert.True(hook.Press(VirtualKeys.A));
        Assert.True(hook.Release(VirtualKeys.A));
        Assert.False(hook.Release(VirtualKeys.Z));
        Assert.False(hook.Release(VirtualKeys.LeftMenu));
        Assert.False(hook.Release(VirtualKeys.LeftControl));

        hook.End();
        Assert.False(hook.Press(VirtualKeys.A));
        Assert.False(hook.Release(VirtualKeys.A));
    }

    [Fact]
    public void Begin_WhileDrawing_PassesKeyUpForKeyPressedBeforeDrawing()
    {
        var hook = new Harness();
        Assert.False(hook.Press(VirtualKeys.A));
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.Z);
        Assert.True(hook.Gate.IsActive);

        hook.Begin();

        Assert.False(hook.Release(VirtualKeys.A));
    }

    [Theory]
    [InlineData(VirtualKeys.LeftMenu, Tab)]
    [InlineData(VirtualKeys.LeftMenu, VirtualKeys.F4)]
    [InlineData(VirtualKeys.LeftWindows, VirtualKeys.D1)]
    [InlineData(VirtualKeys.RightWindows, VirtualKeys.A)]
    [InlineData(VirtualKeys.LeftShift, Tab)]
    public void PassThroughChord_IsNeitherHiddenNorCaptured(int modifier, int vkCode)
    {
        var hook = new Harness();
        hook.Begin();
        if (modifier is VirtualKeys.LeftWindows or VirtualKeys.RightWindows)
        {
            Assert.False(hook.Press(VirtualKeys.LeftControl));
            Assert.False(hook.Press(VirtualKeys.LeftMenu));
        }

        Assert.False(hook.Press(modifier));
        Assert.False(hook.Press(vkCode));
        Assert.False(hook.Release(vkCode));
        Assert.False(hook.Release(modifier));
        hook.Release(VirtualKeys.LeftMenu);
        hook.Release(VirtualKeys.LeftControl);
        hook.RunQueued();
        Assert.Empty(hook.Results);

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.D7);
        hook.RunQueued();

        Assert.Equal(Captured(CtrlAlt, VirtualKeys.D7), Assert.Single(hook.Results));
        Assert.Equal(0, hook.NeutralKeys);
    }

    [Fact]
    public void CtrlAltF4_IsCaptured()
    {
        var hook = new Harness();
        hook.Begin();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.True(hook.Press(VirtualKeys.F4));
        hook.RunQueued();

        Assert.Equal(Captured(CtrlAlt, VirtualKeys.F4), Assert.Single(hook.Results));
    }

    [Theory]
    [InlineData(VirtualKeys.LeftControl, VirtualKeys.LeftShift)]
    [InlineData(VirtualKeys.RightControl, VirtualKeys.RightShift)]
    [InlineData(VirtualKeys.LeftMenu, VirtualKeys.LeftShift)]
    public void ActionMode_SuppressedCandidateUnderShiftPair_EmitsOneNeutralKeyBeforeModifierRelease(
        int first, int second)
    {
        var hook = new Harness();

        hook.Press(first);
        hook.Press(second);
        Assert.True(hook.Press(VirtualKeys.D3));
        Assert.True(hook.Press(VirtualKeys.D3));
        hook.Release(VirtualKeys.D3);
        hook.Release(second);
        hook.Release(first);
        hook.RunQueued();

        Assert.Equal(
            [$"down {first:X}", $"down {second:X}", "down 33", "neutral", "down 33", "up 33", $"up {second:X}", $"up {first:X}"],
            hook.Log);
        Assert.Equal([Drumroll], hook.Played);
    }

    [Fact]
    public void ActionMode_EachSuppressedPressUnderShiftPair_EmitsItsOwnNeutralKey()
    {
        var hook = new Harness();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftShift);
        hook.Press(VirtualKeys.D3);
        hook.Release(VirtualKeys.D3);
        hook.Press(VirtualKeys.D3);

        Assert.Equal(2, hook.NeutralKeys);
    }

    [Theory]
    [InlineData(VirtualKeys.LeftControl, VirtualKeys.LeftShift, VirtualKeys.A)]
    [InlineData(VirtualKeys.LeftMenu, VirtualKeys.RightShift, VirtualKeys.F12)]
    [InlineData(VirtualKeys.LeftMenu, VirtualKeys.LeftShift, VirtualKeys.D1)]
    public void CaptureMode_SuppressedCandidateUnderShiftPair_EmitsOneNeutralKeyBeforeModifierRelease(
        int first, int second, int vkCode)
    {
        var hook = new Harness();
        hook.Begin();

        hook.Press(first);
        hook.Press(second);
        Assert.True(hook.Press(vkCode));
        hook.Press(vkCode);
        hook.Release(vkCode);
        hook.Release(second);
        hook.Release(first);
        hook.RunQueued();

        Assert.Equal(
            [$"down {first:X}", $"down {second:X}", $"down {vkCode:X}", "neutral", $"down {vkCode:X}", $"up {vkCode:X}", $"up {second:X}", $"up {first:X}"],
            hook.Log);
        Assert.IsType<ShortcutCaptureResult.Captured>(Assert.Single(hook.Results));
        Assert.Empty(hook.Played);
    }

    [Fact]
    public void ThreeModifierCandidate_EmitsNeutralKey()
    {
        var hook = new Harness();
        hook.Begin();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.LeftShift);
        hook.Press(VirtualKeys.A);

        Assert.Equal(1, hook.NeutralKeys);
    }

    [Fact]
    public void CtrlAltCandidate_EmitsNoNeutralKey_InEitherMode()
    {
        var hook = new Harness();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.True(hook.Press(VirtualKeys.D1));
        hook.Release(VirtualKeys.D1);
        hook.Release(VirtualKeys.LeftMenu);
        hook.Release(VirtualKeys.LeftControl);

        hook.Begin();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        Assert.True(hook.Press(VirtualKeys.D4));

        Assert.Equal(0, hook.NeutralKeys);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BareLayoutSwitchPair_EmitsNoNeutralKey(bool capturing)
    {
        var hook = new Harness();
        if (capturing)
        {
            hook.Begin();
        }

        Assert.False(hook.Press(VirtualKeys.LeftMenu));
        Assert.False(hook.Press(VirtualKeys.LeftShift));
        Assert.False(hook.Release(VirtualKeys.LeftShift));
        Assert.False(hook.Release(VirtualKeys.LeftMenu));
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.RightShift);
        hook.Release(VirtualKeys.RightShift);
        hook.Release(VirtualKeys.LeftControl);

        Assert.Equal(0, hook.NeutralKeys);
    }

    [Fact]
    public void UnboundCandidateUnderShiftPair_PassesThroughWithoutNeutralKey()
    {
        var hook = new Harness();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftShift);

        Assert.False(hook.Press(VirtualKeys.D8));
        Assert.False(hook.Release(VirtualKeys.D8));
        Assert.Equal(0, hook.NeutralKeys);
    }

    [Fact]
    public void RightAltCandidateUnderShift_PassesThroughWithoutNeutralKey()
    {
        var hook = new Harness();
        hook.Begin();

        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.RightMenu);
        hook.Press(VirtualKeys.LeftShift);

        Assert.False(hook.Press(VirtualKeys.A));
        Assert.Equal(0, hook.NeutralKeys);
    }

    [Fact]
    public void TaggedEvents_CannotBeCaptured()
    {
        var hook = new Harness();
        hook.Begin();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);

        Assert.False(hook.PressTagged(VirtualKeys.D1));
        Assert.False(hook.ReleaseTagged(VirtualKeys.D1));
        Assert.False(hook.PressTagged(VirtualKeys.Escape));
        hook.RunQueued();
        Assert.Empty(hook.Results);

        hook.Press(VirtualKeys.D2);
        hook.RunQueued();

        Assert.Equal(Captured(CtrlAlt, VirtualKeys.D2), Assert.Single(hook.Results));
    }

    [Fact]
    public void TaggedEvents_CannotDispatch()
    {
        var hook = new Harness();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);

        Assert.False(hook.PressTagged(VirtualKeys.D1));
        Assert.False(hook.PressTagged(VirtualKeys.X));
        Assert.False(hook.PressTagged(VirtualKeys.Z));
        hook.RunQueued();

        Assert.Empty(hook.Played);
        Assert.Empty(hook.States);
    }

    [Fact]
    public void TaggedNeutralKey_PassesThroughDrawMode_WhileUntaggedOneIsHidden()
    {
        var hook = new Harness();
        hook.Press(VirtualKeys.LeftControl);
        hook.Press(VirtualKeys.LeftMenu);
        hook.Press(VirtualKeys.Z);
        Assert.True(hook.Gate.IsActive);

        Assert.False(hook.PressTagged(VirtualKeys.Neutral));
        Assert.False(hook.ReleaseTagged(VirtualKeys.Neutral));
        Assert.True(hook.Press(VirtualKeys.Neutral));
    }

    [Fact]
    public void Begin_RequiresReport() =>
        Assert.Throws<ArgumentNullException>(() => new Harness().Adapter.Begin(null!));

    [Fact]
    public void Hook_AcceptsOnlyOneDecisionHandler()
    {
        var hook = new KeyboardHookEvents();
        hook.SetKeyHandler((_, _) => true);

        Assert.Throws<InvalidOperationException>(() => hook.SetKeyHandler((_, _) => false));
    }

    private static ShortcutCaptureResult Captured(ShortcutModifiers modifiers, int vkCode)
    {
        Assert.True(KeyChord.TryGetShortcutKey(vkCode, out var key));
        return new ShortcutCaptureResult.Captured(Shortcut.Create(modifiers, key));
    }

    private static ShortcutCaptureResult Rejected(ShortcutCaptureRejection reason) =>
        new ShortcutCaptureResult.Rejected(reason);

    /// <summary>
    /// Default draw and clear, <c>Ctrl+Alt+1</c> for applause, and <c>Ctrl+Shift+3</c>, <c>Alt+Shift+3</c>,
    /// and <c>Ctrl+Alt+Shift+3</c> for drumroll.
    /// </summary>
    private sealed class Harness
    {
        private readonly KeyboardHookEvents hook;
        private readonly Queue<Action> queued = new();

        public Harness()
        {
            hook = new KeyboardHookEvents();
            var neutralKeys = new FakeNeutralKeyEmitter(() =>
            {
                Log.Add("neutral");
                NeutralKeys++;
                // SendInput delivers the pair back through the hook, tagged.
                hook!.Handle(VirtualKeys.Neutral, KeyDirection.Down, KeyboardHookEvents.NeutralKeyTag);
                hook.Handle(VirtualKeys.Neutral, KeyDirection.Up, KeyboardHookEvents.NeutralKeyTag);
            });
            var controller = new DrawingSessionController();
            controller.StateChanged += States.Add;
            var three = ShortcutKey.Digit(3);
            Adapter = new GlobalShortcutAdapter(
                hook,
                neutralKeys,
                controller,
                Gate,
                new FakeCursorPositionSource(new ScreenPoint(0, 0)),
                queued.Enqueue,
                new FakeMonitorBoundsSource(new MonitorBounds(-1000, -1000, 1000, 1000)),
                new ShortcutBindings(
                    SettingsSnapshot.Default.DrawShortcut,
                    SettingsSnapshot.Default.ClearShortcut,
                    [
                        (Shortcut.Create(CtrlAlt, ShortcutKey.Digit(1)), Applause),
                        (Shortcut.Create(CtrlShift, three), Drumroll),
                        (Shortcut.Create(AltShift, three), Drumroll),
                        (Shortcut.Create(CtrlAlt | ShortcutModifiers.Shift, three), Drumroll),
                    ]),
                Played.Add);
        }

        public GlobalShortcutAdapter Adapter { get; }

        public DrawingModeInputGate Gate { get; } = new();

        public List<DrawingState> States { get; } = [];

        public List<PlaySoundCommand> Played { get; } = [];

        public List<ShortcutCaptureResult> Results { get; } = [];

        /// <summary>Untagged key events in order, with each neutral key emission.</summary>
        public List<string> Log { get; } = [];

        public int NeutralKeys { get; private set; }

        public void Begin() => Adapter.Begin(Results.Add);

        public void End() => Adapter.End();

        public bool Press(int vkCode)
        {
            Log.Add($"down {vkCode:X}");
            return hook.Handle(vkCode, KeyDirection.Down, 0);
        }

        public bool Release(int vkCode)
        {
            Log.Add($"up {vkCode:X}");
            return hook.Handle(vkCode, KeyDirection.Up, 0);
        }

        public bool PressTagged(int vkCode) =>
            hook.Handle(vkCode, KeyDirection.Down, KeyboardHookEvents.NeutralKeyTag);

        public bool ReleaseTagged(int vkCode) =>
            hook.Handle(vkCode, KeyDirection.Up, KeyboardHookEvents.NeutralKeyTag);

        public void RunQueued()
        {
            while (queued.TryDequeue(out var action))
            {
                action();
            }
        }
    }

    private sealed class FakeNeutralKeyEmitter(Action emit) : INeutralKeyEmitter
    {
        public void EmitNeutralKey() => emit();
    }
}
