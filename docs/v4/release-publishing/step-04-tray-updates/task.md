# v4 / R4-04: add consent-based tray updates

**Status:** pending. **Source:** [roadmap](../ROADMAP.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-05](../step-05-first-release-acceptance/task.md).

## Task

Let the tray app check for updates at startup and every 24 hours, download in
the background, and install only after the user confirms an app restart.
Handle update failures without preventing normal app startup.

**Depends on:** R4-03. **Shared files:** `App.xaml.cs`, `DrawEM.App.csproj`,
`README.md`, and `ci.yml`; follow
[the shared-file rules](../../README.md#rules-for-parallel-work) and
coordinate with visual-effects changes.

See [plan](plan.md) and [acceptance](acceptance.md).
