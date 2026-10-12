# v4 / S4-07: acceptance criteria

**Task:** [stop effects when switching monitors](task.md). **Plan:** [implementation](plan.md).

- [x] Moving within the target monitor continues the effect, and a Cursor effect stays at its captured point; entering another connected monitor stops it and returning does not restart it. Both placements behave the same. Automated: `EffectMonitorGuardTests` for both placements. On one monitor only: confetti kept playing while the pointer moved and pushed past the outside edges. Entering another monitor is unverified on screen (see the last criterion).
- [x] A -> B -> A transitions cannot let the old effect survive, even if UI dispatch is delayed. `LeavingAndReturningBeforeTheUiRuns_StillStopsTheEffect_AndNeverRestartsIt` runs both moves before the queued UI work.
- [x] Touching an outside edge without entering a monitor does not stop the effect. `TouchingAnOutsideDesktopEdge_DoesNotStopTheEffect`, and on screen as above.
- [x] A display-change listener, monitor identity/generation, and surface rebuild are present before this step completes. `Win32DisplayChangeSource` listens to `SystemEvents.DisplaySettingsChanged`; every topology change advances the monitor epoch and drops the cached monitor, so monitors are compared only within one topology, not by bounds alone; the effect stops, which closes its window, and the next effect gets a new window placed on the new layout.
- [x] Tests cover movement outside draw mode, unreported cursor movement caught by the frame check, no target monitor, queued-start ordering, and topology change. `EffectMonitorGuardTests` (the guard listens to the mouse hook directly, with no drawing state involved), `EffectShortcutTests` (every queued start is dequeued with the epoch of its press, including dropped ones), and `StopStartedBefore_StopsOnlyAnInstanceFromAnEarlierMonitorEpoch`.
- [ ] Two-monitor Windows checks confirm the expected behavior, not only record it: the effect stops on entering the other monitor, does not restart on return, survives movement inside its monitor and at an outside edge, and a Cursor effect does not move. Any deviation is a failure of this step. Unverified: the test PC had one monitor. See [manual check](#two-monitor-manual-check).
- [x] Ordinary pointer movement on one monitor does not enqueue per-move UI work while an effect runs; with no effect running/queued it adds no monitor lookup to the hook. `MovingWithinTheTargetMonitor_KeepsTheEffect_AndQueuesNoUiWork`, `FrameCheck_OnTheTargetMonitor_QueuesNoUiWork`, `WithNoEffectRunningOrQueued_PointerMovesDoNoMonitorLookup`, `AfterTheEffectEnds_PointerMovesDoNoMonitorLookup`, `FrameCheck_WithNoEffect_ReadsNothing`.

## Implementation

- `Infrastructure/Effects/EffectMonitorGuard.cs`: subscribes to the mouse hook and the display-change source. A press enqueues its start at key-down (no monitor lookup in the hook) and the guard observes while a start is queued or the channel has an active instance; otherwise a pointer move costs one flag check. A point inside the cached monitor bounds does nothing. A point outside them is looked up once: outside every monitor is an outside edge and is ignored; another monitor advances the monitor epoch and queues one `StopStartedBefore(epoch)`. `CheckCursor` runs the same test with `GetCursorPos` before each animation frame. A display change advances the epoch, forgets the cached monitor, and stops the effect at once.
- `Infrastructure/Effects/IEffectStartQueue.cs`: the shortcut path's effect port. `GlobalShortcutAdapter` enqueues at key-down and dequeues on the dispatcher exactly once, with `null` when a Save or a missing monitor dropped the start. The guard starts a command only if its epoch is still current.
- `Infrastructure/Effects/Win32DisplayChangeSource.cs`, `IDisplayChangeSource.cs`: the display-change listener, raised on the UI thread.
- `Application/Effects/StartEffectCommand.cs`: adds `MonitorEpoch`. `EffectChannelController.StopStartedBefore(epoch)` stops only an instance from an earlier epoch, so a stop queued before a newer start cannot end it.
- `Presentation/Effects/LottieEffectSurface.cs`, `LottiePlayback.cs`: `FrameStarting` is raised before each frame is drawn; `App.xaml.cs` connects it to `EffectMonitorGuard.CheckCursor`.

## Validation

- Test first: `EffectMonitorGuardTests`, the new `EffectShortcutTests` cases, and the `StopStartedBefore` test first failed to build for the expected reason: `EffectMonitorGuard`, `IDisplayChangeSource`, `IEffectStartQueue`, `StartEffectCommand.MonitorEpoch`, and `EffectChannelController.StopStartedBefore` did not exist.
- `dotnet build DrawEM.sln -warnaserror`: succeeded, no warnings.
- `dotnet test DrawEM.sln`: 612 passed, 0 failed.
- Published-app checks, 2026-10-12, branch `S4-07-effect-monitor-transitions`, one monitor (0, 0, 1920, 1200) at 100%, keys and moves sent with `keybd_event`, `mouse_event`, and `SetCursorPos`:
  - Confetti kept playing after 40 relative pointer moves that pushed past the top-left and bottom-right desktop edges.
  - Confetti was gone 300 ms after a broadcast `WM_DISPLAYCHANGE`. Not conclusive: a full-screen game started during this run and may have changed the display mode itself.
- Unverified: everything that needs a second monitor.

## Two-monitor manual check

Needs two monitors, ideally with different scaling, and a published build with Ctrl+Alt+1 bound to Confetti and Ctrl+Alt+2 bound to Focus ring:

1. Press Ctrl+Alt+1 on monitor A and move the mouse around A and against its outside edges. Expected: confetti keeps playing.
2. Press Ctrl+Alt+1 on A and move into monitor B. Expected: the confetti disappears as the pointer enters B. Move back to A: nothing restarts.
3. Press Ctrl+Alt+1 on A, move quickly into B and back to A. Expected: the confetti is gone.
4. Repeat steps 1-3 with Ctrl+Alt+2. Expected: the same, and the ring never moves from the point where the key was pressed.
5. Press Ctrl+Alt+1, then change the display scale or disconnect a monitor. Expected: the effect stops; the next press shows it on the new layout.
