# v4 / S4-04: implementation plan

**Task:** [add effects to action slots](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add stable preset IDs and EffectAction to the slot model while keeping SoundAction and empty slots.
2. Validate supported IDs and one shortcut per occupied slot through SettingsSnapshot. Keep cross-command conflicts in the shared validator.
3. Define a typed command for effect invocation; keep WPF and Win32 types outside Domain/Application.
4. Add focused domain tests for mixed sound/effect slots, invalid IDs, missing shortcuts, and duplicate shortcuts.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
