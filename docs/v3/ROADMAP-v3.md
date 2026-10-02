# drawEM v3 roadmap: drawing and action settings

**Status:** planned; no v3 settings are implemented.

## Goal and dependency

Replace code-owned drawing style, shortcuts, and sound file assignments with a
settings window opened from the tray. Keep the existing drawing behavior and
single global sound channel, except for the changes explicitly listed below.
The v2 [sound stage](../v2/ROADMAP-v2.md) supplies playback and the keyboard
path. Its Step 4 published-app manual checks remain open; v3 acceptance must
repeat them with saved settings. v2 Step 5 microphone routing is independent of
v3 settings and is not a prerequisite.

Only sound is an assignable action type in v3. The eight numbered action slots
must allow other types in a later version without redefining what a slot is.
Video, effects, and their channel behavior need a separate future roadmap.

## User-visible contract

- Open Settings from the tray menu. The window has Drawing and Actions tabs,
  with Save, Cancel, and Restore defaults controls. Edits are drafts until Save;
  Cancel leaves the active configuration unchanged. Restore defaults changes
  the draft and takes effect only after Save.
- Drawing has a color picker with HEX input (default OrangeRed `#FF4500`), an
  integer line width of 1–20 physical pixels (default 4), and separate required
  shortcuts for hold-to-draw and clear. Clear remains monitor-scoped when
  invoked by its shortcut. Physical-pixel width is a v3 rendering change:
  today `StrokeRenderElement` converts points to WPF coordinates but passes
  thickness unchanged as a DIP width (also for a single-point dot). At 150%
  scaling, the current default 4 DIPs renders about 6 physical pixels; v3's
  default 4 physical pixels will look thinner on that display.
- Actions shows eight numbered slots. In v3 a nonempty slot contains one WAV
  or MP3 sound and one required shortcut; no custom name or volume control is
  provided. Show its slot number and file name. A slot without a file has no
  active shortcut. When a file is first selected, propose `Ctrl+Alt+1` through
  `Ctrl+Alt+8` by slot number, but allow a different valid combination.
- `Sample` plays the file selected in the draft before Save, without changing
  active shortcut assignments. It uses the global sound channel and the
  existing ten-second cap. Normal playback retains stop-then-start replacement,
  same-sound restart, and the ten-second cap.
- A shortcut requires at least two modifiers from Ctrl, Alt, and Shift:
  `Ctrl+Alt`, `Ctrl+Shift`, `Alt+Shift`, or all three, plus exactly one letter,
  digit, or F1–F12 key. Single-modifier, Win, and modifier-only combinations
  are invalid. **V3 behavior change:** the current hook counts Right Alt as Alt,
  so an AltGr chord can trigger a configured `Ctrl+Alt` action (including draw
  on `AltGr+Z`). Treat Right Alt as reserved for AltGr: while Right Alt is down,
  do not dispatch or capture any shortcut, even if Windows also reports Left
  Ctrl. Let those keys pass through outside draw mode. A physical
  `Ctrl+RightAlt` chord is deliberately unsupported; use Left Alt for an Alt
  binding. Do not use `LLKHF_INJECTED` as an AltGr detector: that flag means
  input injected by a process, not the keyboard layout's Right Alt behavior
  ([Windows hook structure](https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct)).
  No two active commands may use the same combination; show conflicts and
  block Save rather than silently reassigning another command. Drawing and
  clear cannot be left unbound. A sound cannot be saved without a shortcut.
- When a bound `Alt+Shift` or `Ctrl+Shift` action suppresses its letter, digit,
  or F-key, Windows may see only the modifier pair and switch the input layout
  on release. While the modifiers are still held, emit one neutral key down/up
  (`VK 0xE8`, listed as unassigned by
  [Windows](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes))
  after suppressing the candidate and before the first modifier key-up. Apply
  the same rule when capture suppresses a candidate with either pair, including
  an invalid chord. Tag the two injected events and ignore only those tagged
  events in shortcut resolution and capture so they cannot trigger or record
  an action. Do not inject for a bare
  `Alt+Shift` or `Ctrl+Shift` press without a suppressed candidate; users must
  still be able to switch layouts intentionally. `Ctrl+Alt` needs no neutral
  key. Treat failure to prevent an unintended layout switch as a release
  blocker for these bindings; do not assume injection is reliable without the
  published-app manual check.
- Focusing a shortcut capture field enters capture mode in the global keyboard
  hook. The hook remains installed and sends key events to that field through
  the application composition root; it does not dispatch drawing, clear, or
  sound actions while capturing. Modifier key events pass through; suppress
  only the down/up of a candidate letter, digit, or F1–F12 key that is being
  evaluated for capture. This lets an existing binding such as `Ctrl+Alt+1`
  be recorded without playing its sound. Tab, Alt+Tab, Alt+F4, and chords
  containing Win pass through and are never captured; a candidate key also
  passes through when Right Alt is held. Wait for keys already held when capture
  starts to be released, preserving their prior key-up
  pass-through/suppression decision, then record one complete chord. A candidate
  with too few modifiers (for example `Ctrl+C`) displays an invalid-shortcut
  error; a Right Alt chord (for example `Ctrl+RightAlt+1`) also displays the
  reason but passes through. Neither changes the draft. Keep capture armed for
  a fresh chord after all keys of an invalid attempt are released. Escape is
  consumed to cancel capture; Tab or another focus change leaves it. After a valid chord
  is recorded, keep action dispatch paused until its keys are released; require
  a fresh press to trigger an action. Capture only changes the draft until Save.
- Save applies the complete validated configuration without restarting drawEM.
  It stops current sound, exits draw mode, and clears all drawn strokes on all
  monitors, including when the saved change is unrelated to drawing. A new
  press is required to draw or play a sound under the new configuration.
- Restore defaults sets the original draw and clear shortcuts (`Ctrl+Alt+Z`,
  `Ctrl+Alt+X`), default color and width, and eight empty action slots.

## Persistence and media rules

- Store settings and copied media in the current Windows user's profile, not
  next to the executable. Save settings durably before replacing the active
  configuration. A failed save leaves the previous configuration active and
  reports the failure.
- Import a selected WAV or MP3 by copying it into drawEM's managed library.
  The original file is never moved or deleted. Multiple slots may reference
  one managed copy. After a successful Save, remove a managed file only when no
  saved slot references it. Cancel discards draft imports; cleanup must never
  remove a copy still referenced by saved settings. On startup, after saved
  settings load successfully, delete managed library copies that no saved slot
  references; this also removes draft copies left by a crash or forced exit.
  If saved settings are unreadable, skip this cleanup so recovery cannot lose
  media referenced by the damaged file. Never delete a user's source file.
- If settings cannot be read at startup, retain the damaged file for recovery,
  show the error, and start with defaults. Do not overwrite the damaged file
  merely because defaults were loaded.
- v2 sound paths and assignments live in code, not user settings. Remove the
  `SoundAssignments.Slots` source and its startup wiring in v3. On first v3
  launch with no saved settings, all eight action slots are empty; do not
  import developer/test paths from v2. Later launches load only saved v3
  assignments.

## Delivery order

Each step has a [task](step-1-settings/task.md), implementation plan, and acceptance
criteria in its own directory. Complete automated checks during implementation;
record published-app desktop checks separately in Step 8. The contract above
governs every step.

1. [Define and persist the settings snapshot](step-1-settings/task.md): validation,
   schema/version policy, durable writes, startup fallback, and empty slots on
   first launch.
2. [Build the managed sound library](step-2-sound-library/task.md): copy and share WAV/MP3,
   discard draft imports, collect unreferenced copies, and preserve media when
   settings are unreadable.
3. [Route configurable actions](step-3-action-routing/task.md): one in-memory binding snapshot,
   drawing and sound dispatch, Right Alt pass-through, and existing suppression.
4. [Capture shortcuts and protect layout switching](step-4-shortcut-capture/task.md): hook-owned
   capture, invalid-chord retry, release bookkeeping, and neutral-key injection
   for suppressed `Alt+Shift` and `Ctrl+Shift` candidates.
5. [Apply configurable drawing style](step-5-drawing-style/task.md): color and physical-pixel
   width for lines and dots on the stroke's monitor, including mixed DPI.
6. [Apply saved settings at runtime](step-6-runtime-settings/task.md): retire v2 code assignments,
   connect playback to the active snapshot, stop sound, exit drawing, and clear
   every monitor before exposing the saved snapshot.
7. [Add the Settings window](step-7-settings-window/task.md): tray entry, Drawing and Actions
   drafts, Save/Cancel/defaults, file selection, errors, and draft `Sample`.
8. [Publish and verify on Windows](step-8-publish/task.md): automated suite, Windows x64
   publish, and recorded desktop acceptance including v1/v2 regressions.
