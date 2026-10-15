# R4-07: implementation plan

**Task:** [WinGet](task.md). **Acceptance:** [criteria](acceptance.md).

1. Prepare and validate the initial multi-file manifest for the stable
   Setup.exe, using a version-specific official GitHub release URL and the
   exact SHA-256. Confirm installer type and unattended install behavior.
2. Submit the first manifest PR manually; address automatic validation and
   moderator feedback. Record acceptance separately from release CI.
3. Add release-workflow automation for subsequent version-specific manifest
   PRs only after the first accepted manifest establishes package identity.
4. Use a narrowly scoped external token only if the selected submission method
   requires it. Store it in the reviewer-protected `release` Environment and
   document its repository permissions without recording its value.
5. Test the local manifest and install/update/uninstall through WinGet on
   Windows. Do not block GitHub release publication on catalog review.
