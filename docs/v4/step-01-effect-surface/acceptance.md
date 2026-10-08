# v4 / S4-01: acceptance criteria

**Task:** [prove the effect window](task.md). **Plan:** [implementation](plan.md).

`[gate]` blocks S4-02 if it fails. `[record]` requires evidence but its observed result is not an S4-01 pass/fail threshold.

- [ ] [gate] A moving shape appears on the monitor containing the cursor at trigger, above existing strokes, and disappears when its animation completes.
- [ ] [gate] With draw mode active, strokes still reach the drawing overlay under the effect, with no missed stroke updates in a side-by-side recording with and without the effect. Use the hook-delay threshold below as the measured drawing-responsiveness gate.
- [ ] [gate] While the effect is visible, clicks, typing, and scrolling still reach the other application. The foreground window is unchanged after the effect appears; neither Alt+Tab nor Win+Tab lists the effect window.
- [ ] [gate] Desktop checks cover one and two monitors, including two monitors with different scaling, 100%, 150%, and 200% scaling, and negative desktop coordinates. The shape is clipped at the target monitor edge and never draws onto a neighboring monitor.
- [ ] [record] During monitor disconnect/reconnect with the effect visible, record whether the app crashes and what happens to the effect/window. S4-07 owns the final display-change handling.
- [ ] [record] Record the reference environment: build hash, Windows version, CPU/GPU, monitor resolutions and scaling, timing method and its measurement error.
- [ ] [gate] Across 100 warm effect invocations, render callback time is no more than 4 ms at p95, with no sustained stutter visible in the recording. Record the callback samples, frame-pacing recording, and cold-start results separately.
- [ ] [gate] During animation with active drawing, hook event-to-callback delay is no more than 25 ms at p95 and 100 ms maximum. Record at least 1,000 keyboard and mouse events in total across the 100 invocations, with both input types represented; confirm both hooks still respond after 100 effect cycles. Exceeding either delay threshold blocks the current renderer.
- [ ] [record] Record first appearance across the 100 warm invocations as a renderer estimate against the 100 ms p95 release target. It is not an S4-01 pass/fail gate because the temporary trigger omits the shortcut path; S4-12 owns the full shortcut-to-present measurement.
- [ ] [gate] Fill in the **Renderer decision** below with the chosen option, measurements and desktop evidence, and why each other roadmap option was rejected or skipped. If the WPF UI thread fails the render-callback or hook-delay threshold, prove a separate effect UI thread or another renderer against the same criteria before S4-02. Limit each fallback proof to two working days, then escalate to the sign-off owner. If no option meets the criteria, stop and revise the roadmap thresholds or scope before S4-02.
- [ ] [record] A locally published Windows x64 build contains the temporary S4-01 trigger in `src/DrawEM.App/Presentation/Effects/EffectSurfaceProbe.cs`, wired only when `DRAWEM_EFFECT_PROBE=1` in `App.xaml.cs`. Record its exact executable path, version, hash, and trigger setting; S4-06 removes this trigger.
- [ ] [gate] After 100 effect cycles, the probe has no `CompositionTarget.Rendering` subscription or effect window left. Record process handle, GDI object, and USER object counts after warm-up and after the cycles; each final count is within 5% of its warm-up count.

## Validation

- Automated checks: unverified. Add `EffectSurfaceCoordinateTests` under `tests/DrawEM.Tests/Presentation/Effects/` for physical-to-DIP mapping and clipping with negative coordinates at 100%, 150%, and 200% scaling; record the exact test command and result.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.

## Renderer decision

Complete this section after the Windows experiment; it is not a preselected decision.

**Sign-off owner:** drawEM project owner (`maxViolet`). Record approval here before S4-02.

- Chosen option and reason: pending.
- Evidence: reference PC, build identity, 100-invocation measurements, hook-delay samples, visible frame pacing, input/focus and mixed-DPI observations: pending.
- Dedicated WPF effect surface — chosen, rejected, or skipped, with reason: pending.
- Effects in the virtual-desktop drawing window — chosen, rejected, or skipped, with reason: pending.
- DirectComposition/Direct2D adapter — chosen, rejected, or skipped, with reason: pending.
- Decision on proceeding to S4-02, including any separate-thread or renderer proof, two-working-day fallback timeboxes, and the project owner's sign-off: pending. If no option meets the criteria, stop and revise the roadmap thresholds or scope before S4-02.
