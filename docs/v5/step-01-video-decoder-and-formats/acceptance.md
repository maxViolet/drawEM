# v5 / S5-01: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] [gate] The target Windows playback path is named, with the tested OS
  edition/build and app build identity recorded.
- [ ] [gate] Every supported MP4 codec profile has a checked-in fixture that
  opens, renders video, plays embedded audio when present, reaches natural
  end-of-file, and stops/disposes. A silent MP4 fixture is supported and
  covered.
- [ ] [gate] The committed fixture generator, exact FFmpeg version, and
  generation commands reproduce the recorded fixture set; tests run from the
  checked-in fixture files without FFmpeg installed.
- [ ] [gate] At least one unsupported or corrupt MP4 fixture is rejected with
  a diagnostic identifying the failure class and, when available, codec or
  container; this gate checks decoder validation only.
- [ ] [gate] A forced playback-open/decode failure after successful validation
  releases partial player resources and does not crash the tray application.
- [ ] [gate] Validation of malformed and unsupported fixtures completes
  within 30 seconds per file on the recorded target Windows machine. S5-01
  creates no managed import or Settings draft.
- [ ] [record] Record fixture names, hashes, container, video/audio codecs,
  generator version and commands, playback path, Windows version/build, app
  build identity, and observed limitations.

## Evidence

No implementation or playback evidence has been recorded. All criteria
remain unverified.
