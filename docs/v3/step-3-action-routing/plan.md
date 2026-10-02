# v3 / Step 3: implementation plan

**Task:** [configurable action shortcuts](task.md). **Acceptance:** [criteria](acceptance.md).

1. Extend `Infrastructure/Drawing/GlobalShortcutAdapter.cs` to resolve draw,
   clear, and filled action slots from one immutable binding snapshot. Keep hook
   callbacks free of storage access and media work. Translate platform key
   events to the validated shortcut model at the infrastructure boundary.
2. Preserve first-press, repeat, release, and suppression bookkeeping for
   assigned and empty slots. A held draw command remains active until release;
   clear still targets the cursor's monitor. Sound continues to queue directly
   to `SoundChannelHost` and does not interrupt drawing.
3. Detect Right Alt by its actual key state, not `LLKHF_INJECTED`. Ignore the
   Windows-reported Left Ctrl companion of AltGr for action resolution. Pass
   candidate keys through outside draw mode while Right Alt is held.
4. Test remapping of every command type, held/repeated keys, key-up matching,
   input suppression, Left Alt, Right Alt with reported Left Ctrl, physical
   `Ctrl+RightAlt`, and cross-monitor drawing. Keep real global-hook checks in
   [Step 8](../step-8-publish/task.md).
