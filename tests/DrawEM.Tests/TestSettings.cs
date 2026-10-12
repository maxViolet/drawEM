using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;

namespace DrawEM.Tests;

internal static class TestSettings
{
    /// <summary>Default settings with each given slot bound to <c>Ctrl+Alt+</c> its number.</summary>
    public static SettingsSnapshot WithSounds(params (int Slot, SoundReference Sound)[] sounds) =>
        WithSlots((number, shortcut) => sounds.FirstOrDefault(entry => entry.Slot == number) is { Sound: { } sound }
            ? new SoundAction(sound, shortcut)
            : null);

    /// <summary>Default settings with each given slot bound to <c>Ctrl+Alt+</c> its number.</summary>
    public static SettingsSnapshot WithEffects(params (int Slot, EffectId Effect)[] effects) =>
        WithSlots((number, shortcut) => effects.FirstOrDefault(entry => entry.Slot == number) is { Effect: { } effect }
            ? new EffectAction(effect, shortcut)
            : null);

    private static SettingsSnapshot WithSlots(Func<int, Shortcut, SlotAction?> actionFor) =>
        SettingsSnapshot.Validate(
            SettingsSnapshot.Default.Style,
            SettingsSnapshot.Default.DrawShortcut,
            SettingsSnapshot.Default.ClearShortcut,
            Enumerable.Range(1, ActionSlot.Count).Select(number =>
                actionFor(number, Shortcut.Create(ShortcutModifiers.Control | ShortcutModifiers.Alt, ShortcutKey.Digit(number))) is { } action
                    ? new ActionSlot(number, action)
                    : ActionSlot.Empty(number))).Snapshot!;
}
