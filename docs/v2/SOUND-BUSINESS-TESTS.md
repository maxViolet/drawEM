# Sound command: business test scenarios

**Status:** business scenarios and test boundaries confirmed before
implementation.
Automated tests will be added one scenario at a time, with a failing test before
the corresponding implementation. Manual checks remain necessary for audible
output and global input behavior.

The sound channel is global and independent of drawing and monitor state. The
sound paths and assignments to `Ctrl+Alt+1` through `Ctrl+Alt+8` are set in code
for this stage. Each assigned shortcut identifies one WAV or MP3 file.

## Playback

1. **Start an assigned sound.** Given an idle channel and a configured sound,
   when its command is triggered, then that sound starts once.
2. **Replace a different sound.** Given sound A is playing, when sound B is
   triggered, then A stops before B starts, B starts immediately, and the two
   sounds never play together in the global sound channel.
3. **Restart the same sound.** Given sound A is playing, when A is triggered
   again by a fresh key press, then the current A stops before a new A starts
   immediately from the beginning.
4. **Finish naturally.** Given a sound shorter than ten seconds is playing,
   when the file ends, then playback stops without starting again.
5. **Limit each playback.** Given a sound longer than ten seconds is playing,
   when ten seconds have elapsed since playback started, then it stops. A new
   command starts a new ten-second allowance.
6. **Ignore an old finish.** Given A has been replaced by B, when an ending,
   failure, or timeout from A arrives later, then B keeps playing.

7. **Ignore an old finish after restarting the same sound.** Given A was
   restarted, when the first A's ending, failure, or timeout arrives later,
   then the new A keeps playing.

## Shortcuts and independent channels

8. **Start once per press.** Given an assigned sound shortcut is held, when
   Windows repeats its key-down event, then the sound does not restart. After
   release, a new press starts it again.
9. **Leave an unassigned slot alone.** Given a sound is playing, when an
   unassigned sound shortcut slot is pressed, then the current sound continues.
10. **Suppress only assigned sound-slot keys.** Given draw mode is inactive,
    when an assigned `Ctrl+Alt+1` through `Ctrl+Alt+8` slot is pressed, then its
    sound starts and the keypress does not reach the focused app. When an
    unassigned slot is pressed, the keypress reaches the focused app and
    playback does not change. During draw mode, the existing input block still
    applies.
11. **Play while drawing.** Given a drawing stroke is active, when an assigned
    sound shortcut is pressed, then the sound starts and the stroke remains
    active. Pointer clicks, scrolling, and other blocked input still do not
    reach the application beneath the overlay.
12. **Keep sound across monitors.** Given a sound is playing, when the cursor
    moves to another monitor, then that sound continues.
13. **Preserve drawing shortcuts.** `Ctrl+Alt+Z` still holds and releases draw
    mode; `Ctrl+Alt+X` still clears only the monitor under the cursor. Neither
    command starts or stops the sound channel.

## Failure and exit

14. **Handle an invalid sound.** Given sound A is playing, when an assigned
    command requests a missing or unplayable file, then A stops first, the new
    playback fails, the failure is logged in the current user's profile, and
    drawEM remains running and able to handle later commands.
15. **Stop on exit.** Given a sound is playing, when the user exits from the
    tray, then playback stops and its resources are released.

## Confirmed test boundaries

- **Sound command:** exercise the public play command with controlled playback
  and time boundaries. Observe start, stop, completion, failure, and the
  ten-second limit without audio hardware.
- **Global shortcut routing:** feed key events through the keyboard adapter and
  observe command dispatch and existing drawing behavior. Keep the real Win32
  hook outside unit tests.
- **Published Windows app:** manually verify audible WAV and MP3 playback,
  global shortcuts with another app focused, drawing input suppression,
  multi-monitor behavior, and tray exit.

Do not create all test cases in one batch: use one failing business scenario
and the smallest implementation that passes it for each TDD cycle.
