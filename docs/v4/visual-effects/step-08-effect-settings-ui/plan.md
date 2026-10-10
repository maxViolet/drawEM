# v4 / S4-08: implementation plan

**Task:** [choose effects in Settings](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a Sound/Effect selector and an effect selector grouped under Monitor and Cursor to each Actions row without creating a ninth slot.
2. When first filling an empty slot, propose Ctrl+Alt+N. On type changes preserve the existing shortcut; on Clear remove it.
3. Keep edits isolated in SettingsEditor until Save and show validation errors for unknown IDs and shortcut conflicts.
4. Protect saved sound media when a Sound -> Effect draft is cancelled.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
