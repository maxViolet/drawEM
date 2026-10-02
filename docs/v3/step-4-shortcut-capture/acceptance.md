# v3 / Step 4: acceptance criteria

**Task:** [capture and layout switching](task.md). **Plan:** [implementation](plan.md).

- [ ] An already-bound chord can be captured without drawing, clearing, or
  playing; dispatch resumes only after its keys release and a fresh press.
- [ ] Held keys at focus entry keep their earlier suppression decision; Tab,
  Alt+Tab, Alt+F4, Win, and Right Alt candidates pass through as specified.
- [ ] Invalid `Ctrl+C` and `Ctrl+RightAlt+1` attempts show reasons, preserve
  the draft, and allow a fresh attempt; Escape and blur leave capture mode.
- [ ] Tests show one tagged neutral key pair after each suppressed candidate
  with `Alt+Shift` or `Ctrl+Shift` and before modifier release, in both action
  and capture modes. Tagged events cannot dispatch or be recorded.
- [ ] Bare layout-switch pairs cause no injection. Windows behavior remains a
  release check in [Step 8](../step-8-publish/task.md).
