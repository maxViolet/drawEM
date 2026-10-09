# v4 / S4-03: implementation plan

**Task:** [play built-in Lottie effects](task.md). **Acceptance:** [criteria](acceptance.md).

## S4-03.1: speed gate (branch `S4-03.1-lottie-spike`, about half a day)

1. Add `SkiaSharp.Skottie` and `SkiaSharp.Views.WPF`. Change the S4-01 probe so its measured render path plays a Lottie file instead of the code-drawn shape. Add a second window type: sized to the Lottie canvas in DIP, centered on the cursor, clipped to the monitor. The monitor-sized window uses cover scaling.
2. Run the probe benchmark once per placement; the report names the placement and the window bounds. Record render callback p95, hook delay p95 and maximum, and resource counts against the roadmap targets.
3. If the Monitor surface misses 4 ms p95, render it at half resolution and measure again. If that also misses, stop: revise the roadmap to Cursor effects only and get the project owner's confirmation before S4-03 continues.

## S4-03: built-in effects

1. Add the built-in catalog in `Domain/Effects`: effect ID, display name, placement, and resource name for `confetti` (Monitor) and `focus-ring` (Cursor).
2. Embed the two Lottie files, selected from LottieFiles and adapted where needed, with a record of each file's source URL, author, and license. Confetti must end within 3 seconds and focus ring within 2 seconds, with no rapid flashing. Load and parse each file once, outside the hook callback.
3. Build the Monitor surface: cover the target monitor, scale the canvas to cover it, keep proportions, crop the overflow.
4. Build the Cursor surface: canvas size in DIP at the monitor's DPI, centered on the captured cursor point, intersected with the monitor bounds. Never shift it inward.
5. Play once from monotonic elapsed time; subscribe to the frame callback only while rendering. Use the S4-01 probe trigger until S4-06 supplies shortcuts.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
