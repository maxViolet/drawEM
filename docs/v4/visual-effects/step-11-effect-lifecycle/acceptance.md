# v4 / S4-11: acceptance criteria

**Task:** [stop effects through app lifecycle](task.md). **Plan:** [implementation](plan.md).

- [ ] Tray Stop effects removes the active visual and pending starts while preserving sound and drawings. Automated: `EffectLifecycleTests.Event_RemovesTheRunningEffect_AndAStartQueuedBeforeItNeverAppears("Stop effects")`, `Interruption_KeepsDrawingsAndSound("Stop effects")` (a completed stroke and the playing sound stay; the player gets no stop), `TrayApplicationTests.TrayStopEffects_StopsEffectsUntilExit_AndReleasesNothingElse`. Unverified: the tray menu item in the running app (see [desktop checks](#windows-desktop-checks)).
- [ ] Each of display change, lock, suspend, exit, and partial startup failure removes visible effects and cancels pending starts; a start queued before the event never appears after it. Unlock/resume never restarts an old effect. Automated: `Event_RemovesTheRunningEffect_AndAStartQueuedBeforeItNeverAppears` for every event; `AfterAnInterruption_NothingRestartsByItself_ButAFreshPressStarts` (display change, lock, suspend, Stop effects); `SessionLockInterrupts_UnlockDoesNot` and `SystemSuspendInterrupts_ResumeDoesNot` (unlock and resume are not reported). Unverified: real Windows events.
- [x] Runtime effect failures are written to the local log, with partial visuals cleared and callbacks detached; a test forces a failure and checks the log entry. Sample failures stay visible in Settings. `EffectLifecycleTests.RuntimeFailure_RemovesTheEffect_DetachesItsCallbacks_AndIsWrittenToTheLocalLog` (real `LoggingEffectFailureReporter` and `EffectFailureLog` on a temp file: the playback is disposed, has no subscribers, its deadline is gone, and the log has one line). Sample failures: existing `EffectSamplerTests.SampleThatCannotShow_ReportsInSettings_AndLeavesNothingOnScreen` and `SampleThatFailsWhilePlaying_ReportsInSettings_AndReleasesItsWindow`.
- [ ] Repeated cleanup leaves no effect window, callback, or live instance; startup failure does not strand a surface. Automated: `RepeatedEvent_IsSafe_AndLeavesNoEffectOrDeadline` for every event (one removal, no active instance, no deadline timer). Startup failure releases the effect channel through `ReleaseStartedResources`, which now also releases the session source. Unverified: window and handle counts in the running app.
- [ ] Automated tests cover pending-start cancellation for every event above; real Windows lifecycle observations are recorded separately. Automated part done (see above); Windows observations unverified.

## Implementation

- `Infrastructure/Effects/EffectMonitorGuard.cs`: new `StopEffects()` advances the monitor epoch and removes every effect started before it. Each queued start carries an earlier epoch, so `Dequeue` drops it. A display change and a session interruption call it; the guard takes the new `ISessionInterruptionSource`.
- `Infrastructure/Effects/ISessionInterruptionSource.cs`, `Win32SessionInterruptionSource.cs`: report `SessionSwitchReason.SessionLock` and `PowerModes.Suspend` on the UI thread through the dispatch delegate. Unlock and resume are not reported.
- `Infrastructure/TrayApplication.cs`, `NotifyIconTrayHost.cs`: tray menu item Stop effects and `StopEffectsRequested`, not raised after exit.
- `App.xaml.cs`: wires the session source into the guard, routes tray Stop effects to `EffectMonitorGuard.StopEffects`, and releases the session source on exit and on startup failure.
- Exit and startup failure: unchanged. `TrayApplication` and `ReleaseStartedResources` dispose the `EffectChannelController`, which removes the running effect; a later queued start finds the channel disposed and shows nothing.
- Runtime failures: unchanged. `LottiePlayback` unsubscribes its frame callback and raises `Failed`; the channel disposes the playback (closes its window), detaches its handlers and deadline, then reports to the log.

## Validation

- Test first: with stub members (`StopEffects` empty, session source not subscribed, the Win32 mappings returning `false`, tray event never raised), 12 tests failed for the expected reason: the queued start appeared and the effect stayed (`Event_RemovesTheRunningEffect_...`, `RepeatedEvent_...`, `Interruption_KeepsDrawingsAndSound` for Stop effects, lock, suspend), both mapping tests for lock and suspend, and the tray test (`Actual: ["tray shown"]`). Display change, exit, and startup failure passed at once: S4-07 and S4-02/S4-03 already cover them, so these tests record that behavior.
- `dotnet test DrawEM.sln`: 700 passed, 0 failed.
- `dotnet build DrawEM.sln -c Release -warnaserror`: succeeded, 0 warnings. `dotnet test DrawEM.sln -c Release --no-build`: 700 passed, 0 failed.
- `dotnet format DrawEM.sln --verify-no-changes`: reports whitespace only in `AssemblyInfo.cs` and in comment lines of `App.xaml.cs` that this step did not change; CI does not run it.
- Mutation checks: `StopEffects` calling `channel.Stop()` without advancing the epoch failed 4 `EffectLifecycleTests`; removing the session subscription failed 6. Both reverted.
- Windows desktop checks: unverified.

## Windows desktop checks

Unverified. Run with a build of this branch from the tray, with a Monitor effect (Confetti) and a Cursor effect (Focus ring) assigned. Record observations here, separately from the automated results.

1. Start an effect and a sound, draw a stroke, then choose tray > Stop effects. Expected: the effect disappears; the sound and the stroke stay.
2. Press an effect shortcut and lock the session (`Win+L`) during the animation. Unlock. Expected: no effect after unlock.
3. Start an effect and suspend the PC (Start > Power > Sleep). Resume. Expected: no effect after resume.
4. Start an effect, then change display scaling or disconnect a monitor. Expected: the effect disappears and does not come back.
5. Start an effect and choose tray > Exit during the animation. Expected: the effect window closes with the app; no drawEM process remains.
6. Repeat steps 1 to 5 several times, then compare drawEM's window and handle counts in Task Manager with the idle start. Expected: no growth.

Note: the lock and suspend notices reach the UI thread through `Dispatcher.BeginInvoke`. If the system suspends before the UI thread runs the stop, the stop runs on resume, before any new start; step 3 checks that no stale frame is visible.
