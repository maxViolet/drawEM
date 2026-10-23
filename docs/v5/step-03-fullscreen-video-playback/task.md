# v5 / S5-03: play video fullscreen on the active monitor

**Status:** planned. **Source:** [v5 roadmap](../ROADMAP-v5.md#delivery-order).
**Depends on:** [S5-02](../step-02-video-action-and-library/task.md).
**Next:** [S5-04](../step-04-video-controls-and-lifecycle/task.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Task

Add the video playback channel and fullscreen rendering surface. Play the
selected managed file on the monitor containing the cursor, preserve the
complete source frame with black bars, and keep input and focus with the
underlying application.

## Required behavior

- The fullscreen video surface spans the target monitor's physical bounds,
  including negative desktop coordinates and mixed DPI.
- Preserve aspect ratio and show the full frame. Letterbox with black; never
  crop the source to fill the display.
- Keep video below drawEM drawings and effects, above ordinary application
  windows (including the focused application), and above the desktop.
- Do not activate the surface or intercept mouse, keyboard, or wheel input.
- Play the video's embedded audio through the system volume. It can overlap
  the global sound channel; stopping video stops only its own audio.
- Release the player and surface at natural completion.

## Out of scope

- User-facing trigger and stop routes and session lifecycle: S5-04.
- Supporting untested codec profiles beyond the S5-01 contract.
- Pause/resume controls, cropping modes, transparency, or multiple simultaneous
  videos.
