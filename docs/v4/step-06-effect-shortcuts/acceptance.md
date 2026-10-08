# v4 / S4-06: acceptance criteria

**Task:** [launch effects through shortcuts](task.md). **Plan:** [implementation](plan.md).

- [ ] A bound effect launches once on first key-down; holding does not retrigger and a released/repressed shortcut restarts it.
- [ ] Prepared bindings include sound and effect slots from one settings snapshot without dropping either type.
- [ ] A stale queued invocation cannot publish under a newer configuration.
- [ ] Automated hook/binding checks cover suppression, key-up, AltGr, capture, conflict, position capture, and sound independence.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
