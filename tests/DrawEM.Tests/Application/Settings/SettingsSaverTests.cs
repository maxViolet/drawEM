using DrawEM.App.Application.Input;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;
using static DrawEM.App.Application.Input.ShortcutCommand;

namespace DrawEM.Tests.Application.Settings;

public class SettingsSaverTests
{
    private static readonly InputKey LeftCtrl = InputKey.Modifier(ModifierKey.LeftControl);
    private static readonly InputKey LeftAlt = InputKey.Modifier(ModifierKey.LeftAlt);
    private static readonly InputKey Z = InputKey.Candidate(ShortcutKey.Letter('Z'));
    private static readonly InputKey X = InputKey.Candidate(ShortcutKey.Letter('X'));
    private static readonly InputKey A = InputKey.Candidate(ShortcutKey.Letter('A'));
    private static readonly InputKey One = InputKey.Candidate(ShortcutKey.Digit(1));

    /// <summary>Differs from the defaults only in color: an unrelated change still resets runtime state.</summary>
    private static readonly SettingsSnapshot ColorOnly = SettingsSnapshot.Validate(
        new DrawingStyle(new HexColor(0, 0, 255), new StrokeWidth(4)),
        SettingsSnapshot.Default.DrawShortcut,
        SettingsSnapshot.Default.ClearShortcut,
        SettingsSnapshot.Default.Slots).Snapshot!;

    private readonly Runtime runtime = new();

    [Fact]
    public void Save_PersistsThenStopsSoundThenResetsDrawingThenActivates()
    {
        var result = runtime.Saver.Save(ColorOnly);

        Assert.True(result.Succeeded);
        Assert.Null(result.Error);
        Assert.Equal(["persist:old", "stop-sound:old", "reset-drawing:old"], runtime.Log);
        Assert.Same(ColorOnly, runtime.Active.Current);
        Assert.Same(ColorOnly, runtime.Store.Saved);
    }

    [Fact]
    public void Save_WhenPersistenceFails_ReportsAndChangesNothing()
    {
        runtime.Store.Failure = new SettingsStoreException("Disk is full.");
        HoldSoundChord();

        var result = runtime.Saver.Save(ColorOnly);

        Assert.False(result.Succeeded);
        Assert.Equal("Disk is full.", result.Error);
        Assert.Equal(["persist:old"], runtime.Log);
        Assert.Same(SettingsSnapshot.Default, runtime.Active.Current);

        // The held chord is still the old configuration's press: repeat suppressed, release suppressed,
        // and no fresh press was required.
        Assert.True(runtime.Down(One).Suppress);
        Assert.True(runtime.Up(One).Suppress);
        Assert.Equal([new PlaySlot(1)], runtime.Down(One).Commands);
    }

    [Fact]
    public void Save_WhenPersistenceFailsDuringStroke_KeepsDrawing()
    {
        runtime.Store.Failure = new SettingsStoreException("Access denied.");
        HoldDrawChord();

        runtime.Saver.Save(ColorOnly);

        Assert.True(runtime.Gate.IsActive);
        Assert.True(runtime.Down(A).Suppress);
        Assert.Equal([new EndDraw()], runtime.Up(Z).Commands);
    }

    [Fact]
    public void Save_WithSoundKeyHeld_RequiresFreshPress_AndKeepsItsSuppression()
    {
        HoldSoundChord();

        runtime.Saver.Save(ColorOnly);

        var repeat = runtime.Down(One);
        var release = runtime.Up(One);
        var fresh = runtime.Down(One);

        Assert.Empty(repeat.Commands);
        Assert.True(repeat.Suppress);
        Assert.True(release.Suppress);
        Assert.Equal([new PlaySlot(1)], fresh.Commands);
    }

    [Fact]
    public void Save_WithDigitThatCompletedChordWhileHeld_PassesItsRelease()
    {
        runtime.Down(One);
        runtime.Down(LeftCtrl);
        Assert.Equal([new PlaySlot(1)], runtime.Down(LeftAlt).Commands);

        runtime.Saver.Save(ColorOnly);

        // The application saw this key's first down, so it must also see the release.
        Assert.True(runtime.Down(One).Suppress);
        Assert.False(runtime.Up(One).Suppress);
    }

    [Fact]
    public void Save_DuringStroke_ExitsDrawing_AndRequiresFreshDrawPress()
    {
        HoldDrawChord();

        runtime.Saver.Save(ColorOnly);

        Assert.False(runtime.Gate.IsActive);
        var repeat = runtime.Down(Z);
        var release = runtime.Up(Z);
        var fresh = runtime.Down(Z);

        Assert.Empty(repeat.Commands);
        Assert.False(repeat.Suppress);
        Assert.Empty(release.Commands);
        Assert.False(release.Suppress);
        Assert.Equal([new BeginDraw()], fresh.Commands);
    }

    [Fact]
    public void Save_WithKeySuppressedByDrawing_KeepsItsReleaseSuppressed()
    {
        HoldDrawChord();
        Assert.True(runtime.Down(A).Suppress);

        runtime.Saver.Save(ColorOnly);

        Assert.True(runtime.Down(A).Suppress);
        Assert.True(runtime.Up(A).Suppress);
        Assert.False(runtime.Down(A).Suppress);
    }

    [Fact]
    public void Save_WithClearKeyHeld_DoesNotClearAgain_UntilFreshPress()
    {
        runtime.Down(LeftCtrl);
        runtime.Down(LeftAlt);
        Assert.Equal([new ClearMonitor()], runtime.Down(X).Commands);

        runtime.Saver.Save(ColorOnly);

        Assert.True(runtime.Down(X).Suppress);
        Assert.Empty(runtime.Down(X).Commands);
        Assert.True(runtime.Up(X).Suppress);
        Assert.Equal([new ClearMonitor()], runtime.Down(X).Commands);
    }

    [Fact]
    public void Save_WithUnrelatedKeyHeld_PassesItThrough()
    {
        runtime.Down(A);

        runtime.Saver.Save(ColorOnly);

        Assert.False(runtime.Down(A).Suppress);
        Assert.False(runtime.Up(A).Suppress);
    }

    [Fact]
    public void Save_WhenRuntimeStepThrows_StillActivatesPersistedSnapshot()
    {
        runtime.DrawingFailure = new InvalidOperationException("overlay gone");
        HoldSoundChord();

        Assert.Throws<InvalidOperationException>(() => runtime.Saver.Save(ColorOnly));

        Assert.Same(ColorOnly, runtime.Active.Current);
        Assert.Empty(runtime.Down(One).Commands);
    }

    private void HoldDrawChord()
    {
        runtime.Down(LeftCtrl);
        runtime.Down(LeftAlt);
        Assert.Equal([new BeginDraw()], runtime.Down(Z).Commands);
    }

    private void HoldSoundChord()
    {
        runtime.Down(LeftCtrl);
        runtime.Down(LeftAlt);
        Assert.Equal([new PlaySlot(1)], runtime.Down(One).Commands);
    }

    /// <summary>A shortcut engine, drawing gate, sound channel, and store, recording what Save touches.</summary>
    private sealed class Runtime : ISoundStopper, IDrawingReset
    {
        public Runtime()
        {
            Store = new FakeStore(this);
            Active = new ActiveSettings(SettingsSnapshot.Default);
            Engine = new ShortcutDecisionEngine(slot => slot == 1);
            Saver = new SettingsSaver(Store, this, this, Engine, Active);
        }

        public FakeStore Store { get; }

        public ActiveSettings Active { get; }

        public ShortcutDecisionEngine Engine { get; }

        public ISettingsSaver Saver { get; }

        public DrawGateStatus Gate { get; private set; }

        public List<string> Log { get; } = [];

        public Exception? DrawingFailure { get; set; }

        public string ActiveName => ReferenceEquals(Active.Current, SettingsSnapshot.Default) ? "old" : "new";

        public ShortcutDecision Down(InputKey key) => Apply(Engine.Handle(key, KeyDirection.Down, Gate));

        public ShortcutDecision Up(InputKey key) => Apply(Engine.Handle(key, KeyDirection.Up, Gate));

        public void StopSound() => Log.Add("stop-sound:" + ActiveName);

        public void ExitDrawingAndClearAllMonitors()
        {
            Log.Add("reset-drawing:" + ActiveName);
            Gate = new DrawGateStatus(IsActive: false, IsBlockedUntilReleased: false);
            if (DrawingFailure is not null)
            {
                throw DrawingFailure;
            }
        }

        private ShortcutDecision Apply(ShortcutDecision decision)
        {
            foreach (var command in decision.Commands)
            {
                Gate = command switch
                {
                    BeginDraw => Gate with { IsActive = true },
                    EndDraw => Gate with { IsActive = false },
                    ReleaseDrawBlock => Gate with { IsBlockedUntilReleased = false },
                    ClearMonitor => new DrawGateStatus(false, true),
                    _ => Gate,
                };
            }

            return decision;
        }
    }

    private sealed class FakeStore(Runtime runtime) : ISettingsStore
    {
        public SettingsStoreException? Failure { get; set; }

        public SettingsSnapshot? Saved { get; private set; }

        public SettingsLoadResult Load() => new SettingsLoadResult.Missing();

        public void Save(SettingsSnapshot snapshot)
        {
            runtime.Log.Add("persist:" + runtime.ActiveName);
            if (Failure is not null)
            {
                throw Failure;
            }

            Saved = snapshot;
        }
    }
}
