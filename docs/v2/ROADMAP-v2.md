# drawEM v2 roadmap: screen actions

**Status:** proposed; Steps 1 and 2 define the sound command, configuration, and global sound channel, while sound shortcuts remain unimplemented.

## Delivery order

1. [Sound playback through code, with global shortcuts configured in code](STAGE-1-SOUND-IMPLEMENTATION-PLAN.md).
2. Settings UI and media library — TODO.
3. Video — TODO.
4. Screen effects — TODO.

This order stages the proposed screen-action model in
[ARCHITECTURE.md](../ARCHITECTURE.md). Only the sound stage is planned in detail
below. The settings UI will later replace code-based sound configuration with
file import, shortcut assignment, and `Sample`. Video and effects do not need
to be designed or implemented for the sound stage.

## Stage 1: sound command

Implementation: [Stage 1 sound plan](STAGE-1-SOUND-IMPLEMENTATION-PLAN.md).

Use TDD in vertical slices. The expected behaviors are listed in
[SOUND-BUSINESS-TESTS.md](SOUND-BUSINESS-TESTS.md); add one failing automated
test at a time during implementation. The test boundaries in that document
are confirmed.

### Behavior and boundaries

- There is one global sound channel, independent of the active monitor and of
  the drawing channel. Every new sound command first stops the current sound
  action, then immediately starts the requested sound. Requesting the same
  sound again restarts it from the beginning.
- Each command plays once and stops at the earlier of the file's end or ten
  seconds after playback starts. Holding a shortcut does not retrigger it;
  releasing and pressing it again does.
- WAV and MP3 are the supported formats for this stage. Paths to external
  local files and assignments to `Ctrl+Alt+1` through `Ctrl+Alt+8` are set in
  code. There is no import, library, settings window, or `Sample` button yet.
- Outside draw mode, an assigned sound-slot keypress starts its sound and is
  suppressed before it reaches the focused application. An unassigned slot
  passes through. During draw mode, existing input suppression remains in
  force. On layouts where AltGr produces `Ctrl+Alt+digit`, assigning that slot
  can block the corresponding character; this impact is accepted for stage 1.
  Pressing AltGr with that digit starts the assigned sound instead of typing
  the character.
- `Ctrl+Alt+Z` continues to draw and `Ctrl+Alt+X` continues to clear. Sound
  shortcuts work during an active drawing stroke without stopping or changing
  that stroke. Existing input blocking for the application beneath the overlay
  remains in force while drawing.
- Moving the cursor between monitors does not stop a sound. A missing or
  unplayable file is logged locally and does not terminate drawEM or affect
  drawing. Exit stops playback and releases its resources.

### 1. [Define the command and code configuration](stage-1/step-1/task.md)

- Define a playback command that identifies a configured sound without
  depending on WPF, Win32 hooks, or the future settings UI.
- Define one code-owned mapping from the eight sound shortcut slots to
  external file paths. An unassigned slot has no sound command. Keep the
  drawing shortcuts fixed for this stage.
- Define a local log location in the current Windows user's profile and the
  information recorded for a failed playback request.

**Done when:** the mapping and playback command have one clear owner and can be
tested without real audio hardware or a settings window.

### 2. [Implement the global sound channel](stage-1/step-2/task.md)

- Add a playback adapter for WAV and MP3 and a controller that owns at most
  one active sound. For every request, stop the active sound action before
  immediately starting the requested one, including when both requests name
  the same sound.
- Enforce the ten-second cap in code for every start and restart. A shorter
  file ends naturally. Cancel an obsolete completion or timeout so it cannot
  stop a newer sound.
- Keep sound state separate from drawing and monitor state. Handle missing
  files and decode or device failures without crashing the tray application.
  Release the player on replacement, natural completion, timeout, and exit.

**Done when:** controller tests cover replacement, restart, completion,
ten-second timeout, stale callbacks, and recoverable failure using a fake
playback adapter.

### 3. [Connect the sound shortcuts](stage-1/step-3/task.md)

- Extend the existing global keyboard path to recognize the configured
  `Ctrl+Alt+1` through `Ctrl+Alt+8` commands on the first key-down of a press.
  Auto-repeat must not cause another start; a fresh press must. Suppress
  assigned slots and pass through unassigned slots outside draw mode.
- Route playback outside the keyboard-hook callback. Allow sound commands
  while draw mode is active, while keeping pointer and keyboard input blocked
  from the application beneath the overlay as before.
- Preserve `Ctrl+Alt+Z` hold/release drawing behavior and `Ctrl+Alt+X`
  monitor-scoped clearing.

**Done when:** shortcut tests cover first press, auto-repeat, release and
repress, assigned-slot suppression, unassigned-slot pass-through, and sound
invocation during an active stroke.

### 4. [Publish and verify](stage-1/step-4/task.md)

- Run the automated tests and publish the self-contained Windows x64 app.
- Manually play configured WAV and MP3 files through shortcuts while another
  app has focus. Verify audible output, stop-then-start replacement,
  same-sound restart, natural completion, and the ten-second cutoff.
- While drawing, start a sound and verify that drawing continues and the app
  beneath the overlay still receives no input. Move between monitors while a
  sound plays and verify that playback continues.
- Verify a bad path or invalid file is logged without stopping drawEM. Exit
  while audio plays and verify that playback stops. Repeat the v1 drawing
  smoke test and record manual results separately from unit-test results.

**Done when:** the published app passes the sound checks and the v1 drawing
checks on the tested Windows setup. Unit tests alone cannot establish audible
output or global input behavior.

## Stage 2: settings UI and media library — TODO

Replace code-based file paths and slot assignments with the architecture's
user-profile media library and settings UI. Plan file import and copying,
persistent assignments, reassignment of the ten predefined slots including
the drawing commands, and `Sample` here. Preserve the independent sound and
drawing channels. Detail this stage in its own plan before implementation.

## Stage 3: video — TODO

Plan the monitor-scoped video channel, its media import, shortcut and sample
behavior, and interaction with the separate global sound channel after the
settings UI exists.

## Stage 4: screen effects — TODO

Plan code-defined effects, their monitor-scoped channel, shortcut and sample
behavior, and interaction with drawing and video after video is planned.
