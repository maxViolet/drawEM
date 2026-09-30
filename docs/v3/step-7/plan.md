# v3 / Step 7: implementation plan

**Task:** [Settings window](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a Settings command to the tray host and a WPF window with Drawing
   and Actions tabs. Bind controls to a draft copy of the active snapshot.
   Drawing exposes HEX color, color picker, physical width, and required draw
   and clear shortcut capture fields.
2. Show eight numbered action slots with file names, file selection, shortcut
   capture, and `Sample`. A newly filled slot proposes its matching
   `Ctrl+Alt+1` through `Ctrl+Alt+8` chord; the user may replace it. An empty
   slot has no active shortcut. No custom name or volume control is offered.
3. Route focused capture fields through Step 4's hook-owned capture state.
   Show invalid chords and duplicate binding conflicts beside the draft; block
   Save until all required shortcuts and slots validate. Do not silently
   reassign another command.
4. `Sample` plays the draft-selected file through the global channel with the
   same ten-second cap. Save calls Step 6's operation; Cancel discards draft
   edits and imports; Restore defaults changes the draft only. Show storage,
   copy, and preview errors without losing active settings.
5. Test draft isolation, default restoration, conflicts, file selection,
   shared copies, preview routing, Save, Cancel, and reopen/reload behavior.
