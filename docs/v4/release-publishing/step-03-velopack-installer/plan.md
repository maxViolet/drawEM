# R4-03: implementation plan

**Task:** [Velopack installer](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a failing startup/package smoke test describing Velopack bootstrap,
   package identity, and preservation of the existing settings path.
2. Add the Velopack package/tooling and define stable package ID, title, main
   executable, version, and release channel. Use package ID `drawEM.Desktop`,
   which differs from `drawEM` in a case-insensitive comparison. The goal is
   that the default install and uninstall directories are not, and do not
   contain, the settings directory `%LOCALAPPDATA%\drawEM`; step 5 verifies
   this on Windows. Add a test that fails if the package ID equals `drawEM`
   in a case-insensitive comparison. Custom install destinations are out of
   scope unless a separate check proves that uninstall preserves settings.
3. Replace generated WPF entry point with a custom `Main`; call
   `VelopackApp.Build().Run()` before WPF initialization, and preserve startup
   and shutdown behavior.
4. Package `Setup.exe` and Velopack update artifacts on Windows. Publish the
   installer as `drawEM-<version>-win-x64-Setup.exe`; keep tool-generated names
   for Velopack feed files. Keep the self-contained publish settings and retain
   `drawEM-<version>-win-x64.zip` as a separate portable artifact. Extend
   `SHA256SUMS.txt` and
   `actions/attest@v4` subjects to every new Velopack download; verify final
   hashes and attestations.
5. Install to the default location and uninstall as a standard Windows user;
   verify settings stay in `%LOCALAPPDATA%\drawEM` and the tray app still
   starts with global hooks. Record the actual default install directory.
   After the app exits and immediately before uninstall, record recursive
   SHA-256 hashes of every file under `%LOCALAPPDATA%\drawEM` except
   `logs\`. After uninstall, repeat the hashes and confirm they match.
   Exclude `logs\` because launching the app can append to its log files.
6. Add a package smoke check to CI without making the v4 visual-effects package
   a prerequisite.
