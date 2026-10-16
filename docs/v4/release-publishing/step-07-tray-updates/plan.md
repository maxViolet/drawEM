# R4-07: implementation plan

**Task:** [tray updates](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add deterministic tests with a fake update source and clock for startup
   checks, the 24-hour cadence, duplicate suppression, deferral, and failures.
   Add a test that a non-Velopack install (portable ZIP or WinGet portable)
   performs no check and shows no prompt.
2. Check for updates after tray startup and once per 24 hours while running;
   do not run network work on the UI, hook, or render thread.
3. Download the update in the background and show one actionable notification
   with version and release notes. Let the user defer or confirm restart.
4. On confirmation, use Velopack's apply-and-restart flow. Do not interrupt an
   active drawing/sound operation without giving the user the confirmation.
   Keep `SetAutoApplyOnStartup(false)` from R4-06.
5. Preserve settings and managed sound files under `%LOCALAPPDATA%\drawEM`.
   Confirm update with schema 1, 2, and 3 settings and recovery after a
   failed update or interrupted shutdown.
6. Upgrade from the first published installer release to a newer draft on
   real Windows; record both versions and artifact IDs.
7. Document in the root README how to check, defer, install, and recover from
   an update, and that portable installs still update manually.
