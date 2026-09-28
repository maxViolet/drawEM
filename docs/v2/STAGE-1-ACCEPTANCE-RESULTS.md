# Stage 1 acceptance results: sound command

**Plan:** [Stage 1 / Step 4](stage-1/step-4/plan.md). **Criteria:** [acceptance](stage-1/step-4/acceptance.md).

**Status:** automated gate passed; manual gate in progress.

## Environment

- Date: 2026-09-28, UTC+05:00
- Windows version: Windows 11 Pro 10.0.26200.9457
- Branch and commit: `S1-04-publish-and-verify`, `ef36abe` plus the local test assignments below
- Executable: `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`
- Monitor count and scaling: one monitor detected (`\\.\DISPLAY1`, primary); scaling not recorded yet
- Underlying application used for input checks: not recorded yet

## Test media

The assignments are a local, uncommitted edit of `src/DrawEM.App/Infrastructure/Sound/SoundAssignments.cs`. Durations come from Windows Explorer file properties.

| Slot | Sound | File | Duration | Purpose |
| --- | --- | --- | --- | --- |
| `Ctrl+Alt+1` | `short-wav` | `C:\Windows\Media\Alarm07.wav` | 6 s | WAV natural completion |
| `Ctrl+Alt+2` | `long-wav` | `C:\Windows\Media\Ring05.wav` | 12 s | WAV ten-second cutoff |
| `Ctrl+Alt+3` | `short-mp3` | `...\Metal Slug 3\resources\sounds\mslug3_eri.mp3` | 3 s | MP3 natural completion |
| `Ctrl+Alt+4` | `long-mp3` | `...\Metal Slug 3\resources\sounds\mslug3_menus.mp3` | 57 s | MP3 ten-second cutoff |
| `Ctrl+Alt+5` | `invalid` | `%LOCALAPPDATA%\drawEM\test-media\invalid.mp3` (text file) | — | Invalid media |
| `Ctrl+Alt+6` | `missing` | `%LOCALAPPDATA%\drawEM\test-media\missing.wav` (absent) | — | Missing file |
| `Ctrl+Alt+7`, `Ctrl+Alt+8` | unassigned | — | — | No command |

## Automated gate

| Check | Result | Evidence |
| --- | --- | --- |
| `dotnet test DrawEM.sln` | Pass | 121 passed, 0 failed. New lifecycle tests in `CompositeDisposableTests` and `TrayApplicationTests` fail without commit `ef36abe` and pass with it. |
| `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64` | Pass | Self-contained win-x64 output in `publish\`, `PublishSingleFile=false`. No drawEM process was running before publish. |

## Manual gate

Only a person at the desktop can confirm audible output, input blocking, multi-monitor behavior and tray exit. Unit-test output does not count as evidence here.

| Check | Result: Pass / Fail / Not tested | Evidence or reason |
| --- | --- | --- |
| WAV shortcut plays audibly once per press with another app focused |  |  |
| MP3 shortcut plays audibly once per press with another app focused |  |  |
| Holding the shortcut does not restart the sound (no auto-repeat) |  |  |
| Another sound replaces the current sound |  |  |
| Same sound again audibly restarts it |  |  |
| Short file ends naturally |  |  |
| File longer than ten seconds stops by the ten-second deadline |  |  |
| Sound continues while the cursor moves between monitors |  |  |
| Sound starts during a stroke without ending it |  |  |
| Click, typing and scroll stay blocked while drawing with sound |  |  |
| Missing and invalid files are logged; drawEM keeps running | Partial pass | Injected `Ctrl+Alt+5` and `Ctrl+Alt+6` with `keybd_event`. `sound.log` recorded `sound=invalid ... reason=0xC00D11B1` and `sound=missing ... reason=File not found.`, and the process stayed alive. Drawing after the failures is not checked yet. |
| Tray Exit during playback stops sound and ends the process |  |  |

## v1 smoke test

Repeat the checks in [v1 SMOKE-TEST.md](../v1/SMOKE-TEST.md) on the same executable.

| Check | Result: Pass / Fail / Not tested | Evidence or reason |
| --- | --- | --- |
| Hidden startup and tray icon |  |  |
| Alt+Tab and Win+Tab do not list drawEM |  |  |
| Persistent OrangeRed (#FF4500) 4 px stroke |  |  |
| Click blocked while drawing |  |  |
| Keyboard input blocked while drawing |  |  |
| Scrolling blocked while drawing |  |  |
| Click, scrolling and keyboard input pass through after release |  |  |
| Stroke persists across windows and monitors |  |  |
| Stroke stops at monitor boundary and stays within starting monitor |  |  |
| Clear shortcut removes only strokes on cursor monitor and exits draw mode |  |  |
| Tray Exit stops overlay, shortcuts and process |  |  |

## Untested cases and limits

- Only one monitor was detected, so the multi-monitor checks cannot run on this setup unless a second monitor is connected.

## Outcome

Pending manual gate.
