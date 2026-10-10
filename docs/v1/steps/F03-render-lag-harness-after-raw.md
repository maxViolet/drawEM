# F03: raw output of the latest harness run

Generated: 2026-09-22 22:36:16. Tool: tools/RenderLagHarness.
Run: `dotnet run --project tools/RenderLagHarness -c Release`.
This file is overwritten on every run. The curated before/after comparison is docs/v1/steps/F03-render-lag-harness-results.md; update it by hand.

Limitation: the harness reproduces the adapter -> Dispatcher -> controller -> renderer chain synthetically, without a real Win32 hook or window composition. It does not replace a measurement on a real build (plan, steps 1/3).

Queue age / Max pending ops are not meaningful (the pacer sends events at Send priority, so the Normal queue does not drain in parallel); see the explanation in the curated report.
Duration for TIMEOUT rows is not the full stream processing time but the time until the 8-second drain safeguard fires; Completed < Requested shows that the stream was not fully processed.

| Scenario | Rate (ev/s) | Completed/Requested | Duration (ms) | Queue age p50/p95/p99/max (ms) | Processing p50/p95/p99/max (ms) | Max pending ops | Processing growth (first 10% -> last 10%, ms) |
| --- | --- | --- | --- | --- | --- | --- | --- |
| empty-canvas [cold] | 500 | 500/500 | 1012.0 | 508.66/956.65/996.47/1003.10 | 0.00/0.01/0.02/4.71 | 500 | 0.106 -> 0.005 |
| empty-canvas | 125 | 125/125 | 994.0 | 497.30/944.73/984.68/992.51 | 0.00/0.01/0.02/0.14 | 125 | 0.014 -> 0.002 |
| empty-canvas | 500 | 500/500 | 1005.5 | 501.80/948.61/988.39/998.20 | 0.00/0.01/0.01/0.12 | 500 | 0.005 -> 0.003 |
| empty-canvas | 1000 | 1000/1000 | 1006.1 | 502.08/949.53/989.30/999.13 | 0.00/0.01/0.01/0.10 | 1000 | 0.002 -> 0.003 |
| long-completed-line | 125 | 125/125 | 993.0 | 496.61/944.25/984.22/992.18 | 0.00/0.00/0.01/0.03 | 125 | 0.003 -> 0.000 |
| long-completed-line | 500 | 500/500 | 1001.7 | 500.50/948.69/988.33/998.20 | 0.00/0.00/0.00/0.03 | 500 | 0.002 -> 0.001 |
| long-completed-line | 1000 | 1000/1000 | 1004.7 | 502.21/949.38/989.20/999.13 | 0.00/0.00/0.00/0.05 | 1000 | 0.001 -> 0.001 |
| many-completed-strokes | 125 | 125/125 | 992.8 | 496.48/944.20/984.18/992.14 | 0.00/0.00/0.00/0.03 | 125 | 0.004 -> 0.001 |
| many-completed-strokes | 500 | 500/500 | 1004.8 | 503.63/952.58/988.22/998.15 | 0.00/0.00/0.00/4.16 | 500 | 0.084 -> 0.001 |
| many-completed-strokes | 1000 | 1000/1000 | 1007.0 | 503.90/949.66/989.27/999.15 | 0.00/0.00/0.00/0.02 | 1000 | 0.001 -> 0.001 |

Worst case by processing cost (p99): `empty-canvas` @ 125 ev/s, processing p99 0.02 ms, max 0.14 ms. Full trace: F03-render-lag-harness-latest-trace.csv.
