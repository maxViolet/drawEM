# v4 / S4-06: implementation plan

**Task:** [launch effects through shortcuts](task.md). **Acceptance:** [criteria](acceptance.md).

1. Extend immutable shortcut bindings to prepare either sound or effect commands from one active snapshot.
2. Capture the cursor position and configuration epoch at key-down; decide suppression and enqueue the action without rendering in the hook callback.
3. Dispatch effects to a start delegate on the UI thread (production: `EffectChannelController.Start`). Check generation before publication and drop obsolete queued starts.
4. Preserve sound dispatch, draw/clear behavior, AltGr, capture mode, and paired-key suppression decisions. Removing the S4-01 probe trigger and composing `EffectChannelController.Start` moved to S4-03 (owner decision, 2026-10-11).

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
