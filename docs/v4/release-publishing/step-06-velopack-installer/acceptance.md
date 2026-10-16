# R4-06: acceptance criteria

**Task:** [Velopack installer](task.md). **Plan:** [implementation](plan.md).

- [x] Startup/package test fails before implementation and passes afterwards.
- [ ] `drawEM-<version>-win-x64-Setup.exe` and required Velopack update
  packages are produced for the tag version; the separately named portable ZIP
  is still produced.
- [ ] Every new installer/update download is listed in `SHA256SUMS.txt` and has
  a verified attestation under the repository identity.
- [ ] A standard user installs without elevation, launches the tray app, and
  sees Start menu and uninstall entries.
- [x] The Velopack package ID is `drawEM.Desktop`; an automated test fails if
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
- [x] Custom install destinations are out of scope for this step; they are not
  claimed to preserve settings.
- [x] The custom WPF `Main` runs Velopack hooks before WPF initialization and
  does not initialize tray, hook, or settings services during fast-exit hooks.
- [ ] Settings remain under `%LOCALAPPDATA%\drawEM`; schema 1, 2, and 3
  files are preserved across install and uninstall.
- [ ] Windows CI packaging smoke succeeds with the app self-contained,
  not trimmed, and not single-file.
- [ ] If R4-04 is done: app binaries, Setup.exe, and Update.exe carry valid
  signatures from the approved publisher, and checksums and attestations
  identify the signed outputs. Otherwise the unsigned installer is recorded.
- [ ] If R4-05 is done: the WinGet manifest still points at the portable ZIP
  and WinGet install of the new version still starts drawEM.
- [x] Root README has install, uninstall, and verification instructions for
  the Setup.exe and states that in-app updates are not yet available.

## Evidence

- Workflow run and artifact names: no tag or CI run yet. Local run on
  2026-10-16 (`dotnet publish -p:PublishProfile=win-x64
  -p:Version=0.0.1-local.1`, then `New-VelopackPackage.ps1` with the real
  `vpk` 1.2.161): vpk logged "Verified VelopackApp.Run() in
  'System.Void DrawEM.App.Program::Main()'" and "Shortcuts:
  Desktop,StartMenuRoot", and the script listed
  `drawEM-0.0.1-local.1-win-x64-Setup.exe` (86,612,984 bytes),
  `drawEM.Desktop-0.0.1-local.1-full.nupkg` (79,048,184 bytes), and
  `releases.win.json`. `RELEASES` and `assets.win.json` stayed unlisted.
  `Write-Sha256Sums.ps1` hashed all three. Every file is unsigned (vpk: "No
  signing parameters provided"); R4-04 is not done.
- Windows version and account type:
- Install, launch, uninstall results:
- Default install directory (manual, unverified until recorded):
- Settings after app exit and after uninstall (recursive file list and
  SHA-256, `logs\` excluded):

## Validation

- Automated: before implementation, the 3 `VelopackPackageIdentityTests`
  facts failed (no `VelopackPackId` metadata), and 6 of 10
  `New-VelopackPackage.ps1` cases failed (no script). `VelopackHookExitTests`
  was not run red, because before `Program.Main` it starts the real tray app
  against the user's `%LOCALAPPDATA%\drawEM`. After, on 2026-10-16: full
  suite 836 passed, 0 failed; both hook arguments exit with code 0.
- CI: not run yet (not pushed).
- Manual Windows: not done. Install, launch, uninstall, settings hashes, the
  default install directory, and SmartScreen text are unverified.
