# Stage 1 / Step 3: acceptance criteria

**Task:** [sound shortcuts](task.md). **Plan:** [implementation](plan.md).

- [x] An assigned `Ctrl+Alt+1` through `Ctrl+Alt+8` slot queues exactly one sound command on the first key-down of a press.
- [x] Repeated key-down while held queues no additional command; releasing and pressing again queues a new command.
- [x] An unassigned slot queues no sound command.
- [x] An assigned slot key is suppressed from press to release, including auto-repeat; if the digit was held before `Ctrl+Alt`, only its first key-down and its key-up pass through; an unassigned slot and a digit without `Ctrl+Alt` pass through outside draw mode.
- [x] The hook callback does not open a file, decode media, or start the player.
- [x] The hook queues sound directly through `SoundChannelHost.Play`, without waiting for the UI Dispatcher or queued drawing commands; playback starts only when the sound Dispatcher runs.
- [x] A sound command during an active stroke leaves that stroke and the drawing gate active; keyboard and pointer suppression rules for the underlying app remain in effect.
- [x] Existing `Ctrl+Alt+Z` hold/release and monitor-scoped `Ctrl+Alt+X` tests still pass, together with the new shortcut tests. Recorded: `dotnet test DrawEM.sln --no-restore -m:1` on 2026-09-28 — Passed 117, Failed 0.

Actual global-hook behavior and input blocking are checked in [Step 4](../step-4-publish/acceptance.md).
