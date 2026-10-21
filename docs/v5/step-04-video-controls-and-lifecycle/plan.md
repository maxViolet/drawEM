# v5 / S5-04: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Routing tests first.** Add failing tests for a video slot shortcut,
   key-down versus held-key behavior, same-shortcut restart, cursor monitor
   transition, and isolation from sound/drawing/effect actions.
2. **Shortcut composition.** Resolve saved `VideoAction` slots through the
   existing shortcut binding path. Preserve one shortcut to one action and
   suppress a bound shortcut for the same key lifecycle as sound/effect
   actions.
3. **Restart behavior.** A fresh press of the currently playing slot ends the
   old instance and starts at time zero. Test that loop mode does not turn a
   shortcut press into pause or stop.
4. **Monitor transition.** Connect active-monitor changes to video stop. Test
   that returning the cursor never resumes the old playback and that an
   invocation on the new monitor targets that monitor.
5. **Stop commands.** Add tray > Stop video for all monitors. Extend clear so
   it clears drawings and stops video/effects on the cursor monitor while
   preserving the global sound action.
6. **Session lifecycle.** Subscribe to Windows lock and suspend events and the
   application exit path. Hold starts while stopping, release every player and
   surface, and never resume automatically after unlock or wake.
7. **Failure behavior.** Force missing-file, decoder, and stop failures. Route
   a user-readable error and failure log entry; keep the tray host running.
8. **Manual integration.** Check shortcut suppression and pass-through with an
   unrelated focused application, plus menu stop, clear, cursor transition,
   lock, sleep, and exit on Windows.
