# v4 / S4-07: implementation plan

**Task:** [stop effects when switching monitors](task.md). **Acceptance:** [criteria](acceptance.md).

1. Observe pointer monitor transitions even when draw mode is inactive, but only when an effect is running or queued. Check cached monitor bounds in the hook and dispatch only on a transition; resolve the invocation's captured point against a monitor.
2. Record transition order or a monotonic monitor epoch so A -> B -> A cannot be reduced to no change.
3. Stop the effect on entering another monitor; invalidate earlier queued starts. Treat outside desktop edges separately from an actual monitor transition.
4. Add a display-change listener and stop/rebuild the surface when topology changes; identify monitors with topology generation, not bounds alone. During active animation, check current cursor position each frame as a backstop for unreported movement.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
