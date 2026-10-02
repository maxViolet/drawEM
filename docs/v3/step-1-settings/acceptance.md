# v3 / Step 1: acceptance criteria

**Task:** [settings snapshot](task.md). **Plan:** [implementation](plan.md).

- [x] One immutable snapshot contains drawing style, two required commands,
  and exactly eight numbered slots; only sound is a filled action type in v3.
- [x] Validation rejects missing/duplicate active shortcuts, one-modifier and
  Win chords, illegal keys, invalid HEX color, and width outside 1–20.
- [x] Defaults are `#FF4500`, 4 physical pixels, `Ctrl+Alt+Z`, `Ctrl+Alt+X`,
  and eight empty slots. No v2 developer path is imported.
- [x] Schema/version policy is documented and storage tests cover first load,
  saved restart, malformed/unsupported data, failed write, and recovery of the
  original damaged file without overwriting it.
- [x] Domain and Application interfaces contain no WPF, Win32, or filesystem
  types. Record automated test commands and results when implemented.

## Validation

- `dotnet test` (2026-09-30): 255 passed, 0 failed. Covers shortcut, color,
  width, slot, and snapshot validation (`Domain/Settings`), startup fallback
  (`Application/Settings`), JSON storage in a temporary directory
  (`Infrastructure/Settings`), and the no-filesystem rule for Domain and
  Application (`Architecture/LayerDependencyTests`).
- Schema and version policy: `src/DrawEM.App/Infrastructure/Settings/README.md`.
- Not wired into the running app yet; startup loading and retiring
  `SoundAssignments.Slots` happen in [Step 6](../step-6-runtime-settings/task.md).
