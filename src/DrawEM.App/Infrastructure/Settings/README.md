# Settings storage

`JsonSettingsStore` keeps the current Windows user's settings in
`%LOCALAPPDATA%\drawEM\settings.json`. Domain and Application see only
`SettingsSnapshot` and the `ISettingsStore` port.

## Schema version 2

```json
{
  "schemaVersion": 2,
  "drawing": {
    "color": "#FF4500",
    "width": 4,
    "drawShortcut": "Ctrl+Alt+Z",
    "clearShortcut": "Ctrl+Alt+X"
  },
  "slots": [
    { "slot": 1, "action": { "type": "sound", "file": "<library file>", "name": "applause.wav", "shortcut": "Ctrl+Alt+1" } },
    { "slot": 2, "action": { "type": "effect", "effectId": "confetti", "shortcut": "Ctrl+Alt+2" } },
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
- A shortcut is at least two of `Ctrl`, `Alt`, `Shift`, then one key `A`–`Z`,
  `0`–`9`, or `F1`–`F12`. `Alt` means Left Alt; Right Alt is reserved for AltGr
  and has no token. The file is written in canonical order (`Ctrl+Alt+Shift+K`).
- The loaded file must pass the same validation as the Settings window: HEX
  color, width 1–20, draw and clear shortcuts present, a shortcut on every
  filled slot, a known effect ID in every effect slot, and no duplicate active
  shortcut.

Schema 1 (v3) has the same layout and allows only `sound` actions.

## Version policy

- The writer always writes `CurrentSchemaVersion` (2), including when every
  slot holds a sound or is empty.
- The reader accepts versions 1 and 2. A missing, non-integer, older unknown,
  or newer version is reported as unsupported, with the version number in the
  reason; the file is treated like any other unreadable file.
- An unknown action type, an `effect` action in a schema 1 file, an empty
  `effectId`, or an ID the catalog does not know makes the file unreadable,
  with the slot number and the type or ID in the reason. The slot is never
  silently emptied.
- Schema 1 loads into the same model with its sounds, shortcuts, and file
  names unchanged. A migration never rewrites the file on load; the new version
  is written on the next Save.
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
- Before Save replaces a schema 1 `settings.json`, it copies the file's bytes
  to `settings.schema1-backup.json` (through a flushed temporary file, so the
  backup is complete or absent). If the backup cannot be made, or the current
  file cannot be read to check its version, Save raises
  `SettingsStoreException` and writes nothing. An existing backup is never
  overwritten, so after a rollback and a second upgrade it still holds the
  first schema 1 file. A missing, damaged, or schema 2 file gets no backup.
- Load never writes `settings.json` and never creates the backup. A missing
  file means first launch. A file that cannot be read (for example, locked) is
  reported without a copy. A malformed, invalid, or unsupported file is
  reported, and its bytes are kept as
  `settings.unreadable-<UTC timestamp>.json`; the same bytes reuse one copy
  across launches. The damaged `settings.json` stays until the user saves.
- `SettingsStartup` falls back to defaults on a missing or unusable file, reports
  the failure, and never saves those defaults.

## Rolling back to a v3 drawEM

A v3 drawEM reads only schema 1, so it reports a schema 2 file as unsupported
and starts with defaults. To return to v3 with the last v3 settings:

1. Exit drawEM from the tray icon, and check that no `DrawEM.App.exe` process
   is running. A running drawEM may save over the restored file.
2. In `%LOCALAPPDATA%\drawEM`, copy `settings.schema1-backup.json` to
   `settings.json`, replacing it. Keep the backup.
3. Start the v3 drawEM.

Limits:

- The backup holds the settings from before the first v4 Save. Changes saved in
  v4 after that, including effect slots, are lost.
- A v4 Save removes managed sound copies that its settings no longer reference.
  If a later v4 Save removed a copy that the backup names, that slot's sound is
  missing after the rollback: the slot stays assigned, and each press logs a
  failure to `logs\sound.log` until a file is chosen again.
- Sounds imported in v4 after the backup are not named by the backup. The v3
  startup cleanup removes every managed copy that the loaded settings do not
  reference, so those copies are deleted at the first v3 launch.
- If the restored file cannot be loaded, v3 starts with defaults and removes
  nothing.
