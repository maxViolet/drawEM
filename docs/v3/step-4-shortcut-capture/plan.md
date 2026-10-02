# v3 / Step 4: implementation plan

**Task:** [capture and layout switching](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a hook-owned capture state and a composition-root event path to the
   settings draft. On focus, wait for already-held keys to release; retain each
   key's existing key-up suppression decision. Pause draw, clear, and sound
   dispatch until the captured chord is fully released.
2. Let modifier events, Tab, Alt+Tab, Alt+F4, Win chords, and Right Alt
   candidates pass through. Suppress only the candidate letter, digit, or
   F1–F12 down/up evaluated for capture. Consume Escape to cancel. Blur leaves
   capture mode. Show invalid reasons for too few modifiers or Right Alt, leave
   the draft unchanged, and rearm after all invalid-attempt keys are released.
3. Add a narrow Win32 adapter to emit tagged `VK 0xE8` down/up once after a
   candidate is suppressed with `Alt+Shift` or `Ctrl+Shift`, before the first
   modifier key-up. Ignore only those tagged events in both resolution paths.
   Do not inject for bare modifier pairs or `Ctrl+Alt` candidates.
4. Test already-bound chord capture without dispatch, held-key entry, valid
   completion, invalid retry, Escape/blur, pass-through, tag filtering, and
   injection order in action and capture modes. Verify real Windows layout
   behavior in [Step 8](../step-8-publish/task.md).
