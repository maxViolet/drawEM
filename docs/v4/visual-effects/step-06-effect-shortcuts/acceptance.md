# v4 / S4-06: acceptance criteria

**Task:** [launch effects through shortcuts](task.md). **Plan:** [implementation](plan.md).

- [x] A bound effect of either placement sends one start request on first key-down; holding does not send another, and a released and repressed shortcut sends a new one, which restarts the effect. Checked against the start delegate with a fake dispatcher.
- [x] The start request for a Cursor effect carries the cursor point captured at key-down, not a later position, and the monitor that contains that point.
- [ ] On screen: a bound effect of either placement launches, and a Cursor effect appears centered on the key-down point. Moved to [S4-03](../step-03-built-in-effects/acceptance.md) with the surface (see Scope).
- [x] Prepared bindings include sound and effect slots from one settings snapshot without dropping either type.
- [x] A stale queued invocation cannot publish under a newer configuration.
- [ ] The S4-01 probe is removed: `EffectSurfaceProbe.cs`, `EffectProbeHotkeys.cs`, `EffectProbeInputLoad.cs`, and the `DRAWEM_EFFECT_PROBE` wiring in `App.xaml.cs` no longer exist, and a search of `src/` for `DRAWEM_EFFECT_PROBE` finds nothing. Moved to [S4-03](../step-03-built-in-effects/acceptance.md) by owner decision on 2026-10-11 (see Scope).
- [x] Automated hook/binding checks cover suppression, key-up, AltGr, capture, conflict, position capture, layout protection under Alt+Shift, and sound independence.

## Scope

S4-06 ran before S4-03 was done: S4-03.1 had not run its benchmarks, and no `IEffectSurface` exists yet. On 2026-10-11 the owner chose to build the shortcut path now and keep the probe:

- `App.xaml.cs` passes a start delegate that does nothing, because there is no surface for `EffectChannelController` yet. A saved effect slot's shortcut is now recognized and hidden from other applications, but shows nothing. S4-03 composes `EffectChannelController.Start` at that point.
- The probe trigger stays, because the S4-03.1 benchmarks and the S4-03 visual checks use it.
- Both items are unchecked criteria in the [S4-03 acceptance](../step-03-built-in-effects/acceptance.md) and steps in its plan. STEPS.md and ROADMAP-v4.md now name S4-03 as the step that removes the probe.

## Implementation

- `Infrastructure/Drawing/ShortcutBindings.cs`: `FromSnapshot` binds `SoundAction` and `EffectAction` slots from one snapshot. Each effect binding holds the effect ID and its placement from `EffectCatalog`. An effect missing from the catalog throws instead of being dropped. The duplicate-shortcut check covers draw, clear, sounds, and effects.
- `Infrastructure/Drawing/GlobalShortcutAdapter.cs`: effect slots use the same once-per-press rule as sounds (`TryStartOncePerPress`), so suppression, key-up, AltGr, capture mode, and paused dispatch are unchanged. At key-down the hook reads only the cursor point and the configuration generation, then queues the start. On the dispatcher, the queued start is dropped if `Interrupt` has advanced the generation; otherwise the monitor is resolved from the captured point and `StartEffectCommand` is passed to the start delegate. A point outside every monitor starts nothing. The generation that already dropped stale drawing commands is renamed `configurationGeneration` and now also guards effect starts.
- `Application/Settings/SettingsSaveOperation.cs`: the `IActiveShortcuts.Interrupt` contract now says it also drops queued effect commands.

## Validation

- Test first: the new tests in `EffectShortcutTests.cs` and `ShortcutBindingsTests.cs` first failed to build for the expected reason: no effect binding API (`EffectBinding`, `ShortcutBindings.Effects`, the four-argument `ShortcutBindings` constructor, and the ten-argument `GlobalShortcutAdapter` constructor did not exist).
- Tests: `tests/DrawEM.Tests/Infrastructure/Drawing/EffectShortcutTests.cs`, the effect cases in `ShortcutBindingsTests.cs`, and `EffectShortcutUnderShiftPair_EmitsOneNeutralKeyBeforeModifierRelease_AndStartsOnlyTheEffect` in `ShortcutCaptureTests.cs`.
- `dotnet test tests/DrawEM.Tests`: 602 passed, 0 failed.
- Unverified: pressing an effect shortcut in the published app. Nothing is visible until S4-03 composes the surface.

## Review

- `/simplify` (Claude Code): moved the test snapshot builder into `TestSettings.WithEffects`. Its change to `GlobalShortcutAdapter.DispatchCommand` (an `Action<int>` overload so the generation is read once) was reverted. Both reads already run back to back on the hook thread, and the overload added a second closure to every drawing dispatch.
- Role reviews (correctness, security, spec, test quality): correctness and security found nothing. Fixed: an assertion that could never fail in the cursor-unavailable test; on-screen claims checked off before a surface exists; deferred items not recorded in S4-03, STEPS.md, or ROADMAP-v4.md; no layout-protection test for effect shortcuts.
