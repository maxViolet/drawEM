# v4 / S4-05: implementation plan

**Task:** [save and load effect assignments](task.md). **Acceptance:** [criteria](acceptance.md).

> Historical S4-05 plan. FEAT-009 later advanced the current schema to 3;
> schema 2 remains a supported migration input.

1. Add schema 2's effect payload, holding only the effect ID and no placement, while retaining the existing slot shape and sound payload.
2. Read schema 1 into the new model without rewriting it at startup. Every explicit v4 Save writes schema 2. Before replacing a valid schema 1 file, keep an immutable backup: `settings.schema1-backup.json` is number 1, and each later migration adds the next numbered backup unless the highest-numbered backup already has the same bytes. Never overwrite a backup. Rollback restores the highest-numbered backup while drawEM is closed.
3. Keep durable replacement and unreadable-file preservation. Reject unknown action types and effect IDs instead of silently clearing their slots.
4. Keep sound-library reference accounting correct for saved SoundAction slots, including after a draft type change. Document rollback limits for sound imports and cleanup after restoring older settings.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
