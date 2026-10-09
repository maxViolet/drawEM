# v4 / S4-10: acceptance criteria

**Task:** [apply settings safely](task.md). **Plan:** [implementation](plan.md).

- [ ] After Save, a newly assigned effect works through its shortcut and survives app restart.
- [ ] No old queued or active effect becomes visible after successful Save, even if it was dispatched before Save.
- [ ] Validation, backup, or persistence failure leaves active configuration and actions unchanged.
- [ ] Sound stops, drawing exits and clears on successful Save as v3 specifies; mixed-slot startup and save-race tests record results.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
