# v4 / S4-05: acceptance criteria

**Task:** [save and load effect assignments](task.md). **Plan:** [implementation](plan.md).

- [x] Schema 1 sound settings load with all slots, shortcuts, and file references unchanged.
- [x] Schema 2 round-trips mixed sound/effect/empty slots and stable effect IDs; no placement is written.
- [x] Every explicit v4 Save writes schema 2. A failed backup, write, or replacement leaves previous settings recoverable; loading alone never rewrites settings, and an existing migration backup is not overwritten.
- [x] Unknown types/IDs yield a useful error and preserve the damaged file; storage/migration tests record results.
- [x] Rollback instructions explain restoring the backup with drawEM closed and the limits for sounds imported or deleted after that backup, including older-app startup cleanup.

## Evidence

- Schema: `src/DrawEM.App/Infrastructure/Settings/SettingsJson.cs`. `CurrentSchemaVersion` is 2; the reader accepts 1 and 2. An effect is written as `{ "type": "effect", "effectId": "<id>", "shortcut": "<keys>" }`. A schema 1 file with an `effect` action, an unknown type, an empty `effectId`, an ID missing from `EffectCatalog`, or a string that is not valid UTF-8 is unreadable, and the reason names the slot and the type or ID, or the property.
- Migration backup: `JsonSettingsStore.Save` copies a schema 1 `settings.json` that passes every Load check to `settings.schema1-backup.json` before the first replacement, through a flushed temporary file. That backup is never overwritten. After a rollback, a different schema 1 file gets a timestamped `settings.schema1-backup-<UTC>.json`; bytes already in a backup are not copied again. A damaged file gets no backup. A failed backup, or a current file that cannot be read, stops the Save before anything is written.
- Sound accounting: `ManagedSoundLibrary` counts only `SoundAction` slots, so saving a slot changed from sound to effect removes the copy only after its last saved sound reference; startup cleanup with a mixed schema 2 file keeps saved sound copies. Cancel after a sound-to-effect change in the Settings window is not reachable until S4-08 and is checked in S4-10.
- Rollback: "Rolling back to a v3 drawEM" in `src/DrawEM.App/Infrastructure/Settings/README.md`, including keeping the v4 file, choosing the newest backup, and the case with no backup.
- Tests: `tests/DrawEM.Tests/Infrastructure/Settings/JsonSettingsStoreTests.cs` (schema 1 load without writes, schema 2 round-trip and payload shape, invalid schema 2 files, invalid UTF-8, effect in schema 1, backup creation, second upgrade after a rollback, restored backup not copied twice, no backup for damaged or missing schema 1 files, backup failure, replace failure after backup) and `tests/DrawEM.Tests/Infrastructure/Sound/ManagedSoundLibraryTests.cs` (sound-to-effect slot change, startup with mixed slots).
- Review: code, security, and spec reviewers ran on PR #60. Their findings (damaged schema 1 file taken as the backup, a second upgrade replacing newer v3 settings without a copy, invalid UTF-8 stopping startup, and stale rollback and status text) are fixed in this step.
- Not yet connected: shortcut bindings (S4-06) ignore effect slots, so a saved effect slot's shortcut does nothing yet; the Settings window (S4-08) cannot assign effects.

## Validation

- New tests ran first and failed for the expected reasons: 15 failed (`NotSupportedException: Action type EffectAction has no schema`, missing backup, schema 2 files rejected). Review-fix tests: 5 failed first (`InvalidOperationException: Cannot transcode invalid UTF-8`, backup made for damaged schema 1 files, no second backup).
- `dotnet test tests/DrawEM.Tests`: 587 passed, 0 failed (2026-10-11).
- `dotnet build src/DrawEM.App -c Release`: 0 warnings, 0 errors.
- Manual check, unverified: with drawEM v3 settings in `%LOCALAPPDATA%\drawEM`, start this build, press Save in Settings, and confirm `settings.schema1-backup.json` matches the v3 file and `settings.json` has `"schemaVersion": 2`; then follow the rollback steps and start v3.
