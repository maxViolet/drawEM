using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Input;

/// <summary>Drawing input-gate state that the mouse path can also change.</summary>
public readonly record struct DrawGateStatus(bool IsActive, bool IsBlockedUntilReleased);

/// <summary>A side effect the hook adapter must perform for one key event, in order.</summary>
public abstract record ShortcutCommand
{
    private ShortcutCommand()
    {
    }

    /// <summary>Start drawing at the cursor's monitor, if the cursor and monitor can be found.</summary>
    public sealed record BeginDraw : ShortcutCommand;

    /// <summary>Deactivate the gate and exit draw mode.</summary>
    public sealed record EndDraw : ShortcutCommand;

    /// <summary>Allow drawing to begin again after a boundary stop or clear.</summary>
    public sealed record ReleaseDrawBlock : ShortcutCommand;

    /// <summary>Deactivate and block the gate, then clear the monitor under the cursor.</summary>
    public sealed record ClearMonitor : ShortcutCommand;

    /// <summary>Play the sound assigned to <paramref name="Slot"/>.</summary>
    public sealed record PlaySlot(int Slot) : ShortcutCommand;
}

/// <summary>
/// The answer for one key event. <see cref="Suppress"/> is needed before the hook callback returns;
/// <see cref="Commands"/> must be performed by the caller in order, without blocking.
/// </summary>
public sealed record ShortcutDecision(bool Suppress, IReadOnlyList<ShortcutCommand> Commands);

/// <summary>
/// Synchronous shortcut state machine. Tracks pressed keys, resolves drawing, clear, and sound-slot
/// chords, and decides suppression for each platform-neutral key event. Performs no I/O and knows no
/// platform key codes, so it runs inside the keyboard hook callback.
/// </summary>
/// <remarks>
/// Current chords are v2's fixed <c>Ctrl+Alt+Z</c> (hold to draw), <c>Ctrl+Alt+X</c> (clear), and
/// <c>Ctrl+Alt+1</c>–<c>8</c> (sound slots). Either Ctrl and either Alt count. Call from one thread.
/// </remarks>
public sealed class ShortcutDecisionEngine
{
    private static readonly ShortcutKey DrawKey = ShortcutKey.Letter('Z');
    private static readonly ShortcutKey ClearKey = ShortcutKey.Letter('X');
    private static readonly ShortcutCommand[] NoCommands = [];

    private readonly Func<int, bool> isSlotAssigned;
    private readonly HashSet<InputKey> pressedKeys = [];
    private readonly HashSet<InputKey> startedSoundKeys = [];
    private readonly HashSet<InputKey> soundKeysSuppressedOnPress = [];
    private readonly HashSet<InputKey> staleRepeatsSuppressed = [];
    private readonly HashSet<InputKey> firstDownSuppressed = [];
    private readonly HashSet<InputKey> staleKeys = [];
    private bool clearShortcutActive;

    /// <param name="isSlotAssigned">
    /// Whether slot 1–8 has a sound. Runs inside the hook callback, so it must only look up the assignment.
    /// </param>
    public ShortcutDecisionEngine(Func<int, bool> isSlotAssigned) => this.isSlotAssigned = isSlotAssigned;

    /// <summary>
    /// Updates key state and returns the decision for one event. A key-down decides suppression after its
    /// commands take effect; a key-up decides it from the state before release.
    /// </summary>
    /// <param name="gate">The gate's state when the event arrives.</param>
    public ShortcutDecision Handle(InputKey key, KeyDirection direction, DrawGateStatus gate)
    {
        if (staleKeys.Contains(key))
        {
            return HandleStale(key, direction);
        }

        List<ShortcutCommand>? commands = null;
        bool suppress;
        if (direction == KeyDirection.Down)
        {
            var firstKeyDown = pressedKeys.Add(key);
            UpdateChords(ref gate, ref commands);
            UpdateSoundShortcuts(firstKeyDown ? key : null, ref commands);
            suppress = ShouldSuppress(key, direction, gate);
            if (firstKeyDown)
            {
                Track(firstDownSuppressed, key, suppress);
            }
        }
        else
        {
            suppress = ShouldSuppress(key, direction, gate);
            pressedKeys.Remove(key);
            startedSoundKeys.Remove(key);
            soundKeysSuppressedOnPress.Remove(key);
            firstDownSuppressed.Remove(key);
            UpdateChords(ref gate, ref commands);
        }

        return new ShortcutDecision(suppress, commands ?? (IReadOnlyList<ShortcutCommand>)NoCommands);
    }

    /// <summary>
    /// Makes every held non-modifier key stale: it triggers nothing until released and pressed again.
    /// Its release is suppressed exactly when its first key-down was, so the focused application never
    /// sees half a press. Its repeats stay suppressed when the first key-down was, or when the key had
    /// started a sound; otherwise they pass. Held modifiers still count toward new chords. Clears active
    /// chord state. The caller has already stopped sound and closed the drawing gate.
    /// </summary>
    public void RequireFreshPress()
    {
        foreach (var key in pressedKeys.Where(key => key.Kind != InputKeyKind.Modifier).ToArray())
        {
            pressedKeys.Remove(key);
            staleKeys.Add(key);
            if (firstDownSuppressed.Contains(key) || startedSoundKeys.Contains(key))
            {
                staleRepeatsSuppressed.Add(key);
            }
        }

        startedSoundKeys.Clear();
        soundKeysSuppressedOnPress.Clear();
        clearShortcutActive = false;
    }

    private ShortcutDecision HandleStale(InputKey key, KeyDirection direction)
    {
        if (direction == KeyDirection.Down)
        {
            return new ShortcutDecision(staleRepeatsSuppressed.Contains(key), NoCommands);
        }

        var suppress = firstDownSuppressed.Contains(key);
        staleKeys.Remove(key);
        staleRepeatsSuppressed.Remove(key);
        firstDownSuppressed.Remove(key);
        return new ShortcutDecision(suppress, NoCommands);
    }

    private static void Track(HashSet<InputKey> set, InputKey key, bool suppressed)
    {
        if (suppressed)
        {
            set.Add(key);
        }
        else
        {
            set.Remove(key);
        }
    }

    /// <summary>
    /// Starts an assigned slot once per press: when its key and Ctrl+Alt are first down together.
    /// Auto-repeat does nothing; the slot key must be released before it can start again.
    /// </summary>
    /// <remarks>
    /// A started slot key's later key-downs are suppressed until release. Its key-up is suppressed only
    /// when its own first key-down started the sound. If the chord forms while the key is already held,
    /// the application saw that key-down, so it also receives the matching key-up.
    /// </remarks>
    private void UpdateSoundShortcuts(InputKey? firstKeyDown, ref List<ShortcutCommand>? commands)
    {
        if (!IsCtrlDown() || !IsAltDown())
        {
            return;
        }

        for (var slot = 1; slot <= ActionSlot.Count; slot++)
        {
            var key = InputKey.Candidate(ShortcutKey.Digit(slot));
            if (!pressedKeys.Contains(key) || startedSoundKeys.Contains(key) || !isSlotAssigned(slot))
            {
                continue;
            }

            startedSoundKeys.Add(key);
            if (key == firstKeyDown)
            {
                soundKeysSuppressedOnPress.Add(key);
            }

            Add(ref commands, new ShortcutCommand.PlaySlot(slot));
        }
    }

    /// <summary>
    /// Emits gate commands and applies their expected effect to <paramref name="gate"/>, so the same event's
    /// suppression sees the gate as the caller will leave it.
    /// </summary>
    private void UpdateChords(ref DrawGateStatus gate, ref List<ShortcutCommand>? commands)
    {
        var modifiersDown = IsCtrlDown() && IsAltDown();
        var drawShortcutDown = modifiersDown && IsCandidateDown(DrawKey);
        var clearShortcutDown = modifiersDown && IsCandidateDown(ClearKey);

        if (drawShortcutDown && !gate.IsActive && !gate.IsBlockedUntilReleased)
        {
            Add(ref commands, new ShortcutCommand.BeginDraw());
            gate = gate with { IsActive = true };
        }
        else if (!drawShortcutDown && gate.IsActive)
        {
            Add(ref commands, new ShortcutCommand.EndDraw());
            gate = gate with { IsActive = false };
        }

        if (!drawShortcutDown && !clearShortcutDown && gate.IsBlockedUntilReleased)
        {
            Add(ref commands, new ShortcutCommand.ReleaseDrawBlock());
            gate = gate with { IsBlockedUntilReleased = false };
        }

        if (clearShortcutDown && !clearShortcutActive)
        {
            clearShortcutActive = true;
            Add(ref commands, new ShortcutCommand.ClearMonitor());
            gate = new DrawGateStatus(IsActive: false, IsBlockedUntilReleased: true);
        }
        else if (!clearShortcutDown)
        {
            clearShortcutActive = false;
        }
    }

    private bool ShouldSuppress(InputKey key, KeyDirection direction, DrawGateStatus gate) =>
        (gate.IsActive && !IsDrawChordKey(key)) ||
        (clearShortcutActive && key == InputKey.Candidate(ClearKey)) ||
        (direction == KeyDirection.Down && startedSoundKeys.Contains(key)) ||
        soundKeysSuppressedOnPress.Contains(key);

    private static bool IsDrawChordKey(InputKey key) =>
        key == InputKey.Candidate(DrawKey) ||
        (key.Kind == InputKeyKind.Modifier && key.ModifierKey is
            ModifierKey.LeftControl or ModifierKey.RightControl or ModifierKey.LeftAlt or ModifierKey.RightAlt);

    private bool IsCandidateDown(ShortcutKey key) => pressedKeys.Contains(InputKey.Candidate(key));

    private bool IsCtrlDown() =>
        pressedKeys.Contains(InputKey.Modifier(ModifierKey.LeftControl)) ||
        pressedKeys.Contains(InputKey.Modifier(ModifierKey.RightControl));

    private bool IsAltDown() =>
        pressedKeys.Contains(InputKey.Modifier(ModifierKey.LeftAlt)) ||
        pressedKeys.Contains(InputKey.Modifier(ModifierKey.RightAlt));

    private static void Add(ref List<ShortcutCommand>? commands, ShortcutCommand command) =>
        (commands ??= []).Add(command);
}
