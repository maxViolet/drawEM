# v4 / S4-12: acceptance criteria

**Task:** [publish and verify on Windows](task.md). **Plan:** [implementation](plan.md).

- [x] Automated suite and Windows x64 publish results are recorded for the exact executable/build. See [automated gate](#automated-gate) and [build identity](#build-identity).
- [ ] Manual results identify Windows version, CPU/GPU, monitor layouts/scaling, input layouts, and the tested external applications. Unverified: the agent recorded the [build machine](#build-machine) only; no manual run took place.
- [ ] Visible effects, focus/input pass-through, mixed DPI, Cursor-effect clipping at monitor edges, monitor transitions, drawing/sound regression, Settings, and cleanup checks are recorded separately from automated results. Unverified: see [published-app checks](#published-app-checks).
- [ ] 100-invocation shortcut-to-present latency, callback, hook delay, and resource results meet the roadmap targets for each placement; visible pacing is observed and both hooks still work after the cycles. Unverified: see [measurements](#measurements).
- [ ] Full-display and window-only sharing results are recorded separately; unrun desktop checks are marked unverified. Unverified: see [screen sharing](#screen-sharing).
- [ ] A missed target blocks the release. Only the project owner can accept it, and only by revising the roadmap targets or scope with a recorded rationale and confirmation before release; the revised targets are then met and recorded. Not reached: no target has been measured.
- [ ] The v3 Step 8 baseline is complete before claiming a v4 release: none of its scenarios remains unverified. Not met: see [release blockers](#release-blockers).
- [ ] v4 is not released while any roadmap published-app check for either placement is unverified, including mixed DPI, Cursor effects at each monitor edge, monitor transitions, lifecycle events, full-display sharing, and drawing plus sound regression. Window-only sharing may stay unverified; record it separately. Not met: see [release blockers](#release-blockers).

Result values: **Pass**, **Fail**, **Unverified** (not run, or the environment
cannot run it). Automated tests and publish success do not close any
published-app check.

## Automated gate

Run on 2026-10-13 by an agent in the `S4-12-effect-publish` worktree, at
commit `335de887ee0dacd7c6b862783606ba99009b987d` (equal to `origin/main`;
this step changes no code). .NET SDK `8.0.425`.

| Command | Result |
| --- | --- |
| `dotnet restore DrawEM.sln` | Pass |
| `dotnet test DrawEM.sln --no-restore -m:1` | Pass: 701 passed, 0 failed, 0 skipped. |
| `dotnet build DrawEM.sln -c Release -warnaserror` | Pass: 0 warnings, 0 errors. |
| `dotnet test DrawEM.sln -c Release --no-build` | Pass: 701 passed, 0 failed, 0 skipped. |
| `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64` | Pass: self-contained win-x64 output, 472 files. No drawEM process was running. |

## Build identity

- Executable: `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`
- File version `1.0.0.0`; product version
  `1.0.0+335de887ee0dacd7c6b862783606ba99009b987d`; written
  2026-10-13 18:18:22 UTC.
- `DrawEM.App.exe` SHA-256:
  `7C6B014EACB980512EDDAC92739CA7F159FDEBF2BC50ADBE6B432433E8E08ACF`
- `DrawEM.App.dll` SHA-256 (the application code; the executable is the
  .NET host):
  `4850D86969350CCA73B6A404DF275BE41C129C664AF8EB49C5AFF70291C8B8CE`
- The built-in Lottie files are embedded resources in `DrawEM.App.dll`; their
  license and sources are in `licenses\` in the publish directory.

A published-app run must use this build, or record the identity of the build
it used.

## Build machine

Recorded by the agent from the system; this is the build machine, not a
tested desktop environment.

- Windows 11 Pro 26H2, build 10.0.26300.9457
- CPU: Intel Core Ultra 7 155H; GPUs: NVIDIA GeForce RTX 4070 Laptop GPU,
  Intel Arc Graphics
- Monitors: one, `\\.\DISPLAY1` (primary), 1920x1200. Scaling not recorded.
- Input layouts and external applications: not recorded.

## Published-app checks

All **Unverified**: no one has run the published app for this step. Run each
check with the build above, once with a Monitor effect (Confetti) and once with
a Cursor effect (Focus ring), with another application focused unless the
check says otherwise. Record the result and a short note.

| # | Check | Result |
| --- | --- | --- |
| 1 | Settings: effect draft, Save, Cancel, and Sample for both placements. | Unverified |
| 2 | Tray Stop effects removes the effect; sound and drawings stay. | Unverified |
| 3 | Effect shortcuts start, restart, and replace effects across placements. | Unverified |
| 4 | Effects appear over the focused app; focus and keyboard/pointer input pass through. | Unverified |
| 5 | Drawing plus effects plus sound together; no drawing or sound regression. | Unverified |
| 6 | Monitor switching stops the effect; outside desktop edges do not. | Unverified |
| 7 | Two mixed-DPI monitors, including negative coordinates. | Unverified |
| 8 | Cursor effect at each monitor edge is clipped to that monitor. | Unverified |
| 9 | Lifecycle: task switching, display disconnect/reconnect, lock/unlock, suspend/resume, exit during animation. | Unverified |
| 10 | Cleanup: no effect window or growing handle count after repeated use. | Unverified |
| 11 | Neither effect flashes more than three times in any one second (S4-03). | Unverified |

## Measurements

All **Unverified**: no reference PC run took place, and no measurement
tooling was added in this step. The plan defines what to measure; the timing
method and its error must be chosen and recorded with the run. Targets come
from the [roadmap](../ROADMAP-v4.md#acceptance-strategy).

| Measurement | Target | Monitor | Cursor |
| --- | --- | --- | --- |
| First appearance, 100 warm shortcut invocations, hook event to first presented effect frame | p95 within 100 ms | Unverified | Unverified |
| Cold start first appearance (record separately) | Recorded | Unverified | Unverified |
| Render callback time with Skia drawing inside | p95 within 4 ms | Unverified | Unverified |
| Frame interval | p95 within 25 ms; at most 1% of frames over 25 ms | Unverified | Unverified |
| Hook event-to-callback delay during animation and drawing | p95 within 25 ms, maximum 100 ms | Unverified | Unverified |
| Visible pacing in a recording | No sustained stutter or drawing slowdown | Unverified | Unverified |
| After 100 restart/replacement cycles | No live instances, subscriptions, or growing window/handle counts; both hooks still work | Unverified | Unverified |

Reference PC (CPU, GPU, memory, monitor resolution and scaling): not recorded.

## Screen sharing

| Check | Result |
| --- | --- |
| Full-display share seen on a second receiving device shows both placements. | Unverified |
| Window-only share (recorded separately; may stay unverified for release). | Unverified |

## Release blockers

v4 is not releasable:

- [v3 Step 8](../../../v3/STEP-8-ACCEPTANCE-RESULTS.md) still has checks
  2.7, 2.8, 4.2, 4.3, 4.5, 6.1, and 6.2 unverified. The v4 release needs all of them Pass first.
- Every published-app check, measurement, and full-display sharing check above
  is unverified.

## Validation

- Automated checks: Pass; see [automated gate](#automated-gate).
- Published-app Windows desktop checks, measurements, and screen sharing:
  unverified.
