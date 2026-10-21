# v5 / S5-02: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] [gate] Each of the eight slots validates zero or one action of type
  sound, effect, or video; duplicate slot numbers and invalid shortcuts fail.
- [ ] [gate] Loop defaults off, round-trips through settings, and does not
  alter older sound/effect assignments.
- [ ] [gate] Import copies an accepted MP4 into the managed library and stores
  no absolute source path in settings.
- [ ] [gate] Unsupported, corrupt, missing, or copy-failing imports leave the
  previous draft action and shortcut unchanged. The message identifies the
  failure class; codec/container details are included when available, and copy
  failures identify that managed import failed.
- [ ] [gate] Cancel/close stops Sample before removing a newly imported
  draft-only copy and retains every file referenced by saved settings.
- [ ] [gate] Save removes a copy after its last saved slot reference is
  removed; shared references retain the copy until all are removed and saved.
- [ ] [gate] Settings Sample starts the draft video without persisting the
  draft assignment. A saved slot shortcut stops Sample and starts the saved
  assignment. Save stops affected playback before replacing settings or
  removing an unreferenced copy.
- [ ] [gate] Startup removes orphan managed copies; cleanup failures are logged
  to `video.log` and do not block startup.
- [ ] [gate] Before the first new-schema Save, immutable numbered settings and
  matching library backups are preserved. Simulated backup failure leaves
  settings and library contents unchanged; rollback instructions restore both.
- [ ] [gate] Existing v3/v4 settings fixtures load with their previous sound
  and effect actions unchanged; new video settings round-trip.
- [ ] [record] Record schema version, migration fixtures, managed library
  location, and import/cleanup test results.

## Evidence

No implementation or settings evidence has been recorded. All criteria remain
unverified.
