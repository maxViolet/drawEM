# v3 / Step 6: implementation plan

**Task:** [runtime application](task.md). **Acceptance:** [criteria](acceptance.md).

1. Load settings in `App.xaml.cs` before building the shortcut, drawing, and
   sound paths. On first v3 launch, use eight empty slots; remove the
   production use of `SoundAssignments.Slots` without importing developer/test
   paths. Route recoverable load errors to the user while retaining the file.
   [Step 5](../step-5-drawing-style/acceptance.md#validation) already loads
   settings at startup, shows load errors with `SettingsFailureDialog`, and
   passes the drawing style; reuse that load for shortcuts and sounds.
2. Give sound creation and failure reporting a consistent view of the active
   snapshot, including managed file references. Preserve one global channel,
   stop-then-start replacement, same-sound restart, and the ten-second cap.
3. Implement one Save operation: validate the complete draft, persist it
   durably, stop active sound, close the drawing gate, exit an active stroke,
   clear all monitors, and publish the new immutable snapshot. Keep old and
   new binding presses from crossing the publication boundary. Require a
   fresh press after Save, including when only an unrelated setting changed.
4. Retire unreferenced media only after the persisted settings and active
   snapshot agree. Test failed persistence, active stroke and sound during
   Save, all-monitor clear, restart, missing media, and fresh-press behavior.
