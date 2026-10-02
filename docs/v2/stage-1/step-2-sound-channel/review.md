# Stage 1 / Step 2: implementation review

**Task:** [global sound channel](task.md). **Acceptance:** [criteria](acceptance.md).

## What was added

| Criterion | Where |
| --- | --- |
| Playback port | `src/DrawEM.App/Application/Sound/ISoundPlayer.cs`, `ISoundPlayerFactory.cs`, `ISoundFailureReporter.cs`, `SoundPlaybackException.cs` |
| One active attempt, replacement, same-sound restart | `src/DrawEM.App/Application/Sound/SoundChannelController.cs` (`Play`) |
| Ten-second deadline per attempt | `SoundChannelController.MaxDuration`; one `TimeProvider` timer per attempt, marshalled to the sound thread |
| Stale callbacks ignored | Each attempt object is its generation token; completion, failure, and deadline act only while that attempt is active |
| Recoverable failures logged | `SoundPlaybackException` from create, play, stop, or dispose, and `ISoundPlayer.Failed`, go to `LoggingSoundFailureReporter` after the player is released; it queues the Step 1 `sound.log` record for a background writer |
| WAV and MP3 playback | `src/DrawEM.App/Infrastructure/Sound/MediaSoundPlayer.cs` (WPF `MediaPlayer`, volume 1.0), `MediaSoundPlayerFactory.cs` |
| Deadline independent of a busy UI thread | `src/DrawEM.App/Infrastructure/Sound/SoundChannelHost.cs`: the channel and every `MediaPlayer` live on a dedicated STA dispatcher thread |
| Engine errors never escape | `MediaSoundPlayer` wraps every WPF exception, including from `Stop` and `Close`, in `SoundPlaybackException`; the controller still disposes a player whose `Stop` failed and logs both; a failed `Open` closes the half-built player |
| Exit releases playback | `SoundChannelHost.Dispose` runs `SoundChannelController.Dispose` on the sound thread, then ends the thread even if that throws; the log then drains for at most one second. Order and thread ownership: [ARCHITECTURE.md, Sound thread](../../../ARCHITECTURE.md#sound-thread) |

Failure reasons from the factory: `Sound is not assigned to a file.`,
`Path is not absolute.`, `File not found.`, or the media engine's message.
Decode and output-device errors arrive through `MediaPlayer.MediaFailed`. The
controller does not know the slot, so failure records write `slot=-`.

## TDD cycles

Controller tests were written first against missing types (red: compile errors
for `SoundChannelController`, `ISoundPlayer`, `ISoundPlayerFactory`,
`ISoundFailureReporter`), then the controller made all eleven pass. Behaviors
covered with a fake player and `ManualTimeProvider`: first start, replacement
order, same-sound restart with a new deadline, natural completion, deadline at
exactly ten seconds, stale completion and failure, a stale deadline queued
before replacement, player failure, create failure, play failure, and
disposal on exit. Factory and reporter tests cover the unassigned, missing,
and relative path cases and the log record.

## Automated result

Command, run on 2026-09-28 with .NET SDK 8.0.425 on Windows 11:

```powershell
dotnet test DrawEM.sln
```

Result: `Passed! - Failed: 0, Passed: 80, Skipped: 0, Total: 80`. The tests
need no audio device. The run used an alternate `BaseOutputPath` because a
running Debug `DrawEM.App` locked `bin\Debug`.

## Not in this step

No shortcut routing: nothing calls `SoundChannelHost.Play` yet
([Step 3](../step-3-sound-shortcuts/task.md)). `MediaSoundPlayer` has no automated test;
audible WAV/MP3 output and the exact ten-second cutoff need
[Step 4 manual checks](../step-4-publish/acceptance.md).

## Review fixes

### First review

A code review of the first version found three defects:

1. `MediaSoundPlayer` let WPF exceptions escape, and the controller caught
   only `SoundPlaybackException`. A throwing `Stop` skipped `Dispose` and the
   log record. Fixed in the adapter (wrap every engine call) and in the
   controller (release each step separately, log each failure).
2. The deadline was queued on the UI dispatcher, so a busy UI thread could
   let a sound run past ten seconds. The channel now runs on its own sound
   thread, because `MediaPlayer` can be stopped only on its owning thread.
3. A failed `Open` left the half-built `MediaPlayer` subscribed and open. The
   constructor now closes it before rethrowing.

Each fix started with a failing test: three controller tests for stop and
dispose failures, three `SoundChannelHostTests` for the sound thread. No
automated test forces `MediaPlayer.Open` to throw; fix 3 is covered by review
only.

### Second review

1. `SoundChannelHost` left the sound thread running when `createChannel`
   threw, and skipped the dispatcher shutdown when `channel.Dispose` threw.
   The constructor now ends the thread before rethrowing; `Dispose` ends it in
   `finally`. `Dispose` on the sound thread throws `InvalidOperationException`,
   because it would join itself.
2. The controller logged before it released the player, and the log wrote to
   disk on the sound thread. The controller now releases first and reports
   afterwards. `LoggingSoundFailureReporter` queues records for a background
   writer, keeps the report time, and drains for at most one second at exit.
3. `MediaSoundPlayer` set `Volume` outside the cleanup block. Creation,
   subscription, `Volume`, and `Open` now share one cleanup block.

New tests: release-before-report order in the controller; three host tests
(failed channel creation, throwing channel disposal, `Dispose` from the sound
thread); three reporter tests (slow log does not block `Report`, stuck log
bounds `Dispose`, a throwing write does not stop later records). Fix 3 is
covered by review only, for the same reason as before.

### Third review

`MediaSoundPlayer` removed its event handlers outside any protected block. In
WPF, removing a `MediaPlayer` handler can re-enter the lazy native engine
setup. If that setup failed during construction, it can throw again, which
skipped `Close` and let a raw WPF exception escape. The same gap existed in
`Dispose`. Both paths now use one `Release` method: it runs each handler
removal and `Close` in its own `try`, so every step runs. The constructor
ignores release failures and reports the construction error. `Dispose`
reports the first release failure as `SoundPlaybackException`. The scenario
was not reproduced on this machine, and no automated test forces it.
