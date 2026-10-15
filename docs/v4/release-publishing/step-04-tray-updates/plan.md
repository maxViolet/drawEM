# R4-04: implementation plan

**Task:** [tray updates](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add deterministic tests with a fake update source and clock for startup
   checks, the 24-hour cadence, duplicate suppression, deferral, and failures.
2. Check for updates after tray startup and once per 24 hours while running;
   do not run network work on the UI, hook, or render thread.
3. Download the update in the background and show one actionable notification
   with version and release notes. Let the user defer or confirm restart.
4. On confirmation, use Velopack's apply-and-restart flow. Do not interrupt an
   active drawing/sound operation without giving the user the confirmation.
5. Preserve settings and managed sound files under `%LOCALAPPDATA%\drawEM`.
   Confirm update with schema 1 and schema 2 settings and recovery after a
   failed update or interrupted shutdown.
6. Document how to check, defer, install, and recover from an update.
