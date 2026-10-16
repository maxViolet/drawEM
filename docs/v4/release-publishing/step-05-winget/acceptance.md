# R4-05: acceptance criteria

**Task:** [WinGet](task.md). **Plan:** [implementation](plan.md).

- [ ] Initial manifest passes `winget validate --manifest <path>`.
- [ ] Manifest uses a stable version-specific GitHub URL and the exact
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

- Manifest validation output:
- Initial submission PR and result:
- Automation workflow run:
- Windows install/upgrade/uninstall:

## Validation

- Automated:
- CI:
- External WinGet review:
- Manual Windows:
