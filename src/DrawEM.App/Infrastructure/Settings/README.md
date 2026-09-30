# Settings storage

`JsonSettingsStore` keeps the current Windows user's settings in
`%LOCALAPPDATA%\drawEM\settings.json`. Domain and Application see only
`SettingsSnapshot` and the `ISettingsStore` port.

## Schema version 1

```json
{
  "schemaVersion": 1,
  "drawing": {
    "color": "#FF4500",
    "width": 4,
    "drawShortcut": "Ctrl+Alt+Z",
    "clearShortcut": "Ctrl+Alt+X"
  },
  "slots": [
    { "slot": 1, "action": { "type": "sound", "id": "<library id>", "name": "applause.wav", "shortcut": "Ctrl+Alt+1" } },
    { "slot": 2, "action": null }
  ]
}
```

- `slots` lists slots 1–8 in order. `"action": null` is an empty slot.
- `type` selects the action. v3 defines only `sound`; later versions add types
  without changing the slot entry.
- `id` is the opaque managed-library identity. Only the media adapter maps it
  to a file; settings code never interprets it. `name` is
  the original file name shown to the user.
- A shortcut is at least two of `Ctrl`, `Alt`, `Shift`, then one key `A`–`Z`,
  `0`–`9`, or `F1`–`F12`. `Alt` means Left Alt; Right Alt is reserved for AltGr
  and has no token. The file is written in canonical order (`Ctrl+Alt+Shift+K`).
- The loaded file must pass the same validation as the Settings window: HEX
  color, width 1–20, draw and clear shortcuts present, a shortcut on every
  filled slot, and no duplicate active shortcut.

## Version policy

- The writer always writes `CurrentSchemaVersion` (1).
- The reader accepts only versions it knows. A missing, non-integer, older
  unknown, or newer version is reported as unsupported, with the version number
  in the reason; the file is treated like any other unreadable file.
- A later schema bumps the version and adds a forward migration from every
  earlier version in the reader. A migration never rewrites the file on load;
  the new version is written on the next Save.
- v2 kept sound assignments in code, so there is nothing to migrate: a first
  v3 launch has no file and starts with defaults and eight empty slots.

## Durable writes and failures

- Save writes `settings.json.tmp`, flushes it to disk, then moves it over
  `settings.json`. A failed write or move raises `SettingsStoreException`,
  deletes the temporary file when possible, and leaves the previous
  `settings.json` unchanged. A temporary file left by a crash is overwritten by
  the next Save.
- Load never writes `settings.json`. A missing file means first launch. A file
  that cannot be read (for example, locked) is reported without a copy. A
  malformed, invalid, or unsupported file is reported, and its bytes are kept as
  `settings.unreadable-<UTC timestamp>.json`; the same bytes reuse one copy
  across launches. The damaged `settings.json` stays until the user saves.
- `SettingsStartup` falls back to defaults on a missing or unusable file, reports
  the failure, and never saves those defaults.
