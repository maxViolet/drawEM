# R4-07: acceptance criteria

**Task:** [tray updates](task.md). **Plan:** [implementation](plan.md).

- [ ] Deterministic tests prove one startup check and no more than one
  periodic check per 24 hours, including clock-boundary behavior.
- [ ] Update checks/downloads run off the UI, keyboard-hook, and render paths.
- [ ] Network/service failure leaves the current app usable and is logged.
- [ ] User can defer an update without losing the notification or current
  session; no update installs or restarts before confirmation.
- [ ] Confirming update closes/restarts safely and launches the new version.
- [ ] Portable ZIP and WinGet portable installs make no update check and show
  no update prompt.
- [ ] Windows upgrade preserves schema 1, 2, and 3 settings, sound files, and
  tray behavior; failed/interrupted update has a documented recovery path.
- [ ] Manual Windows checks record the exact old/new versions and artifact IDs.
- [ ] Root README documents check, defer, install, and recovery.

## Evidence

- Test results:
- Windows version and account type:
- Old/new build identities:
- Defer, accept, restart, and recovery results:

## Validation

- Automated:
- CI:
- Manual Windows:
