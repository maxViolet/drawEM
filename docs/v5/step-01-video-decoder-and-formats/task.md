# v5 / S5-01: prove MP4 playback and choose supported profiles

**Status:** planned. **Source:** [v5 roadmap](../ROADMAP-v5.md#delivery-order).
**Depends on:** none. **Next:** [S5-02](../step-02-video-action-and-library/task.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Task

Prove a playback path in the target Windows environment before the action
model or settings schema depends on it. Choose a bounded set of supported MP4
codec profiles and make import validation distinguish supported fixtures from
unsupported or corrupt files.

The `.mp4` extension alone does not define the video or audio codec. This step
must record the actual fixture codecs, Windows playback components, and the
limits of the support claim. Do not describe all MP4 files as supported unless
the tested matrix proves that claim.

## Required outcome

- A recorded playback-path decision based on tests in the target Windows
  environment, including video and embedded audio.
- A named set of supported MP4 codec profiles, each backed by a positive
  fixture, plus at least one unsupported or corrupt negative fixture.
- A bounded validation result that Settings can show before changing the draft
  slot when the selected file is unsupported.
- A documented decoder failure result for a file that passes import checks but
  fails when opened for playback.

## Out of scope

- Implementing user-facing playback or changing action slots; S5-02 onward owns
  those changes.
- Web URLs, streaming, non-MP4 import, pause/resume, and video editing.
- Claiming support for a codec profile that was not tested on Windows.
