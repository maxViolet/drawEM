# R4-05: acceptance criteria

**Task:** [WinGet](task.md). **Plan:** [implementation](plan.md).

- [x] Initial manifest passes `winget validate --manifest <path>`.
- [x] Manifest uses a stable version-specific GitHub URL and the exact
  `InstallerSha256` of the portable ZIP, with a nested portable
  `DrawEM.App.exe`.
- [ ] WinGet repository accepts the initial package manifest; review URL and
  PR are recorded.
- [ ] Subsequent stable release workflow creates a manifest PR with matching
  version and hash; it does not publish or modify the GitHub release.
- [ ] Any required cross-repository token is stored only in the protected
  `release` Environment with minimum needed permission.
- [ ] Clean Windows install, upgrade, and uninstall through WinGet succeed;
  uninstall keeps `%LOCALAPPDATA%\drawEM`.
- [ ] Catalog review status is not reported as a GitHub release CI result.

## Evidence

- Manifest validation output: `Manifest validation succeeded.` for
  [`packaging/winget/4.0.0`](../../../../packaging/winget/4.0.0) (schema
  1.12.0, winget v1.29.380). Package ID `maxViolet.drawEM`,
  `License: Proprietary` (owner decision, 2026-10-19). `InstallerUrl` is the
  `v4.0.0` ZIP; `InstallerSha256` matches its `SHA256SUMS.txt` line.
- Initial submission PR and result:
  [microsoft/winget-pkgs#455876](https://github.com/microsoft/winget-pkgs/pull/455876),
  opened 2026-10-19. CLA signed; automatic checks 01–07 (including Installers
  Scan) passed; Installation Validation and moderator review pending.
- Automation workflow run: not started; waits for the accepted manifest.
- Windows install/upgrade/uninstall (local manifest, 2026-10-19, same machine
  as R4-03): `winget install --manifest` verified the hash and extracted the
  complete publish output (265 files) to
  `%LOCALAPPDATA%\Microsoft\WinGet\Packages\maxViolet.drawEM__DefaultSource`.
  The first manifest used `PortableCommandAlias`; WinGet's symlink failed with
  `The application to execute does not exist: '...\WinGet\Links\DrawEM.App.dll'`,
  because the .NET app host resolves its DLL next to the link. The submitted
  manifest sets `ArchiveBinariesDependOnPath: true`, which adds the install
  folder to the user PATH instead; `DrawEM.App` then started from that folder
  and showed the tray icon. Uninstall (by the ARP ID, since `--id` does not
  match a local-manifest install) removed the folder and kept
  `%LOCALAPPDATA%\drawEM`, but left the PATH entry; recheck with a catalog
  install. WinGet adds no Start menu shortcut for a portable package. Upgrade
  is not tested: it needs a second release.

## Validation

- Automated: `winget validate` (above). No code change.
- CI: not run yet for this branch.
- External WinGet review: pending on #455876.
- Manual Windows: local-manifest install, start, and uninstall above.
