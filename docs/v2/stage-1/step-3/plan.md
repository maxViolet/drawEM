# Stage 1 / Step 3: implementation plan

**Task:** [sound shortcuts](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add sound-slot virtual-key codes to `src/DrawEM.App/Infrastructure/VirtualKeys.cs`. Extend `src/DrawEM.App/Infrastructure/Drawing/GlobalShortcutAdapter.cs` through the existing `IKeyboardHookSource` events.
2. Track each slot key's pressed state. Resolve an assigned slot when the Ctrl+Alt chord first becomes active; ignore auto-repeat and require release before another start. An empty slot does nothing.
3. Queue the resolved sound command through the existing Dispatcher path. Keep file access, decoding, and player creation outside the hook callback; do not query monitor bounds for sound.
4. Keep the drawing gate unchanged while a sound slot is pressed. During an active stroke, route sound while the existing keyboard and pointer suppression still applies to the underlying app.
5. Use red-to-green behavior tests at the shortcut adapter boundary for first press, repeat, release and repress, empty slot, deferred dispatch, and sound during a stroke. Rerun existing draw and clear shortcut tests after each relevant change.

Global delivery, audible playback, and underlying-app input are checked manually in [Step 4](../step-4/task.md).
