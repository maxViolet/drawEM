# v3 / Step 8 acceptance results

**Task:** [publish and verify](step-8-publish/task.md). **Plan:** [verification](step-8-publish/plan.md). **Criteria:** [acceptance](step-8-publish/acceptance.md).

**Status:** automated gate passed; manual desktop gate run on one 100% monitor
with en-US and ru layouts: 28 of 35 checks pass, 7 are unverified because the
environment cannot run them. v3 is not releasable until those checks are Pass
or have an accepted reason.

Result values: **Pass**, **Fail**, **Unverified** (not run, or the environment
cannot run it). Automated tests and publish success do not close any manual
check.

## Environment

- Date: 2026-10-06, UTC+05:00
- Windows version: Windows 11 Pro 25H2, build 10.0.26200.9457 (the registry
  `ProductName` value reports "Windows 10 Pro")
- Branch and commit: `S3-08-publish`, `83d4fa1` (equal to `main` at the time of
  the run)
- Executable: `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`
- Build identity: file version `1.0.0.0`, product version
  `1.0.0+83d4fa16922fcee3246f57629240433916e4eb80`, written
  2026-10-06 14:58:24 UTC, SHA256
  `20184F0339D4F86E0FBF1247B390377C3779AA49A823F4D1C3404969606DBEC0`
- Input layouts installed: English (United States) `0409:00000409`, Russian
  `0419:00000419`. German (Germany) and Polish (Programmers) are not installed.
- Input-language hotkeys: the operator reports that both `Alt+Shift` and
  `Ctrl+Shift` were enabled for the group 3 checks. No
  `HKCU\Keyboard Layout\Toggle` values were set when the environment was
  rechecked on 2026-10-07.
- Monitors: one monitor (`\\.\DISPLAY1`, primary), 1920x1200, 96 DPI (100%).
  No 150% or mixed-DPI configuration is available.
- Audio output device: not recorded yet.
- Underlying application used for input checks: not recorded yet.

## Automated gate

| Check | Result | Evidence |
| --- | --- | --- |
| `dotnet restore DrawEM.sln` | Pass | Required once in this worktree; `--no-restore` failed first with `NETSDK1004` (missing `project.assets.json`). |
| `dotnet test DrawEM.sln --no-restore -m:1` | Pass | 472 passed, 0 failed, 0 skipped. |
| `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64` | Pass | Self-contained win-x64 output in `publish\`. A running drawEM process was stopped before publishing. |

## Manual checks

Run every check with the executable identified above, with another
application focused unless the check says otherwise. Replace **Unverified**
with the observed result and a short note.

### 1. Tray, settings, storage, and media recovery

| # | Check | Result | Notes |
| --- | --- | --- | --- |
| 1.1 | Tray icon visible; tray menu opens Settings. | Pass | 2026-10-07, reported by the operator. |
| 1.2 | Draft isolation: edits in an open Settings window do not change live behavior before Save. | Pass | 2026-10-07, reported by the operator. |
| 1.3 | Save applies the draft; Cancel discards it; defaults restore the default values. | Pass | 2026-10-07, reported by the operator. |
| 1.4 | Shortcut conflict error and sound copy error are shown and leave saved settings unchanged. | Pass | 2026-10-07, reported by the operator. |
| 1.5 | WAV and MP3 selection, and `Sample` plays the selected file. | Pass | 2026-10-07, reported by the operator. |
| 1.6 | Saved settings persist after tray Exit and restart. | Pass | 2026-10-07, reported by the operator. |
| 1.7 | Corrupt `settings.json` falls back to defaults and the app starts. | Pass | 2026-10-07, reported by the operator. |
| 1.8 | Orphaned media in `sounds\` is cleaned at startup after a forced exit. | Pass | 2026-10-07, reported by the operator. |
| 1.9 | Managed media is preserved when `settings.json` is unreadable. | Pass | 2026-10-07, reported by the operator. |

### 2. Real keys and layouts

| # | Check | Result | Notes |
| --- | --- | --- | --- |
| 2.1 | Bound drawing shortcut draws over the focused app. | Pass | 2026-10-07, reported by the operator with the default `Ctrl+Alt+Z`; focused app not recorded. |
| 2.2 | Bound sound shortcut plays its sound. | Pass | 2026-10-07, reported by the operator. |
| 2.3 | Ordinary `Ctrl+C` and `Alt+F4` reach the focused app. | Pass | 2026-10-07, reported by the operator. |
| 2.4 | Capturing an already-bound chord does not dispatch its action. | Pass | 2026-10-07, reported by the operator. |
| 2.5 | `Tab`, `Alt+Tab`, `Alt+F4`, and `Win` pass through during capture. | Pass | 2026-10-07, reported by the operator. |
| 2.6 | Capturing `Ctrl+C` and `Ctrl+RightAlt+1` shows invalid-chord feedback. | Pass | 2026-10-07, reported by the operator. |
| 2.7 | German (Germany): Right Alt/AltGr typing works; Left Alt bindings work. | Unverified | Layout not installed. |
| 2.8 | Polish (Programmers): Right Alt/AltGr typing works; Left Alt bindings work. | Unverified | Layout not installed. |

### 3. Input-language hotkeys (release blocker on failure)

Enable both `Alt+Shift` and `Ctrl+Shift` input-language hotkeys before these
checks.

| # | Check | Result | Notes |
| --- | --- | --- | --- |
| 3.1 | A bound action with `Alt+Shift` does not switch the input layout. | Pass | 2026-10-07, reported by the operator. |
| 3.2 | A bound action with `Ctrl+Shift` does not switch the input layout. | Pass | 2026-10-07, reported by the operator. |
| 3.3 | Capturing chords with `Alt+Shift` and `Ctrl+Shift` does not switch the layout. | Pass | 2026-10-07, reported by the operator. |
| 3.4 | Bare `Alt+Shift` still switches the layout. | Pass | 2026-10-07, reported by the operator. |
| 3.5 | Bare `Ctrl+Shift` still switches the layout. | Pass | 2026-10-07, reported by the operator. |

### 4. DPI, monitors, and drawing style

| # | Check | Result | Notes |
| --- | --- | --- | --- |
| 4.1 | Line and dot physical width match the setting at 100%. | Pass | 2026-10-07, reported by the operator. |
| 4.2 | Line and dot physical width match the setting at 150%. | Unverified | No 150% monitor available. |
| 4.3 | Line and dot physical width match across a mixed-DPI pair. | Unverified | One monitor only. |
| 4.4 | Saved color is used for new strokes. | Pass | 2026-10-07, reported by the operator. |
| 4.5 | Save clears drawings on all monitors. | Unverified | Clear on Save passed on the single monitor (operator, 2026-10-07); the all-monitor part needs a second monitor. |

### 5. Audible sound

| # | Check | Result | Notes |
| --- | --- | --- | --- |
| 5.1 | WAV shortcut playback is audible. | Pass | 2026-10-07, reported by the operator. |
| 5.2 | MP3 shortcut playback is audible. | Pass | 2026-10-07, reported by the operator. |
| 5.3 | `Sample` playback is audible. | Pass | 2026-10-07, reported by the operator. |
| 5.4 | A different sound replaces the playing sound. | Pass | 2026-10-07, reported by the operator. |
| 5.5 | The same sound restarts from the beginning. | Pass | 2026-10-07, reported by the operator. |
| 5.6 | Playback stops after ten seconds. | Pass | 2026-10-07, reported by the operator. |

### 6. Regressions

| # | Check | Result | Notes |
| --- | --- | --- | --- |
| 6.1 | [v1 smoke test](../v1/SMOKE-TEST.md) checks 1–10 with saved settings. | Unverified | Single-monitor checks passed (operator, 2026-10-07); multi-monitor parts need a second monitor. |
| 6.2 | [v2 sound scenarios](../v2/SOUND-BUSINESS-TESTS.md) 1–15 (open v2 Step 4 checks) with saved settings. | Unverified | Scenarios 1–11 and 13–15 passed (operator, 2026-10-07); scenario 12 needs a second monitor. |

## Release blockers

- Checks 2.7 and 2.8 need the German (Germany) and Polish (Programmers)
  layouts installed.
- Checks 4.2, 4.3, 4.5 (all-monitor part), 6.1 (multi-monitor part), and 6.2
  scenario 12 need a 150% monitor and a second monitor with different scaling.
- Group 3 passed; any later Fail in group 3 blocks the release.

## Outcome

All four acceptance criteria are met for the tested environment: the
automated gate is recorded, every plan group has manual results, no tested
`Alt+Shift` or `Ctrl+Shift` binding switched the layout, and every unrun check
is marked Unverified with its blocker. Desktop results are operator reports;
no screenshots or measurements are attached. Release still needs checks 2.7,
2.8, 4.2, 4.3, 4.5, 6.1, and 6.2 on a machine with the German and Polish
layouts and a second monitor at different scaling, or an accepted reason to
release without them.
