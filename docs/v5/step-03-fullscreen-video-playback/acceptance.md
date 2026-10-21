# v5 / S5-03: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] [gate] Automated tests prove start, stop, replacement, restart, natural
  completion, Loop, and failure release the owned playback instance exactly
  once.
- [ ] [gate] Automated layout tests prove the full monitor bounds are used and
  the complete source aspect ratio is preserved with black letterboxing.
- [ ] [gate] Repeated start/stop and decoder failure leave no live playback
  instance, surface, event handler, or scheduled completion callback.
- [ ] [gate] Stopping video does not stop global sound; ending global sound
  does not stop video.
- [ ] [gate] Windows checks confirm the video surface does not take focus or
  intercept keyboard, mouse, or wheel input.
- [ ] [gate] Windows checks confirm desktop -> ordinary application windows,
  including the focused application -> video -> drawings -> effects ordering
  while starting and stopping each surface.
- [ ] [gate] Windows manual check confirms audible embedded audio for a fixture
  with an audio track; a silent fixture follows the support decision recorded
  in S5-01.
- [ ] [gate] Windows checks on 100% and non-100% DPI monitors, including a
  monitor at negative coordinates, show video inside the selected monitor
  without clipping across its bounds.
- [ ] [record] Record the test artifact, Windows build, monitor resolution and
  scaling, source fixture details, and manual observations.

## Evidence

No implementation or Windows surface evidence has been recorded. All criteria
remain unverified.
