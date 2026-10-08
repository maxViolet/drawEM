# v4 / S4-11: acceptance criteria

**Task:** [stop effects through app lifecycle](task.md). **Plan:** [implementation](plan.md).

- [ ] Tray Stop effects removes the active visual and pending starts while preserving sound and drawings.
- [ ] Display change, lock, suspend, and exit remove effects; unlock/resume never restarts an old one.
- [ ] Repeated cleanup leaves no effect window, callback, or live instance; startup failure does not strand a surface.
- [ ] Automated event tests and separate real Windows lifecycle observations are recorded.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
