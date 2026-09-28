# Stage 1 / Step 2: implementation review

**Task:** [global sound channel](task.md). **Acceptance:** [criteria](acceptance.md).

## What was added

| Criterion | Where |
| --- | --- |
| Playback port | `src/DrawEM.App/Application/Sound/ISoundPlayer.cs`, `ISoundPlayerFactory.cs`, `ISoundFailureReporter.cs`, `SoundPlaybackException.cs` |
| One active attempt, replacement, same-sound restart | `src/DrawEM.App/Application/Sound/SoundChannelController.cs` (`Play`) |
| Ten-second deadline per attempt | `SoundChannelController.MaxDuration`; one `TimeProvider` timer per attempt, marshalled to the sound thread |
| Stale callbacks ignored | Each attempt object is its generation token; completion, failure, and deadline act only while that attempt is active |
| Recoverable failures logged | `SoundPlaybackException` from create or play, and `ISoundPlayer.Failed`, go to `LoggingSoundFailureReporter`, which writes the Step 1 `sound.log` record |
| WAV and MP3 playback | `src/DrawEM.App/Infrastructure/Sound/MediaSoundPlayer.cs` (WPF `MediaPlayer`, volume 1.0), `MediaSoundPlayerFactory.cs` |
| Deadline independent of a busy UI thread | `src/DrawEM.App/Infrastructure/Sound/SoundChannelHost.cs`: the channel and every `MediaPlayer` live on a dedicated STA dispatcher thread |
| Engine errors never escape | `MediaSoundPlayer` wraps every WPF exception, including from `Stop` and `Close`, in `SoundPlaybackException`; the controller still disposes a player whose `Stop` failed and logs both; a failed `Open` closes the half-built player |
| Exit releases playback | `SoundChannelHost.Dispose` runs `SoundChannelController.Dispose` on the sound thread, then ends the thread; `App.xaml.cs` adds the host to the `TrayApplication` disposal list |

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

Result: `Passed! - Failed: 0, Passed: 73, Skipped: 0, Total: 73`. The tests
need no audio device. The run used an alternate `BaseOutputPath` because a
running Debug `DrawEM.App` locked `bin\Debug`.

## Not in this step

No shortcut routing: nothing calls `SoundChannelHost.Play` yet
([Step 3](../step-3/task.md)). `MediaSoundPlayer` has no automated test;
audible WAV/MP3 output and the exact ten-second cutoff need
[Step 4 manual checks](../step-4/acceptance.md).

## Review fixes

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
