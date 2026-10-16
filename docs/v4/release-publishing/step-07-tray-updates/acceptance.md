# R4-07: acceptance criteria

**Task:** [tray updates](task.md). **Plan:** [implementation](plan.md).

- [x] `UpdateControllerTests` fail before implementation and pass afterwards.
- [x] Deterministic tests prove one startup check and no more than one
  periodic check per 24 hours, including the +24 h boundary, and no
  overlapping checks.
- [ ] Update checks/downloads run off the UI, keyboard-hook, and render paths.
- [ ] Network/service failure leaves the current app usable and is logged to
  `%LOCALAPPDATA%\drawEM\logs\update.log`.
- [ ] One balloon per downloaded version; the tray item
  `Restart to update to <version>` stays until applied.
- [ ] User can defer an update without losing the tray item or current
  session; a deferred update is offered again after an app restart without a
  new download; no update installs or restarts before Yes in the
  confirmation dialog.
- [ ] Confirming the update runs the normal exit cleanup first, then the new
  version starts once with no "already running" dialog.
- [ ] Portable ZIP and WinGet portable installs make no update check and show
  no update prompt.
- [x] `LayerDependencyTests` pass; only Infrastructure references `Velopack`
  besides `Program.Main`.
- [ ] Manual Windows, unverified until done: a local-feed upgrade
  (`DRAWEM_UPDATE_FEED`) preserves schema 1, 2, and 3 settings and sound
  files (recursive SHA-256, `logs\` excluded) and tray behavior.
- [ ] Manual Windows, unverified until done: a GitHub upgrade between two
  published releases that both contain R4-07 succeeds; old/new versions and
  artifact IDs are recorded.
- [x] Root README documents check, defer, install, recovery, `update.log`,
  manual updates for portable copies, and the one-time Setup.exe run for
  R4-06 installs.
- [x] Delta packages are listed in [out of scope](../../out-of-scope/README.md).

## Evidence

- Test results: `UpdateControllerTests` first failed to compile (types
  missing), then 15 of 15 passed. The Infrastructure layer rule failed once
  `VelopackUpdateService` existed and passed after `Velopack` was allowed.
  Full suite on branch `R4-07.1-tray-updates` (on top of R4-06): 867 passed,
  0 failed (`dotnet test DrawEM.sln -c Release`, 2026-10-16).
- Windows version and account type:
- Old/new build identities:
- Defer, accept, restart, and recovery results:
- Time from Yes to the new tray icon:
- Settings hashes before and after upgrade (`logs\` excluded):

## Validation

- Automated: controller cadence, single in-flight check, one notification
  per version, pending version at startup, consent No/Yes order, failure
  reporting, and disposal; tray item and `UpdateRequested` forwarding;
  `update.log` format; `DRAWEM_UPDATE_FEED` feed choice and its log lines;
  `VelopackUpdateService` fallback when Velopack refuses to create a manager
  (logged, treated as not installed). Not tested automatically: the Velopack
  calls themselves. `CheckAndDownloadAsync` runs them in `Task.Run`, so the
  off-UI-thread criterion holds by code review only until step 6 runs.
- CI: not run (not pushed).
- Manual Windows (local feed): not done. Step 6 has not been run, including
  the portable-copy check that `update.log` gains no line.
- Manual Windows (GitHub feed): not done; needs two published releases that
  contain R4-07.
