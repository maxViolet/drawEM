# drawEM smoke test

## Publish

```powershell
dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64
```

Run `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`.

## Manual checks

1. Confirm no normal application window appears and the drawEM icon is visible in the notification area.
2. Hold `Ctrl+Alt+Z`, move the pointer, and confirm an orange 4 px line remains visible after release.
3. While holding `Ctrl+Alt+Z`, click a button in an underlying application; confirm it does not activate.
4. Release `Ctrl+Alt+Z`, click the same button, and confirm it activates.
5. Press `Ctrl+Alt+X` and confirm every line disappears.
6. Choose `Exit` from the tray icon menu and confirm the overlay disappears and `DrawEM.App.exe` is no longer in Task Manager.

Record results as manual verification: Win32 hooks, the notification area, input suppression, DPI, and multi-monitor behavior cannot be proven by the unit tests.
