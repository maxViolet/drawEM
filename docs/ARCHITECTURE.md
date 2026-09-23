# drawEM architecture

**Status:** accepted  
**Date:** 2026-09-16

## Purpose

drawEM is a small Windows 10/11 x64 utility that draws persistent 4 px
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
│   ├── Win32KeyboardHookAdapter
│   └── TrayAdapter
│
├── Application
│   └── DrawingSessionController
│
├── Domain
│   ├── DrawingState
│   ├── Stroke
│   └── ScreenPoint
│
└── Presentation
    └── OverlayWindow
        └── StrokeRenderElement (VisualCollection of DrawingVisual)
```

## Modules and responsibilities

| Module | Owns | Must not own |
| --- | --- | --- |
| `Domain` | points, strokes, active drawing state, colour and thickness rules | WPF types, Win32 calls, tray behavior |
| `Application` | drawing-session commands and immutable state publication | window handles, event callbacks, rendering |
| `Infrastructure` | low-level keyboard and mouse hooks, synchronous input gate, tray icon, process lifecycle integration | stroke storage or drawing rules |
| `Presentation` | transparent WPF overlay and rendering | the authoritative stroke list or global input |
| `App.xaml.cs` | object creation, startup and orderly shutdown | business logic |

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

### Exit

```text
Tray Exit
  → unregister keyboard and mouse hooks
  → close overlay
  → dispose tray icon
  → terminate application
```

If either global hook cannot be registered at startup, drawEM reports the error
and exits rather than leaving a partially working overlay.

## Threading

Win32 hook callbacks are not WPF UI callbacks. The hook adapters do no
rendering and no expensive work. Keyboard commands and mouse coordinates are
forwarded to the controller through the WPF `Dispatcher`; pointer-button
suppression is decided synchronously from the active drawing state. State
delivery to `OverlayWindow` is also marshalled to the WPF `Dispatcher`.

This keeps input responsive and prevents cross-thread access to WPF objects.

## Rendering and display coordinates

- The process uses Per-Monitor V2 DPI awareness.
- Domain coordinates are physical pixels in the virtual desktop coordinate
  space, so monitors with different scaling factors do not shift lines.
- `StrokeRenderElement` renders through a `VisualCollection` of small
  `DrawingVisual`s: completed strokes are drawn once and never reopened, and
  the active stroke only gets a new `DrawingVisual` for its newest segments
  each update, so redraw cost does not grow with total accumulated points
  (see `docs/steps/F03-render-lag-investigation-plan.md`). The app does not
  create a WPF `Polyline` control for every mouse movement.
- The overlay covers every connected monitor as one virtual desktop surface.
- Each stroke stores its starting monitor bounds. The renderer clips the whole
  stroke, including its thickness, to those bounds.

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

**Status:** under discussion; not implemented except for the monitor-scoped
drawing and clearing behavior above. This section describes a future extension
of the accepted architecture.

### Vocabulary

- **Action** is an option selectable in the UI: drawing, a specific video, a
  specific screen effect, or a specific sound. Drawing has two commands: draw
  while held and clear the drawing.
- **Command** is one operation of an action triggered by one shortcut. Drawing
  needs two distinct commands and two shortcuts.
- **Shortcut binding** maps one shortcut to one command; a shortcut never
  starts several commands at once.
- **Shortcut slot** is one of ten predefined key combinations. The UI assigns
  a command to a slot but cannot change the combination itself. Drawing uses
  two slots when both of its commands are assigned.
- **Sample** starts a selected sound, video, or effect from the UI without a
  shortcut.

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
current video; starting a new sound interrupts the current sound globally.
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

The ten predefined shortcut slots are `Ctrl+Alt+Z`, `Ctrl+Alt+X`, and
`Ctrl+Alt+1` through `Ctrl+Alt+8`. The UI assigns one command to each slot;
the combinations themselves are fixed. By default, `Ctrl+Alt+Z` draws and
`Ctrl+Alt+X` clears. Both bindings may be changed. The UI presents these as
one Drawing option with two assignable commands.

Video, sound, and effects start once on shortcut key-down; holding the keys
does not retrigger them. Users add sound and video files through the UI; the
app copies each imported file into its managed library. Effects are written in
code and shown in the UI for assignment. The `Sample` button starts the
selected sound, video, or effect without a shortcut. Sampled video and effects
cover the full active monitor, just as they do when started by a shortcut.

The media library and shortcut bindings are stored in the current Windows
user profile, separately from the executable directory.
