# v4 / S4-04: acceptance criteria

**Task:** [add effects to action slots](task.md). **Plan:** [implementation](plan.md).

- [ ] Each of eight slots is empty, SoundAction, or EffectAction; no slot starts two actions.
- [ ] Unknown effect IDs and missing/conflicting shortcuts are rejected with specific validation errors.
- [ ] EffectAction stores the effect ID only; placement is read from the catalog.
- [ ] A saved sound slot's model behavior remains valid alongside effect slots.
- [ ] Domain and Application contracts for effect actions contain no WPF or Win32 types; domain tests record results.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
