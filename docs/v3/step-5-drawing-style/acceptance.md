# v3 / Step 5: acceptance criteria

**Task:** [configurable drawing style](task.md). **Plan:** [implementation](plan.md).

- [x] New strokes use the configured color and physical-pixel width; existing
  strokes retain their creation-time style until cleared.
- [x] Line pens and single-point dots render at the chosen physical width on
  each stroke's monitor at 100%, 150%, and mixed DPI in automated rendering
  tests with explicit rasterization tolerance.
- [x] Widths 1 and 20 and a nondefault HEX color are covered by tests.
  Measured desktop appearance remains for [Step 8](../step-8-publish/task.md).

## Validation

- `dotnet test` (2026-10-04): 378 passed, 0 failed.
- `Application/Drawing/DrawingSessionControllerTests`: a stroke keeps the style
  read when it starts; a style change during or after a stroke affects only
  the next stroke. The parameterless controller uses `#FF4500`, 4 pixels.
- `Presentation/Drawing/StrokeRenderElementTests`: lines and single-point dots
  render widths 1, 4, and 20 at overlay scales 100% and 150%; strokes on two
  monitors render the same physical width at either overlay scale; `#1E90C8`
  and per-stroke colors render exactly. Width is the summed alpha coverage of
  a column (line) or the circle diameter of the covered area (dot), in a bitmap
  with one pixel per physical pixel. Rasterization tolerance: ±0.25 pixel.
- Mutation check: with the width passed as DIPs (the v2 behavior), all seven
  150% render cases fail.
- Mixed DPI: the overlay is one Per-Monitor-V2 window. Windows does not
  bitmap-scale it on a monitor whose DPI differs from the window's, so one
  physical pixel has the same local size on every monitor. Width therefore
  uses the overlay's device-to-local transform, the same transform as the
  stroke points. Converting with the stroke monitor's own DPI would draw the
  wrong width on the monitor that does not match the window.
- Not wired to saved settings yet: the app draws with the default style until
  [Step 6](../step-6-runtime-settings/task.md) passes the active snapshot's style.
