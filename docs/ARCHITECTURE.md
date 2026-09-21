# drawEM architecture

**Status:** accepted  
**Date:** 2026-09-16

## Purpose

drawEM is a small Windows 10/11 x64 utility that draws persistent orange 4 px
annotations over all monitors while `Ctrl+Alt+Z` is held. `Ctrl+Alt+X` clears
every annotation. It normally stays hidden in the system tray.

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
        └── DrawingVisual / DrawingContext
```

## Modules and responsibilities

| Module | Owns | Must not own |
| --- | --- | --- |
| `Domain` | points, strokes, active drawing state, colour and thickness rules | WPF types, Win32 calls, tray behavior |
| `Application` | drawing-session commands and immutable state publication | window handles, event callbacks, rendering |
| `Infrastructure` | low-level keyboard and mouse hooks, tray icon, process lifecycle integration | stroke storage or drawing rules |
| `Presentation` | transparent WPF overlay and rendering | the authoritative stroke list or global input |
| `App.xaml.cs` | object creation, startup and orderly shutdown | business logic |

`DrawingSessionController` is the only authoritative owner of the drawing
state. The overlay receives read-only snapshots and never modifies a stroke
list itself.

## Runtime flow

### Draw

```text
Ctrl+Alt+Z pressed
  → Win32KeyboardHookAdapter starts a drawing session
  → Win32MouseHookAdapter forwards physical pointer movement
  → DrawingSessionController appends ScreenPoint values to the active stroke
  → immutable DrawingState is published on the WPF Dispatcher
  → OverlayWindow redraws the current state through one DrawingVisual
```

The `Win32MouseHookAdapter` forwards physical mouse movement to the controller.
When `Ctrl+Alt+Z` is released, the controller completes the active stroke. The
mouse hook suppresses pointer-button messages and the keyboard hook suppresses
non-chord keys during draw mode, so clicks and typed symbols do not reach the
application underneath. The overlay is always click-through.

### Clear

```text
Ctrl+Alt+X pressed
  → Win32KeyboardHookAdapter
  → DrawingSessionController.Clear()
  → empty DrawingState
  → OverlayWindow redraws with no strokes
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
- A single `DrawingVisual`/`DrawingContext` renders the overlay. The app does
  not create a WPF `Polyline` control for every mouse movement.
- The overlay covers every connected monitor as one virtual desktop surface.

## Test seams

| Seam | Tests verify | Tests do not verify |
| --- | --- | --- |
| `DrawingSessionController` | stroke lifetime, point collection, clear behavior | WPF controls or private collections |
| `Win32KeyboardHookAdapter` | shortcut events become controller commands | the OS hook implementation itself |
| `Win32MouseHookAdapter` | pointer movement becomes controller input and pointer buttons are suppressed only while drawing | the OS hook implementation itself |
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
