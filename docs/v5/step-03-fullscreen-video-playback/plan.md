# v5 / S5-03: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Playback contract tests first.** Add failing tests for start, stop,
   completion, looping, restart, error, and disposal using a deterministic
   playback seam. Include a start failure and an asynchronous decoder failure.
2. **Playback channel.** Implement a channel that owns one active video
   instance, replaces/restarts it from the beginning, reports failures, and
   releases all callbacks and decoder resources on stop or completion.
3. **Fullscreen surface tests.** Add tests for monitor bounds, letterboxing,
   negative coordinates, DPI changes, and surface cleanup before implementing
   the WPF surface.
4. **Rendering and input behavior.** Implement a borderless non-activating,
   click-through surface with a black background and aspect-fit video content.
   Confirm it does not intercept mouse, keyboard, or wheel events.
5. **Cross-window layering.** Integrate the video surface with the drawing
   overlay and effect surfaces. Verify the order desktop -> video -> drawings
   -> effects, including when either surface starts or stops while video is
   active.
6. **Audio and completion.** Play embedded audio at system volume, handle
   natural completion and Loop, and ensure a video stop leaves global sound
   untouched.
7. **Windows surface check.** On the target Windows display, record visual
   results for 16:9 and non-16:9 content, mixed DPI, negative coordinates,
   input pass-through, focus, and layer order. Record OBS Display Capture
   evidence in S5-05.
