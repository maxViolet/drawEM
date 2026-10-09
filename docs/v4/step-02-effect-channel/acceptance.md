# v4 / S4-02: acceptance criteria

**Task:** [control one running effect](task.md). **Plan:** [implementation](plan.md).

- [ ] Starting a second effect replaces the first, including when both use the same effect and when the placements differ (Monitor -> Cursor, Cursor -> Monitor).
- [ ] An old completion, deadline, or failure callback cannot stop or clear a newer effect.
- [ ] Stop and disposal are safe to repeat; each ending path releases the active instance exactly once.
- [ ] Deterministic tests with fake clock/surface cover replacement, completion, deadline, stale callbacks, failure, and repeated Stop.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
