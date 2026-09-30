# v3 / Step 7: acceptance criteria

**Task:** [Settings window](task.md). **Plan:** [implementation](plan.md).

- [ ] Tray Settings opens Drawing and Actions tabs and can edit/reload every
  v3 setting.
- [ ] Cancel leaves active settings unchanged; Restore defaults affects only
  the draft until Save; failed Save preserves the active configuration.
- [ ] Eight numbered slots show file names. A first selection proposes the
  slot's default shortcut, and a valid alternative can be saved.
- [ ] Conflicts and invalid or missing bindings block Save with visible
  feedback. Capture uses the global hook without dispatching active actions.
- [ ] `Sample` plays the draft file through the global sound channel without
  changing active shortcuts; copy and playback failures are visible.
- [ ] Automated UI/application tests record their commands and results.
  Audible, tray, and real-key checks remain for [Step 8](../step-8/task.md).
