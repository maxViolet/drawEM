# v4 / S4-05: acceptance criteria

**Task:** [save and load effect assignments](task.md). **Plan:** [implementation](plan.md).

- [x] Schema 1 sound settings load with all slots, shortcuts, and file references unchanged.
- [x] Schema 2 round-trips mixed sound/effect/empty slots and stable effect IDs; no placement is written.
- [x] Every explicit v4 Save writes schema 2. A failed backup, write, or replacement leaves previous settings recoverable; loading alone never rewrites settings, and an existing migration backup is not overwritten.
- [x] Unknown types/IDs yield a useful error and preserve the damaged file; storage/migration tests record results.
- [x] Rollback instructions explain restoring the backup with drawEM closed and the limits for sounds imported or deleted after that backup, including older-app startup cleanup.

## Evidence

- Schema: `src/DrawEM.App/Infrastructure/Settings/SettingsJson.cs`. `CurrentSchemaVersion` is 2; the reader accepts 1 and 2. An effect is written as `{ "type": "effect", "effectId": "<id>", "shortcut": "<keys>" }`. A schema 1 file with an `effect` action, an unknown type, an empty `effectId`, or an ID missing from `EffectCatalog` is unreadable, and the reason names the slot and the type or ID.
- Migration backup: `JsonSettingsStore.Save` copies a schema 1 `settings.json` to `settings.schema1-backup.json` before the first replacement, through a flushed temporary file. A failed backup, or a current file that cannot be read, stops the Save before anything is written. An existing backup is kept.
- Sound accounting: `ManagedSoundLibrary` counts only `SoundAction` slots, so changing a slot to an effect removes the copy only after its last saved sound reference; startup cleanup with a mixed schema 2 file keeps saved sound copies.
- Rollback: "Rolling back to a v3 drawEM" in `src/DrawEM.App/Infrastructure/Settings/README.md`.
- Tests: `tests/DrawEM.Tests/Infrastructure/Settings/JsonSettingsStoreTests.cs` (schema 1 load without writes, schema 2 round-trip and payload shape, invalid schema 2 files, effect in schema 1, backup creation, backup kept on later saves and after a rollback, no backup without a schema 1 file, backup failure, replace failure after backup) and `tests/DrawEM.Tests/Infrastructure/Sound/ManagedSoundLibraryTests.cs` (sound-to-effect slot change, startup with mixed slots).
- Not yet connected: shortcut bindings (S4-06) ignore effect slots, so a saved effect slot's shortcut does nothing yet; the Settings window (S4-08) cannot assign effects.

## Validation

- New tests ran first and failed for the expected reasons: 15 failed (`NotSupportedException: Action type EffectAction has no schema`, missing backup, schema 2 files rejected).
- `dotnet test tests/DrawEM.Tests`: 582 passed, 0 failed (2026-10-11).
- `dotnet build src/DrawEM.App -c Release`: 0 warnings, 0 errors.
- Manual check, unverified: with drawEM v3 settings in `%LOCALAPPDATA%\drawEM`, start this build, press Save in Settings, and confirm `settings.schema1-backup.json` matches the v3 file and `settings.json` has `"schemaVersion": 2`; then follow the rollback steps and start v3.
