# v3 / Step 3: configurable action shortcuts

**Status:** planned. **Source:** [v3 roadmap](../ROADMAP-v3.md#delivery-order).

## Task

Replace fixed draw/clear and in-code sound shortcut resolution with one
immutable in-memory binding snapshot in the global keyboard path. Preserve
hold/release drawing, monitor-scoped clear, single-fire sound playback,
input suppression, and cross-monitor drawing behavior. Right Alt is reserved
for AltGr: while held, no action dispatches and candidate keys pass through
outside draw mode, even if Windows reports Left Ctrl. A physical
`Ctrl+RightAlt` chord is unsupported; Left Alt bindings remain usable.

**Depends on:** [Step 1](../step-1/task.md). **Enables:** [Step 4](../step-4/task.md) and [Step 6](../step-6/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
