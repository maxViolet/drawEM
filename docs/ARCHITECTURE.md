# drawEM architecture

**Status:** accepted  
**Date:** 2026-09-16

## Purpose

drawEM is a small Windows 10/11 x64 utility that draws persistent 4 DIP
OrangeRed (#FF4500) annotations over all monitors while `Ctrl+Alt+Z` is held.
`Ctrl+Alt+X` clears annotations on the monitor under the cursor and exits draw
mode. It normally stays hidden in the system tray.

The architecture keeps drawing rules testable without WPF or Win32, while
isolating the operating-system-specific behavior needed for an overlay and
global input.

## Decision

Use a **modular monolith with ports and adapters inside one WPF project**.
Do not split the first version into separate assemblies and do not add a
dependency-injection container.

```text
App.xaml.cs  (composition root)
│
├── Infrastructure
│   ├── shared keyboard hook and tray lifecycle
│   ├── Drawing (mouse input, shared drawing/sound shortcut adapter, input gate)
│   └── Sound (code assignments, failure log, MediaPlayer adapter)
│
├── Application
│   ├── Drawing (DrawingSessionController)
│   └── Sound (play command, playback ports, SoundChannelController)
│
├── Domain
│   ├── Drawing (DrawingState, Stroke, ScreenPoint)
│   └── Sound (placeholder)
│
└── Presentation
    ├── Drawing (OverlayWindow, StrokeRenderElement)
    └── Sound (placeholder for the later settings UI)
```

## Modules and responsibilities

| Module | Owns | Must not own |
| --- | --- | --- |
| `Domain` | points, strokes, active drawing state, colour and thickness rules | WPF types, Win32 calls, tray behavior |
| `Application` | drawing-session commands and immutable state publication | window handles, event callbacks, rendering |
| `Infrastructure` | low-level keyboard and mouse hooks, synchronous input gate, tray icon, process lifecycle integration | stroke storage or drawing rules |
| `Presentation` | transparent WPF overlay and rendering | the authoritative stroke list or global input |
| `App.xaml.cs` | object creation, startup and orderly shutdown | business logic |

The four modules remain in one WPF project. Each has a `Drawing` and a `Sound`
folder. `Application/Sound` defines the play command, the playback ports, and the
global sound channel controller; `Infrastructure/Sound` defines code
assignments, failure logging, and WAV/MP3 playback through WPF `MediaPlayer`.
`Infrastructure/Drawing/GlobalShortcutAdapter` owns the shared pressed-key and
suppression state for drawing and sound shortcuts. It routes `Ctrl+Alt+1`
through `Ctrl+Alt+8` to the sound channel without changing the drawing gate.
The settings UI remains unimplemented. Shared
keyboard-hook and tray code stays at the `Infrastructure` root; the composition
root stays in `App.xaml.cs`.

`DrawingSessionController` is the only authoritative owner of the drawing
state. The overlay receives read-only snapshots and never modifies a stroke
list itself.

## Runtime flow

### Draw

```text
Ctrl+Alt+Z pressed
  → Win32KeyboardHookAdapter opens DrawingModeInputGate synchronously
  → DrawingSessionController starts a drawing session on the WPF Dispatcher
  → Win32MouseHookAdapter reads the gate and forwards physical pointer movement
  → DrawingSessionController appends ScreenPoint values to the active stroke
  → immutable DrawingState is published on the WPF Dispatcher
  → OverlayWindow redraws only the new points since the last update
```

`DrawingModeInputGate` is the shared, atomic decision point for keyboard and
mouse hooks. It changes synchronously in the keyboard callback, before the
controller action is queued on the WPF Dispatcher. The `Win32MouseHookAdapter`
forwards physical mouse movement to the controller only when this gate is open.
When `Ctrl+Alt+Z` is released, the gate closes and the controller completes the
active stroke. If the cursor crosses to another monitor first, the gate closes
synchronously and drawing stays blocked until the shortcut is released and
pressed again. The mouse hook suppresses pointer-button and wheel messages, and
the keyboard hook suppresses non-chord keys during draw mode, so clicks,
scrolling, and typed symbols do not reach the application underneath. The
overlay is always click-through and is not an input source.

### Clear

```text
Ctrl+Alt+X pressed
  → Win32KeyboardHookAdapter
  → resolve the monitor under the cursor
  → DrawingSessionController.ClearMonitorAndExitDrawMode(bounds)
  → DrawingState without strokes from that monitor
  → OverlayWindow redraws, retaining strokes on other monitors
```

### Play sound

```text
Assigned Ctrl+Alt+1 through Ctrl+Alt+8 chord becomes active
  → Win32KeyboardHookSource forwards the key event to GlobalShortcutAdapter
  → resolve the slot's command in memory and mark it started for this press
  → call SoundChannelHost.Play directly inside the hook callback
  → SoundChannelHost queues SoundChannelController.Play on the sound Dispatcher
  → stop the current player, then create and start the requested player
```

An unassigned slot queues nothing. Auto-repeat cannot start another command;
the digit must be released before it can start again. Playback work runs on
the sound thread, outside the hook callback. Sound does not wait for queued
drawing commands and may start before the UI processes the beginning of a
stroke; the drawing input gate has already changed synchronously.

`IKeyboardHookSource.KeySuppressionRequested` includes `KeyDirection`: it is
queried after `KeyDown` handlers and before `KeyUp` handlers. An assigned sound
digit's later key-downs are suppressed until release. Its key-up is suppressed
only if its initial key-down started the sound; a digit held before Ctrl+Alt
receives its matching key-up outside draw mode. Existing drawing suppression
still applies in both directions.

### Exit

```text
Tray Exit
  → unregister keyboard and mouse hooks
  → stop sound: release the player on the sound thread, then end that thread
  → drain the sound failure log (at most one second)
  → close overlay
  → dispose tray icon
  → drain the application failure log (at most one second)
  → terminate application
```

If either global hook cannot be registered at startup, drawEM reports the error
and exits rather than leaving a partially working overlay.

## Threading

Win32 keyboard and mouse hooks are installed on the WPF UI thread, where their
native callbacks run. The hook adapters do no rendering and no expensive work.
Drawing commands and mouse coordinates are forwarded to the drawing controller
through the WPF `Dispatcher`; sound commands are queued directly on the sound
Dispatcher through `SoundChannelHost.Play`. Pointer-button
suppression is decided synchronously from the active drawing state. State
delivery to `OverlayWindow` is also marshalled to the WPF `Dispatcher`.

This keeps input responsive and prevents cross-thread access to WPF objects.

### Sound thread

drawEM runs three kinds of thread:

| Thread | Owner | Runs |
| --- | --- | --- |
| WPF UI thread | `App` | keyboard/mouse hook callbacks (including direct sound enqueue), queued drawing commands, drawing controller, overlay |
| Sound thread (`drawEM sound`, STA, own `Dispatcher`) | `SoundChannelHost` | `SoundChannelController`, every WPF `MediaPlayer`, deadline callbacks |
| Log writer (thread-pool task) | `LoggingSoundFailureReporter` | file writes to `sound.log` |

WPF `MediaPlayer` can be stopped only on the thread that created it. The sound
thread therefore owns every player, and a busy UI thread cannot delay the
ten-second stop. The deadline timer fires on the thread pool and queues the
stop on the sound thread. Only sound work runs there, so the stop waits only
for earlier sound work. This removes UI delays; it does not prove an exact
cutoff. Step 4 measures the cutoff manually.

The shortcut path calls `SoundChannelHost.Play` directly from the hook callback,
without adding a UI Dispatcher queue. A busy UI thread can still delay the hook
callback itself, because the hook is installed on that thread. The dedicated
sound thread keeps an already playing sound's deadline independent of that delay.

`SoundChannelHost.Play` may be called from any thread and returns at once.
The controller reports a failure only after it has released the player, and
the reporter only queues the record. No file I/O runs on the sound thread.

Shutdown order:

1. `SoundChannelHost.Dispose` runs on a thread other than the sound thread;
   the sound thread rejects it, because it would wait for itself.
2. On the sound thread, the controller stops and releases the active player
   and cancels its deadline.
3. The host requests shutdown of the sound `Dispatcher` and joins the thread.
   Controller cleanup and thread termination share one two-second budget. The
   shutdown request runs even if cleanup throws or times out. A timeout raises
   `SoundChannelShutdownTimeoutException`; queued cleanup may finish later,
   but the caller does not wait any longer.
4. `LoggingSoundFailureReporter.Dispose` stops accepting records and waits at
   most one second for queued records, so a slow disk cannot hold exit.

`CleanupSteps` attempts the remaining cleanup after a sound timeout: drain the
sound log, close the overlay, and dispose the tray icon. `ApplicationExitPolicy`
records lifecycle failures in the background application log. At `OnExit`, it
drains that log for at most one second, then calls `Environment.Exit(1)` only
if a sound shutdown timeout was recorded. The sound thread is already a
background thread: it cannot keep the managed process alive once foreground
threads have ended ([.NET thread semantics](https://learn.microsoft.com/en-us/dotnet/standard/threading/foreground-and-background-threads)).
`Environment.Exit` is a deliberate timeout policy to leave
the still-running UI thread immediately after attempted cleanup and log drain,
without continuing WPF callbacks or waiting for a startup dialog. It is not
required merely because the sound thread is blocked. Ordinary cleanup failures
retain normal WPF shutdown. Process termination stops any remaining playback.
These are individual wait budgets, not a deadline for every possible UI or
native operation during application exit.

`AppFailureLog.Append` queues a timestamped record without file I/O on the
caller. A stuck writer cannot delay its disposal beyond one second; unwritten
records may be lost when the process exits.

`AppFailureLog` and `LoggingSoundFailureReporter` share `BackgroundLogWriter<T>`
for queueing and bounded drain. `TextFileLogSink` owns synchronized file append,
directory creation, and recoverable write failures for both log files. Each
log retains its own record format. App logging accepts either a file path or
an append delegate; supplying a delegate requires no unused path.

`TrayApplication` has one constructor: the composition root explicitly supplies
both exit scheduling and cleanup-failure reporting. There is no overload that
silently selects a different failure policy.

If the channel cannot be created at startup, the host requests sound-thread
shutdown with the same two-second budget. If that also fails, both the original
startup error and shutdown error are preserved in an aggregate. Startup cleanup
still attempts the remaining resources. A sound timeout exits after log drain
without waiting for a modal error dialog; ordinary startup failures show their
error before normal shutdown.

## Rendering and display coordinates

- The process uses Per-Monitor V2 DPI awareness.
- Domain coordinates are physical pixels in the virtual desktop coordinate
  space, so monitors with different scaling factors do not shift lines.
- Current `StrokeRenderElement` converts stroke points to WPF coordinates but
  passes the numeric thickness unchanged to the WPF pen and dot radius. The
  current 4-unit default is therefore rendered as 4 DIPs, not a guaranteed
  4 physical pixels; at 150% scaling it is about 6 physical pixels wide.
- `StrokeRenderElement` renders through a `VisualCollection` of small
  `DrawingVisual`s: completed strokes are drawn once and never reopened, and
  the active stroke only gets a new `DrawingVisual` for its newest segments
  each update, so redraw cost does not grow with total accumulated points
  (see `v1/steps/F03-render-lag-investigation-plan.md`). The app does not
  create a WPF `Polyline` control for every mouse movement.
- The overlay covers every connected monitor as one virtual desktop surface.
- Each stroke stores its starting monitor bounds. The renderer clips the whole
  stroke, including its thickness, to those bounds.

## Layer dependency rules

`tests/DrawEM.Tests/Architecture/LayerDependencyTests.cs` checks the compiled
application assembly through NetArchTest. The rules run with the normal xUnit
suite; run only these checks with `dotnet test DrawEM.sln --filter Category=Architecture`.

| Layer | Allowed project dependencies |
| --- | --- |
| `Domain` | `Domain` |
| `Application` | `Domain`, `Application` |
| `Infrastructure` | `Domain`, `Application`, `Infrastructure` |
| `Presentation` | `Domain`, `Application`, `Presentation`, the existing `Infrastructure.IOverlayLifetime` port |

The `IOverlayLifetime` exception preserves `OverlayWindow`'s tray-exit contract;
it does not permit dependencies on other Infrastructure types. `App.xaml.cs`
is outside the four layer namespaces and composes all layers.

Framework dependencies under `System` and `Microsoft` are allowed, but
`Domain` and `Application` must not depend on Windows UI/platform namespaces
or declare native imports (`DllImport` or `LibraryImport`). They also must not
reference `NativeLibrary` or call `Marshal.GetDelegateForFunctionPointer` or
`Marshal.GetFunctionPointerForDelegate`, including inside compiler-generated
closures and state machines. This prevents dynamic native calls from bypassing
the import-declaration checks. Managed helpers
such as `CollectionsMarshal` remain allowed. Failures name the violating
types or native methods. These checks enforce static dependencies; resource
lifetime, thread behavior, and dependencies loaded by name at runtime require
separate tests or review.

## Test seams

| Seam | Tests verify | Tests do not verify |
| --- | --- | --- |
| `DrawingSessionController` | stroke lifetime, point collection, clear behavior | WPF controls or private collections |
| `Win32KeyboardHookAdapter` | shortcut events become controller commands | the OS hook implementation itself |
| `Win32MouseHookAdapter` | pointer movement becomes controller input; pointer buttons and wheel input are suppressed only while drawing | the OS hook implementation itself |
| `OverlayWindow` adapter | state is rendered | pixel-perfect WPF internals |
| `TrayAdapter` | startup is hidden and `Exit` releases resources | individual menu/control implementation |

TDD proceeds in vertical slices: one behavior-level failing test, the smallest
implementation that makes it pass, then the next behavior.

## Alternatives rejected

| Alternative | Reason not chosen now |
| --- | --- |
| Separate Clean Architecture assemblies | More project references and wiring than this small app needs. |
| WPF code-behind as the source of truth | Drawing rules would become coupled to UI and harder to test. |
| DI container | Four modules can be wired transparently in `App.xaml.cs`. |
| `RegisterHotKey` only | It does not reliably express the required key-hold/release lifecycle. |
| Direct2D | Extra interop complexity without a first-version performance need. |

## Known boundaries

- drawEM does not run elevated; behavior over elevated applications is not
  guaranteed.
- Exclusive fullscreen games and protected surfaces can prevent expected
  overlay or input behavior.
- Shortcut customization, colour controls, undo, export and autostart are out
  of scope for the first version.

## Differences from the first version

- A stroke formerly continued across monitor boundaries while the shortcut
  remained held. It is now confined to the monitor where drawing started;
  crossing the boundary ends it, and drawing resumes only after release and
  another press. The stroke thickness is clipped at the monitor edge.
- `Ctrl+Alt+X` formerly cleared strokes on every monitor. It now clears only
  the monitor under the cursor, preserving drawings on other monitors.

## Proposed evolution: screen actions

**Status:** proposed evolution. Monitor-scoped drawing/clearing and the v2
global sound channel are implemented as described above. The v3 settings
behavior below is planned in [the v3 roadmap](v3/ROADMAP-v3.md); video and
effects remain future work.

### Vocabulary

- **Action** is an option selectable in the UI: drawing, a specific video, a
  specific screen effect, or a specific sound. Drawing has two commands: draw
  while held and clear the drawing.
- **Command** is one operation of an action triggered by one shortcut. Drawing
  needs two distinct commands and two shortcuts.
- **Action slot** is one of eight numbered positions for an assignable action.
  In v3 only a sound can occupy one; later versions may add other action types.
  Each occupied slot has one editable shortcut. Drawing and clearing have two
  separate required editable shortcuts outside the eight action slots.
- **Shortcut binding** maps one valid key combination to one command; a
  combination never starts several commands at once.
- **Sample** starts a selected action from the UI without a shortcut. In v3
  only sound can be sampled.

### Screen mode

```text
GLOBAL_SCREEN_MODE = SINGLE_SCREEN
MULTIPLE_SCREEN = TODO
```

The rules below apply only to `SINGLE_SCREEN`. `MULTIPLE_SCREEN` behavior,
including cursor movement between monitors, remains to be designed.

In `SINGLE_SCREEN`, the active screen is the monitor containing the cursor,
regardless of the focused window. Moving the cursor to another monitor
interrupts the active drawing, video, and effect on the previous monitor, but
does not interrupt the global sound channel. Existing drawings on other
monitors remain visible. A new screen action starts on the monitor containing
the cursor when its shortcut is pressed.

### Channels

Each monitor has one drawing channel, one video channel, and one effect channel.
The application has one global sound channel, independent of any monitor.
Each channel has at most one active action. Starting a new video interrupts the
current video. Every new sound command first stops the current sound action
globally, then immediately starts the requested sound. Requesting the same
sound restarts it from the beginning.
Embedded video audio and the separate sound channel may play at the same time.
Stopping a video also stops its embedded audio, without stopping the global
sound channel. An effect has its own channel and may appear over video.

Drawing is active while its assigned shortcut is held, but each stroke belongs
to the monitor containing the cursor at the start of that stroke. Stroke
rendering is clipped to that monitor's bounds. Crossing to another monitor
interrupts drawing; the shortcut must be released and pressed again to start a
new stroke. Clear affects only the monitor containing the cursor and preserves
drawings on other monitors.

### Shortcuts, media, and UI

The v3 UI has separate Drawing and Actions tabs, opened from the tray. Drawing
has two required editable bindings: draw while held and clear. Their defaults
are `Ctrl+Alt+Z` and `Ctrl+Alt+X`. The eight action slots initially support
sound only; selecting a file proposes `Ctrl+Alt+1` through `Ctrl+Alt+8` by
slot number, but the combination can be changed. An empty slot has no active
binding. A binding requires at least two of Ctrl, Alt, and Shift (`Ctrl+Alt`,
`Ctrl+Shift`, `Alt+Shift`, or all three), plus one letter, digit, or F1–F12 key.
Single-modifier and Win combinations are excluded. This is a v3 behavior
change: the current hook counts Right Alt as Alt, so AltGr can trigger a
`Ctrl+Alt` action. Right Alt is reserved for AltGr in v3: while it is held,
no action is dispatched or captured, even if Windows also reports Left Ctrl.
Its input passes through outside draw mode. Physical
`Ctrl+RightAlt` is unsupported; Alt bindings use Left Alt. Duplicate active
combinations block saving. Drawing and clearing cannot be unbound; an
occupied action slot also requires a binding.

`Alt+Shift` and `Ctrl+Shift` may be Windows input-language hotkeys. When the
hook suppresses a bound or captured candidate key with either modifier pair,
it injects a neutral `VK 0xE8` down/up while the modifiers remain held and
before their first key-up, intending to prevent a bare layout-switch gesture
on release. The hook tags and ignores only its own injected events. A bare pair
without a suppressed candidate is untouched and can still switch layouts.
This behavior must be verified on the published app with both Windows layout
hotkeys enabled; if layout switching still occurs, these bindings are not
ready for release.

When a shortcut capture field has focus, the global keyboard hook stays
installed but enters capture mode. It routes key activity to the settings draft
through the composition root rather than resolving or dispatching drawing,
clear, or sound commands. Modifier events pass through; only the down/up of a
candidate letter, digit, or F1–F12 key evaluated for capture and Escape used to
cancel it are suppressed.
Tab, Alt+Tab, Alt+F4, and Win chords pass through and are not captured. A
candidate key also passes through with Right Alt. Capture waits for any keys
held on entry to be released, retaining their prior key-up
pass-through/suppression decision, then records one chord. An invalid chord
displays the reason without changing the draft; after its keys are released,
capture waits for a new attempt. Escape cancels capture, and Tab or loss of
field focus leaves it. After a valid recording, action dispatch remains
paused until the captured keys are released, and a fresh press is required for
an action. The hook keeps its pressed-key and
suppression state consistent across entry, cancellation, and exit. Other
settings controls use ordinary keyboard input when no capture field has focus.

Drawing color and physical-pixel width become settings, defaulting to OrangeRed
and 4 physical pixels. This changes high-DPI rendering: lines and single-point
dots will be thinner than the current 4-DIP default at scaling above 100%.
The renderer must convert width for the stroke's monitor DPI rather than reuse
one overlay-wide DPI matrix; v3 tests and manual checks must cover line and dot
width on mixed-DPI monitors. Saving settings stops sound, exits draw mode, and
clears strokes on all monitors before new bindings become active. Shortcut
clear remains scoped to the monitor under the cursor. The settings snapshot is
persisted in the current Windows user's profile; a damaged settings file is
retained while the app starts with defaults.

Video, sound, and effects start once on shortcut key-down; holding the keys
does not retrigger them. In v3 users add WAV and MP3 files through the UI; the
app copies each imported file into its managed library, and removes a copy
after the last slot reference is removed and saved. The `Sample` button plays
the selected draft sound without saving its assignment. Future versions may
add video import, code-defined effects, and sampling for those action types.
Sampled video and effects would cover the full active monitor, as they would
when started by a shortcut.

The media library and shortcut bindings are stored in the current Windows
user profile, separately from the executable directory. V3 removes the v2
`SoundAssignments.Slots` code mapping and starts with eight empty action slots
when no saved v3 configuration exists; v2 code paths are not migrated. On
startup, after settings load successfully, remove managed copies not referenced
by any saved slot, including copies left by an unsaved draft after a crash. If
settings cannot be read, retain the damaged file and skip media cleanup to
preserve recovery options.
