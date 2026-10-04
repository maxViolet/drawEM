# v3 / Step 5: acceptance criteria

**Task:** [configurable drawing style](task.md). **Plan:** [implementation](plan.md).

- [x] New strokes use the configured color and physical-pixel width; existing
  strokes retain their creation-time style until cleared.
- [x] Line pens and single-point dots render at the chosen physical width on
  each stroke's monitor at 100% and 150% in automated rendering tests with
  explicit rasterization tolerance.
- [ ] Mixed DPI: satisfied by design, not by an automated test (see
  [Validation](#validation)). Confirm on a mixed-DPI desktop in
  [Step 8](../step-8-publish/task.md).
- [x] Widths 1 and 20 and a nondefault HEX color are covered by tests.
  Measured desktop appearance remains for [Step 8](../step-8-publish/task.md).

## Validation

- `dotnet test -c Release` (2026-10-04): 380 passed, 0 failed.
- `Infrastructure/Settings/JsonSettingsStoreTests`: a saved style reaches new
  strokes after a restart through `SettingsStartup.Load`; unreadable settings
  show the reason and the recovery copy path.
- `Application/Drawing/DrawingSessionControllerTests`: a stroke keeps the style
  read when it starts; a style change during or after a stroke affects only
  the next stroke. The parameterless controller uses `#FF4500`, 4 pixels.
- `Presentation/Drawing/StrokeRenderElementTests`: lines and single-point dots
  render widths 1, 4, and 20 at overlay scales 100% and 150%; strokes clipped
  to two monitors keep the same physical width at either overlay scale; `#1E90C8`
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
  wrong width on the monitor that does not match the window. This guarantee
  comes from that design, not from a test: no test input carries a monitor
  DPI, so the two-monitor test shows only that monitor clipping keeps the
  width. Mixed-DPI desktop appearance is a [Step 8](../step-8-publish/task.md) check.
- App wiring: `App.xaml.cs` loads saved settings at startup and passes their
  style to the controller. An unreadable file shows a message and the app
  starts with defaults. Shortcuts and sounds still come from code, and a
  saved change applies after restart; [Step 6](../step-6-runtime-settings/task.md)
  builds them from settings and applies Save at runtime.
