# v4 / FEAT-012: smooth strokes with Bézier curves

**Status:** implemented; manual UI check pending. **Source:** [v4 out of scope](../README.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Previous behavior

- `StrokeRenderElement` draws a stroke as straight `DrawLine` segments between
  the raw mouse points. Only the caps and joins are round.
- During a fast movement the points are far apart, so an arc shows as a
  polygon with visible corners.

## Required behavior

### Smoothing strategy

- A new enum `StrokeSmoothing { None, Bezier }` selects how a stroke is drawn.
  - `None`: straight segments between the points, as before.
  - `Bezier`: quadratic Bézier curves through the midpoints of the segments.
    Each point is the control point of the curve between the midpoints of its
    two neighboring segments.
- Smoothing changes only the rendering. Input points are not filtered, moved,
  or delayed.
- The default is `Bezier`.

### While drawing

- The active stroke is smooth while it is drawn, not only after the button is
  released.
- Each new point adds the finished curve piece up to the previous midpoint.
  The short tail from the last midpoint to the cursor is a temporary straight
  segment, replaced when the next point arrives.
- The cost of an update stays proportional to the new points only
  ([F03](../../../v1/steps/F03-render-lag-investigation-plan.md)).
- A completed stroke uses the same algorithm, so its shape does not change
  when the button is released.

### Per stroke

- `StrokeSmoothing` is part of `DrawingStyle`, like color and width. Each
  stroke keeps the smoothing it was drawn with.
- Changing the setting affects only new strokes. Strokes already on the screen
  keep their shape, also after a full re-render.

### Settings window

- A third tab, `Developer settings`, follows `Actions · 8 slots`.
- The tab has one check box, `Bezier smoothing`. Checked is `Bezier`,
  unchecked is `None`. It is applied on Save, like the other settings.
- The tab is always visible.

### Settings file

Schema version 4 adds `smoothing` to the `drawing` object:

```json
"drawing": { "color": "#FF4500", "width": 4, "smoothing": "bezier", "drawShortcut": "Ctrl+Alt+Z", "clearShortcut": "Ctrl+Alt+X" }
```

- `smoothing`: `"none"` or `"bezier"`.
- Schema 1–3 files load with `bezier`.
- Any other value fails validation like any other invalid value.

## Out of scope

- Separate dev/debug and release builds.
- Hiding the `Developer settings` tab in release builds.
- Input filtering against hand jitter (a stabilizer or moving average).
- Other curve types, for example Catmull-Rom.
