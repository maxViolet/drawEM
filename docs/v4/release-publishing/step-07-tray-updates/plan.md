# R4-07: implementation plan

**Task:** [tray updates](task.md). **Acceptance:** [criteria](acceptance.md).

## Starting point

- R4-06 must be merged and its installer release published. R4-06 adds
  `Program.Main` with `VelopackApp.Build().SetAutoApplyOnStartup(false).Run()`,
  the `Velopack` NuGet package, package ID `drawEM.Desktop`, channel `win`,
  and the release assets `drawEM.Desktop-<version>-full.nupkg` and
  `releases.win.json`. R4-07 keeps auto-apply off.
- The tray today: `ITrayHost` (events `ExitRequested`, `SettingsRequested`,
  `StopEffectsRequested`; `Show()`), implemented by `NotifyIconTrayHost` with
  the menu Settings… / Stop effects / separator / Exit. `TrayApplication`
  forwards the events and never raises them after exit. Its `Exit()` disposes
  hooks and other registrations, closes the overlay, disposes the tray icon,
  and calls `Application.Shutdown()`. `App.OnExit` then releases
  `SingleInstanceGuard` last. Tests: `TrayApplicationTests`,
  `NotifyIconTrayHostTests`, fakes in `FakeTrayParts.cs`.
- Background work pattern: Application controllers take `TimeProvider` and an
  `Action<Action>` UI dispatcher (`EffectChannelController`,
  `SoundChannelController`, timers through `time.CreateTimer`). Failure logs:
  `LoggingEffectFailureReporter` + `EffectFailureLog`
  (`%LOCALAPPDATA%\drawEM\logs\effect.log`) on `BackgroundLogWriter`.
- `LayerDependencyTests`: Application may depend only on `System`,
  `Microsoft`, Domain, and Application, with no `System.IO` and no Windows
  UI. Infrastructure may not depend on `Velopack` yet.
- Velopack facts used below, from the
  [`UpdateManager` reference](https://docs.velopack.io/reference/cs/Velopack/UpdateManager)
  and [update sources](https://docs.velopack.io/integrating/update-sources):
  `IsInstalled` is false for a non-Velopack copy; `CheckForUpdatesAsync`
  returns `UpdateInfo?`; `DownloadUpdatesAsync(info, progress, token)` takes a
  global update lock; `UpdatePendingRestart` returns a downloaded asset that
  has not been applied; `WaitExitThenApplyUpdates(asset, silent, restart)`
  starts the updater, which waits up to 60 seconds for this process to exit;
  `ApplyUpdatesAndRestart` exits the process immediately.
  `GithubSource(repoUrl, accessToken: null, prerelease: false)` uses public
  release data, so drafts and pre-releases are never offered; the
  unauthenticated limit is 60 requests per hour per IP.

## Decisions

| Topic | Decision | Reason |
|---|---|---|
| Cadence | Check once after tray startup, then every 24 hours from the last check while running. No persisted check time. | Matches "startup and at most every 24 hours" without extra state. |
| Download | Download right after a successful check finds a newer version, in the background. No prompt before download. | Roadmap: background download, consent before install. |
| Notification | One balloon tip per downloaded version, plus a tray menu item `Restart to update to <version>` above Exit that stays until applied. | Deferral keeps the update reachable without repeated pop-ups. |
| Consent | Clicking the menu item or the balloon shows a Yes/No dialog: "Restart drawEM now to install <version>? Drawing, sound, and effects stop." No closes the dialog and keeps the item. | Nothing installs or restarts without this confirmation. |
| Apply | `WaitExitThenApplyUpdates(asset, silent: false, restart: true)`, then `TrayApplication.Exit()`. Do not use `ApplyUpdatesAndRestart`. | The normal exit path releases hooks, the overlay, sound, logs, and the single-instance mutex before the new version starts; an immediate exit would skip that cleanup and could hit "already running". |
| Pending at startup | If `UpdatePendingRestart` is set, show the menu item and balloon without downloading again. | A deferred update survives an app restart; auto-apply stays off. |
| Non-Velopack copies | When `IsInstalled` is false (portable ZIP, WinGet portable, test output), create no timer and make no network call. | Portable copies update manually. |
| Failures | Log to `%LOCALAPPDATA%\drawEM\logs\update.log`; no dialog; retry at the next 24-hour tick. | A failed check must not disturb the tray app. |
| Deltas | No delta packages in R4-07. Add an [out-of-scope](../../out-of-scope/README.md) entry for them. | Full packages work. Deltas need the previous release downloaded during packaging. |
| Test feed | Environment variable `DRAWEM_UPDATE_FEED`, read only in `App.OnStartup`: an existing local directory replaces `GithubSource` with `SimpleFileSource`, and startup logs the override to `update.log`. Any other value is ignored and logged. | The first in-app upgrade can only start from a build that already contains R4-07. A local feed lets the upgrade be tested before publishing. A process that can set the user's environment can already replace files in the user's own install folder, so the override grants no new access. |

## Files

| File | Change |
|---|---|
| `src/DrawEM.App/Application/Updates/IUpdateService.cs` | New port: `bool IsInstalled`, `string? PendingVersion`, `Task<string?> CheckAndDownloadAsync(CancellationToken)`, `void ApplyAfterExit()`. Versions are strings, with no Velopack types. |
| `src/DrawEM.App/Application/Updates/IUpdateFailureReporter.cs` | New: `void Report(string operation, string reason)`. |
| `src/DrawEM.App/Application/Updates/UpdateController.cs` | New: cadence, single in-flight check, one notification per version, consent, apply. |
| `src/DrawEM.App/Infrastructure/Updates/VelopackUpdateService.cs` | New: `UpdateManager` + `GithubSource("https://github.com/maxViolet/drawEM", null, false)` or `SimpleFileSource`; runs Velopack calls on the thread pool. |
| `src/DrawEM.App/Infrastructure/Updates/UpdateFailureLog.cs`, `LoggingUpdateFailureReporter.cs` | New, following `EffectFailureLog` / `LoggingEffectFailureReporter`. |
| `src/DrawEM.App/Infrastructure/TrayApplication.cs` | `ITrayHost`: add `event Action? UpdateRequested` and `void ShowUpdateReady(string version)`. `TrayApplication`: forward `UpdateRequested` (never after exit) and expose `ShowUpdateReady`. |
| `src/DrawEM.App/Infrastructure/NotifyIconTrayHost.cs` | Hidden `Restart to update…` item above the separator; `ShowUpdateReady` sets its text, shows it, and shows one balloon; `BalloonTipClicked` and the item raise `UpdateRequested`. |
| `src/DrawEM.App/App.xaml.cs` | Compose the service, reporter, and controller after `trayApplication.Start()`; confirmation through `MessageBox`; dispose the controller first on exit. Shared file: follow [the shared-file rules](../../README.md#rules-for-parallel-work). |
| `tests/DrawEM.Tests/Architecture/LayerDependencyTests.cs` | Infrastructure may also depend on `Velopack` (comment: release roadmap, installer and updates row). |
| `tests/DrawEM.Tests/Application/UpdateControllerTests.cs` | New. |
| `tests/DrawEM.Tests/Infrastructure/TrayApplicationTests.cs`, `NotifyIconTrayHostTests.cs`, `FakeTrayParts.cs` | Extend for the update event and item. |
| `README.md` | Updates section. Shared file. |

## Steps

1. **Failing controller tests (about 1.5 hours).** `UpdateControllerTests`
   with a fake `IUpdateService`, `ManualTimeProvider`, an inline dispatcher, a
   fake tray notifier, and a fake consent function:
   - `IsInstalled == false`: `Start()` makes no check and creates no timer;
     advancing 48 hours makes no check.
   - Startup makes one check; the next check happens at exactly +24 h, not at
     +23:59:59. Each later one follows 24 h after the previous one.
   - A check still running when the timer fires does not start a second one.
   - A downloaded version notifies once. The same version found again
     notifies nothing new. A newer version notifies once more.
   - `PendingVersion` set at startup notifies without calling
     `CheckAndDownloadAsync`.
   - Consent No keeps the notification state and applies nothing; a later
     request asks again. Consent Yes calls `ApplyAfterExit()`, then the exit
     action, in that order, once.
   - An exception or a faulted task from the service is reported with the
     operation name. The controller keeps running, and the next tick checks
     again.
   - After `Dispose()`, nothing more happens: the timer stops, the in-flight
     token is cancelled, and late completions are ignored.

   Run `dotnet test DrawEM.sln -c Release --filter FullyQualifiedName~UpdateControllerTests`.
   Expected: compile failure, because the types do not exist.
2. **Controller (about 2 hours).** Implement `UpdateController` in
   `Application/Updates` until step 1 passes. Run the full suite; the
   Application layer rules must stay green with no `System.IO` and no Windows
   types.
3. **Tray (about 1.5 hours).** Write the failing tests first:
   - `TrayApplicationTests`: `UpdateRequested` is forwarded, and not after
     `Exit()` or `Dispose()`.
   - `NotifyIconTrayHostTests`: the item is hidden at first; after
     `ShowUpdateReady("1.2.3")` it is visible with text
     `Restart to update to 1.2.3`, placed above Exit; clicking it raises
     `UpdateRequested`.

   Then implement both and update `FakeTrayParts.cs`.
4. **Velopack adapter and log (about 1.5 hours).**
   - Add `Velopack` to the Infrastructure allowed list in
     `LayerDependencyTests` and see the new adapter compile.
   - `VelopackUpdateService` runs `CheckForUpdatesAsync` and
     `DownloadUpdatesAsync` inside `Task.Run`, so no Velopack work runs on the
     UI, hook, or render thread. It returns `TargetFullRelease.Version` as a
     string, and keeps the downloaded `VelopackAsset` for `ApplyAfterExit`.
   - `UpdateFailureLog` tests follow the `EffectFailureLog` tests: line
     format, append, and tab/newline flattening.
   - There is no unit test for the Velopack calls themselves; steps 6–7
     cover them on Windows.
5. **Composition (about 1 hour).** In `App.OnStartup`, after
   `trayApplication.Start()`:
   - Build the source from `DRAWEM_UPDATE_FEED` or `GithubSource`, then the
     service, reporter, and controller.
   - Subscribe `trayApplication.UpdateRequested` to the controller. Consent is
     a `MessageBox` Yes/No owned by the UI thread. The exit action is
     `trayApplication.Exit`.
   - Put the controller and reporter first in the `CompositeDisposable`, so
     update work stops before hooks and sound.
   - Run the full suite.
6. **Local upgrade check (about 2.5 hours, manual Windows, standard user).**
   1. Pack version A (this branch) and version B (A plus a patch-version
      bump) with R4-06's `New-VelopackPackage.ps1` into `C:\drawem-feed`.
   2. Install A from its Setup.exe. Put a schema 3 `settings.json` and the
      managed sounds it references in `%LOCALAPPDATA%\drawEM` (startup
      deletes unreferenced ones). Start and exit A once, then record recursive
      SHA-256 of `%LOCALAPPDATA%\drawEM` without `logs\`.
   3. Start A with `DRAWEM_UPDATE_FEED=C:\drawem-feed`. Confirm a balloon and
      the menu item for B, and that drawing works while B downloads.
   4. Choose No. Confirm the item stays. Exit and restart A with the feed
      folder renamed away. Confirm the item returns: the pending package is
      used, not a new download.
   5. Choose Yes. Confirm A exits fully, B starts once, there is no "already
      running" dialog, the tray icon and global shortcuts work, and the hashes
      match. Record the time from Yes to B's tray icon.
   6. Repeat 2–5 (reinstall A) with a schema 1 and then a schema 2
      `settings.json`. B migrates them only on Save, so the hashes must still
      match after the upgrade.
   7. With the feed folder renamed away, start B. Confirm the failure is in
      `update.log` and the app works.
   8. Start the portable ZIP build of B. Confirm `update.log` gains no line
      and the menu has no update item.
7. **GitHub upgrade check (manual, after publication).** After the owner
   publishes the R4-07 release and the next release, upgrade from the first
   to the second through `GithubSource` without `DRAWEM_UPDATE_FEED`. Record
   both versions and artifact IDs. Until then this criterion stays
   unverified in [acceptance](acceptance.md).
8. **Docs (about 45 minutes).**
   - Root `README.md` Updates section: when checks happen, how to defer and
     install, where `update.log` is, and that portable and WinGet copies
     update manually.
   - Installs from the R4-06 release have no updater. Those users run the
     newer Setup.exe once to get it.
   - Recovery: if an update does not start, uninstall from Apps & Features
     (settings stay, as R4-06 verified) and install the previous Setup.exe
     from GitHub Releases.
   - Add the delta-package entry to the out-of-scope list.
9. **PR.** Branch `R4-07.1-tray-updates` (this plan is on
   `R4-07-tray-updates-plan`). The body names the release
   publishing package, lists the shared files (`App.xaml.cs`, `README.md`),
   and states the rebase on the latest visual-effects changes.

## Risks

- `WaitExitThenApplyUpdates` waits 60 seconds. Exit cleanup is bounded (sound
  shutdown timeout, one-second log drains), but a hung exit would leave the
  update pending; the next start offers it again. Step 6.5 records the actual
  exit time.
- `DownloadUpdatesAsync` takes Velopack's global lock and throws
  `AcquireLockFailedException` if another operation runs. The controller
  treats it as a normal failure and retries at the next tick.
- Unsigned releases: the updater replaces files without a SmartScreen prompt,
  but a new `Update.exe` run may still be screened. Record what Windows shows
  in step 6.
