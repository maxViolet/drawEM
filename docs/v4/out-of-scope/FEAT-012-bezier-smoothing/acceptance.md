# v4 / FEAT-012: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [x] `StrokeSmoothing` has two values, `None` and `Bezier`; the default style
  uses `Bezier`.
- [x] With `None`, a stroke is drawn exactly as before: straight segments
  between the points.
- [x] With `Bezier`, a completed stroke is drawn as quadratic curves through
  the segment midpoints and passes through its first and last points.
- [x] With `Bezier`, the active stroke is smooth while it is drawn; the tail
  to the cursor is replaced on each new point.
- [x] An update of the active stroke draws only the new pieces and the tail,
  not the whole stroke.
- [x] `tools/RenderLagHarness` with `Bezier` shows no regression against the
  same run with `None`; both results are recorded in the evidence.
- [x] Releasing the button does not change the shape of a `Bezier` stroke: the
  pixels of the incrementally drawn stroke match a full re-render of the same
  stroke, within anti-aliasing tolerance.
- [x] A stroke that ends with a last point the renderer never saw as active
  (coalesced update) still ends exactly at that point.
- [x] Single-point strokes are still a dot, with either smoothing.
- [x] Active Bézier pieces and the completed stroke remain clipped to the
  stroke's starting monitor. `UpdateState_ClipsIncrementalBezierPiecesToTheirStartingMonitor`.
- [x] Each stroke keeps its own smoothing: changing the setting does not
  change strokes already on the screen, also after a full re-render.
- [x] Settings shows a third tab, `Developer settings`, with a `Bezier
  smoothing` check box, checked by default; Save applies it to new strokes.
- [x] Restore defaults checks `Bezier smoothing`.
- [x] Cancel or closing Settings leaves the active smoothing unchanged.
- [x] Schema 1–3 files load with `bezier`; saving writes schema 4 with
  `drawing.smoothing`.
- [x] A schema 4 file with `"smoothing": "none"` saves and loads back as
  `None`; the same holds for `bezier`.
- [x] A settings file with an unknown `smoothing` value fails validation.
- [x] `Infrastructure/Settings/README.md` documents schema version 4.
- [x] `dotnet test DrawEM.sln` passes.
- [ ] Manual check: a fast circle drawn with `Bezier` shows no corners; the
  same circle with `None` shows corners.

## Evidence

The implementation includes a pure geometry helper and domain/renderer/editor/UI
coverage. `StrokeRenderElementTests.UpdateState_BezierIncrementalStrokeMatchesFullReRenderAfterCompletion`
compares active-then-completed pixels against a full render; its observed maximum
channel difference is 19 at pixel (9, 11), within the 20/255 channel tolerance.
Existing coalesced completion and dot tests cover those fallbacks. Settings tests
cover schema 1–3 migration defaults, both schema 4 values, unknown values, draft
Save/Cancel, Restore defaults, and the Developer settings binding.

`dotnet test DrawEM.sln` passed 899 tests. The `tools/RenderLagHarness` runs
completed all samples for both smoothing values. Worst observed processing p99
was 0.06 ms for `None` and 0.07 ms for `Bezier`; the 0.01 ms difference is
within run-to-run noise at this resolution. Manual fast-circle UI check remains
unverified.
