# F03: synthetic harness results (before/after the fix)

Tool: `../../../tools/RenderLagHarness` (`dotnet run --project tools/RenderLagHarness -c Release`).
It reproduces the adapter → Dispatcher → controller → renderer chain synthetically, with a fake
mouse source instead of the real Win32 hook and without window composition (no real screen).
It does not replace a measurement on a real build (plan, steps 1/3); that data does not exist yet.

This file is curated and hand-written. `../../../tools/RenderLagHarness` does not touch it: each
run writes raw output to `harness-raw/` (not in git, see `../../../.gitignore`) and
overwrites only those files. Update this file by hand after a run.

The BEFORE numbers below come from a harness run against the `StrokeRenderElement` code from commit
`0f1631e` (the last one before the fix), temporarily restored, with the drain safeguard raised
to 30 s. This was done on purpose so that all 10 scenarios could process the whole stream
(`Completed == Requested` everywhere) instead of being cut off by the timeout, as in the first
version of this report. The AFTER numbers are a normal run (8 s safeguard, no run reaches it).

The tables below are a hand-written summary (the first version of this report mixed up numbers
from different rows in its summary). The unedited harness output is in
`F03-render-lag-harness-before-raw.md` and `F03-render-lag-harness-after-raw.md` next to this
file; when you edit the tables, check them against those files, not only against the text here.

## How to read the metrics

- **Duration does not measure the ability to keep up with input in real time.** The harness
  first queues all events (injection: each `Invoke(..., Send)` returns right after queuing,
  without waiting for processing), and only then drains the accumulated `Normal`-priority queue
  in one call. Injection and processing do not overlap in time, unlike a real message loop,
  where new events keep arriving while earlier ones are processed. Therefore
  `Duration ≈ (events / rate) + total processing cost of the whole batch` almost always, even when
  the cost of one event clearly fits in the interval between events and the system would keep up
  in a real interleaved scenario. Use this metric to compare the total batch processing cost
  BEFORE and AFTER the fix, not to draw conclusions about real on-screen lag.
- **Processing** is the time of one `controller.ReportPointer(...)` plus the synchronous
  `StrokeRenderElement.UpdateState(...)`, that is, the cost of one redraw.
- **Queue age / Max pending ops are not meaningful in this harness**: the pacer sends events through
  `Dispatcher.Invoke(..., DispatcherPriority.Send)`, which prevents the `Normal`-priority queue
  (the redraws themselves) from draining in parallel; it drains in one call at the end of the run.
  So `max pending ops` always equals the number of events regardless of real overload, and
  `queue age` is the accumulated processing debt spread over the run, not a live delivery delay.
  Do not use these two columns as evidence; they stay in the raw tables for completeness, not as
  an argument.

## BEFORE the fix (StrokeRenderElement from 0f1631e, 30 s safeguard, all runs complete)

| Scenario | Rate | Duration (ms) | Processing p99 (ms) | Processing max (ms) | Processing growth (first 10%→last 10%, ms) |
| --- | --- | --- | --- | --- | --- |
| empty-canvas [cold] | 500 | 2203.6 | 6.40 | 7.70 | 0.09 → 2.98 |
| empty-canvas | 125 | 1011.6 | 0.94 | 1.14 | 0.02 → 0.38 |
| empty-canvas | 500 | 1683.5 | 5.89 | 7.85 | 0.02 → 3.71 |
| empty-canvas | 1000 | **8492.4** | 27.49 | 33.51 | 0.08 → 19.60 |
| long-completed-line | 125 | 4068.4 | 61.16 | 62.85 | 32.82 → 18.11 |
| long-completed-line | 500 | **16398.2** | 55.74 | 64.04 | 28.95 → 39.49 |
| long-completed-line | 1000 | **29551.4** | 55.99 | 80.46 | 21.73 → 39.09 |
| many-completed-strokes | 125 | 1188.6 | 4.59 | 6.26 | 1.58 → 1.05 |
| many-completed-strokes | 500 | 2523.1 | 12.64 | 16.64 | 1.09 → 4.91 |
| many-completed-strokes | 1000 | **10532.8** | 32.99 | 44.83 | 1.67 → 20.66 |

Exact numbers: `F03-render-lag-harness-before-raw.md`. Duration reached ~29.6 seconds per
nominal second of input, so the total batch processing cost (injection + drain, see "How to read
the metrics") reached ~28.6 seconds above nominal, not 0.
The cost of one redraw grew with the number of accumulated points (for
`long-completed-line @ 1000ev/s`, 21.73 ms at the start of the run → 39.09 ms by the end; with
1000 points already pre-filled on the line, the start is clearly more expensive than on an empty
canvas). Hypothesis 2 of the plan (full redraw on every point) is confirmed. These numbers do not
directly prove an inability to keep up with real input (see above), but this processing cost
clearly does not fit the frame budget at any reasonable frame rate.

## AFTER the fix (current StrokeRenderElement, 8 s safeguard, no run reaches it)

Exact numbers: `F03-render-lag-harness-after-raw.md`.

| Scenario | Rate | Duration (ms) | Processing p99 (ms) | Processing max (ms) | Processing growth (first 10%→last 10%, ms) |
| --- | --- | --- | --- | --- | --- |
| empty-canvas [cold] | 500 | 1012.0 | 0.02 | 4.71 | 0.11 → 0.005 |
| empty-canvas | 125 | 994.0 | 0.02 | 0.14 | 0.01 → 0.002 |
| empty-canvas | 500 | 1005.5 | 0.01 | 0.12 | 0.005 → 0.003 |
| empty-canvas | 1000 | 1006.1 | 0.01 | 0.10 | 0.002 → 0.003 |
| long-completed-line | 125 | 993.0 | 0.01 | 0.03 | 0.003 → 0.000 |
| long-completed-line | 500 | 1001.7 | 0.00 | 0.03 | 0.002 → 0.001 |
| long-completed-line | 1000 | 1004.7 | 0.00 | 0.05 | 0.001 → 0.001 |
| many-completed-strokes | 125 | 992.8 | 0.00 | 0.03 | 0.004 → 0.001 |
| many-completed-strokes | 500 | 1004.8 | 0.00 | 4.16 | 0.08 → 0.001 |
| many-completed-strokes | 1000 | 1007.0 | 0.00 | 0.02 | 0.001 → 0.001 |

After the optimization, processing the accumulated batch takes a small share of the run time:
Duration ≈ nominal (`events/rate`) in all scenarios and rates. The excess over nominal
(the total batch processing cost, see "How to read the metrics") fell from seconds to
milliseconds. The cost of one redraw does not grow with the number of points (flat profile,
~0.00–0.03 ms p99, rare outliers up to a few ms, probably GC/JIT, not the redraw itself).
This harness does not measure the delay under continuous input (a real message loop, where events
and processing overlap in time). That needs a real Win32/screen run (plan, step 1), see
"Limitations" below; it has not been done.

## What changed (plan, step 5)

`src/DrawEM.App/Presentation/StrokeRenderElement.cs`: previously a single `DrawingVisual`
was reopened (`RenderOpen`) and fully rewritten on every `UpdateState`, so the cost and the total
work grew with the total number of points (hypothesis 2, confirmed). It now uses a
`VisualCollection`: completed lines are drawn once and never touched again; the active line is
extended with a new `DrawingVisual` that contains only the segments added since the previous
update. The cost of one update is now O(number of new points), not O(total number of points).

Review found that the `CompletedStrokes.Count` counter is not a sufficient reset signal: an
intermediate state skipped by coalescing (clear → new stroke → end in one call) can leave the
counter unchanged, and the old line stays on screen instead of the new one. `DrawingState.Generation`
was added (incremented in `ClearAndExitDrawMode`) as the authoritative reset signal. Review also
found that completing a stroke whose points the renderer had not fully seen as active (a missed
move before `End`) left stale active tracking, which could attach the next, unrelated stroke to it.
Now any new completed stroke unconditionally clears active tracking, either by marking the visuals
permanent (if they matched) or by removing the stale ones (if not).

Regression tests: `tests/DrawEM.Tests/Presentation/StrokeRenderElementTests.cs` —
`UpdateState_GrowingActiveStroke_RendersEachIncrementalSegment`,
`UpdateState_StrokeCompletesThenNewOneStarts_KeepsCompletedPixelsVisible`,
`UpdateState_FewerCompletedStrokesThanBefore_ResetsAndDropsOldPixels`,
`UpdateState_CoalescedClearThenNewStrokeOfSameLength_DropsOldAndDrawsNew`,
`UpdateState_StrokeEndsWithUnseenPoint_DropsStalePartialVisualAndDrawsFullStroke`,
`ClearDuringFirstActiveStroke_DropsPixelsThroughRealController` (integration test wiring a
real `DrawingSessionController` to a real `StrokeRenderElement`, locking in the
`Generation` fix for the exact 0 → 0 `CompletedStrokes.Count` case).
Full run: 33/33 passed.

## Limitations of this pass

- Hypotheses 3 (GC), 4 (delay of the Win32 hook itself before the dispatcher) and 5 (window
  composition/GPU) were not checked: they need a profiler and a real interactive desktop, which
  this execution environment does not have. The harness neither confirms nor rules them out.
- The real Win32/screen run (plan, step 1) has not been done; it needs a manual check on the
  user's machine with a fresh Release build.
- Queue age / max pending ops in the raw harness tables are a methodology artifact (see
  above), not evidence of queue overload in the real application.

## Next in the plan (step 6)

The start point without movement, the shape/last point of a line, Ctrl+Alt+Z/X, input blocking
and click-through were checked by the existing 27 tests (unchanged) plus 6 new ones. A manual run
on the user's desktop after the fix has not been done yet; it is recommended before merge.
Until then, the original random lag the user reported cannot be considered fixed: the harness
confirms and removes a specific code-level mechanism (hypothesis 2), but it does not measure or
reproduce the symptom itself on a real screen.

An optional review item, the growth in the number of `Visual`/`DrawingVisual` objects in the
`VisualCollection` during long drawing, was not measured; it is left as a note for the future and
does not block the merge.
