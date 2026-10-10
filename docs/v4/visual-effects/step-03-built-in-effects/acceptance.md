# v4 / S4-03: acceptance criteria

**Task:** [play built-in Lottie effects](task.md). **Plan:** [implementation](plan.md).

## S4-03.1: speed gate

- [ ] The probe's measured render path plays a Lottie file through the Skottie player, not the S4-01 code-drawn shape, and its report names the placement and window bounds.
- [ ] Skia draws inside the measured `CompositionTarget.Rendering` handler into the effect's own `WriteableBitmap`; `SKElement` is not used.
- [ ] Render callback p95, frame interval p95, dropped frames, hook delay p95 and maximum, and resource counts are recorded for a Monitor effect and a Cursor effect against the roadmap targets.
- [ ] `LayerDependencyTests` passes with `SkiaSharp` allowed in Presentation only.
- [ ] If the Monitor effect missed a target, the half-resolution result is recorded; if that also missed, the roadmap is revised to Cursor effects only with the project owner's confirmation before S4-03 continues.

## S4-03: built-in effects

- [ ] Confetti covers the monitor and focus ring appears centered on the captured cursor point; both start, animate once, and leave no residual pixels or active frame subscription.
- [ ] The focus ring stays at its captured point while the pointer moves within the monitor, and is clipped, not shifted, at each monitor edge.
- [ ] Replacement across placements and Stop clear old visuals; animation failures clear partial visuals and leave drawing and sound usable.
- [ ] Automated tests check that every built-in file loads, lasts no more than 10 seconds and no more than its specified length (confetti 3 s, focus ring 2 s), and has a source and license record; and check Cursor surface bounds, clipping, negative coordinates, and 100%/150%/200% scaling, and Monitor cover scaling for 16:9, 16:10, 21:9, and portrait monitors.
- [ ] Windows visual checks with the development-only trigger are recorded separately, including that neither effect flashes more than three times in any one second. S4-06 removes this trigger.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
