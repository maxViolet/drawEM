# v4 / S4-06: acceptance criteria

**Task:** [launch effects through shortcuts](task.md). **Plan:** [implementation](plan.md).

- [ ] A bound effect of either placement launches once on first key-down; holding does not retrigger and a released/repressed shortcut restarts it.
- [ ] A Cursor effect appears centered on the cursor point captured at key-down, not a later position.
- [ ] Prepared bindings include sound and effect slots from one settings snapshot without dropping either type.
- [ ] A stale queued invocation cannot publish under a newer configuration.
- [ ] The S4-01 probe is removed: `EffectSurfaceProbe.cs`, `EffectProbeHotkeys.cs`, `EffectProbeInputLoad.cs`, and the `DRAWEM_EFFECT_PROBE` wiring in `App.xaml.cs` no longer exist, and a search of `src/` for `DRAWEM_EFFECT_PROBE` finds nothing.
- [ ] Automated hook/binding checks cover suppression, key-up, AltGr, capture, conflict, position capture, and sound independence.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
