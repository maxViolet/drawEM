# v3 / Step 8: acceptance criteria

**Task:** [publish and verify](task.md). **Plan:** [verification](plan.md).

- [x] Automated suite and Windows x64 publish commands, results, and build
  identity are recorded.
- [ ] Separate manual results cover every group in the verification plan,
  including tray/settings, storage/media recovery, real keys and layouts,
  DPI/monitors, audible sound, and v1/v2 regressions.
- [ ] No tested `Alt+Shift` or `Ctrl+Shift` binding causes an unintended
  input-layout switch; bare layout-switch pairs still work.
- [ ] Every failed or unrun desktop check is marked failed or unverified, with
  tested environment and release blockers stated. Automated tests or publish
  success alone do not close manual acceptance.
