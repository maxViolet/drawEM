# Stage 1 / Step 2: implementation review

**Task:** [global sound channel](task.md). **Acceptance:** [criteria](acceptance.md).

## What was added

| Criterion | Where |
| --- | --- |
| Playback port | `src/DrawEM.App/Application/Sound/ISoundPlayer.cs`, `ISoundPlayerFactory.cs`, `ISoundFailureReporter.cs`, `SoundPlaybackException.cs` |
| One active attempt, replacement, same-sound restart | `src/DrawEM.App/Application/Sound/SoundChannelController.cs` (`Play`) |
| Ten-second deadline per attempt | `SoundChannelController.MaxDuration`; one `TimeProvider` timer per attempt, marshalled to the UI thread |
| Stale callbacks ignored | Each attempt object is its generation token; completion, failure, and deadline act only while that attempt is active |
| Recoverable failures logged | `SoundPlaybackException` from create or play, and `ISoundPlayer.Failed`, go to `LoggingSoundFailureReporter`, which writes the Step 1 `sound.log` record |
| WAV and MP3 playback | `src/DrawEM.App/Infrastructure/Sound/MediaSoundPlayer.cs` (WPF `MediaPlayer`, volume 1.0), `MediaSoundPlayerFactory.cs` |
| Exit releases playback | `SoundChannelController.Dispose`; `App.xaml.cs` adds the controller to the `TrayApplication` disposal list |

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

Command, run on 2026-09-25 with .NET SDK 8.0.425 on Windows 11:

```powershell
dotnet test DrawEM.sln
```

Result: `Passed! - Failed: 0, Passed: 67, Skipped: 0, Total: 67`. The tests
need no audio device. The run used an alternate `BaseOutputPath` because a
running Debug `DrawEM.App` locked `bin\Debug`.

## Not in this step

No shortcut routing: nothing calls `SoundChannelController.Play` yet
([Step 3](../step-3/task.md)). `MediaSoundPlayer` has no automated test;
audible WAV/MP3 output and the exact ten-second cutoff need
[Step 4 manual checks](../step-4/acceptance.md).
