using DrawEM.App.Application.Input;
using DrawEM.App.Domain.Settings;
using static DrawEM.App.Application.Input.ShortcutCommand;

namespace DrawEM.Tests.Application.Input;

public class ShortcutDecisionEngineTests
{
    private static readonly InputKey LeftCtrl = InputKey.Modifier(ModifierKey.LeftControl);
    private static readonly InputKey LeftAlt = InputKey.Modifier(ModifierKey.LeftAlt);
    private static readonly InputKey RightAlt = InputKey.Modifier(ModifierKey.RightAlt);
    private static readonly InputKey Z = InputKey.Candidate(ShortcutKey.Letter('Z'));
    private static readonly InputKey X = InputKey.Candidate(ShortcutKey.Letter('X'));
    private static readonly InputKey A = InputKey.Candidate(ShortcutKey.Letter('A'));
    private static readonly InputKey One = InputKey.Candidate(ShortcutKey.Digit(1));
    private static readonly InputKey Two = InputKey.Candidate(ShortcutKey.Digit(2));
    private static readonly InputKey Escape = InputKey.Other(0x1B);

    private readonly FakeGate gate = new();

    [Fact]
    public void DrawChord_EmitsBeginDrawOnce_AndEndDrawOnRelease()
    {
        var engine = new ShortcutDecisionEngine(_ => false);

        Down(engine, LeftCtrl);
        Down(engine, LeftAlt);
        var press = Down(engine, Z);
        var repeat = Down(engine, Z);
        var release = Up(engine, Z);

        Assert.Equal([new BeginDraw()], press.Commands);
        Assert.Empty(repeat.Commands);
        Assert.Equal([new EndDraw()], release.Commands);
        Assert.False(press.Suppress);
        Assert.False(release.Suppress);
    }

    [Fact]
    public void ActiveGate_SuppressesEveryKeyExceptTheDrawingChord()
    {
        var engine = new ShortcutDecisionEngine(_ => false);
        HoldDrawChord(engine);

        Assert.True(Down(engine, A).Suppress);
        Assert.True(Down(engine, Escape).Suppress);
        Assert.True(Down(engine, InputKey.Modifier(ModifierKey.LeftShift)).Suppress);
        Assert.False(Down(engine, LeftCtrl).Suppress);
        Assert.False(Down(engine, RightAlt).Suppress);
        Assert.False(Down(engine, Z).Suppress);
    }

    [Fact]
    public void KeyUp_DecidesSuppressionFromStateBeforeRelease()
    {
        var engine = new ShortcutDecisionEngine(_ => false);
        HoldDrawChord(engine);
        Down(engine, A);

        Up(engine, Z);

        // The gate was active when A went down, but draw mode ended before A's release.
        Assert.False(Up(engine, A).Suppress);
    }

    [Fact]
    public void BlockedGate_DoesNotBeginDraw_UntilChordReleased()
    {
        var engine = new ShortcutDecisionEngine(_ => false);
        HoldDrawChord(engine);

        // The mouse path stops drawing at a monitor boundary while the chord is held.
        gate.Status = new DrawGateStatus(IsActive: false, IsBlockedUntilReleased: true);

        Assert.Empty(Down(engine, Z).Commands);

        Assert.Equal([new ReleaseDrawBlock()], Up(engine, Z).Commands);
        Assert.Equal([new BeginDraw()], Down(engine, Z).Commands);
    }

    [Fact]
    public void ClearChord_EmitsClearOnce_AndSuppressesXUntilReleased()
    {
        var engine = new ShortcutDecisionEngine(_ => false);
        Down(engine, LeftCtrl);
        Down(engine, LeftAlt);

        var press = Down(engine, X);
        var repeat = Down(engine, X);
        var release = Up(engine, X);

        Assert.Equal([new ClearMonitor()], press.Commands);
        Assert.True(press.Suppress);
        Assert.Empty(repeat.Commands);
        Assert.True(repeat.Suppress);
        Assert.True(release.Suppress);
        Assert.Equal([new ReleaseDrawBlock()], release.Commands);
    }

    [Fact]
    public void ClearWhileDrawing_ClearsWithoutEndDraw_AndBlocksRedrawUntilRelease()
    {
        var engine = new ShortcutDecisionEngine(_ => false);
        HoldDrawChord(engine);

        Assert.Equal([new ClearMonitor()], Down(engine, X).Commands);
        Assert.Empty(Down(engine, Z).Commands);
    }

    [Fact]
    public void SoundChord_PlaysOncePerPress_AndSuppressesDownAndUp()
    {
        var engine = new ShortcutDecisionEngine(slot => slot == 1);
        Down(engine, LeftCtrl);
        Down(engine, LeftAlt);

        var press = Down(engine, One);
        var repeat = Down(engine, One);
        var release = Up(engine, One);
        var again = Down(engine, One);

        Assert.Equal([new PlaySlot(1)], press.Commands);
        Assert.Empty(repeat.Commands);
        Assert.Equal([new PlaySlot(1)], again.Commands);
        Assert.True(press.Suppress);
        Assert.True(repeat.Suppress);
        Assert.True(release.Suppress);
    }

    [Fact]
    public void SoundChord_CompletedWhileDigitHeld_PlaysOnce_ButPassesKeyUp()
    {
        var engine = new ShortcutDecisionEngine(slot => slot == 1);
        Down(engine, One);
        Down(engine, LeftCtrl);

        var completion = Down(engine, LeftAlt);

        Assert.Equal([new PlaySlot(1)], completion.Commands);
        Assert.True(Down(engine, One).Suppress);
        Assert.False(Up(engine, One).Suppress);
    }

    [Fact]
    public void UnassignedSlot_PlaysNothingAndPassesThrough()
    {
        var engine = new ShortcutDecisionEngine(slot => slot == 1);
        Down(engine, LeftCtrl);
        Down(engine, LeftAlt);

        var press = Down(engine, Two);

        Assert.Empty(press.Commands);
        Assert.False(press.Suppress);
    }

    [Fact]
    public void RightAlt_CountsAsAlt_InCurrentV2Chords()
    {
        // v2 behavior kept by this refactor; v3 Step 3 reserves Right Alt for AltGr.
        var engine = new ShortcutDecisionEngine(_ => false);
        Down(engine, LeftCtrl);
        Down(engine, RightAlt);

        Assert.Equal([new BeginDraw()], Down(engine, Z).Commands);
    }

    [Fact]
    public void EveryDecision_IsReturnedSynchronously_WithoutCallingOutExceptSlotLookup()
    {
        var lookups = new List<int>();
        var engine = new ShortcutDecisionEngine(slot =>
        {
            lookups.Add(slot);
            return false;
        });

        Down(engine, LeftCtrl);
        Down(engine, LeftAlt);
        Down(engine, One);

        Assert.Equal([1], lookups);
    }

    private void HoldDrawChord(ShortcutDecisionEngine engine)
    {
        Down(engine, LeftCtrl);
        Down(engine, LeftAlt);
        Down(engine, Z);
    }

    private ShortcutDecision Down(ShortcutDecisionEngine engine, InputKey key) =>
        gate.Apply(engine.Handle(key, KeyDirection.Down, gate.Status));

    private ShortcutDecision Up(ShortcutDecisionEngine engine, InputKey key) =>
        gate.Apply(engine.Handle(key, KeyDirection.Up, gate.Status));

    /// <summary>Applies gate commands the way the hook adapter does when the cursor is available.</summary>
    private sealed class FakeGate
    {
        public DrawGateStatus Status { get; set; }

        public ShortcutDecision Apply(ShortcutDecision decision)
        {
            foreach (var command in decision.Commands)
            {
                Status = command switch
                {
                    BeginDraw => Status with { IsActive = true },
                    EndDraw => Status with { IsActive = false },
                    ReleaseDrawBlock => Status with { IsBlockedUntilReleased = false },
                    ClearMonitor => new DrawGateStatus(false, true),
                    _ => Status,
                };
            }

            return decision;
        }
    }
}
