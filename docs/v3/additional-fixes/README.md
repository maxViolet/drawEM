# v3 additional fixes

This package tracks reported defects that are outside the eight numbered
settings steps in the [v3 roadmap](../ROADMAP-v3.md#delivery-order).

Each fix has one folder named after its work id. A fix starts with a single
`task.md` that holds current behavior, required behavior, and acceptance. The
implementation PR adds `plan.md` when the change needs one, and records
validation results in `task.md`.

- [FIX-006: keep drawing active at a single screen edge](FIX-006-single-screen-edge/task.md)
