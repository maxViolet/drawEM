# v3 / Step 1: settings snapshot and storage

**Status:** implemented; see [acceptance](acceptance.md). **Source:** [v3 roadmap](../ROADMAP-v3.md#delivery-order).

## Task

Define one validated settings snapshot for drawing color and width, required draw
and clear shortcuts, and eight numbered action slots. In v3 a filled slot holds
one sound reference and one shortcut; an empty slot has no active shortcut.
Keep the slot model open to later action types without implementing them now.
Choose the on-disk schema and version/migration policy before writing settings.
Load and save in the current user's profile, with durable replacement and clear
failure behavior. First launch uses defaults and eight empty slots; code-owned
v2 assignments are not migrated. Keep filesystem, WPF, and Win32 types outside
Domain and Application.

**Depends on:** v2 playback and keyboard path. **Enables:** [Step 2](../step-2-sound-library/task.md), [Step 3](../step-3-action-routing/task.md), and [Step 5](../step-5-drawing-style/task.md).

See [plan](plan.md) and [acceptance criteria](acceptance.md).
