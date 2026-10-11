# v4 / S4-06: acceptance criteria

**Task:** [launch effects through shortcuts](task.md). **Plan:** [implementation](plan.md).

- [x] A bound effect of either placement launches once on first key-down; holding does not retrigger and a released/repressed shortcut restarts it. Checked against the start delegate with a fake dispatcher; no effect shows on screen until S4-03 adds a surface (see Scope).
- [x] A Cursor effect appears centered on the cursor point captured at key-down, not a later position. The start request carries the key-down point and the monitor that contains it; the surface centers on that point in S4-03.
- [x] Prepared bindings include sound and effect slots from one settings snapshot without dropping either type.
- [x] A stale queued invocation cannot publish under a newer configuration.
- [ ] The S4-01 probe is removed: `EffectSurfaceProbe.cs`, `EffectProbeHotkeys.cs`, `EffectProbeInputLoad.cs`, and the `DRAWEM_EFFECT_PROBE` wiring in `App.xaml.cs` no longer exist, and a search of `src/` for `DRAWEM_EFFECT_PROBE` finds nothing. Deferred by owner decision on 2026-10-11 (see Scope).
- [x] Automated hook/binding checks cover suppression, key-up, AltGr, capture, conflict, position capture, and sound independence.

## Scope

S4-06 ran before S4-03 was done: S4-03.1 had not run its benchmarks, and no `IEffectSurface` exists yet. On 2026-10-11 the owner chose to build the shortcut path now and keep the probe:

- `App.xaml.cs` passes a start delegate that does nothing, because there is no surface for `EffectChannelController` yet. A saved effect slot's shortcut is now recognized and hidden from other applications, but shows nothing. S4-03 composes `EffectChannelController.Start` at that point.
- The probe trigger stays, because the S4-03.1 benchmarks and the S4-03 visual checks use it. Remove it when S4-03 is done, as the unchecked criterion above describes.

## Implementation

- `Infrastructure/Drawing/ShortcutBindings.cs`: `FromSnapshot` binds `SoundAction` and `EffectAction` slots from one snapshot. Each effect binding holds the effect ID and its placement from `EffectCatalog`. An effect missing from the catalog throws instead of being dropped. The duplicate-shortcut check covers draw, clear, sounds, and effects.
- `Infrastructure/Drawing/GlobalShortcutAdapter.cs`: effect slots use the same once-per-press rule as sounds (`TryStartOncePerPress`), so suppression, key-up, AltGr, capture mode, and paused dispatch are unchanged. At key-down the hook reads only the cursor point and the configuration generation, then queues the start. On the dispatcher, the queued start is dropped if `Interrupt` has advanced the generation; otherwise the monitor is resolved from the captured point and `StartEffectCommand` is passed to the start delegate. A point outside every monitor starts nothing. The generation that already dropped stale drawing commands is renamed `configurationGeneration` and now also guards effect starts.
- `Application/Settings/SettingsSaveOperation.cs`: the `IActiveShortcuts.Interrupt` contract now says it also drops queued effect commands.

## Validation

- Test first: the new tests in `EffectShortcutTests.cs` and `ShortcutBindingsTests.cs` first failed to build for the expected reason: no effect binding API (`EffectBinding`, `ShortcutBindings.Effects`, the four-argument `ShortcutBindings` constructor, and the ten-argument `GlobalShortcutAdapter` constructor did not exist).
- Tests: `tests/DrawEM.Tests/Infrastructure/Drawing/EffectShortcutTests.cs` and the effect cases in `ShortcutBindingsTests.cs`.
- `dotnet test tests/DrawEM.Tests`: 601 passed, 0 failed.
- Unverified: pressing an effect shortcut in the published app. Nothing is visible until S4-03 composes the surface.
