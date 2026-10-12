# F03: locating the drawing delay

Date: 2026-09-22. Branch: `F03-render-lag-investigation`.

## Status (updated 2026-09-22)

Steps 2–3 were done with a synthetic harness (`../../../tools/RenderLagHarness`, without a real
Win32 hook or screen; step 1 stays open). Step 4: hypothesis 2 (full redraw on every point) is
confirmed by measurements. The redraw cost grew linearly with the number of points (up to
50–60 ms), so an event stream at 500–1000/s was not processed in real time (one second stretched
to 2–28.6 seconds, depending on the scenario). Hypotheses 3–5 were not checked (they need a
profiler and a real desktop). Step 5: a fix was applied, incremental rendering in
`StrokeRenderElement` (cached completed lines, only new segments of the active line are appended).
After the fix, the redraw cost no longer grows, and harness run durations match the nominal value.
Details and numbers: `F03-render-lag-harness-results.md`.

Review (second round) found 2 critical correctness bugs in the first version of the fix.
`CompletedStrokes.Count` does not distinguish "nothing changed" from "clear + a new stroke of the
same length", and completing a stroke with an unseen point left stale active tracking, which could
wrongly continue the next, unrelated stroke. Both are fixed: `DrawingState.Generation` was added as
the authoritative reset signal, and active tracking is now unconditionally cleared when any new
completed stroke appears. Methodology problems in the harness were also found and fixed: the raw
report/trace overwrote the curated comparison file (they are now written separately, to
`harness-raw/`, outside git), and Duration for runs not fully processed (cut off by the timeout)
was presented as the full stream processing time. The BEFORE numbers are now taken without
timeout cut-off (all runs completed).

Step 6 (regressions): 32/32 unit tests (5 new ones for incremental rendering); a manual run on a
real desktop has not been done.

## Symptom and status

The user observes that a line sometimes appears with a large lag; there is no stable pattern yet.
It is not established whether the start of the line, its continuation, or both are delayed.
The analysis below is based on the current code, not on a reproduced lag.
The cause is not confirmed. This document does not change the application.

## What the code shows

- `App.xaml.cs`: both global hooks are created in `OnStartup`, on the UI thread.
  Commands are passed through `Dispatcher.BeginInvoke(action)`.
- `Infrastructure/GlobalMouseInputAdapter.cs`: each move in draw mode
  creates a separate Dispatcher task; incoming points are not merged.
- `Application/DrawingSessionController.cs`: each move calls
  `PublishState`, which copies the active line's point array and the list of completed
  lines. The points of completed lines are not copied again here.
- `Presentation/OverlayWindow.xaml.cs`: on the UI thread, `Render` calls
  `UpdateState` immediately. In the normal chain the controller already runs on the UI thread, so
  the `pendingState` branch that merges updates is not used.
- `Presentation/StrokeRenderElement.cs`: each update reopens the single
  `DrawingVisual` and writes all segments of all lines. The cost of one
  update grows with the total number of points. For one growing line, the total
  work of copying and writing segments grows quadratically with the number of points.
- The transparent window covers the whole virtual desktop. The composition cost on
  specific monitors and GPUs is not measured yet.
- The single-point test checks a pixel through `RenderTargetBitmap`, but not the time
  from real input delivery to the frame appearing on screen.

## Hypotheses in order of checking

| Priority | Possible cause | Testable prediction |
| --- | --- | --- |
| 1 | The UI queue is overloaded with mouse tasks; event processing crowds out frame preparation | During the lag, task age and the number of pending tasks grow; lowering the event rate or batching reduces the delay |
| 2 | The whole drawing is rewritten on every point | `UpdateState` time grows with the number of segments; after a clear, at the same event rate, it drops substantially |
| 3 | Snapshot copying creates GC pressure | Delay spikes coincide with GC pauses and allocation growth; a long line is worse than a short one with the same number of completed lines |
| 4 | A busy UI thread delays global hook delivery | The Win32 event age grows already on entry to the callback, before our queue; a separately measured Dispatcher delay does not explain the whole lag |
| 5 | Window composition, GPU, or external load/hook chain | Internal stages are fast but the visual lag remains; reproducibility changes when one external factor is changed in a controlled way |

The first four mechanisms can reinforce each other. The random nature of the symptom
is consistent with short load spikes, but proves nothing by itself.
PowerToys is not considered a cause without a comparison; disabling it is a separate manual
experiment, only if internal measurements point to an external delay.

Microsoft states that `Normal` has a higher priority than `Render`, and that a low-level mouse
callback runs on the thread that installed the hook. So not drawing inside the
callback does not by itself isolate input delivery from a busy UI thread.

Sources:
- [DispatcherPriority](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherpriority)
- [LowLevelMouseProc](https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelmouseproc)

## Work plan

1. **Record the baseline behavior.** Record the commit, path and version of the Release build
   that actually runs, the number of processes, monitors, DPI, screen refresh rate, and the
   available mouse polling rate. Distinguish start delay, line lag, and catch-up drawing after
   stopping/releasing. Start with a fresh launch and an empty drawing, then repeat the same
   scenario after lines have accumulated.

2. **Add bounded measurements.** Link session and event sequence IDs with timestamps: hook entry,
   Dispatcher task queue/start, controller update start/end, `UpdateState` start/end, and the
   nearest `CompositionTarget.Rendering`. Collect queue depth, events/s, points, segments, and
   p50/p95/p99/max delays; investigate GC with a profiler when there is a correlation.
   Use the monotonic `Stopwatch`; compute the age of `MSLLHOOKSTRUCT.time` in the matching Win32
   time base, accounting for wraparound, and do not subtract it from Stopwatch.
   Keep the data in a bounded buffer and export it outside the callback and the hot path.
   Measure the diagnostics overhead. The Rendering marker does not prove that a pixel is shown:
   check the visual delay with a screen/camera recording or a presentation trace.

3. **Build a repeatable scenario.** In an STA/WPF harness, reproduce the real chain
   adapter → Dispatcher → controller → OverlayWindow → renderer.
   Feed fixed trajectories at controlled rates, for example 125/500/1000 events/s; warm up the
   code separately and check a cold start.
   Compare an empty canvas, a long line, and many completed lines. In the harness,
   explicitly check queue/update age and the absence of unbounded growth, not only successful
   completion. It does not replace a real Win32/screen run.
   Save the run command, configuration, baseline result, and the trace of at least one observed
   lag. If the harness does not reproduce the symptom, use measurements from the real build and
   do not declare a similar synthetic problem the cause.

4. **Check the hypotheses one at a time.** First the queue and the redraw cost, then GC and
   callback age. Compare on the same trajectory and the same number of points. Check external
   factors only after the delay is located: one monitor/current configuration, current
   load/idle system, another application with a hook enabled/disabled. Keep before/after results
   and their spread.

5. **Choose the fix based on measurements.** If queue overload is confirmed, batch points, bound
   the number of pending tasks, and publish state at the frame rate. If the redraw is expensive,
   cache completed geometry and update the active line separately. If GC is the cause, reduce
   copies while keeping published data immutable. If the hook is delayed, consider a dedicated
   thread with a message loop and an explicit command order.
   Changing priority alone does not remove extra work. Do not lose intermediate points by simply
   taking the last position: that can cut corners.

6. **Check the result and regressions.** Repeat the baseline scenario and compare delay
   distributions, maximums, queue, and allocations. Set a budget based on the screen refresh rate
   before evaluating the fix: the target is an update within 1–2 frames under normal load, with no
   queue buildup; record absolute values and stress mode separately. Check the start point without
   movement, the shape/last point of a line, the start/move/end/clear order under queueing,
   that cleared lines do not come back, Ctrl+Alt+Z/X, input blocking only while drawing, and
   click-through after exit. Run the relevant tests, a fresh publish, and a manual run on the
   user's desktop.

## Expected changes during implementation

Measurements: `App.xaml.cs`, `Infrastructure/Win32MouseHookSource.cs`,
`Infrastructure/GlobalMouseInputAdapter.cs`, `Application/DrawingSessionController.cs`,
`Presentation/OverlayWindow.xaml.cs`, `Presentation/StrokeRenderElement.cs`;
when measuring the start, also the keyboard hook/shortcut adapter. A separate diagnostic
harness and a results file. The exact files for the fix are decided after measurement.

Done when: a specific delay stage is confirmed by a trace, the fix improves the same scenario,
regressions are checked, and a fresh build is checked manually. Until then the problem stays
open, even if the regular unit tests pass.
