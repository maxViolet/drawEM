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
- [ ] The Velopack package ID is `drawEM.Desktop`; an automated test fails if
  the package ID equals `drawEM` in a case-insensitive comparison.
- [ ] Manual Windows check, unverified until done: install to the default
  location and record the actual install directory. It is not
  `%LOCALAPPDATA%\drawEM` and does not contain it.
- [ ] Uninstall removes application files without deleting user settings.
- [ ] Manual Windows check, unverified until done: create settings in
  `%LOCALAPPDATA%\drawEM`, install to the default location, launch drawEM,
  and exit it. Immediately before uninstall, record recursive SHA-256 hashes
  of every file under `%LOCALAPPDATA%\drawEM` except `logs\`. Uninstall
  drawEM. After uninstall, `%LOCALAPPDATA%\drawEM` still exists and the
  recursive hashes, with `logs\` excluded, match the recorded hashes. `logs\`
  is excluded because launch can append to log files.
- [ ] Custom install destinations are out of scope for this step; they are not
  claimed to preserve settings.
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
- Default install directory (manual, unverified until recorded):
- Settings after app exit and after uninstall (recursive file list and
  SHA-256, `logs\` excluded):

## Validation

- Automated:
- CI:
- Manual Windows:
