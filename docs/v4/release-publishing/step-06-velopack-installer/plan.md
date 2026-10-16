# R4-06: implementation plan

**Task:** [Velopack installer](task.md). **Acceptance:** [criteria](acceptance.md).

## Starting point

- R4-01, R4-02, and R4-03 must be merged and the first portable-only release
  published. R4-01 is merged: `release.yml` verifies the tag with
  `tools/release/Get-ReleaseVersion.ps1`, builds the ZIP with
  `tools/release/New-ReleasePackage.ps1`, and creates a draft with
  `--verify-tag`. R4-02 adds `SHA256SUMS.txt` through
  `Write-Sha256Sums.ps1`, and the attestation reads its subjects from that
  file. Follow the same convention: release scripts in `tools/release/`,
  tested from `tests/DrawEM.Tests/Release/ReleaseScriptsTests.cs`. If R4-04
  (signing) or R4-05 (WinGet) is done, steps 5a and 9 apply.
- `App.xaml` is the generated WPF entry point (`ApplicationDefinition`);
  `App.OnStartup` acquires `SingleInstanceGuard`, then loads settings, sounds,
  effects, and global hooks. No `Main` exists yet.
- The main executable is `DrawEM.App.exe` (no `AssemblyName` override). The
  publish profile `Properties/PublishProfiles/win-x64.pubxml` is self-contained,
  not trimmed, and not single-file; keep it unchanged.
- Settings, sounds, and logs live under `%LOCALAPPDATA%\drawEM`
  (`JsonSettingsStore.DefaultDirectory`, `ManagedSoundLibrary.DefaultDirectory`,
  `*FailureLog.DefaultPath`). The settings schema is version 3; the reader
  accepts 1–3.
- Velopack facts used below, from the
  [installer docs](https://docs.velopack.io/packaging/installer) and
  [integration docs](https://docs.velopack.io/integrating/overview):
  `Setup.exe` installs to `%LocalAppData%\{packId}`; uninstall removes the
  whole `{packId}` folder; `VelopackApp.Build().Run()` must be the first code
  in `Main` and exits inside `Run()` on `--veloapp-install`, `--veloapp-updated`,
  `--veloapp-obsolete`, and `--veloapp-uninstall`; `Run()` auto-applies a newer
  locally downloaded package on launch unless `SetAutoApplyOnStartup(false)`.

## Steps

1. **Red tests.** Add `tests/DrawEM.Tests/Release/` with two test classes and
   run them; record the failures in [acceptance](acceptance.md).
   - `VelopackPackageIdentityTests` reads the package ID from the app
     assembly's `AssemblyMetadata("VelopackPackId")`. It asserts the ID is
     `drawEM.Desktop`, differs from `drawEM` under
     `StringComparison.OrdinalIgnoreCase`, and that
     `%LocalAppData%\<id>` is neither equal to nor an ancestor of
     `JsonSettingsStore.DefaultDirectory`. Compare full, normalized paths by
     segment, ignoring case; a plain `StartsWith` would miss the
     `drawEM` / `drawEM.Desktop` boundary. Expected failure: the metadata does
     not exist.
   - `VelopackHookExitTests` starts `DrawEM.App.exe` from the test output
     folder with `--veloapp-install 0.0.0` and with
     `--veloapp-uninstall 0.0.0`. Each process must exit with code 0 within
     10 seconds; otherwise the test kills it and fails. Expected failure
     today: the app ignores the arguments and keeps running as a tray app (or
     shows the "already running" dialog), so it times out.
2. **Single package ID source.** In `DrawEM.App.csproj`, add the property
   `<VelopackPackId>drawEM.Desktop</VelopackPackId>` and emit it as
   `<AssemblyMetadata Include="VelopackPackId" Value="$(VelopackPackId)" />`.
   Workflows read the same value with
   `dotnet msbuild src/DrawEM.App/DrawEM.App.csproj -getProperty:VelopackPackId`,
   so the test and `vpk pack` cannot drift. The ID must not change after the
   first release.
3. **Velopack tooling.** Add the `Velopack` NuGet package to
   `DrawEM.App.csproj` and the `vpk` CLI as a local tool in
   `.config/dotnet-tools.json`, both at the same reviewed stable version;
   record that version here. Workflows run `dotnet tool restore`, not a global
   install.
4. **Custom `Main`.** Add `src/DrawEM.App/Program.cs` in namespace
   `DrawEM.App`:
   `[STAThread] static void Main()` calls
   `VelopackApp.Build().SetAutoApplyOnStartup(false).Run()` as its first
   statement, then `var app = new App(); app.InitializeComponent(); app.Run();`.
   In the csproj, change `App.xaml` from `ApplicationDefinition` to `Page`
   (`<ApplicationDefinition Remove="App.xaml" />`, `<Page Include="App.xaml" />`).
   - Register no fast callbacks: install, update, and uninstall hooks must not
     touch settings, sounds, logs, the single-instance mutex, tray, or hooks.
   - `SetAutoApplyOnStartup(false)`: R4-07 owns consent-based updates; a
     silent apply-and-restart on launch is rejected in the
     [roadmap](../ROADMAP.md).
   - `App.OnStartup` and `OnExit` stay unchanged. Run step 1 tests and the
     full suite, including `LayerDependencyTests`; they must pass.
5. **Release packaging.** Add `tools/release/New-VelopackPackage.ps1`, with a
   failing `ReleaseScriptsTests` case first for the output file names and the
   rename. Call it in `release.yml` after the `Package` step and before
   checksums. It runs:
   ```text
   dotnet tool restore
   dotnet vpk pack --packId <VelopackPackId> --packVersion <tag version>
     --packDir src/DrawEM.App/bin/Release/net8.0-windows/win-x64/publish
     --mainExe DrawEM.App.exe --packTitle drawEM
     --icon src/DrawEM.App/Assets/drawEM.ico
     --runtime win-x64 --channel win --noPortable --outputDir velopack
   ```
   - Rename `drawEM.Desktop-win-Setup.exe` to
     `drawEM-<version>-win-x64-Setup.exe`.
   - Keep tool-generated names for the update feed:
     `drawEM.Desktop-<version>-full.nupkg` and `releases.win.json`.
   - Do not upload `RELEASES` (Squirrel migration only) or `assets.win.json`
     (build metadata, not a user download).
   - `--noPortable`: the existing `drawEM-<version>-win-x64.zip` stays the only
     portable download; it is built from the same publish folder.
   - No delta packages in R4-06: the first installer release has no Velopack
     predecessor. Adding deltas (download the previous release, then pack) is
     R4-07 or later.
   - Use Velopack's default shortcuts (Start menu root and Desktop); record
     both in acceptance evidence.

   **5a. Signing, only if R4-04 is done.** Keep app-file signing before
   `vpk pack`, so the `.nupkg` and Setup.exe contain signed binaries. Then
   prototype signing Setup.exe and Update.exe and prove it does not break
   package metadata or install. Wire it into the release only after the
   prototype passes; checksums and attestations run after signing.
6. **Checksums and attestations.** Extend the R4-02 release-asset test first
   (red), then pass the Setup.exe, the full `.nupkg`, and `releases.win.json`
   to `Write-Sha256Sums.ps1` next to the portable ZIP. The attestation
   subjects follow from `SHA256SUMS.txt`, and `gh release create` uploads the
   same files. Verify each file with
   `gh attestation verify <file> -R maxViolet/drawEM` and
   `Get-FileHash -Algorithm SHA256`.
7. **CI packaging smoke.** In `ci.yml`, after `Publish`, run
   `New-VelopackPackage.ps1` with version `0.0.0-ci.<run number>` into a temp
   folder, then fail unless exactly the expected Setup.exe, full `.nupkg`, and
   `releases.win.json` exist. Do not upload them. The job must not depend on
   visual-effects work.
8. **Manual Windows check** as a standard (non-admin) user, recorded in
   [acceptance](acceptance.md):
   1. Create settings in `%LOCALAPPDATA%\drawEM` with the portable ZIP build.
   2. Run the Setup.exe from a draft or CI build; confirm no UAC prompt.
      Record the actual install directory (expected
      `%LOCALAPPDATA%\drawEM.Desktop`), the Start menu, Desktop, and
      Apps & Features entries.
   3. Confirm the tray icon appears, a drawing shortcut works (global hooks),
      and the saved settings loaded.
   4. Exit drawEM. Record recursive SHA-256 hashes of every file under
      `%LOCALAPPDATA%\drawEM` except `logs\`.
   5. Uninstall from Apps & Features. Confirm the install directory is gone,
      `%LOCALAPPDATA%\drawEM` exists, and the hashes match.
   6. Start the portable ZIP build again; confirm it still runs with the same
      settings.
9. **WinGet, only if R4-05 is done.** Leave the WinGet manifest on the
   portable ZIP. Switching it to Setup.exe changes the installed location
   and upgrade path for WinGet users, so it needs its own task under
   [out of scope](../../out-of-scope/README.md).
10. **README.** Add install, uninstall, and verification instructions for
    `drawEM-<version>-win-x64-Setup.exe` to the root `README.md`. State that
    in-app updates arrive with R4-07 and that portable users switch by
    installing; settings are shared.
11. **PR.** Branch `R4-06-velopack-installer`. The body names the release
    publishing package, lists the shared files touched (`App.xaml`,
    `DrawEM.App.csproj`, `README.md`, `ci.yml`), and states the rebase on
    the latest visual-effects changes.

## Risks

- `Run()` behavior when the app is not installed (portable ZIP, test output):
  the docs describe hooks and auto-apply, not this case. Step 1's process test
  and step 8.6 cover it; if `Run()` fails outside an install, stop and record
  the behavior before adding workarounds.
- `vpk pack` checks that the main executable calls `VelopackApp`; do not pass
  `--skipVeloAppCheck`. A failing check means step 4 is incomplete.
- Unsigned Setup.exe will trigger SmartScreen. Signing is R4-04 and may be
  blocked on owner decisions; record the warning text in acceptance evidence.
