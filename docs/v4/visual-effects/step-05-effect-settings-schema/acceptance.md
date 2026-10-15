# v4 / S4-05: acceptance criteria

**Task:** [save and load effect assignments](task.md). **Plan:** [implementation](plan.md).

> Historical S4-05 evidence. FEAT-009 later advanced the current schema to 3;
> schema 2 remains a supported migration input.

- [x] Schema 1 sound settings load with all slots, shortcuts, and file references unchanged.
- [x] Schema 2 round-trips mixed sound/effect/empty slots and stable effect IDs; no placement is written.
- [x] Every explicit v4 Save writes schema 2. A failed backup, write, or replacement leaves previous settings recoverable; loading alone never rewrites settings, and an existing migration backup is not overwritten.
- [x] Unknown types/IDs yield a useful error and preserve the damaged file; storage/migration tests record results.
- [x] Rollback instructions explain restoring the backup with drawEM closed and the limits for sounds imported or deleted after that backup, including older-app startup cleanup.

## Evidence

- Schema: `src/DrawEM.App/Infrastructure/Settings/SettingsJson.cs`. `CurrentSchemaVersion` is 2; the reader accepts 1 and 2. An effect is written as `{ "type": "effect", "effectId": "<id>", "shortcut": "<keys>" }`. A schema 1 file with an `effect` action, an unknown type, an empty `effectId`, an ID missing from `EffectCatalog`, or a string that is not valid UTF-8 is unreadable, and the reason names the slot and the type or ID, or the property.
- Migration backup: `JsonSettingsStore.Save` copies a schema 1 `settings.json` that passes every Load check to `settings.schema1-backup.json` before the first replacement, through a flushed temporary file. Each later migration writes `settings.schema1-backup-<N>.json` with the next number, so the highest number holds the file the latest migration replaced (A → B → A gives backups A, B, A). Nothing is copied when the highest-numbered backup already has the same bytes; no backup is overwritten. A damaged file gets no backup. A failed backup, or a current file or backup list that cannot be read, stops the Save before anything is written.
- Sound accounting: `ManagedSoundLibrary` counts only `SoundAction` slots, so saving a slot changed from sound to effect removes the copy only after its last saved sound reference; startup cleanup with a mixed schema 2 file keeps saved sound copies. Cancel after a sound-to-effect change in the Settings window is not reachable until S4-08 and is checked in S4-10.
- Rollback: "Rolling back to a v3 drawEM" in `src/DrawEM.App/Infrastructure/Settings/README.md`, including keeping the v4 file, choosing the highest-numbered backup, and the case with no backup.
- Tests: `tests/DrawEM.Tests/Infrastructure/Settings/JsonSettingsStoreTests.cs` (schema 1 load without writes, schema 2 round-trip and payload shape, invalid schema 2 files, invalid UTF-8, effect in schema 1, backup creation, second upgrade after a rollback, A → B → A upgrades, restored backup not copied twice, no backup for damaged or missing schema 1 files, backup failure, replace failure after backup) and `tests/DrawEM.Tests/Infrastructure/Sound/ManagedSoundLibraryTests.cs` (sound-to-effect slot change, startup with mixed slots).
- Review: code, security, and spec reviewers ran on PR #60. Their findings (damaged schema 1 file taken as the backup, a second upgrade replacing newer v3 settings without a copy, invalid UTF-8 stopping startup, and stale rollback and status text) are fixed in this step. A follow-up review found that restoring an older backup and upgrading again (A → B → A) made no new backup, so "restore the newest backup" restored B; backups are now numbered and compared only with the highest-numbered one.
- Not connected when this step merged: shortcut bindings ignored effect slots, so a saved effect slot's shortcut did nothing, and the Settings window could not assign effects. S4-06 later connected the shortcuts and S4-08 the Settings window.

- Documentation review: `2867663` clarified preserving separate v3/v4 settings, cleanup on any v4 Save including migration, and numbered backups in the roadmap and plan. A subsequent documentation check clarified backing up the whole hash-named `sounds` folder before either version switch and restoring it before starting the version with matching settings. The manual version-switch check remains unverified.

## Validation

- New tests ran first and failed for the expected reasons: 15 failed (`NotSupportedException: Action type EffectAction has no schema`, missing backup, schema 2 files rejected). Review-fix tests: 5 failed first (`InvalidOperationException: Cannot transcode invalid UTF-8`, backup made for damaged schema 1 files, no second backup). A → B → A test: failed first (`settings.schema1-backup-2.json` not found).
- `dotnet test tests/DrawEM.Tests`: 588 passed, 0 failed (2026-10-11).
- `dotnet build src/DrawEM.App -c Release`: 0 warnings, 0 errors.
- Manual check, unverified: with drawEM v3 settings in `%LOCALAPPDATA%\drawEM`, start this build, press Save in Settings, and confirm `settings.schema1-backup.json` matches the v3 file and `settings.json` has `"schemaVersion": 2`; then follow the rollback steps and start v3.
