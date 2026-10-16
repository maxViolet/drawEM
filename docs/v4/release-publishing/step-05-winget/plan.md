# R4-05: implementation plan

**Task:** [WinGet](task.md). **Acceptance:** [criteria](acceptance.md).

1. Prepare and validate the initial multi-file manifest for the stable
   `drawEM-<version>-win-x64.zip`, using a version-specific official GitHub
   release URL and the exact SHA-256. Use `InstallerType: zip`,
   `NestedInstallerType: portable`, and `NestedInstallerFiles` pointing at
   `DrawEM.App.exe`. Confirm on Windows that WinGet extracts the complete
   publish output, not only the executable, and that the installed app
   starts.
2. Submit the first manifest PR manually; address automatic validation and
   moderator feedback. Record acceptance separately from release CI.
3. Add release-workflow automation for subsequent version-specific manifest
   PRs only after the first accepted manifest establishes package identity.
4. Use a narrowly scoped external token only if the selected submission method
   requires it. Store it in the reviewer-protected `release` Environment and
   document its repository permissions without recording its value.
5. Test the local manifest and install/upgrade/uninstall through WinGet on
   Windows. Confirm uninstall leaves `%LOCALAPPDATA%\drawEM` in place. Do not
   block GitHub release publication on catalog review.
