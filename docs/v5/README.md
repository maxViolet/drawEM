# drawEM v5: fullscreen video

**Status:** planning. **Date:** 2026-10-21.

V5 adds imported MP4 video as an action in drawEM's existing action slots and
plays it fullscreen on the active monitor. The package follows the confirmed
behavior in the [roadmap](ROADMAP-v5.md) and delivers it in the order listed
in [steps](STEPS.md).

| Outcome | Task IDs | Status |
|---|---|---|
| Fullscreen MP4 actions, including import, playback, controls, and published-app verification | `S5-01`–`S5-05` | Planned |

## Package structure

- `ROADMAP-v5.md` is the product and technical contract.
- `STEPS.md` gives the implementation order and completion condition for each
  step.
- Every step folder has `task.md`, `plan.md`, and `acceptance.md`:
  - `task.md` states the outcome, status, dependencies, and next step.
  - `plan.md` lists the numbered implementation steps.
  - `acceptance.md` records gate criteria and evidence. Until evidence is
    collected, criteria remain unchecked and manual checks remain unverified.
- Each file links to the other two. Follow the [v4 package structure](../v4/README.md#package-structure).

## Steps

- [S5-01: prove MP4 playback and choose supported profiles](step-01-video-decoder-and-formats/task.md)
- [S5-02: add video actions and managed imports](step-02-video-action-and-library/task.md)
- [S5-03: play video fullscreen on the active monitor](step-03-fullscreen-video-playback/task.md)
- [S5-04: connect controls and lifecycle](step-04-video-controls-and-lifecycle/task.md)
- [S5-05: verify the published application](step-05-video-publish/task.md)

## Terms

The product terms used throughout this package are defined in the
[roadmap](ROADMAP-v5.md#terms).
