# v5 / S5-04: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] [gate] A shortcut starts a saved video once on key-down; key hold and
  auto-repeat do not start additional instances.
- [ ] [gate] A fresh press of the same shortcut restarts at the beginning;
  tests cover Loop off and Loop on.
- [ ] [gate] Cursor transition stops video on the old monitor, never resumes
  it on return, and a new invocation targets the new cursor monitor.
- [ ] [gate] Tray > Stop video stops every active video while preserving
  drawings, effects, and global sound.
- [ ] [gate] The default Clear shortcut (`Ctrl+Alt+X`) and a changed configured
  Clear shortcut both remove drawings and stop video/effects on the cursor
  monitor while preserving sound and actions on other monitors.
- [ ] [gate] Pressing a different video slot during playback replaces the
  current video; slot playback stops an active Settings Sample.
- [ ] [gate] Display topology change or removal of the playing monitor stops
  playback, releases its resources, and does not resume it automatically.
- [ ] [gate] Exit, session lock, and system sleep stop video and release all
  player/surface resources; unlock/wake does not restart playback.
- [ ] [gate] Forced missing-file, decoder, and stop failures are reported and
  leave the tray application available for another action.
- [ ] [gate] Windows manual input check confirms the target application keeps
  focus and receives keyboard, mouse, and wheel input during playback.
- [ ] [record] Record event traces, failure log entries, tested Windows
  version, and manual session-interruption results.

## Evidence

No implementation or lifecycle evidence has been recorded. All criteria
remain unverified.
