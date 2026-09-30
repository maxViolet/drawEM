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

### 1. Define settings and storage boundaries

Define one validated settings snapshot covering drawing style, two drawing
commands, eight action slots, and sound references. Define application-facing
ports for loading, saving, importing, and previewing media. Keep filesystem,
WPF, and Win32 types outside Domain and Application. Decide the on-disk schema
and migration/version policy before writing stored settings. Replace v2's
code-owned `SoundAssignments.Slots` with eight empty slots when no v3 settings
exist; there is no automatic migration of those code paths.

**Done when:** validation covers required/empty bindings, rejection of
single-modifier shortcuts, legal keys, Right Alt/AltGr, conflicts, color, and
width; storage tests cover first launch with empty slots, restart, corrupt
data, failed write, shared media references, crash-orphan cleanup, and media
preservation when settings cannot be read.

### 2. Route configurable shortcuts and drawing style

Replace fixed drawing shortcuts and in-code sound slot resolution with one
immutable in-memory binding snapshot used by the hook. Keep hook callbacks free
of file I/O. Add a hook-owned capture state that routes captured key events to
the settings draft through the composition root instead of resolving actions.
Keep suppression and pressed-key bookkeeping correct through focus changes,
cancel, chord completion, and release. Preserve hold/release drawing,
single-fire sound actions, input suppression, and monitor-scoped clear outside
capture. Add a narrow platform adapter for the neutral key injection; keep
injected events out of capture and action dispatch, and verify that injection
precedes modifier release for both normal actions and capture. Apply the chosen
color and physical-pixel width when a new stroke
starts. Convert that width to WPF units using the DPI of the stroke's monitor
for both line pens and single-point dots; the overlay's one
`TransformFromDevice` matrix is not sufficient evidence for mixed-DPI monitors.
Save resets drawing state and clears all monitors before the new snapshot
becomes usable.

**Done when:** automated tests cover remapping, conflict rejection, key
repeat/release, pass-through for Right Alt with Windows-reported Left Ctrl and
for physical `Ctrl+RightAlt`, Left Alt activation, cross-monitor behavior,
capture of an already-bound chord without dispatch, Tab/Alt+Tab/Win
pass-through, invalid-chord feedback and retry, cancel/blur, held-key transitions,
neutral-key ordering for suppressed `Alt+Shift` and `Ctrl+Shift` candidates in
action and capture modes, and no injection for bare layout-switch pairs,
physical line and dot width at 100%, 150%, and mixed monitor DPI, and saving
during an active stroke or sound. No test may infer global-hook or audible
behavior from fakes alone.

### 3. Add the managed sound library and settings window

Add Settings to the tray menu and the Drawing/Actions tabs. The window edits a
draft; it validates before Save, supports selecting and sampling a draft sound,
and shows file/copy failures without losing the active settings. Import and
deduplicate media safely, then retire unreferenced managed copies after a
successful configuration change. Wire all sound consumers to the same active
settings snapshot.

**Done when:** the UI can persist and reload every v3 setting; Cancel leaves
the active configuration intact; Restore defaults requires Save; file sharing,
replacement, and last-reference cleanup work without touching original files.

### 4. Publish and verify on Windows

Run the automated suite and publish the self-contained Windows x64 app. Record
manual results separately: tray Settings access, draft/Save/Cancel/defaults,
real keyboard combinations with another app focused, recording an already-bound
chord without playback or drawing, Tab, Alt+Tab, Alt+F4 and Win behavior while
capturing, invalid capture feedback for `Ctrl+C` and `Ctrl+RightAlt+1`, ordinary
one-modifier shortcuts such as `Ctrl+C` and `Alt+F4` remaining usable outside
capture, Right Alt/AltGr typing on German
(Germany) and Polish (Programmers) layouts, Left Alt bindings on those
layouts, drawing color and measured physical width of lines and dots across
DPI/monitors, all-monitor clear on Save, audible WAV/MP3 preview and shortcut
playback, replacement and cutoff, restart
persistence, corrupt settings recovery, startup orphan cleanup (including a
draft import left by forced exit), and media preservation with corrupt
settings. Enable the Windows `Alt+Shift` and `Ctrl+Shift` input-language hotkeys
and verify that bound actions and captured chords containing those pairs do not
switch layouts, while pressing either bare pair still does. Recheck existing
v1 drawing and v2 sound manual scenarios.

**Done when:** the published app passes the recorded desktop checks on the
tested Windows setup. Automated tests alone do not establish sound output,
global input behavior, or multi-monitor/DPI acceptance.
