# v5 / S5-04: connect controls and lifecycle

**Status:** planned. **Source:** [v5 roadmap](../ROADMAP-v5.md#delivery-order).
**Depends on:** [S5-03](../step-03-fullscreen-video-playback/task.md).
**Next:** [S5-05](../step-05-video-publish/task.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Task

Connect saved video actions to shortcut dispatch and provide the agreed stop
routes. Ensure a video never remains on a monitor after the cursor moves away,
and stop playback during shutdown or Windows session interruption.

## Required behavior

- A saved video shortcut starts on key-down; holding it does not repeat.
- Pressing the same shortcut again after release stops the current instance
  and starts it again from the beginning.
- Moving the cursor to another monitor stops video on the previous monitor and
  does not restart it when the cursor returns.
- Tray > Stop video stops video on every monitor and leaves drawings, effects,
  and global sound alone.
- Clear (default `Ctrl+Alt+X`; user-configurable) clears drawings and stops
  video and effects on only the monitor under the cursor. Global sound
  continues. This v5 behavior intentionally extends the v4 clear contract.
- A different video slot shortcut replaces active video on the invocation
  monitor. Any slot shortcut stops Settings Sample before starting the saved
  assignment.
- Display topology changes or removal of the playing monitor stop playback
  and release resources without automatic restart.
- Exit, session lock, and system sleep stop video and release playback
  resources. Playback does not resume after unlock or wake.
- A missing or corrupt file, decoder failure, or stop timeout is recoverable;
  the tray application stays available and reports the failure.

## Out of scope

- Changing the accepted input formats or decoder support profile from S5-01.
- Pause, resume, seek, or stop-on-click controls.
- Stopping global sound when video stops.
