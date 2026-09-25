# Step 7 acceptance results

## Environment

- Date and time: 2026-09-22; local time not recorded.
- Windows version: Not recorded.
- Executable path: `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`
- Monitor count and scaling: Not recorded.
- Underlying application used for click checks: Not recorded; Notepad was used for keyboard checks.

## Automated gate

- `dotnet test DrawEM.sln --no-restore`: Pass — 23 tests passed.
- `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64 --no-restore`: Pass.

## Manual checks

| Check | Result: Pass / Fail / Not tested | Evidence or reason |
| --- | --- | --- |
| Hidden startup and tray icon | Pass | Observed during published-build runs. |
| Persistent orange 4 px stroke | Pass | Drawn strokes remained after releasing `Ctrl+Alt+Z`. |
| Click blocked while drawing | Pass | Verified against an ordinary underlying application. |
| Keyboard input blocked while drawing | Pass | Verified in Notepad; Herdr is excluded from v1 because it uses a different input path. |
| Scrolling blocked while drawing | Pass | Verified during the published-build run. |
| Click, scrolling, and keyboard input pass through after release | Not tested | The immediate post-`Ctrl+Alt+X` pass-through check for the final shared-gate build was launched but not recorded. |
| Stroke persists across windows and monitors | Not tested | Monitor count and mixed-DPI coverage were not recorded. |
| Clear shortcut removes all strokes and exits draw mode | Pass | Verified in the published build: a new stroke requires releasing and holding `Ctrl+Alt+Z` again. |
| Tray Exit stops overlay, shortcuts, and process | Pass | Verified by closing the published application through the tray. |

## Boundaries observed

- Elevated applications: Not tested.
- Fullscreen or protected surfaces: Not tested.
- DPI or monitor hot-plug: Not tested.

## Outcome

Conditional pass — all recorded applicable checks passed; multi-monitor/DPI and the final immediate pass-through observation remain unrecorded.
