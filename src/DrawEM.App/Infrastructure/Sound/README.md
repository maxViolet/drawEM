# Sound infrastructure

- `SoundAssignments.cs` holds the code-owned assignments for `Ctrl+Alt+1`
  through `Ctrl+Alt+8`. Edit it before building to point slots at external
  local WAV or MP3 files. Files are not copied into the build.
- `SoundConfiguration` resolves a slot to a `PlaySoundCommand`, or to none for
  an unassigned slot.
- `SoundFailureLog` appends failures to `%LOCALAPPDATA%\drawEM\logs\sound.log`:
  time, slot, sound identifier, configured path, and reason. Write errors are
  swallowed so logging cannot end the tray app.
- `MediaSoundPlayerFactory` looks up a sound's path and opens a
  `MediaSoundPlayer` (WPF `MediaPlayer`, WAV and MP3). An unassigned sound, a
  relative path, a missing file, or a media-engine error becomes a
  `SoundPlaybackException`.
- `LoggingSoundFailureReporter` queues channel failures with the configured
  path and writes them to `SoundFailureLog` on a background task. Disposing it
  waits at most one second for queued records.
- `SoundChannelHost` runs `SoundChannelController` on a dedicated STA
  dispatcher thread, so `MediaPlayer` can be stopped at the deadline even when
  the UI thread is busy. Call `SoundChannelHost.Play` from any thread; call
  `Dispose` from any thread except the sound thread. See
  `docs/ARCHITECTURE.md`, "Sound thread", for the shutdown order.

TODO (Stage 1, Step 3): add sound slots to the keyboard shortcut adapter. See
`../../../../docs/v2/ROADMAP-v2.md`.
