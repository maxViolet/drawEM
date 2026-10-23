# v5 / S5-02: add video actions and managed imports

**Status:** planned. **Source:** [v5 roadmap](../ROADMAP-v5.md#delivery-order).
**Depends on:** [S5-01](../step-01-video-decoder-and-formats/task.md).
**Next:** [S5-03](../step-03-fullscreen-video-playback/task.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Task

Represent a video as one selectable action in any of the eight existing
action slots. Persist a managed video reference; provide Settings import,
assignment, Sample, Save, Cancel, and cleanup behavior.

## Required behavior

- A slot contains at most one sound, effect, or video action.
- The user can import only MP4 files that pass the support profile delivered by
  S5-01. Import copies the source into a per-user drawEM-managed video library.
- Store the library file name and display name in settings, never the original
  absolute source path.
- Sample previews the draft-selected video without saving its assignment.
- Cancel, closing Settings, and Save stop any Settings Sample before discarding
  its draft or deleting a now-unreferenced imported copy. Save stops playback
  that would use a changed or removed saved reference before replacing settings.
  A managed copy is deleted only when no saved slot or active Sample refers to
  it; active normal playback must have been stopped first.
- At startup, remove unreferenced managed video copies after settings load.
  Report cleanup failures in `%LOCALAPPDATA%/drawEM/video.log` without blocking
  startup.
- Existing sound/effect slots and older settings files continue to load.
- Before the new schema replaces settings, preserve an immutable numbered
  backup and a matching snapshot of the video library. Document rollback to the
  prior app/schema by restoring both; failed backup creation must leave current
  settings and library contents unchanged.

## Out of scope

- Playback surface and video channel: S5-03.
- Global shortcuts, Stop video menu item, monitor transitions, and app/session
  interruption: S5-04.
- Formats outside the S5-01 support profile, URLs, and streaming.
