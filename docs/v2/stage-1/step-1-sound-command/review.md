# Stage 1 / Step 1: implementation review

**Task:** [command and code configuration](task.md). **Acceptance:** [criteria](acceptance.md).

## What was added

| Criterion | Where |
| --- | --- |
| Command names one sound, no WPF or Win32 types | `src/DrawEM.App/Application/Sound/PlaySoundCommand.cs`, `SoundId.cs` |
| One code-owned mapping for eight slots | `src/DrawEM.App/Infrastructure/Sound/SoundSlot.cs`, `SoundAssignments.cs`, `SoundConfiguration.cs` (`Resolve` for slot to command, `PathOf` for sound to path; one sound with two different paths is rejected) |
| Paths stay external; nothing imported or copied | `SoundAssignment` stores the path only; `SoundAssignments.Slots` is empty by default |
| Failure log under `%LOCALAPPDATA%` | `src/DrawEM.App/Infrastructure/Sound/SoundFailureLog.cs`, `SoundFailure.cs` |

To assign sounds for a local build, edit `SoundAssignments.Slots`. The class
comment shows the entry format.

A failure record is one tab-separated line in
`%LOCALAPPDATA%\drawEM\logs\sound.log`:

```text
2026-09-25T14:30:05.0000000+00:00	slot=2	sound=applause	path=C:\Sounds\applause.mp3	reason=File not found.
```

A field that is not known is written as `-`. Tabs and line breaks inside a field
become spaces. `SoundFailureLog.Append` catches I/O and
access errors, so logging cannot end the tray application.

## TDD cycles

1. Assigned slot resolves to one command: red (types missing), then green.
2. Unassigned slot resolves to none: red (`KeyNotFoundException`), then green.
3. Sound to path lookup and conflicting paths: red (`PathOf` missing), then green.
4. Failure log: default path, line format, append, and unwritable log path: red
   (types missing), then green.

## Automated result

Command, run on 2026-09-25 with .NET SDK 8.0.425 on Windows 11:

```powershell
dotnet test DrawEM.sln
```

Result: `Passed! - Failed: 0, Passed: 52, Skipped: 0, Total: 52`. The tests
need no audio device and no settings UI. The last run used an alternate
`BaseOutputPath` because a running Debug `DrawEM.App` locked `bin\Debug`.

## Not in this step

No playback, keyboard-hook integration, or composition in `App.xaml.cs`.
Nothing calls `SoundConfiguration` or `SoundFailureLog` yet; Steps 2 and 3
connect them.
