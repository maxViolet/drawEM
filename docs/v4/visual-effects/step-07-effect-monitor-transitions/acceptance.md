# v4 / S4-07: acceptance criteria

**Task:** [stop effects when switching monitors](task.md). **Plan:** [implementation](plan.md).

- [ ] Moving within the target monitor continues the effect, and a Cursor effect stays at its captured point; entering another connected monitor stops it and returning does not restart it. Both placements behave the same.
- [ ] A -> B -> A transitions cannot let the old effect survive, even if UI dispatch is delayed.
- [ ] Touching an outside edge without entering a monitor does not stop the effect.
- [ ] A display-change listener, monitor identity/generation, and surface rebuild are present before this step completes.
- [ ] Tests cover movement outside draw mode, unreported cursor movement caught by the frame check, no target monitor, queued-start ordering, and topology change; two-monitor Windows checks are recorded.
- [ ] Ordinary pointer movement on one monitor does not enqueue per-move UI work while an effect runs; with no effect running/queued it adds no monitor lookup to the hook.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
