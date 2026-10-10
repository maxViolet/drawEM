# v4 / S4-09: implementation plan

**Task:** [preview draft effects](task.md). **Acceptance:** [criteria](acceptance.md).

1. Route Sample through the same controller and target-monitor rules as a shortcut invocation.
2. Tie each Sample to an editor and instance ID; Cancel or close may stop only that editor's still-current Sample.
3. Report sample rendering failures in Settings without leaving visual residue or changing active bindings.
4. Keep Settings focused while the sample appears on the monitor containing the cursor: a Monitor effect covers it, and a Cursor effect appears around the cursor, usually over the Sample button.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
