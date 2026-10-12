# F03: raw output of the latest harness run

Generated: 2026-09-22 22:35:45. Tool: tools/RenderLagHarness.
Run: `dotnet run --project tools/RenderLagHarness -c Release`.
This file is overwritten on every run. The curated before/after comparison is docs/v1/steps/F03-render-lag-harness-results.md; update it by hand.

Limitation: the harness reproduces the adapter -> Dispatcher -> controller -> renderer chain synthetically, without a real Win32 hook or window composition. It does not replace a measurement on a real build (plan, steps 1/3).

Queue age / Max pending ops are not meaningful (the pacer sends events at Send priority, so the Normal queue does not drain in parallel); see the explanation in the curated report.
Duration for TIMEOUT rows is not the full stream processing time but the time until the 8-second drain safeguard fires; Completed < Requested shows that the stream was not fully processed.

| Scenario | Rate (ev/s) | Completed/Requested | Duration (ms) | Queue age p50/p95/p99/max (ms) | Processing p50/p95/p99/max (ms) | Max pending ops | Processing growth (first 10% -> last 10%, ms) |
| --- | --- | --- | --- | --- | --- | --- | --- |
| empty-canvas [cold] | 500 | 500/500 | 2203.6 | 945.26/1173.05/1195.41/1201.54 | 2.43/5.70/6.40/7.70 | 500 | 0.094 -> 2.982 |
| empty-canvas | 125 | 125/125 | 1011.6 | 498.64/944.46/984.31/992.25 | 0.06/0.56/0.94/1.14 | 125 | 0.015 -> 0.384 |
| empty-canvas | 500 | 500/500 | 1683.5 | 617.73/948.45/988.21/998.14 | 0.94/3.74/5.89/7.85 | 500 | 0.016 -> 3.707 |
| empty-canvas | 1000 | 1000/1000 | 8492.4 | 1398.23/6483.38/7241.99/7478.95 | 4.97/21.11/27.49/33.51 | 1000 | 0.079 -> 19.598 |
| long-completed-line | 125 | 125/125 | 4068.4 | 2180.55/3004.54/3040.97/3057.02 | 21.07/46.60/61.16/62.85 | 125 | 32.817 -> 18.107 |
| long-completed-line | 500 | 500/500 | 16398.2 | 7926.41/14362.77/15115.81/15341.91 | 30.07/49.87/55.74/64.04 | 500 | 28.950 -> 39.486 |
| long-completed-line | 1000 | 1000/1000 | 29551.4 | 11921.08/26535.13/28106.94/28494.95 | 26.67/47.72/55.99/80.46 | 1000 | 21.728 -> 39.090 |
| many-completed-strokes | 125 | 125/125 | 1188.6 | 614.83/952.59/985.42/992.57 | 0.98/3.39/4.59/6.26 | 125 | 1.580 -> 1.051 |
| many-completed-strokes | 500 | 500/500 | 2523.1 | 994.90/1439.14/1499.30/1511.04 | 2.32/7.60/12.64/16.64 | 500 | 1.085 -> 4.906 |
| many-completed-strokes | 1000 | 1000/1000 | 10532.8 | 2670.21/8422.47/9265.95/9503.11 | 8.10/23.82/32.99/44.83 | 1000 | 1.674 -> 20.661 |

Worst case by processing cost (p99): `long-completed-line` @ 125 ev/s, processing p99 61.16 ms, max 62.85 ms. Full trace: F03-render-lag-harness-latest-trace.csv.
