# v3 / Step 5: acceptance criteria

**Task:** [configurable drawing style](task.md). **Plan:** [implementation](plan.md).

- [ ] New strokes use the configured color and physical-pixel width; existing
  strokes retain their creation-time style until cleared.
- [ ] Line pens and single-point dots render at the chosen physical width on
  each stroke's monitor at 100%, 150%, and mixed DPI in automated rendering
  tests with explicit rasterization tolerance.
- [ ] Widths 1 and 20 and a nondefault HEX color are covered by tests.
  Measured desktop appearance remains for [Step 8](../step-8/task.md).
