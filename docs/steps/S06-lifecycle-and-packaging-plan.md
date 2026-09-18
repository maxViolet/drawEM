# Step 6: tray lifecycle and packaging plan

## Goal

Deliver a hidden Windows tray application whose `Exit` command reliably stops
drawEM and a self-contained x64 executable that can be manually smoke-tested.

This plan implements only roadmap step 6. Mixed-DPI rendering belongs to step
3; the complete business acceptance pass belongs to step 7.

## Scope

- Add a tray icon with an `Exit` command.
- Establish one ownership path for shortcut registration, overlay, tray icon,
  and application shutdown.
- Add lifecycle tests using fake adapters.
- Add a self-contained `win-x64` publish profile.
- Add a short manual smoke-test document for the published executable.

## Out of scope

- Shortcut configuration, autostart, settings, undo, export, or elevation.
- Pixel-level overlay rendering and mixed-DPI behavior.
- Treating Win32/tray interactions as unit-test coverage.

## Implementation sequence

1. Define lifecycle seams in `Infrastructure`.

   Create `ITrayHost`, `IOverlayLifetime`, and `IApplicationLifetime` so the
   lifecycle can be tested without a real WPF window, NotifyIcon, or process.

2. Write the lifecycle tests first.

   Use fakes to verify that startup exposes the tray host and that `Exit`
   unregisters shortcuts, closes the overlay, disposes the tray host, then
   requests application shutdown. Test that repeated exit requests are safe
   and that normal application disposal releases resources without requesting
   shutdown again.

3. Implement `TrayApplication` as the single lifecycle owner.

   It subscribes to `ITrayHost.ExitRequested`, performs teardown once, and
   unsubscribes before disposing the tray host. The tray callback must schedule
   `Exit` through the WPF dispatcher so NotifyIcon is not disposed within its
   own click handler.

4. Implement `NotifyIconTrayHost`.

   Use a Windows Forms `NotifyIcon` with one context-menu item named `Exit`.
   The host reports the command through `ExitRequested`; it does not own
   application shutdown policy.

5. Wire lifecycle in `App.xaml.cs`.

   Set `ShutdownMode` to `OnExplicitShutdown`, construct the overlay, keyboard
   hook, shortcut adapter, and tray lifecycle at the composition root, then
   show the overlay and tray icon. On startup failure, release any partial
   resources, show the error, and shut down.

6. Add packaging and smoke-test artifacts.

   Create `Properties/PublishProfiles/win-x64.pubxml` with Release,
   `win-x64`, self-contained, single-file publishing, and trimming disabled.
   Add `docs/SMOKE-TEST.md` with the command and manual checks below.

## Files expected to change

| File | Change |
| --- | --- |
| `../../src/DrawEM.App/App.xaml.cs` | Compose lifecycle and startup rollback. |
| `../../src/DrawEM.App/DrawEM.App.csproj` | Enable Windows Forms for `NotifyIcon`. |
| `../../src/DrawEM.App/Infrastructure/TrayApplication.cs` | Add lifecycle owner and test seams. |
| `../../src/DrawEM.App/Infrastructure/NotifyIconTrayHost.cs` | Add tray implementation. |
| `../../src/DrawEM.App/Infrastructure/WpfApplicationLifetime.cs` | Adapt WPF application shutdown. |
| `../../src/DrawEM.App/Presentation/OverlayWindow.xaml.cs` | Expose overlay close lifecycle seam. |
| `../../tests/DrawEM.Tests/Infrastructure/TrayApplicationTests.cs` | Add lifecycle behavior tests. |
| `../../src/DrawEM.App/Properties/PublishProfiles/win-x64.pubxml` | Add self-contained publish settings. |
| `../SMOKE-TEST.md` | Add manual executable checks. |
| `../../README.md` | Link publish command and smoke test. |

## Acceptance criteria

### Automated

- The lifecycle test proves `Exit` unregisters shortcuts and closes the overlay.
- Lifecycle teardown is idempotent.
- `dotnet test DrawEM.sln --no-restore` passes.
- `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64` produces a self-contained x64 `.exe`.

### Manual

1. Launch the published executable; no normal application window appears.
2. Confirm the drawEM tray icon is visible.
3. Draw with `Ctrl+Alt+Z` and clear with `Ctrl+Alt+X`.
4. Choose tray `Exit` and confirm the overlay disappears.
5. Confirm `DrawEM.App.exe` is absent from Task Manager after exit.

Record these results in `docs/SMOKE-TEST.md`. They are manual verification,
not unit-test evidence.

## Verification order

1. Run the unit tests.
2. Publish the self-contained executable.
3. Run the manual smoke test against that executable.
4. Record the manual results before declaring step 6 complete.
