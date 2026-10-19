# v4 / FEAT-012: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] `StrokeSmoothing` has two values, `None` and `Bezier`; the default style
  uses `Bezier`.
- [ ] With `None`, a stroke is drawn exactly as before: straight segments
  between the points.
- [ ] With `Bezier`, a completed stroke is drawn as quadratic curves through
  the segment midpoints and passes through its first and last points.
- [ ] With `Bezier`, the active stroke is smooth while it is drawn; the tail
  to the cursor is replaced on each new point.
- [ ] An update of the active stroke draws only the new pieces and the tail,
  not the whole stroke.
- [ ] Releasing the button does not change the shape of a `Bezier` stroke.
- [ ] Single-point strokes are still a dot, with either smoothing.
- [ ] Each stroke keeps its own smoothing: changing the setting does not
  change strokes already on the screen, also after a full re-render.
- [ ] Settings shows a third tab, `Developer settings`, with a `Bezier
  smoothing` check box, checked by default; Save applies it to new strokes.
- [ ] Restore defaults checks `Bezier smoothing`.
- [ ] Schema 1–3 files load with `bezier`; saving writes schema 4 with
  `drawing.smoothing`.
- [ ] A settings file with an unknown `smoothing` value fails validation.
- [ ] `Infrastructure/Settings/README.md` documents schema version 4.
- [ ] `dotnet test DrawEM.sln` passes.
- [ ] Manual check: a fast circle drawn with `Bezier` shows no corners; the
  same circle with `None` shows corners.

## Evidence

Not started.
