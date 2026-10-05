# FIX-006: keep drawing active at a single screen edge

**Status:** implemented; two-monitor check pending. **Package:** [v3 additional fixes](../README.md).

## Current behavior

Reported on a laptop with one connected screen: while the draw shortcut is
held and a stroke reaches a physical edge of the screen, drawing stops. Moving
the pointer back toward the screen center while still holding the shortcut
does not continue the stroke; drawing requires a release and a new press.

The [screen-mode contract](../../../ARCHITECTURE.md#screen-mode) runs in
`SINGLE_SCREEN` mode: one active monitor at a time, whatever the number of
connected monitors. Its interruption rule covers moving the cursor to another
monitor, not reaching an outside edge of the desktop. The input gate and the
drawing controller instead end drawing when a pointer coordinate falls outside
the starting monitor's bounds, without checking whether that coordinate lies
on another monitor:

- `DrawingModeInputGate.StopAtBoundary` closes the gate when
  `MonitorBounds.Contains` is false.
- `DrawingSessionController` exits draw mode on the same condition.

The likely cause is that the low-level mouse hook reports coordinates past the
desktop edge. A desktop reproduction has not yet confirmed it.

## Required behavior

An outside edge is a monitor edge with no adjacent monitor. This includes
every edge of a single connected screen and the outer edges of a
multi-monitor desktop.

In `SINGLE_SCREEN` mode, with any number of connected monitors, touching or
pushing the pointer past an outside edge must not end draw mode or complete
the active stroke. While the draw shortcut stays held, moving the pointer back
inward continues the same stroke without a new shortcut press. A point outside
the starting monitor and outside every monitor is treated as a position on
the starting monitor, not as a monitor change. Rendering remains clipped to
the starting monitor's bounds.

Only a pointer coordinate that lies on another connected monitor ends drawing
on the starting monitor and requires shortcut release and a new press, as
specified in the [screen-mode contract](../../../ARCHITECTURE.md#screen-mode).

## Monitor lookup and test seam

`IMonitorBoundsSource.TryGetBounds` returns false for a point outside every
monitor (`Win32MonitorBoundsSource` uses `MONITOR_DEFAULTTONULL`). Today only
`GlobalShortcutAdapter` uses it, to resolve the monitor when drawing starts
and when clear is pressed. The pointer-move path does not use it:
`GlobalMouseInputAdapter.OnPointerMoved` calls
`DrawingModeInputGate.StopAtBoundary`, which checks only
`activeBounds.Contains`, and then forwards the raw point to
`DrawingSessionController.ReportPointer`, which repeats that check.

The fix connects monitor lookup to the pointer-move path. For a point outside
the starting monitor, Infrastructure resolves it through
`IMonitorBoundsSource`: a point on another monitor ends drawing; a point
outside every monitor does not. `DrawingSessionController` is in the
Application layer and must not depend on `IMonitorBoundsSource`, so the
pointer-move path must not forward a point outside every monitor as a point
outside the starting monitor; for example, it clamps that point to the
starting monitor's bounds. Fake monitor layouts in tests cover one screen and
two adjacent screens without the real mouse hook.

## Acceptance

- [ ] Reproduce and record the current edge interruption on a one-screen
  Windows desktop, including the edge and the pointer coordinates the hook
  reports.
- [x] With one connected screen, holding the draw shortcut while touching
  each outside edge leaves draw mode active; moving back inward continues the
  same stroke without release and repress.
- [ ] With two connected monitors, touching an outer edge of the starting
  monitor leaves draw mode active; crossing the shared edge to the other
  monitor still ends the stroke and requires release and repress.
- [x] Strokes remain clipped to the starting monitor.
- [x] The pointer-move path (`GlobalMouseInputAdapter` or
  `DrawingModeInputGate`) receives `IMonitorBoundsSource` and uses it to
  decide a monitor change; `MonitorBounds.Contains` alone no longer ends
  drawing.
- [x] Automated tests drive the mouse input path (`GlobalMouseInputAdapter`,
  `DrawingModeInputGate`, `DrawingSessionController`) with fake
  `IMonitorBoundsSource` layouts for a point past an outside edge and a point
  on another monitor.
- [x] Documentation states that only entering another monitor ends a stroke
  and that an outside edge does not:
  - `docs/ARCHITECTURE.md`, `Runtime flow` → `Draw`: the gate closes only
    when the cursor enters another monitor.
  - `docs/ARCHITECTURE.md`, `Differences from the first version`: replace
    "crossing the boundary ends it" with entering another monitor.
  - `README.md`, `Use drawEM`: reaching a screen edge does not end the stroke.
- [x] Record automated regression results separately from the manual desktop
  check of the real mouse hook and screen edge.

## Validation

Automated (FIX-006.1):

- `dotnet test -c Release`: 403 passed, 0 failed.
- `GlobalMouseInputAdapterTests` covers a point past each edge of one screen,
  a corner past two edges, an outer edge of two adjacent monitors, and the
  shared edge, with fake `IMonitorBoundsSource` layouts.

Manual desktop check on a one-screen laptop with the real mouse hook (FIX-006.1
build): passed, as reported by the user. Holding the draw shortcut past each
edge kept draw mode active, moving back inward continued the same stroke, and
the stroke stayed clipped to the screen.

Unverified: the recorded reproduction with hook coordinates on the old build,
and the two-monitor check (outer edge and shared edge).
