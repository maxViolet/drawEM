# v4 / FEAT-009: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [x] Each effect slot shows Loop, Max length, and No limit on a second line;
  sound and empty slots do not.
- [x] A new or migrated effect slot shows Loop off, Max length 10 s, No limit
  off.
- [x] The stepper stays within 1–60 seconds and disables a button only at its
  limit.
- [x] Loop on disables Max length and No limit; No limit on disables the
  stepper.
- [x] Loop off, limit N: the effect ends at the file end or after N seconds,
  whichever comes first.
- [x] Loop off, No limit: the effect ends at the file end.
- [x] Loop on: the effect repeats until stopped and never ends by itself.
- [x] Loop on: pressing the slot's shortcut while its effect runs stops it;
  the next press starts it again.
- [x] Loop off: pressing the slot's shortcut while its effect runs restarts it,
  as before.
- [x] Loop on: Sample reads `Stop` while the effect runs and stops it when
  pressed; Cancel or closing Settings also stops it.
- [x] A looping effect still stops on another effect, Tray > Stop effects,
  monitor change, topology change, session lock, suspend, Save, and exit.
- [x] Schema 1 and 2 files load with Loop off and limit 10 s; saving writes
  schema 3 with `loop` and `maxSeconds`.
- [x] A settings file with `maxSeconds` outside 1–60 fails validation.
- [x] The v4 roadmap and the settings README describe the new behavior.
- [x] `dotnet test DrawEM.sln` passes.

## Evidence

- Tests were added first. The focused run failed to compile because
  `IEffectSampler` did not yet expose the slot playback settings and running
  state callback.
- Focused behavior suite: 359 passed, 0 failed. It covers domain validation,
  schema migration and validation, command propagation, channel deadlines and
  loop toggling, sample ownership/state, Lottie cycle timing, view-model enable
  states, and the real off-screen WPF window bindings and effective visibility.
- `dotnet test DrawEM.sln --no-restore`: 782 passed, 0 failed.
- Startup smoke: the built `DrawEM.App.exe` remained running for three seconds,
  then was stopped by the smoke command.
- `dotnet build src/DrawEM.App/DrawEM.App.csproj -c Release --no-restore`:
  0 warnings, 0 errors.
- `graphify update .`: graph rebuilt successfully.
- Manual visual check of spacing and clipping in the on-screen Settings window:
  unverified.
- Review fix (PR #77), sample toggle: a looping sample now stops only a running
  looping sample of the same slot. A one-shot sample running in the slot is
  replaced by a looping one that reports running. The test
  `LoopingSample_WhileAOneShotSampleOfTheSlotRuns_StartsALoopInsteadOfStopping`
  failed first (running states `[]`, expected `[True]`), then passed.
- Review fix (PR #77), slot edits: `SettingsEditor` remembers the slot of the
  latest effect sample and stops that sample when the slot's draft action
  changes. `EditingTheSampledSlot_StopsItsEffectSample` (clear, other effect,
  loop, max length, sound) failed first with 0 stops, then passed.
  `EditingAnotherSlot_KeepsTheEffectSampleRunning` passes.
  `ClearSlot_WhileItsLoopingSampleRuns_StopsItAndShowsSample` failed with the
  stop disabled, then passed: the button returns to "Sample". Restore defaults
  also stops the sample: its new test case failed first with 0 stops, then
  passed.
- Review fix (PR #77), zero-length loop: `PlaybackPosition` returns
  `CannotLoop` for a looping animation without length, and the playback ends
  with the failure "The effect has no length, so it cannot loop." The two new
  `CannotLoop` cases failed first (`Completed`), then passed.
- After the review fixes, `dotnet test DrawEM.sln --no-restore`: 794 passed,
  0 failed. Release build: 0 warnings, 0 errors.
