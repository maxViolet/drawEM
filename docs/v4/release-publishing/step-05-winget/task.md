# v4 / R4-05: list stable releases in WinGet

**Status:** in progress; initial manifest submitted as microsoft/winget-pkgs#455876, review pending. **Source:** [roadmap](../ROADMAP.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-06](../step-06-velopack-installer/task.md).

## Task

Publish the stable portable ZIP in the WinGet community catalog as a `zip`
installer with a nested `portable` executable. Make the first manifest
submission manually and automate later version-specific manifest pull requests
from the release workflow. [R4-06](../step-06-velopack-installer/task.md)
keeps this manifest on the portable ZIP; switching it to the Velopack
Setup.exe needs a separate task.

**Depends on:** R4-03 and a stable public portable ZIP. WinGet catalog review
is not a GitHub release gate.

See [plan](plan.md) and [acceptance](acceptance.md).
