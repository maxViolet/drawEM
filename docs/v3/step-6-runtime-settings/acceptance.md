# v3 / Step 6: acceptance criteria

**Task:** [runtime application](task.md). **Plan:** [implementation](plan.md).

- [x] Startup with no settings has eight empty sound slots and no production
  dependency on v2 code-owned assignments.
- [x] All runtime consumers resolve against the same active snapshot and
  managed media references; normal sound replacement and ten-second cap remain.
- [x] A successful Save stops current sound, exits drawing, clears strokes on
  every monitor, and activates the persisted snapshot only for fresh presses.
- [x] A failed Save leaves sound, drawing, bindings, and saved media references
  under the prior configuration and reports the failure.
- [x] Automated tests cover Save during active stroke and sound, unrelated
  setting changes, all-monitor clear, restart, and failed persistence. Real
  sound and hook behavior remain for [Step 8](../step-8-publish/task.md).

## Validation

- `dotnet build DrawEM.sln -c Release -warnaserror` and
  `dotnet test DrawEM.sln -c Release --no-build` (2026-10-05): 0 warnings;
  397 passed, 0 failed.
- Startup: `Infrastructure/Settings/RuntimeSettingsTests` checks that a first
  launch binds only draw and clear with eight empty slots, that a saved sound
  slot plays its managed copy and failures name that copy, and that playback
  from a settings binding stops after ten seconds.
  `Architecture/RetiredSoundAssignmentsTests` checks that no production type
  depends on `SoundAssignments`.
- One snapshot: `Application/Settings/ActiveSettingsTests` checks that Publish
  replaces the snapshot and its sound commands in one step. Each play command
  carries the managed path of the configuration that built its binding, so a
  request queued before Save never resolves under the new settings.
- Save: `Infrastructure/Settings/SettingsSaveOperationTests` covers Save
  during an active stroke and sound, a play request queued before Save,
  all-monitor clear, keys held across Save (fresh press required), a
  color-only change, a failed Save, and an invalid draft.
- Stuck sound thread: `Infrastructure/Settings/SettingsSaveStuckSoundTests`
  blocks the sound thread past the stop timeout. Save still publishes and
  requires fresh presses; the old request plays its own copy; this Save
  deletes no managed copy and logs the unconfirmed stop once; the next
  startup removes the leftover copy.
- Persistence and media: `Application/Settings/SettingsPersistenceTests` checks
  the order store write → activate → media cleanup, and that a failed write
  or an unconfirmed stop removes no copy. `Infrastructure/Settings/RuntimeMediaTests`
  covers failed persistence, restart, a managed copy deleted outside the app,
  and a locked copy during cleanup.
- Mutation check: with media cleanup running after an unconfirmed stop, both
  stuck-sound persistence tests fail.
- Not verified here: real keyboard and mouse hooks and real sound playback;
  see [Step 8](../step-8-publish/task.md).
