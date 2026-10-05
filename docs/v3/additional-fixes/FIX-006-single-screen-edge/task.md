# FIX-006: keep drawing active at a single screen edge

**Status:** planned. **Package:** [v3 additional fixes](../README.md).

## Current behavior

Reported on a laptop with one connected screen: while the draw shortcut is
held and a stroke reaches a physical edge of the screen, drawing stops. Moving
the pointer back toward the screen center while still holding the shortcut
does not continue the stroke; drawing requires a release and a new press.

The existing [screen-mode contract](../../../ARCHITECTURE.md#screen-mode)
defines `SINGLE_SCREEN`. Its interruption rule covers moving the cursor to
another monitor, not touching the outside edge of the only screen. The current
input gate ends drawing when a pointer coordinate falls outside the starting
monitor's bounds. The cause of the reported single-screen edge event has not
yet been established by a desktop reproduction.

## Required behavior

In `SINGLE_SCREEN`, touching or holding the pointer at any physical outside
edge of the only connected screen must not end draw mode or complete the active
stroke. While the draw shortcut stays held, moving the pointer away from that
edge continues the same stroke without a new shortcut press. Rendering remains
clipped to the monitor bounds.

Moving the pointer from one monitor to another remains a separate case: it
ends drawing on the starting monitor and requires shortcut release and a new
press, as specified in the [screen-mode contract](../../../ARCHITECTURE.md#screen-mode).

## Acceptance

- [ ] Reproduce and record the current edge interruption on a one-screen
  Windows desktop, including the edge and pointer coordinates involved.
- [ ] With one connected screen, holding the draw shortcut while touching each
  outside edge leaves draw mode active; moving back inward continues the same
  stroke without release and repress.
- [ ] Strokes remain clipped to the screen; moving to another connected monitor
  still ends the stroke and requires release and repress.
- [ ] Record automated regression results separately from the manual desktop
  check of the real mouse hook and screen edge.
