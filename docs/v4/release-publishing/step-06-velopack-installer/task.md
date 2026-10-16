# v4 / R4-06: package a per-user installer

**Status:** pending. **Source:** [roadmap](../ROADMAP.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-07](../step-07-tray-updates/task.md).

## Task

Package the Windows x64 app with Velopack, adding a per-user Setup.exe and
update packages while retaining the portable ZIP. Add the WPF startup entry
point Velopack requires.

Set the Velopack package ID to `drawEM.Desktop`. It must differ from `drawEM`
in a case-insensitive comparison, and it must not change after the first
release that ships the installer. This is a defensive requirement: for the
tested default install, the Velopack install directory and any directory that
Velopack removes on uninstall must not be, or contain, the settings directory
`%LOCALAPPDATA%\drawEM`. Record the actual default install directory on
Windows; do not assume its path.

Custom install destinations are out of scope. They are not supported until a
separate check proves that uninstall from such a destination preserves
`%LOCALAPPDATA%\drawEM`.

The installer is added to an already published release line: the portable ZIP,
checksums, provenance, conditional signing, and WinGet listing exist before
this step. Keep each of them working and extend them to the new downloads.

**Depends on:** R4-01, R4-02, R4-03; R4-04 and R4-05 when they are done.
**Shared files:** `App.xaml`, `DrawEM.App.csproj`, `README.md`, and `ci.yml`;
follow [the shared-file rules](../../README.md#rules-for-parallel-work) and
coordinate against the current visual-effects work.

See [plan](plan.md) and [acceptance](acceptance.md).
