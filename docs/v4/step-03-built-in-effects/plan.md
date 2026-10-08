# v4 / S4-03: implementation plan

**Task:** [render confetti and focus ring](task.md). **Acceptance:** [criteria](acceptance.md).

1. Implement the confetti preset as a bounded burst fading within three seconds and the focus ring as a two-second expansion around the invocation point.
2. Use monotonic elapsed time, bounded particle data, and no per-frame layout work. Subscribe to the frame callback only while rendering.
3. Convert the invocation point to the target window's local coordinates and clip both presets to that monitor.
4. Use a development-only trigger until S4-06 supplies shortcuts. Verify completion/replacement and observe coexistence with drawing and global sound on Windows; remove the trigger in S4-06.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
