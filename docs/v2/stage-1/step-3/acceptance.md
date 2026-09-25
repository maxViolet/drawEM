# Stage 1 / Step 3: acceptance criteria

**Task:** [sound shortcuts](task.md). **Plan:** [implementation](plan.md).

- [ ] An assigned `Ctrl+Alt+1` through `Ctrl+Alt+8` slot queues exactly one sound command on the first key-down of a press.
- [ ] Repeated key-down while held queues no additional command; releasing and pressing again queues a new command.
- [ ] An unassigned slot queues no sound command.
- [ ] The hook callback does not open a file, decode media, or start the player.
- [ ] A sound command during an active stroke leaves that stroke and the drawing gate active; keyboard and pointer suppression rules for the underlying app remain in effect.
- [ ] Existing `Ctrl+Alt+Z` hold/release and monitor-scoped `Ctrl+Alt+X` tests still pass, together with the new shortcut tests. Record the command and result.

Actual global-hook behavior and input blocking are checked in [Step 4](../step-4/acceptance.md).
