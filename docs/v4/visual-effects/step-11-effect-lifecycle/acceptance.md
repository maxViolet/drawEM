# v4 / S4-11: acceptance criteria

**Task:** [stop effects through app lifecycle](task.md). **Plan:** [implementation](plan.md).

- [ ] Tray Stop effects removes the active visual and pending starts while preserving sound and drawings.
- [ ] Each of display change, lock, suspend, exit, and partial startup failure removes visible effects and cancels pending starts; a start queued before the event never appears after it. Unlock/resume never restarts an old effect.
- [ ] Runtime effect failures are written to the local log, with partial visuals cleared and callbacks detached; a test forces a failure and checks the log entry. Sample failures stay visible in Settings.
- [ ] Repeated cleanup leaves no effect window, callback, or live instance; startup failure does not strand a surface.
- [ ] Automated tests cover pending-start cancellation for every event above; real Windows lifecycle observations are recorded separately.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
