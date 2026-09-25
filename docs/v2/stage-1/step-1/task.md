# Stage 1 / Step 1: command and code configuration

**Status:** implemented; see [review](review.md). **Source:** [Stage 1 roadmap](../../ROADMAP-v2.md#1-define-the-command-and-code-configuration).

## Task

Define a play-sound command that identifies one configured sound without depending on WPF, Win32 hooks, or the future settings UI. Give the eight `Ctrl+Alt+1` through `Ctrl+Alt+8` slots one code-owned mapping to external local WAV or MP3 paths. An unassigned slot has no command. Keep `Ctrl+Alt+Z` and `Ctrl+Alt+X` fixed.

Choose a local failure-log location in the current Windows user's profile and specify what a failed request records. No import, managed library, or settings window is part of this step.

**Depends on:** no other Stage 1 step. **Enables:** [Step 2](../step-2/task.md) and [Step 3](../step-3/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
