# v3 / Step 3: acceptance criteria

**Task:** [configurable action shortcuts](task.md). **Plan:** [implementation](plan.md).

- [ ] One immutable snapshot resolves all active commands in memory; hook
  callbacks perform no file access, decoding, or player creation.
- [ ] Remapped draw remains hold-to-draw, clear remains monitor-scoped, and an
  assigned sound fires once per press with existing stop-then-start playback.
- [ ] Assigned candidates have matching suppression decisions through release;
  empty slots and ordinary keys pass through outside draw mode.
- [ ] Right Alt prevents dispatch, including with reported Left Ctrl or a
  physical `Ctrl+RightAlt` chord; Left Alt bindings still dispatch.
- [ ] Automated tests cover those cases and drawing across monitors. Real
  keyboard behavior remains for [Step 8](../step-8/task.md).
