# v4 / S4-05: acceptance criteria

**Task:** [save and load effect assignments](task.md). **Plan:** [implementation](plan.md).

- [ ] Schema 1 sound settings load with all slots, shortcuts, and file references unchanged.
- [ ] Schema 2 round-trips mixed sound/effect/empty slots and stable effect IDs; no placement is written.
- [ ] Every explicit v4 Save writes schema 2. A failed backup, write, or replacement leaves previous settings recoverable; loading alone never rewrites settings, and an existing migration backup is not overwritten.
- [ ] Unknown types/IDs yield a useful error and preserve the damaged file; storage/migration tests record results.
- [ ] Rollback instructions explain restoring the backup with drawEM closed and the limits for sounds imported or deleted after that backup, including older-app startup cleanup.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
