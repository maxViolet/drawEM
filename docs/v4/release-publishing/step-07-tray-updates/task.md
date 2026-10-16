# v4 / R4-07: add consent-based tray updates

**Status:** pending. **Source:** [roadmap](../ROADMAP.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** none; this is the last release-publishing step.

## Task

Let the tray app check for updates at startup and every 24 hours, download in
the background, and install only after the user confirms an app restart.
Handle update failures without preventing normal app startup. Updates apply
only to Velopack installs; the portable ZIP and WinGet portable installs keep
updating manually and must not show update prompts.

**Depends on:** R4-06. Installs from the R4-06 release have no updater, so
the first in-app upgrade is tested from a local feed before publication and
through GitHub after two releases containing R4-07 exist. **Shared files:**
`App.xaml.cs`, `README.md`, and test architecture rules; follow
[the shared-file rules](../../README.md#rules-for-parallel-work) and
coordinate with visual-effects changes.

See [plan](plan.md) and [acceptance](acceptance.md).
