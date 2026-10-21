# v5 / S5-05: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] [gate] The required solution tests and Release publish/package checks
  pass for the recorded commit.
- [ ] [gate] The artifact tested on Windows has the same commit and SHA-256 as
  the artifact identified in the acceptance record.
- [ ] [gate] Every S5-01 supported fixture passes and every negative fixture
  fails safely on the published artifact.
- [ ] [gate] Windows Settings checks pass for import, error display, Sample,
  Loop, Save, Cancel, and last-reference cleanup.
- [ ] [gate] Windows checks pass for monitor targeting, transition stop,
  display topology changes, aspect fit, DPI, negative coordinates, ordering
  above the focused application, layer order, click-through, and
  keyboard/mouse/wheel pass-through.
- [ ] [gate] OBS Display Capture of the target monitor visibly includes the
  video and drawEM layers above it.
- [ ] [gate] Embedded audio and global sound can play together; stopping video
  leaves global sound playing.
- [ ] [gate] Stop video, clear, exit, session lock, sleep, and wake scenarios
  match the roadmap playback table.
- [ ] [gate] Playback diagnostics report zero live players and surfaces after
  all playback ends. After each 100-cycle batch and a second repeat, the final
  counts remain zero; permitted delta is zero objects.
- [ ] [record] Record local/CI/release results separately from manual Windows
  and OBS results. Every unavailable manual check remains Unverified with a
  reason and tested artifact identity.

## Evidence

No build, published-app, Windows, or OBS evidence has been recorded. All
criteria remain unverified.
