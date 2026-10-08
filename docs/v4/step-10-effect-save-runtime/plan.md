# v4 / S4-10: implementation plan

**Task:** [apply settings safely](task.md). **Acceptance:** [criteria](acceptance.md).

1. Keep v3 validation and durable write ordering. After persistence succeeds, invalidate older effect invocations and end current effects before publishing the new snapshot/bindings.
2. Serialize the final generation check and effect visual publication on the UI dispatcher.
3. Preserve the sound start-hold, drawing exit/all-monitor clear, and sound-library cleanup policy.
4. Exercise startup reload of mixed sound/effect slots and failure paths.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
