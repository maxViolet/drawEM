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
- One snapshot: `Application/Settings/ActiveSettingsTests` checks that a play
  command carries its sound's managed copy and stays the same across Publish.
  Bindings are built from one snapshot, so a request queued before Save never
  resolves under the new settings.
- Save: `Infrastructure/Settings/SettingsSaveOperationTests` covers Save
  during an active stroke and sound, the order store write → activate → media
  cleanup with copies removed only after a confirmed stop, all-monitor clear,
  keys held across Save (fresh press required), a color-only change, a failed
  Save, and an invalid draft.
- Old play requests: the host runs a request's final check and
  `player.Play()` under one start lock, and Save takes that lock before
  persisting. `Infrastructure/Settings/SettingsSaveStuckSoundTests` covers a
  request blocked while opening its player and a queued one: Save publishes
  and requires fresh presses, neither old request ever plays, this Save
  deletes no managed copy and logs the unconfirmed stop once, and the next
  startup removes the leftover copy. It also blocks a request inside its
  player's start, after the final check: Save persists nothing, changes nothing,
  reports "a sound was still starting" once, and a retry after the start
  stops that sound and retires the old copy.
  `Infrastructure/Sound/SoundChannelHostTests` checks that the start lock
  cannot be held while a player is starting, and that ended requests never
  play while the stop times out.
- Media: `Infrastructure/Settings/RuntimeMediaTests` runs Save against the
  real settings file and sound library and covers failed persistence,
  restart, a managed copy deleted outside the app, and a locked copy during
  cleanup.
- Mutation checks: with media cleanup running after an unconfirmed stop, the
  unconfirmed-stop order case and the stuck-sound copy test fail; with the
  start lock removed from the host's request check, the start-blocked Save
  test and the host start-lock test fail.
- Not verified here: real keyboard and mouse hooks and real sound playback;
  see [Step 8](../step-8-publish/task.md).
