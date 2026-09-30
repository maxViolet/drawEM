# v3 / Step 6: acceptance criteria

**Task:** [runtime application](task.md). **Plan:** [implementation](plan.md).

- [ ] Startup with no settings has eight empty sound slots and no production
  dependency on v2 code-owned assignments.
- [ ] All runtime consumers resolve against the same active snapshot and
  managed media references; normal sound replacement and ten-second cap remain.
- [ ] A successful Save stops current sound, exits drawing, clears strokes on
  every monitor, and activates the persisted snapshot only for fresh presses.
- [ ] A failed Save leaves sound, drawing, bindings, and saved media references
  under the prior configuration and reports the failure.
- [ ] Automated tests cover Save during active stroke and sound, unrelated
  setting changes, all-monitor clear, restart, and failed persistence. Real
  sound and hook behavior remain for [Step 8](../step-8/task.md).
