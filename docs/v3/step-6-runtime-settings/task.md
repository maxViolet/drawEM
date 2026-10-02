# v3 / Step 6: apply saved settings at runtime

**Status:** planned. **Source:** [v3 roadmap](../ROADMAP-v3.md#delivery-order).

## Task

Connect startup loading and Save to the running app. All shortcut resolution,
drawing, sound playback, failure reporting, and media lookup use the same
active settings snapshot. Retire `SoundAssignments.Slots` and its startup
wiring. A successful Save first persists the complete validated snapshot,
then stops sound, exits draw mode, clears strokes on every monitor, and exposes
the new configuration. A failed Save reports the error and keeps the previous
configuration active. A new press is required under the new configuration.

**Depends on:** [Step 2](../step-2-sound-library/task.md), [Step 3](../step-3-action-routing/task.md), and [Step 5](../step-5-drawing-style/task.md). **Enables:** [Step 7](../step-7-settings-window/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
