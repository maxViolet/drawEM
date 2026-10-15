# R4-03: implementation plan

**Task:** [Velopack installer](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a failing startup/package smoke test describing Velopack bootstrap,
   package identity, and preservation of the existing settings path.
2. Add the Velopack package/tooling and define stable package ID, title, main
   executable, version, and release channel.
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
5. Install and uninstall as a standard Windows user; verify settings stay in
   `%LOCALAPPDATA%\drawEM` and the tray app still starts with global hooks.
6. Add a package smoke check to CI without making the v4 visual-effects package
   a prerequisite.
