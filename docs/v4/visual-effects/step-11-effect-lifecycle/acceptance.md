# v4 / S4-11: acceptance criteria

**Task:** [stop effects through app lifecycle](task.md). **Plan:** [implementation](plan.md).

- [ ] Tray Stop effects removes the active visual and pending starts while preserving sound and drawings. Automated through the real `TrayApplication` and `EffectStopRoutes`: `EffectLifecycleTests.Event_RemovesTheRunningEffect_AndAStartQueuedBeforeItNeverAppears("tray Stop effects")`, `Interruption_KeepsDrawingsAndSound("tray Stop effects")` (a completed stroke and the playing sound stay; the player gets no stop), `TrayApplicationTests.TrayStopEffects_StopsEffectsUntilExit_AndReleasesNothingElse`. Unverified: the tray menu item in the running app (see [desktop checks](#windows-desktop-checks)).
- [ ] Each of display change, lock, suspend, exit, and partial startup failure removes visible effects and cancels pending starts; a start queued before the event never appears after it. Unlock/resume never restarts an old effect. Automated, each through its real wiring (`Win32SessionInterruptionSource.Report` for lock and suspend, `EffectStopRoutes.Connect`, the tray host's Exit through `TrayApplication`): `Event_RemovesTheRunningEffect_AndAStartQueuedBeforeItNeverAppears` for tray Stop effects, session lock, system suspend, display change, and tray Exit (a start is queued, then the event, then the UI queue drains: nothing new appears); `AfterAnInterruption_NothingRestartsByItself_ButAFreshPressStarts`; `SessionSwitchOtherThanLock_KeepsTheEffect` and `PowerChangeOtherThanSuspend_KeepsTheEffect` (unlock and resume do nothing); `TrayApplicationTests.TrayExit_StopsEffectsAtOnce_BeforeTheScheduledExitRuns`. Unverified: partial startup failure (`App.ReleaseStartedResources` runs only inside a WPF app, so no automated test covers it), the one `EffectStopRoutes.Connect` line in `App.xaml.cs`, and real Windows events.
- [x] Runtime effect failures are written to the local log, with partial visuals cleared and callbacks detached; a test forces a failure and checks the log entry. Sample failures stay visible in Settings. `EffectLifecycleTests.RuntimeFailure_RemovesTheEffect_DetachesItsCallbacks_AndIsWrittenToTheLocalLog` (real `LoggingEffectFailureReporter` and `EffectFailureLog` on a temp file: the playback is disposed, has neither a `Failed` nor a `Completed` handler, its deadline is gone, and the log has one line). Sample failures: existing `EffectSamplerTests.SampleThatCannotShow_ReportsInSettings_AndLeavesNothingOnScreen` and `SampleThatFailsWhilePlaying_ReportsInSettings_AndReleasesItsWindow`.
- [ ] Repeated cleanup leaves no effect window, callback, or live instance; startup failure does not strand a surface. Automated: `RepeatedEvent_IsSafe_AndLeavesNoEffectOrDeadline` for every event above (one removal, no active instance, no deadline timer). Unverified: startup failure (see above), and window and handle counts in the running app.
- [ ] Automated tests cover pending-start cancellation for every event above; real Windows lifecycle observations are recorded separately. Automated for every event except partial startup failure; Windows observations unverified.

## Implementation

- `Infrastructure/Effects/EffectMonitorGuard.cs`: new `StopEffects()`, safe on any thread. It advances the monitor epoch at once with `Interlocked.Increment`, so every queued start, even one already waiting on the UI thread, carries an earlier epoch and `Dequeue` drops it. Then it queues the removal of the running effect on the UI thread. The epoch is read with `Volatile.Read` and advanced with `Interlocked.Increment` everywhere. A display change still stops the effect at once on the UI thread.
- `Infrastructure/Effects/ISessionInterruptionSource.cs`, `Win32SessionInterruptionSource.cs`: report `SessionSwitchReason.SessionLock` and `PowerModes.Suspend` at once, on the thread Windows reports them on, with no UI dispatch. `Report(...)` takes each Windows notification, so tests drive the real source. Unlock and resume are not reported.
- `Infrastructure/Effects/EffectStopRoutes.cs`: `Connect` routes tray Stop effects and session interruptions to `EffectMonitorGuard.StopEffects`. `App.xaml.cs` calls it, so tests use the same wiring.
- `Infrastructure/TrayApplication.cs`, `NotifyIconTrayHost.cs`: tray menu item Stop effects and `StopEffectsRequested`, not raised after exit. Tray Exit raises it at once, before the exit it schedules runs, so a start queued before Exit cannot appear.
- `App.xaml.cs`: builds the session source, calls `EffectStopRoutes.Connect`, and releases the session source on exit and on startup failure.
- Exit and startup failure: `TrayApplication` and `ReleaseStartedResources` dispose the `EffectChannelController`, which removes the running effect; a later queued start finds the channel disposed and shows nothing.
- Runtime failures: unchanged. `LottiePlayback` unsubscribes its frame callback and raises `Failed`; the channel disposes the playback (closes its window), detaches its handlers and deadline, then reports to the log.

## Validation

- Test first: with stub members (`StopEffects` empty, session source not subscribed, the Win32 mappings returning `false`, tray event never raised), 12 tests failed for the expected reason: the queued start appeared and the effect stayed (`Event_RemovesTheRunningEffect_...`, `RepeatedEvent_...`, `Interruption_KeepsDrawingsAndSound` for Stop effects, lock, suspend), both mapping tests for lock and suspend, and the tray test (`Actual: ["tray shown"]`). Display change, exit, and startup failure passed at once: S4-07 and S4-02/S4-03 already cover them, so these tests record that behavior.
- `dotnet test DrawEM.sln`: 700 passed, 0 failed.
- `dotnet build DrawEM.sln -c Release -warnaserror`: succeeded, 0 warnings. `dotnet test DrawEM.sln -c Release --no-build`: 700 passed, 0 failed.
- `dotnet format DrawEM.sln --verify-no-changes`: reports whitespace only in `AssemblyInfo.cs` and in comment lines of `App.xaml.cs` that this step did not change; CI does not run it.
- Mutation checks: `StopEffects` calling `channel.Stop()` without advancing the epoch failed 4 `EffectLifecycleTests`; removing the session subscription failed 6. Both reverted.
- `/simplify` (four reviewers; efficiency found nothing): session interruptions are wired in `App.xaml.cs` next to tray Stop effects, so `EffectMonitorGuard` keeps its constructor; the lifecycle theories have one row per stop path instead of one per event; the failing effect surface (`FakeEffectSources.cs`) and the sound player fakes (`Infrastructure/Sound/FakeSoundSources.cs`) are shared instead of copied. Skipped: inlining the `Interrupts` predicates (their tests are the only automated evidence that unlock and resume are not reported), a shared app-wiring harness for `EffectLifecycleTests` and `SettingsSaveOperationTests`, and a separate start-queue owner for the epoch. `dotnet build DrawEM.sln -warnaserror`: 0 warnings. `dotnet test DrawEM.sln --no-build`: 690 passed, 0 failed.
- PR #69 review fixes (test first). The lifecycle tests now raise each event through its real wiring: the real `Win32SessionInterruptionSource` (still dispatching to the UI), `EffectStopRoutes.Connect`, and a real `TrayApplication` over a fake tray icon. 3 tests failed for the expected reason: the queued Focus ring start appeared after the event (`Event_RemovesTheRunningEffect_AndAStartQueuedBeforeItNeverAppears` for session lock, system suspend, and tray Exit). The new `TrayExit_StopsEffectsAtOnce_BeforeTheScheduledExitRuns` failed with `Actual: []`. All pass after the fix.
- Mutation checks after the review fixes, each reverted: removing the session route failed 6 tests; removing the tray route failed 4; advancing the epoch only inside the UI dispatch failed 7; not detaching `Completed` in `EffectChannelController` failed 1; tray Exit without the immediate stop failed 2.
- `dotnet build DrawEM.sln -c Release -warnaserror`: succeeded, 0 warnings, 0 errors. `dotnet test DrawEM.sln`: 697 passed, 0 failed. `dotnet test DrawEM.sln -c Release --no-build`: 697 passed, 0 failed.
- Windows desktop checks: unverified.



## Windows desktop checks

Unverified. Run with a build of this branch from the tray, with a Monitor effect (Confetti) and a Cursor effect (Focus ring) assigned. Record observations here, separately from the automated results.

1. Start an effect and a sound, draw a stroke, then choose tray > Stop effects. Expected: the effect disappears; the sound and the stroke stay.
2. Press an effect shortcut and lock the session (`Win+L`) during the animation. Unlock. Expected: no effect after unlock.
3. Start an effect and suspend the PC (Start > Power > Sleep). Resume. Expected: no effect after resume.
4. Start an effect, then change display scaling or disconnect a monitor. Expected: the effect disappears and does not come back.
5. Start an effect and choose tray > Exit during the animation. Expected: the effect window closes with the app; no drawEM process remains.
6. Repeat steps 1 to 5 several times, then compare drawEM's window and handle counts in Task Manager with the idle start. Expected: no growth.

Note: a lock or suspend drops queued starts as soon as Windows reports it, but the running effect is removed on the UI thread. If the system suspends before the UI thread runs that removal, it runs on resume; step 3 checks that no stale frame is visible.
