# Sound infrastructure

- `SoundAssignments.cs` holds the code-owned assignments for `Ctrl+Alt+1`
  through `Ctrl+Alt+8`. Edit it before building to point slots at external
  local WAV or MP3 files. Files are not copied into the build.
- `SoundConfiguration` resolves a slot to a `PlaySoundCommand`, or to none for
  an unassigned slot.
- `SoundFailureLog` appends failures to `%LOCALAPPDATA%\drawEM\logs\sound.log`:
  time, slot, sound identifier, configured path, and reason. Write errors are
  swallowed so logging cannot end the tray app.

TODO (Stage 1, Step 2): add the Windows playback adapter. See
`../../../../docs/v2/ROADMAP-v2.md`.
