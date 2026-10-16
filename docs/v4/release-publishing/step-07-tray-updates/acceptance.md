# R4-07: acceptance criteria

**Task:** [tray updates](task.md). **Plan:** [implementation](plan.md).

- [ ] `UpdateControllerTests` fail before implementation and pass afterwards.
- [ ] Deterministic tests prove one startup check and no more than one
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
- [ ] `LayerDependencyTests` pass; only Infrastructure references `Velopack`
  besides `Program.Main`.
- [ ] Manual Windows, unverified until done: a local-feed upgrade
  (`DRAWEM_UPDATE_FEED`) preserves schema 1, 2, and 3 settings and sound
  files (recursive SHA-256, `logs\` excluded) and tray behavior.
- [ ] Manual Windows, unverified until done: a GitHub upgrade between two
  published releases that both contain R4-07 succeeds; old/new versions and
  artifact IDs are recorded.
- [ ] Root README documents check, defer, install, recovery, `update.log`,
  manual updates for portable copies, and the one-time Setup.exe run for
  R4-06 installs.
- [ ] Delta packages are listed in [out of scope](../../out-of-scope/README.md).

## Evidence

- Test results:
- Windows version and account type:
- Old/new build identities:
- Defer, accept, restart, and recovery results:
- Time from Yes to the new tray icon:
- Settings hashes before and after upgrade (`logs\` excluded):

## Validation

- Automated:
- CI:
- Manual Windows (local feed):
- Manual Windows (GitHub feed):
