# Sound infrastructure

- `ManagedSoundLibrary` keeps drawEM's managed copies of user-selected WAV and
  MP3 files in `%LOCALAPPDATA%\drawEM\sounds`. At startup, after saved settings
  load, `App.xaml.cs` removes copies the settings do not reference; copies that
  cannot be removed go to the sound failure log.
- Sounds come from the active settings (`Application/Settings/ActiveSettings`).
  A sound's `SoundId` is its managed copy's file name. `SoundAssignments.cs` is
  the retired v2 code-owned mapping; nothing at run time reads it.
- `SoundFailureLog` appends failures to `%LOCALAPPDATA%\drawEM\logs\sound.log`:
  time, sound identifier, path, and reason. Write errors are
  swallowed so logging cannot end the tray app.
- `MediaSoundPlayerFactory` opens a `MediaSoundPlayer` (WPF `MediaPlayer`, WAV
  and MP3) for the managed copy a `PlaySoundCommand` carries. A missing copy or
  a media-engine error becomes a `SoundPlaybackException`.
- `LoggingSoundFailureReporter` queues channel failures with the managed copy
  the failed command carries, startup and Save cleanup failures with the copy
  that stayed, and a Save whose sound stop was not confirmed. It writes them to
  `SoundFailureLog` on a background task. Disposing it waits at most one second
  for queued records.
- Sound and application logs share `BackgroundLogWriter<T>` for queueing and
  bounded drain, and `TextFileLogSink` for file append. Their record formats
  remain separate.
- `SoundChannelHost` runs `SoundChannelController` on a dedicated STA
  dispatcher thread, so `MediaPlayer` can be stopped at the deadline even when
  the UI thread is busy. Call `SoundChannelHost.Play` from any thread; call
  `TryHoldStarts`, `Stop`, and `Dispose` from any thread except the sound
  thread. A Save holds player starts while it ends earlier requests, so no
  request made before the Save starts afterwards; `Stop` waits at most two
  seconds and reports whether the stop ran. See
  `docs/ARCHITECTURE.md`, "Sound thread", for the shutdown order.

`GlobalShortcutAdapter` (in `Infrastructure/Drawing`) matches each filled
slot's saved shortcut inside the keyboard hook against `ShortcutBindings`
built from the active settings, and calls `SoundChannelHost.Play` directly to
queue playback on the sound dispatcher without waiting for queued drawing
commands. A bound sound key is suppressed until released, including
auto-repeat. If the key was held before its modifiers, its first key-down
already reached the application, so its key-up passes through too. An empty
slot has no binding and its key passes through.
