# drawEM smoke test

## Publish

```powershell
dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64 --no-restore
```

Run `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`.

## Manual checks

1. Confirm no normal application window appears and the drawEM icon is visible in the notification area.
   Press `Alt+Tab` and `Win+Tab` and confirm drawEM is not listed, including after drawing a stroke.
2. Hold `Ctrl+Alt+Z`, move the pointer, and confirm an OrangeRed (#FF4500) 4 px line remains visible after release.
3. While holding `Ctrl+Alt+Z`, click a button in an underlying application; confirm it does not activate.
4. While holding `Ctrl+Alt+Z`, type a character in an underlying text field; confirm it does not appear.
5. Release `Ctrl+Alt+Z`, click the same button, type in the same text field,
   and scroll; confirm all actions work.
6. Switch windows; when more than one monitor is available, move between
   monitors and confirm existing strokes remain visible.
7. While holding `Ctrl+Alt+Z`, scroll in an application underneath and confirm
   it does not scroll.
8. Press `Ctrl+Alt+X` and confirm every line disappears, draw mode exits, and
   no new stroke starts until `Ctrl+Alt+Z` is released and held again.
9. Choose `Exit` from the tray icon menu and confirm the overlay disappears,
   the shortcuts stop, and `DrawEM.App.exe` is no longer in Task Manager.

Record results as manual verification: Win32 hooks, the notification area, input suppression, DPI, and multi-monitor behavior cannot be proven by the unit tests.

## Results template

Copy this section to `docs/S07-ACCEPTANCE-RESULTS.md` for each acceptance run.

```markdown
# Step 7 acceptance results

## Environment

- Date and time:
- Windows version:
- Executable path:
- Monitor count and scaling:
- Underlying application used for click checks:

## Automated gate

- `dotnet test DrawEM.sln --no-restore`: Pass | Fail
- `dotnet publish ... --no-restore`: Pass | Fail

## Manual checks

| Check | Result: Pass / Fail / Not tested | Evidence or reason |
| --- | --- | --- |
| Hidden startup, tray icon, not listed in Alt+Tab or Win+Tab |  |  |
| Persistent OrangeRed (#FF4500) 4 px stroke |  |  |
| Click blocked while drawing |  |  |
| Keyboard input blocked while drawing |  |  |
| Scrolling blocked while drawing |  |  |
| Click, scrolling, and keyboard input pass through after release |  |  |
| Stroke persists across windows and monitors |  |  |
| Clear shortcut removes all strokes and exits draw mode |  |  |
| Tray Exit stops overlay, shortcuts, and process |  |  |

## Boundaries observed

- Elevated applications:
- Fullscreen or protected surfaces:
- DPI or monitor hot-plug:

## Outcome

Pass | Conditional pass | Fail
```
