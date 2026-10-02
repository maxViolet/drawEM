# v3 / Step 2: acceptance criteria

**Task:** [managed sound library](task.md). **Plan:** [implementation](plan.md).

- [x] WAV and MP3 selections are copied into the user-profile library; source
  files are never moved or deleted.
- [x] Multiple slots can share a managed copy. Cancel and replacement preserve
  every copy still referenced by saved settings.
- [x] Successful Save removes a copy after its last saved reference disappears.
  A failed Save does not remove or change previously saved media.
- [x] Successful startup load removes orphan copies, including a draft import
  left by forced exit. Unreadable settings prevent startup cleanup.
- [x] Automated filesystem tests exercise the rules above and record command
  and result. Audible playback remains for [Step 8](../step-8-publish/task.md).

## Validation

- `dotnet test` (2026-09-30): 282 passed, 0 failed. `ManagedSoundLibraryTests`
  (`Infrastructure/Sound`) runs in a temporary directory and covers WAV/MP3
  import with untouched sources, rejected types, missing, empty, locked, and
  in-library sources, damaged-copy repair, shared copies, Cancel,
  replacement, last-reference removal, locked copy reporting, unmanaged names
  left alone, failed Save, forced-exit orphan cleanup, and skipped cleanup for
  unreadable or missing settings.
- Library layout: `%LOCALAPPDATA%\drawEM\sounds`. A copy is named by the
  SHA-256 of its content plus `.wav` or `.mp3`, so identical content with the
  same extension shares one copy; the same bytes selected as `.wav` and as
  `.mp3` make two copies. Only these names and leftover `import-*.tmp` files are ever removed.
- Not wired into the running app yet. Startup cleanup
  (`SettingsStartup.RemoveOrphanSounds`) and Save (`SettingsPersistence.Save`)
  are connected in [Step 6](../step-6-runtime-settings/task.md); import and Cancel in
  [Step 7](../step-7-settings-window/task.md).
