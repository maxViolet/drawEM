# Stage 1 / Step 2: acceptance criteria

**Task:** [global sound channel](task.md). **Plan:** [implementation](plan.md).

- [x] Starting sound B stops and disposes sound A before B starts; no two sounds own the global channel at once.
- [x] Starting A again while A plays creates a new attempt from the beginning and resets its ten-second deadline.
- [x] Natural completion stops and releases the current player. The current attempt stops no later than ten seconds after its start if the file lasts longer.
- [x] Completion, failure, or timeout from an older attempt cannot stop or dispose the current attempt.
- [x] Missing file, invalid media, decode error, and output-device failure are recoverable and recorded locally; the tray app remains running.
- [x] Exit stops playback, cancels the deadline, and releases the player.
- [x] Controller tests pass with a fake player and controlled time; they require no sound device. Record the test command and result.

Audible WAV/MP3 output and exact cutoff on Windows require [Step 4 manual checks](../step-4-publish/acceptance.md).
