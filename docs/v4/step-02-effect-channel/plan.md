# v4 / S4-02: implementation plan

**Task:** [control one running effect](task.md). **Acceptance:** [criteria](acceptance.md).

1. Define an effect invocation with a unique instance ID, captured target, configuration generation, start time, and preset ID.
2. Add an application controller with Start, Stop, completion, failure, and a ten-second maximum; pass rendering through an injected port and time through an injected clock.
3. On replacement, end the old instance before publishing the new one. Bind callbacks to instance IDs so stale completions cannot stop replacements.
4. Release the rendering resource on completion, Stop, replacement, failure, and disposal. Keep controller state independent of WPF.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
