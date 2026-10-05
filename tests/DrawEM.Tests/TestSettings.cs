using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests;

internal static class TestSettings
{
    /// <summary>Default settings with each given slot bound to <c>Ctrl+Alt+</c> its number.</summary>
    public static SettingsSnapshot WithSounds(params (int Slot, SoundReference Sound)[] sounds) =>
        SettingsSnapshot.Validate(
            SettingsSnapshot.Default.Style,
            SettingsSnapshot.Default.DrawShortcut,
            SettingsSnapshot.Default.ClearShortcut,
            Enumerable.Range(1, ActionSlot.Count).Select(number =>
                sounds.FirstOrDefault(entry => entry.Slot == number) is { Sound: { } sound }
                    ? new ActionSlot(number, new SoundAction(sound, Shortcut.Create(
                        ShortcutModifiers.Control | ShortcutModifiers.Alt, ShortcutKey.Digit(number))))
                    : ActionSlot.Empty(number))).Snapshot!;
}
