# Settings storage

`JsonSettingsStore` keeps the current Windows user's settings in
`%LOCALAPPDATA%\drawEM\settings.json`. Domain and Application see only
`SettingsSnapshot` and the `ISettingsStore` port.

## Schema version 3

```json
{
  "schemaVersion": 3,
  "drawing": {
    "color": "#FF4500",
    "width": 4,
    "drawShortcut": "Ctrl+Alt+Z",
    "clearShortcut": "Ctrl+Alt+X"
  },
  "slots": [
    { "slot": 1, "action": { "type": "sound", "file": "<library file>", "name": "applause.wav", "shortcut": "Ctrl+Alt+1" } },
    { "slot": 2, "action": { "type": "effect", "effectId": "confetti", "shortcut": "Ctrl+Alt+2", "loop": false, "maxSeconds": 10 } },
    { "slot": 3, "action": null }
  ]
}
```

- `slots` lists slots 1–8 in order. `"action": null` is an empty slot.
- `type` selects the action: `sound` or `effect`. Later versions add types
  without changing the slot entry.
- `file` names the managed library copy (a file name, never a path). `name` is
  the original file name shown to the user.
- `effectId` names a built-in effect in `Domain/Effects/EffectCatalog`. The file
  stores only the ID: placement and the Lottie resource come from the catalog,
  so a release can change them without a settings migration.
- `loop` controls whether the Lottie animation restarts at its end. While it is
  `true`, `maxSeconds` has no effect.
- `maxSeconds` is an integer from 1 through 60, or `null` for no time limit.
  The default for effect actions migrated from schema 2 is 10.
- A shortcut is at least two of `Ctrl`, `Alt`, `Shift`, then one key `A`–`Z`,
  `0`–`9`, or `F1`–`F12`. `Alt` means Left Alt; Right Alt is reserved for AltGr
  and has no token. The file is written in canonical order (`Ctrl+Alt+Shift+K`).
- The loaded file must pass the same validation as the Settings window: HEX
  color, width 1–20, draw and clear shortcuts present, a shortcut on every
  filled slot, a known effect ID and a valid maximum length in every effect
  slot, and no duplicate active shortcut.

Schema 1 (v3) has the same layout and allows only `sound` actions. Schema 2
adds effect actions without `loop` and `maxSeconds`; they load as Loop off and
10 seconds.

## Version policy

- The writer always writes `CurrentSchemaVersion` (3), including when every
  slot holds a sound or is empty.
- The reader accepts versions 1, 2, and 3. A missing, non-integer, older
  unknown, or newer version is reported as unsupported, with the version
  number in the reason; the file is treated like any other unreadable file.
- An unknown action type, an `effect` action in a schema 1 file, an empty
  `effectId`, an ID the catalog does not know, a non-boolean `loop`, or a
  non-integer or out-of-range `maxSeconds` makes the file unreadable, with the
  slot number and invalid field in the reason. The slot is never silently
  emptied.
- Schemas 1 and 2 load into the current model with their sounds, shortcuts,
  and file names unchanged. Effect actions from schema 2 receive the playback
  defaults. A migration never rewrites the file on load; schema 3 is written
  on the next Save.
- A later schema bumps the version and adds a forward migration from every
  earlier version in the reader.
- v2 kept sound assignments in code, so there is nothing to migrate: a first
  v3 launch has no file and starts with defaults and eight empty slots.

## Durable writes and failures

- Save writes `settings.json.tmp`, flushes it to disk, then moves it over
  `settings.json`. A failed write or move raises `SettingsStoreException`,
  deletes the temporary file when possible, and leaves the previous
  `settings.json` unchanged. A temporary file left by a crash is overwritten by
  the next Save.
- Before Save replaces a schema 1 `settings.json` that passes every Load check,
  it copies the file's bytes to a backup through a flushed temporary file, so
  each backup is complete or absent. The first backup is
  `settings.schema1-backup.json` (number 1). Each later migration (after a
  rollback to v3) writes `settings.schema1-backup-<N>.json` with the next
  number, so the highest number always holds the file the latest migration
  replaced. Nothing is copied when the highest-numbered backup already has the
  same bytes, and no backup is overwritten. If the backup cannot be made, or the
  current file or the backups cannot be read, Save raises
  `SettingsStoreException` and writes nothing. A missing, schema 2 or 3, or
  damaged file gets no backup: a damaged file was already kept by Load as an
  unreadable copy.
- Load never writes `settings.json` and never creates the backup. A missing
  file means first launch. A file that cannot be read (for example, locked) is
  reported without a copy. A malformed (including a string that is not valid
  UTF-8), invalid, or unsupported file is reported, and its bytes are kept as
  `settings.unreadable-<UTC timestamp>.json`; the same bytes reuse one copy
  across launches. The damaged `settings.json` stays until the user saves.
- `SettingsStartup` falls back to defaults on a missing or unusable file, reports
  the failure, and never saves those defaults.

## Rolling back to a v3 drawEM

A v3 drawEM reads only schema 1, so it reports a schema 3 file as unsupported
and starts with defaults. To return to v3 with the settings v4 replaced:

1. Exit drawEM from the tray icon, and check that no `DrawEM.App.exe` process
   is running. A running drawEM may save over the restored file. Copy the whole
   `%LOCALAPPDATA%\drawEM\sounds` folder, if present, to a new backup folder
   outside `%LOCALAPPDATA%\drawEM`. Keep it with the saved v4 settings;
   library file names are SHA-256 hashes, so preserve their names unchanged.
2. In `%LOCALAPPDATA%\drawEM`, rename `settings.json` to `settings.v4.json` to
   keep the v4 settings. If that name already exists, use a new unused name,
   such as `settings.v4-<date-time>.json`; keep every previous copy.
3. Copy the backup with the highest number to `settings.json`: the
   `settings.schema1-backup-<N>.json` with the largest `N`, or
   `settings.schema1-backup.json` when there is no numbered backup. Keep the
   backups. Older numbers hold v3 settings replaced by earlier migrations.
4. If you have a sound-folder backup for the v3 settings you restored, copy
   its files back into `%LOCALAPPDATA%\drawEM\sounds`, keeping their names,
   before starting v3. Start the v3 drawEM.

With no backup, v4 never replaced a v3 file. A schema 1 `settings.json` needs
no change. A schema 2 or 3 one was created by v4; v3 starts with defaults and
removes no sounds.

To upgrade again, exit drawEM first and copy the whole
`%LOCALAPPDATA%\drawEM\sounds` folder, if present, to a new external backup
folder kept with the current v3 settings. Start v4 with the current v3
`settings.json` and Save to migrate those settings with an automatic backup.
To resume the saved v4 settings instead, first rename the current v3
`settings.json` to an unused `settings.v3-<date-time>.json`, then copy the
chosen saved v4 file to `settings.json`. This path skips migration of the v3
file, so keep that copy yourself. Before starting v4 with the saved v4 settings,
copy the files from their matching sound-folder backup back into
`%LOCALAPPDATA%\drawEM\sounds`, keeping their names. Keep all external
backups: startup cleanup may remove files used only by the other settings.

Limits:

- A backup holds the settings from just before a v4 Save replaced a schema 1
  file. Changes saved in v4 after that, including effect slots, are not in it.
- Any v4 Save, including the one that made the backup, can remove managed sound
  copies that its settings no longer reference. If that removed a copy the
  backup names, that slot's sound is missing after the rollback: the slot stays
  assigned, and each press logs a failure to `logs\sound.log` until a file is
  chosen again. A sound-folder backup made after deletion cannot recover it.
- Sounds imported after the backup are not named by it. The v3 startup cleanup
  removes every managed copy that the loaded settings do not reference, so
  those copies are deleted at the first v3 launch, even if `settings.v4.json`
  still uses them.
- When resuming saved v4 settings, the first v4 startup cleanup removes managed
  sounds imported in v3 that those v4 settings do not reference, even if a
  saved `settings.v3-<date-time>.json` still uses them. Preserve the whole
  sound folder before either version switch, and restore its files before
  starting the version with the matching settings, as described above.
- If the restored file cannot be loaded, v3 starts with defaults and removes
  nothing.
