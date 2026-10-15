# R4-03: acceptance criteria

**Task:** [Velopack installer](task.md). **Plan:** [implementation](plan.md).

- [ ] Startup/package test fails before implementation and passes afterwards.
- [ ] `drawEM-<version>-win-x64-Setup.exe` and required Velopack update
  packages are produced for the tag version; the separately named portable ZIP
  is still produced.
- [ ] Every new installer/update download is listed in `SHA256SUMS.txt` and has
  a verified attestation under the repository identity.
- [ ] A standard user installs without elevation, launches the tray app, and
  sees Start menu and uninstall entries.
- [ ] Uninstall removes application files without deleting user settings.
- [ ] The custom WPF `Main` runs Velopack hooks before WPF initialization and
  does not initialize tray, hook, or settings services during fast-exit hooks.
- [ ] Settings remain under `%LOCALAPPDATA%\drawEM`; schema 1 and schema 2
  files are preserved across install/update.
- [ ] Windows CI packaging smoke succeeds with the app self-contained,
  not trimmed, and not single-file.

## Evidence

- Workflow run and artifact names:
- Windows version and account type:
- Install, launch, uninstall results:
- Settings before/after:

## Validation

- Automated:
- CI:
- Manual Windows:
