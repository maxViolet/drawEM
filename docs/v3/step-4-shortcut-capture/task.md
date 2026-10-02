# v3 / Step 4: shortcut capture and layout switching

**Status:** planned. **Source:** [v3 roadmap](../ROADMAP-v3.md#delivery-order).

## Task

Capture one complete shortcut through the installed global hook while action
dispatch is paused. Preserve key suppression across focus changes, invalid
attempts, and release. For a suppressed candidate with `Alt+Shift` or
`Ctrl+Shift`, emit one tagged neutral key down/up before modifier release so
Windows does not switch input layout. This applies in action and capture modes;
bare modifier pairs still switch layouts.

**Depends on:** [Step 3](../step-3-action-routing/task.md). **Enables:** [Step 7](../step-7-settings-window/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
