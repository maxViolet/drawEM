# drawEM execution roadmap

## Goal

Deliver a small Windows 10/11 x64 tray application that draws an orange 4 px
annotation over the virtual desktop while `Ctrl+Alt+Z` is held, and clears all
annotations on `Ctrl+Alt+X`.

## Architecture at a glance

```text
Global shortcut source
        |
        v
Drawing session controller ----> Stroke store ----> WPF transparent overlay
        |                              |
        +---- clear command -----------+
        |
        +---- tray lifecycle
```

The domain/controller must not depend on WPF or Windows hooks. WPF, global
keyboard and mouse hooks, and the tray icon are adapters around it.

## Test seams

These are proposed public seams. Confirm them before writing tests or code.

| Seam | Public behavior tested | Not tested directly |
| --- | --- | --- |
| `DrawingSessionController` | starts/extends/ends a stroke and clears stored strokes | WPF controls and event handlers |
| `GlobalShortcutAdapter` | translates registered Windows shortcut events into controller commands | Win32 API calls themselves |
| `GlobalMouseInputAdapter` | forwards pointer movement and suppresses pointer buttons while drawing | Win32 API calls themselves |
| `OverlayWindowAdapter` | displays the controller's stroke state | private rendering helpers |
| `TrayApplication` | starts hidden and exits cleanly | individual menu/control implementation |

## Ordered work

### 0. Confirm seams and toolchain — 15 minutes

- Confirm the four public seams above before tests are written.
- Create a C# WPF solution targeting a supported Windows desktop .NET version.
- Add a unit-test project and make a single command run all tests.
- Define a manual smoke-test checklist for multi-monitor and tray behavior.

**Done when:** an empty solution builds and the test command succeeds.

### 1. TDD slice: stroke lifetime — 30 minutes

Write a failing business-level test through `DrawingSessionController`:

> Given no active stroke, when drawing starts at `(100, 200)`, moves to
> `(120, 215)`, and ends, then one orange 4 px stroke contains those points.

Implement only the immutable stroke model and controller behavior needed to
pass it. Add the next test for a second stroke; do not add UI yet.

**Done when:** controller stores separate completed strokes and tests pass.

### 2. TDD slice: clear all annotations — 15 minutes

Write the failing test:

> Given one or more completed strokes, when `Clear` is invoked, then no
> completed or active strokes remain.

Implement the minimal clear operation.

**Done when:** the behavior is covered by a passing test, including clearing
during an active drawing session.

### 3. TDD slice: rendering state — 30 minutes

Write a test at the overlay adapter seam using a fake view:

> Given the controller has strokes, when its state changes, then the overlay
> receives exactly the visible stroke collection.

Implement the WPF transparent, topmost window and bind it to the controller's
observable drawing state. It spans the virtual desktop, not merely the primary
monitor.

**Done when:** manually moving a test cursor path produces an orange 4 px line
on every monitor configuration available.

### 4. TDD slice: draw-mode input blocking — 45 minutes

Write tests through `GlobalMouseInputAdapter` with a fake mouse-hook source:

> Given draw mode is active, pointer movement becomes a stroke and pointer
> buttons are suppressed. Given draw mode is inactive, pointer buttons are not
> suppressed.

Implement a low-level Windows mouse hook. It reports physical pointer
coordinates to the controller and suppresses pointer-button messages during
draw mode. The overlay remains click-through and only renders state.

**Done when:** a manual test proves a click passes through normally, but is not
delivered to the underlying app while `Ctrl+Alt+Z` is held.

### 5. TDD slice: global shortcuts — 45 minutes

Write tests through `GlobalShortcutAdapter` with a fake registration service:

> `Ctrl+Alt+Z` pressed enters draw mode; released exits it.

> `Ctrl+Alt+X` invokes clear.

Implement Windows global keyboard handling. Treat auto-repeat as one active
drawing session, not multiple starts.

**Done when:** shortcuts work while another ordinary application owns focus.

### 6. Tray lifecycle and packaging — 30 minutes

Write a lifecycle test with a fake tray host:

> On launch, the app has no normal window and exposes `Exit`; invoking `Exit`
> unregisters shortcuts and closes the overlay.

Add the tray icon, exit command, self-contained x64 publish profile, and a
short smoke-test script.

**Done when:** published `.exe` launches, draws, clears, exits, and leaves no
background process.

### 7. Acceptance pass — 20 minutes

Run all automated tests and execute these business checks:

1. Hold `Ctrl+Alt+Z`, move the mouse, and see a persistent orange line.
2. While drawing, click a button underneath; its action must not execute.
3. Release `Ctrl+Alt+Z`; clicking underneath must work again.
4. Switch windows and monitors; existing strokes remain visible.
5. Press `Ctrl+Alt+X`; all strokes disappear.
6. Exit from the tray; overlay and shortcuts stop.

Record any untestable Win32 interaction as a manual verification result, not a
passing unit test.

## Risks and boundaries

- Applications launched with administrator privileges may not receive the
  shortcuts because drawEM remains non-elevated.
- Exclusive fullscreen games and protected surfaces may not allow an overlay or
  global input behavior.
- DPI and monitor hot-plug must be manually checked on available hardware.
- Shortcut configuration is intentionally deferred; do not introduce a settings
  model in this version.
