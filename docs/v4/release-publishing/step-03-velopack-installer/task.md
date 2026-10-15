# v4 / R4-03: package a per-user installer

**Status:** pending. **Source:** [roadmap](../ROADMAP.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-04](../step-04-tray-updates/task.md).

## Task

Package the Windows x64 app with Velopack, adding a per-user Setup.exe and
update packages while retaining the portable ZIP. Add the WPF startup entry
point Velopack requires.

**Depends on:** R4-01. **Shared files:** `App.xaml.cs`, `DrawEM.App.csproj`,
and possibly `ci.yml`; coordinate against the current visual-effects work.

See [plan](plan.md) and [acceptance](acceptance.md).
