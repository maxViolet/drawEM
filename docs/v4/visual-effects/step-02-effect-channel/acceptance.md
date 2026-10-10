# v4 / S4-02: acceptance criteria

**Task:** [control one running effect](task.md). **Plan:** [implementation](plan.md).

- [x] Starting a second effect replaces the first, including when both use the same effect and when the placements differ (Monitor -> Cursor, Cursor -> Monitor).
- [x] An old completion, deadline, or failure callback cannot stop or clear a newer effect.
- [x] Stop and disposal are safe to repeat; each ending path releases the active instance exactly once.
- [x] Deterministic tests with fake clock/surface cover replacement, completion, deadline, stale callbacks, failure, and repeated Stop.

## Evidence

- Controller: `src/DrawEM.App/Application/Effects/EffectChannelController.cs`; rendering port `IEffectSurface`/`IEffectPlayback`; time through `TimeProvider`.
- Domain types: `src/DrawEM.App/Domain/Effects/EffectId.cs`, `EffectPlacement.cs`.
- Tests: `tests/DrawEM.Tests/Application/Effects/EffectChannelControllerTests.cs` (fake surface and `ManualTimeProvider`).
- The controller is not wired into the app yet; S4-03 adds the real surface and S4-06/S4-11 the shortcut and lifecycle wiring.

## Validation

- `dotnet test tests/DrawEM.Tests`: 521 passed, 0 failed (2026-10-10).
- `dotnet build src/DrawEM.App -c Release`: 0 warnings, 0 errors.
