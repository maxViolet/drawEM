# v4 / S4-01: implementation plan

**Task:** [prove the effect window](task.md). **Acceptance:** [criteria](acceptance.md).

1. Build the experimental surface in `src/DrawEM.App/Presentation/Effects/EffectSurfaceProbe.cs` and wire a temporary trigger in `App.xaml.cs` only when `DRAWEM_EFFECT_PROBE=1`. Publish this probe as a local Windows x64 build for the desktop checks. Keep the drawing overlay active beneath the shape on the monitor containing the cursor at trigger; remove the trigger in S4-06.
2. Make the window nonactivating, absent from Alt+Tab and Win+Tab, and input-transparent to other processes; keep the existing drawing overlay usable beneath it.
3. Map physical monitor bounds to this window's local coordinates. Add `EffectSurfaceCoordinateTests` for negative coordinates, 100%/150%/200% scaling, and clipping; check two monitors with different scaling and exercise disconnect/reconnect while the effect is visible on Windows.
4. Run 100 warm effect invocations on a named Windows setup. Record preliminary first-appearance and render-callback timing, cold starts separately, and frame pacing; check the recording for sustained stutter and missed stroke updates. Measure hook event-to-callback delay from at least 1,000 keyboard and mouse events during animation with drawing active; check both hooks after 100 cycles. Confirm the frame callback is detached and the effect window closed after the cycles; compare process handle, GDI object, and USER object counts after warm-up and at the end. This temporary trigger cannot prove final shortcut latency.
5. Fill in the Renderer decision section of [acceptance](acceptance.md) before S4-02. Compare all three roadmap options. If render-callback or hook-delay thresholds fail, prove a separate effect UI thread or another renderer against the same criteria before proceeding.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
