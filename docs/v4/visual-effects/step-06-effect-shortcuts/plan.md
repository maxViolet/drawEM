# v4 / S4-06: implementation plan

**Task:** [launch effects through shortcuts](task.md). **Acceptance:** [criteria](acceptance.md).

1. Extend immutable shortcut bindings to prepare either sound or effect commands from one active snapshot.
2. Capture the cursor position and configuration epoch at key-down; decide suppression and enqueue the action without rendering in the hook callback.
3. Dispatch effects to the controller on the UI thread. Check generation before publication and drop obsolete queued starts.
4. Preserve sound dispatch, draw/clear behavior, AltGr, capture mode, and paired-key suppression decisions. Remove the S4-01 development-only probe trigger.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
