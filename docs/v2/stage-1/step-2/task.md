# Stage 1 / Step 2: global sound channel

**Status:** proposed. **Source:** [Stage 1 roadmap](../../ROADMAP-v2.md#2-implement-the-global-sound-channel).

## Task

Implement one global sound channel for WAV and MP3, independent of drawing and cursor monitor. Starting a sound replaces the active one, including when the same sound is requested again. Playback ends naturally or ten seconds after its latest start. Obsolete completion, failure, and timeout callbacks cannot stop a replacement. Missing or unplayable files are logged locally without ending drawEM. Release playback resources on completion, replacement, timeout, and exit.

**Depends on:** [Step 1](../step-1/task.md). **Enables:** [Step 3](../step-3/task.md) and [Step 4](../step-4/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
