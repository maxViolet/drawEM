using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>
/// The draft behind the Settings window. It starts as a copy of the active snapshot, and nothing it does
/// changes the running configuration until <see cref="Save"/> succeeds. Imported sounds belong to the draft
/// until Save or <see cref="Cancel"/>. Call every member on the UI thread.
/// </summary>
public sealed class SettingsEditor
{
    private readonly Func<SettingsSnapshot> active;
    private readonly ISoundLibrary library;
    private readonly ISettingsSave saver;
    private readonly ISoundSampler sampler;
    private readonly Action<SoundCleanupFailure> reportCleanupFailure;
    private ActionSlot[] slots = [];
    private bool ended;

    /// <param name="active">The running configuration. Read now for the draft, and again by Cancel.</param>
    /// <param name="reportCleanupFailure">Receives each draft copy Cancel could not remove.</param>
    public SettingsEditor(
        Func<SettingsSnapshot> active,
        ISoundLibrary library,
        ISettingsSave saver,
        ISoundSampler sampler,
        Action<SoundCleanupFailure> reportCleanupFailure)
    {
        ArgumentNullException.ThrowIfNull(active);
        ArgumentNullException.ThrowIfNull(library);
        ArgumentNullException.ThrowIfNull(saver);
        ArgumentNullException.ThrowIfNull(sampler);
        ArgumentNullException.ThrowIfNull(reportCleanupFailure);
        this.active = active;
        this.library = library;
        this.saver = saver;
        this.sampler = sampler;
        this.reportCleanupFailure = reportCleanupFailure;
        Load(active());
    }

    public HexColor Color { get; private set; }

    /// <summary>The draft width in physical pixels. It may be outside 1–20; <see cref="Validate"/> reports that.</summary>
    public int Width { get; private set; }

    public Shortcut? DrawShortcut { get; private set; }

    public Shortcut? ClearShortcut { get; private set; }

    /// <summary>Slots 1–8, in order. A filled slot's shortcut may be missing in the draft.</summary>
    public IReadOnlyList<ActionSlot> Slots => slots;

    /// <summary>The shortcut a slot proposes when it is first filled: <c>Ctrl+Alt+</c> its number.</summary>
    public static Shortcut ProposedShortcut(int slot) =>
        Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Alt, ShortcutKey.Digit(slot));

    public void SetColor(HexColor color) => Color = color;

    public void SetWidth(int pixels) => Width = pixels;

    /// <summary>Draw and clear always have a shortcut; a slot has one only while it is filled.</summary>
    public bool AcceptsShortcut(SettingsCommand command) =>
        command.Kind != SettingsCommandKind.Slot || !slots[command.Slot - 1].IsEmpty;

    /// <summary>Changes only <paramref name="command"/>; a conflict is reported, never resolved by reassigning.</summary>
    /// <exception cref="InvalidOperationException">The command is an empty slot, which has no shortcut.</exception>
    public void SetShortcut(SettingsCommand command, Shortcut shortcut)
    {
        ArgumentNullException.ThrowIfNull(shortcut);
        if (!AcceptsShortcut(command))
        {
            throw new InvalidOperationException($"{command} has no action, so it cannot have a shortcut.");
        }

        switch (command.Kind)
        {
            case SettingsCommandKind.Draw:
                DrawShortcut = shortcut;
                break;
            case SettingsCommandKind.Clear:
                ClearShortcut = shortcut;
                break;
            default:
                Replace(new ActionSlot(command.Slot, slots[command.Slot - 1].Action! with { Shortcut = shortcut }));
                break;
        }
    }

    public Shortcut? ShortcutOf(SettingsCommand command) => command.Kind switch
    {
        SettingsCommandKind.Draw => DrawShortcut,
        SettingsCommandKind.Clear => ClearShortcut,
        _ => slots[command.Slot - 1].Action?.Shortcut,
    };

    /// <summary>
    /// Copies <paramref name="sourceFile"/> into the sound library and assigns it to the slot. A slot that
    /// was empty proposes <see cref="ProposedShortcut"/>; a filled slot keeps its shortcut.
    /// </summary>
    /// <exception cref="SoundImportException">The file is not a WAV or MP3, or the copy failed. The draft is unchanged.</exception>
    public void SelectSound(int slot, string sourceFile)
    {
        var shortcut = slots[slot - 1].Action?.Shortcut ?? ProposedShortcut(slot);
        Replace(new ActionSlot(slot, new SoundAction(library.Import(sourceFile), shortcut)));
    }

    /// <summary>Empties the slot, which also removes its shortcut.</summary>
    public void RemoveSound(int slot) => Replace(ActionSlot.Empty(slot));

    /// <summary>Replaces the draft with <see cref="SettingsSnapshot.Default"/>. Nothing runs differently until Save.</summary>
    public void RestoreDefaults() => Load(SettingsSnapshot.Default);

    /// <summary>Why the draft cannot be saved; empty when it can.</summary>
    public IReadOnlyList<SettingsError> Validate() =>
        SettingsSnapshot.Validate(Style(), DrawShortcut, ClearShortcut, slots).Errors;

    /// <summary>Plays the slot's draft sound. Does nothing for a slot without a sound.</summary>
    public void Sample(int slot, Action<string> reportFailure)
    {
        if (slots[slot - 1].Action is SoundAction { Sound: { } sound })
        {
            sampler.Sample(sound, reportFailure);
        }
    }

    /// <summary>
    /// Saves the draft. After <see cref="SettingsSaveResult.Saved"/> the draft has ended and Cancel does
    /// nothing. Save already stopped the sound channel, so the draft's samples are only forgotten: they no
    /// longer report failures, and a later draft does not stop a shortcut's sound on their behalf.
    /// </summary>
    public SettingsSaveResult Save()
    {
        var result = saver.Save(Style(), DrawShortcut, ClearShortcut, slots);
        if (result is SettingsSaveResult.Saved)
        {
            ended = true;
            sampler.ForgetSamples();
        }

        return result;
    }

    /// <summary>
    /// Ends the draft without saving: stops a sample that may still play, then removes the draft's imports
    /// the active settings do not reference. If that stop is not confirmed in time, a sample may still hold
    /// a copy open, so nothing is removed and startup cleanup removes the copies. Does nothing after a
    /// successful Save or an earlier Cancel.
    /// </summary>
    public void Cancel()
    {
        if (ended)
        {
            return;
        }

        ended = true;
        if (!sampler.StopSamples())
        {
            return;
        }

        foreach (var failure in library.DiscardDraft(active()))
        {
            reportCleanupFailure(failure);
        }
    }

    private void Load(SettingsSnapshot snapshot)
    {
        Color = snapshot.Style.Color;
        Width = snapshot.Style.Width.Pixels;
        DrawShortcut = snapshot.DrawShortcut;
        ClearShortcut = snapshot.ClearShortcut;
        slots = snapshot.Slots.ToArray();
    }

    private void Replace(ActionSlot slot) => slots[slot.Number - 1] = slot;

    private DrawingStyle Style() =>
        new(Color, StrokeWidth.IsValid(Width) ? new StrokeWidth(Width) : default);
}
