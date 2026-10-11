# v4 / S4-12: acceptance criteria

**Task:** [publish and verify on Windows](task.md). **Plan:** [implementation](plan.md).

- [ ] Automated suite and Windows x64 publish results are recorded for the exact executable/build.
- [ ] Manual results identify Windows version, CPU/GPU, monitor layouts/scaling, input layouts, and the tested external applications.
- [ ] Visible effects, focus/input pass-through, mixed DPI, Cursor-effect clipping at monitor edges, monitor transitions, drawing/sound regression, Settings, and cleanup checks are recorded separately from automated results.
- [ ] 100-invocation shortcut-to-present latency, callback, hook delay, and resource results meet the roadmap targets for each placement; visible pacing is observed and both hooks still work after the cycles.
- [ ] Full-display and window-only sharing results are recorded separately; unrun desktop checks are marked unverified.
- [ ] A missed target blocks the release. Only the project owner can accept it, and only by revising the roadmap targets or scope with a recorded rationale and confirmation before release; the revised targets are then met and recorded.
- [ ] The v3 Step 8 baseline is complete before claiming a v4 release: none of its scenarios remains unverified.
- [ ] v4 is not released while any roadmap published-app check for either placement is unverified, including mixed DPI, Cursor effects at each monitor edge, monitor transitions, lifecycle events, full-display sharing, and drawing plus sound regression. Window-only sharing may stay unverified; record it separately.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
