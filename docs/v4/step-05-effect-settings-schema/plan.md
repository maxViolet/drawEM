# v4 / S4-05: implementation plan

**Task:** [save and load effect assignments](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add schema 2's effect payload while retaining the existing slot shape and sound payload.
2. Read schema 1 into the new model without rewriting it at startup. Every explicit v4 Save writes schema 2. Before the first migration Save, create one immutable backup of the existing schema 1 file; preserve an existing backup on later upgrades.
3. Keep durable replacement and unreadable-file preservation. Reject unknown action types and effect IDs instead of silently clearing their slots.
4. Keep sound-library reference accounting correct for saved SoundAction slots, including after a draft type change. Document rollback limits for sound imports and cleanup after restoring older settings.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
