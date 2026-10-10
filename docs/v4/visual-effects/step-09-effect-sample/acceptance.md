# v4 / S4-09: acceptance criteria

**Task:** [preview draft effects](task.md). **Plan:** [implementation](plan.md).

- [ ] Sample starts the selected draft effect without a saved shortcut, including when the draft shortcut is missing or conflicting.
- [ ] Settings remains focused and the effect plays at normal size: a Monitor effect covers the active monitor and a Cursor effect appears around the cursor, never as a thumbnail.
- [ ] Sample -> shortcut -> old editor Cancel leaves the shortcut effect running; Sample -> Sample replacement has the same ownership rule.
- [ ] Cancel/close stops that editor's current Sample and does not affect sound or drawings; automated ownership and Windows UI checks are recorded.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
