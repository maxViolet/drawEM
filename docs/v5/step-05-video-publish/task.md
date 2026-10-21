# v5 / S5-05: verify the published application

**Status:** planned. **Source:** [v5 roadmap](../ROADMAP-v5.md#completion).
**Depends on:** [S5-04](../step-04-video-controls-and-lifecycle/task.md) and the
existing release-publishing package. **Next:** none; this is the final v5 step.

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Task

Publish the completed video feature through the existing Windows release
process and record automated, target-machine, and manual evidence against the
exact artifact tested. A build or unit test pass does not close a desktop or
OBS capture check.

## Required outcome

- CI and release artifacts include the selected decoder/runtime requirements
  and all supported media dependencies.
- The exact published artifact plays supported MP4 fixtures and fails safely
  on unsupported or corrupt files.
- Windows acceptance covers Settings import/Sample/Save/Cancel, input
  pass-through, monitor selection, aspect fit, drawing/effect layering, audio,
  stop routes, lock/sleep/exit, and OBS Display Capture.
- Resource cleanup is measured through live player/surface diagnostics after
  repeated start, restart, stop, and natural completion cycles; the final count
  must return to zero.
- Unavailable target-machine checks remain Unverified with the artifact
  identity and reason recorded.

## Out of scope

- Installer or updater features not required by the existing release pipeline.
- Claiming target-machine or OBS results from CI evidence alone.
