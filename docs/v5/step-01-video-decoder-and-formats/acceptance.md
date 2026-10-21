# v5 / S5-01: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] [gate] The target Windows playback path is named, with the tested OS and
  app build recorded.
- [ ] [gate] Every supported MP4 codec profile has a fixture that opens,
  renders video, plays embedded audio when present, reaches end-of-file, and
  stops/disposes. Silent-file support or rejection is explicitly decided and
  covered by a fixture.
- [ ] [gate] At least one unsupported or corrupt MP4 fixture is rejected with
  a diagnostic identifying the failure class and, when available, codec or
  container; this gate checks decoder validation only.
- [ ] [gate] A forced failure after successful import releases partial player
  resources and does not crash the tray application.
- [ ] [gate] Validation of malformed and unsupported fixtures completes within
  30 seconds per file on the recorded target Windows machine; no managed import
  or settings draft is created by this decoder-only step.
- [ ] [record] Record fixture names, hashes, container, video/audio codecs,
  playback path, Windows version, build identity, and observed limitations.

## Evidence

No implementation or playback evidence has been recorded. All criteria remain
unverified.
