# v4 / S4-04: acceptance criteria

**Task:** [add effects to action slots](task.md). **Plan:** [implementation](plan.md).

- [x] Each of eight slots is empty, SoundAction, or EffectAction; no slot starts two actions.
- [x] Unknown effect IDs and missing/conflicting shortcuts are rejected with specific validation errors.
- [x] EffectAction stores the effect ID only; placement is read from the catalog.
- [x] A saved sound slot's model behavior remains valid alongside effect slots.
- [x] Domain and Application contracts for effect actions contain no WPF or Win32 types; domain tests record results.

## Evidence

- Action model: `EffectAction` in `src/DrawEM.App/Domain/Settings/ActionSlot.cs`. A slot holds one `SlotAction`, so it cannot hold a sound and an effect at once.
- Catalog: `src/DrawEM.App/Domain/Effects/BuiltInEffect.cs` and `EffectCatalog.cs` (ID, display name, placement, resource name). S4-03 was not done when S4-04 started, so S4-04 added the catalog; S4-03 embeds the Lottie files under these resource names.
- Validation: `SettingsSnapshot.Validate` reports `UnknownEffect` and `MissingEffect`; shortcut checks are shared with draw, clear, and sound slots.
- Typed invocation command: `StartEffectCommand` (Application, from S4-02) is unchanged.
- Tests: `tests/DrawEM.Tests/Domain/Effects/EffectCatalogTests.cs` and the effect cases in `tests/DrawEM.Tests/Domain/Settings/SettingsSnapshotTests.cs`. `LayerDependencyTests` keeps WPF and Win32 types out of Domain and Application.
- Not connected by S4-04: shortcut bindings (S4-06) and the Settings window (S4-08) still handle only sound actions. Settings JSON was connected in [S4-05](../step-05-effect-settings-schema/acceptance.md). Shortcut bindings were connected in [S4-06](../step-06-effect-shortcuts/acceptance.md).

## Validation

- `dotnet test tests/DrawEM.Tests`: 536 passed, 0 failed (2026-10-11).
- `dotnet build src/DrawEM.App -c Release`: 0 warnings, 0 errors.
