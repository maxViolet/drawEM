# v4 / S4-03: implementation plan

**Task:** [play built-in Lottie effects](task.md). **Acceptance:** [criteria](acceptance.md).

## S4-03.1: speed gate (branch `S4-03.1-lottie-spike`, about half a day)

1. Add `SkiaSharp.Skottie` (not `SkiaSharp.Views.WPF`). In `tests/DrawEM.Tests/Architecture/LayerDependencyTests.cs`, add `"SkiaSharp"` to Presentation's allowed dependencies only; Domain, Application, and Infrastructure stay without it.
2. Change the S4-01 probe so its `CompositionTarget.Rendering` handler locks the effect's own `WriteableBitmap`, draws the Skottie frame into it through an `SKSurface` over the back buffer, marks the changed area dirty, and unlocks. Do not use `SKElement`: it draws in `OnRender`, outside the measured handler.
3. Add a second window type: sized to the Lottie canvas in DIP, centered on the cursor, clipped to the monitor. The monitor-sized window uses cover scaling.
4. Run the probe benchmark once per placement; the report names the placement and the window bounds. Record render callback p95, frame interval p95, dropped frames, hook delay p95 and maximum, and resource counts against the roadmap targets.
5. If the Monitor surface misses a target, render it at half resolution in the same monitor-sized window and measure again. Half resolution reduces Skia drawing and bitmap upload; it does not reduce the layered-window cost, which stays at monitor size. If that also misses, stop: revise the roadmap to Cursor effects only and get the project owner's confirmation before S4-03 continues.
6. If the Cursor surface misses a target, stop: there is no fallback. Give the measurements to the project owner, who revises the roadmap targets or scope and confirms the revision before S4-03 continues.

## S4-03: built-in effects

1. The built-in catalog already exists: S4-04 added `Domain/Effects/EffectCatalog.cs` with `confetti` (Monitor, `confetti.json`) and `focus-ring` (Cursor, `focus-ring.json`). Embed the Lottie files under those resource names, or change the names in the catalog and its test.
2. Embed the two Lottie files, selected from LottieFiles and adapted where needed, with a record of each file's source URL, author, and license. Confetti must end within 3 seconds and focus ring within 2 seconds, with no rapid flashing. Load and parse each file once, outside the hook callback.
3. Build the Monitor surface: cover the target monitor, scale the canvas to cover it, keep proportions, crop the overflow.
4. Build the Cursor surface: canvas size in DIP at the monitor's DPI, centered on the captured cursor point, intersected with the monitor bounds. Never shift it inward.
5. Play once from monotonic elapsed time; subscribe to the frame callback only while rendering. Use the S4-01 probe trigger for the visual checks; S4-06 already routes effect shortcuts to a start delegate.
6. Carried over from S4-06 by owner decision on 2026-10-11: compose `EffectChannelController` with the new surface in `App.xaml.cs` and pass its `Start` to `GlobalShortcutAdapter` instead of the do-nothing delegate. After the visual checks, remove the S4-01 probe: `EffectSurfaceProbe.cs`, `EffectProbeHotkeys.cs`, `EffectProbeInputLoad.cs`, and the `DRAWEM_EFFECT_PROBE` wiring in `App.xaml.cs`.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
