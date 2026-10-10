# v4 / S4-04: implementation plan

**Task:** [add effects to action slots](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add stable effect IDs and EffectAction to the slot model while keeping SoundAction and empty slots.
2. Validate supported IDs against the built-in catalog in `Domain/Effects` (effect ID, display name, placement, resource name) and one shortcut per occupied slot through SettingsSnapshot. Keep cross-command conflicts in the shared validator.
3. Define a typed command for effect invocation; keep WPF and Win32 types outside Domain/Application.
4. Keep placement out of EffectAction; it comes from the catalog. Add focused domain tests for mixed sound/effect slots, invalid IDs, missing shortcuts, and duplicate shortcuts.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
