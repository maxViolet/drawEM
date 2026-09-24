# drawEM smoke test

## Publish

```powershell
dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64 --no-restore
```

Run `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`.

## Manual checks

1. Confirm no normal application window appears and the drawEM icon is visible in the notification area.
2. Open `Alt+Tab` and `Win+Tab`; confirm neither shows drawEM as a selectable
   window, including after drawing a stroke.
3. Hold `Ctrl+Alt+Z`, move the pointer, and confirm an OrangeRed (#FF4500)
   4 px line remains visible after release.
4. While holding `Ctrl+Alt+Z`, click a button in an underlying application; confirm it does not activate.
5. While holding `Ctrl+Alt+Z`, type a character in an underlying text field; confirm it does not appear.
6. Release `Ctrl+Alt+Z`, click the same button, type in the same text field,
   and scroll; confirm all actions work.
7. Switch windows; when more than one monitor is available, move between
   monitors and confirm existing strokes remain visible. While holding the draw
   shortcut, cross a monitor boundary: the stroke must stop on its starting
   monitor and must not resume until the shortcut is released and pressed again.
8. While holding `Ctrl+Alt+Z`, scroll in an application underneath and confirm
   it does not scroll.
9. With strokes on two monitors, press `Ctrl+Alt+X` and confirm only strokes on
   the monitor under the cursor disappear. Draw mode exits, and no new stroke
   starts until `Ctrl+Alt+Z` is released and held again.
10. Choose `Exit` from the tray icon menu and confirm the overlay disappears,
   the shortcuts stop, and `DrawEM.App.exe` is no longer in Task Manager.

Record results as manual verification: Win32 hooks, the notification area, input suppression, DPI, and multi-monitor behavior cannot be proven by the unit tests.

## Results template

Copy this section to `../steps/S07-ACCEPTANCE-RESULTS.md` for each acceptance run.

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
| Hidden startup and tray icon |  |  |
| Alt+Tab and Win+Tab do not list drawEM |  |  |
| Persistent OrangeRed (#FF4500) 4 px stroke |  |  |
| Click blocked while drawing |  |  |
| Keyboard input blocked while drawing |  |  |
| Scrolling blocked while drawing |  |  |
| Click, scrolling, and keyboard input pass through after release |  |  |
| Stroke persists across windows and monitors |  |  |
| Stroke stops at monitor boundary and stays within starting monitor |  |  |
| Clear shortcut removes only strokes on cursor monitor and exits draw mode |  |  |
| Tray Exit stops overlay, shortcuts, and process |  |  |

## Boundaries observed

- Elevated applications:
- Fullscreen or protected surfaces:
- DPI or monitor hot-plug:

## Outcome

Pass | Conditional pass | Fail
```
