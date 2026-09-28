# Stage 1 / Step 3: sound shortcuts

**Status:** implemented; manual checks in Step 4. **Source:** [Stage 1 roadmap](../../ROADMAP-v2.md#3-connect-the-sound-shortcuts).

## Task

Connect assigned `Ctrl+Alt+1` through `Ctrl+Alt+8` slots to the global sound command through the existing keyboard hook. Start only on the first key-down of a press; a released and pressed key may start again. Queue playback outside the hook callback. Sound works while a stroke is active without ending or changing it. Preserve drawing input suppression, `Ctrl+Alt+Z`, and `Ctrl+Alt+X`.

**Depends on:** [Step 1](../step-1/task.md) and [Step 2](../step-2/task.md). **Enables:** [Step 4](../step-4/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
