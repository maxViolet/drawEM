# v3 / Step 7: acceptance criteria

**Task:** [Settings window](task.md). **Plan:** [implementation](plan.md).

- [x] Tray Settings opens Drawing and Actions tabs and can edit/reload every
  v3 setting.
- [x] Cancel leaves active settings unchanged; Restore defaults affects only
  the draft until Save; failed Save preserves the active configuration.
- [x] Eight numbered slots show file names. A first selection proposes the
  slot's default shortcut, and a valid alternative can be saved.
- [x] Conflicts and invalid or missing bindings block Save with visible
  feedback. Capture uses the global hook without dispatching active actions.
- [x] `Sample` plays the draft file through the global sound channel without
  changing active shortcuts; copy and playback failures are visible.
- [x] Automated UI/application tests record their commands and results.
  Audible, tray, and real-key checks remain for [Step 8](../step-8-publish/task.md).

## Validation

- `dotnet build DrawEM.sln -c Release -warnaserror` and
  `dotnet test DrawEM.sln -c Release --no-build` (2026-10-05): 0 warnings;
  461 passed, 0 failed.
- Tray: `Infrastructure/TrayApplicationTests` checks that the tray Settings
  command opens Settings and is ignored after exit.
- Draft: `Application/Settings/SettingsEditorTests` uses the real sound
  library. It covers draft isolation from the active snapshot, Restore
  defaults, the `Ctrl+Alt+<slot>` proposal and a saved alternative, a
  conflicting proposal that blocks Save without reassigning, failed copies,
  one shared copy for two slots, Cancel removing only unreferenced draft
  imports, a failed Save keeping the draft, and reopen after Cancel and Save.
- Window state: `Presentation/Settings/SettingsViewModelTests` covers HEX,
  color picker, and width input; capture of valid, rejected, cancelled, and
  stale chords; duplicate conflicts shown on both fields; file selection,
  copy and sample failures; Restore defaults; a failed Save; and close
  with and without Save. One test captures an active `Ctrl+Alt+1` binding
  through the real `GlobalShortcutAdapter` without playing it.
- Sample: `Infrastructure/Sound/SoundSamplerTests` checks that a sample plays
  the managed copy on the sound channel and that only the latest sample's
  failure reaches the window, on the UI thread, including a failure queued
  before a newer sample started. Cancel stops a sample before it removes draft
  copies; when that stop is unconfirmed, it leaves them for startup cleanup.
- XAML: `Presentation/Settings/SettingsWindowTests` shows the real window
  off-screen and fails on any unresolved binding in either tab.
- Not verified here: tray clicks, real key capture, audible samples, whether
  `MediaPlayer` holds a sampled copy open until the stop on Cancel, and the
  file and color dialogs; see [Step 8](../step-8-publish/task.md).
