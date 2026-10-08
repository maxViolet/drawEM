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
- [x] [record] A locally published Windows x64 build contains the temporary S4-01 trigger in `src/DrawEM.App/Presentation/Effects/EffectSurfaceProbe.cs`, wired only when `DRAWEM_EFFECT_PROBE=1` in `App.xaml.cs`. Record its exact executable path, version, hash, and trigger setting; S4-06 removes this trigger.
  - Executable: `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe` in the `S4-01-effect-surface` worktree, published with `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64`.
  - Version: `1.0.0+fc2ff1a06d746dd2c604da27e935ca3cb7577c5e` (commit `fc2ff1a`). SHA-256: `204ca0df919d0de7f3994ecacba074a13020386e6ffa647607c59a111ccd9c50`.
  - Trigger setting: `DRAWEM_EFFECT_PROBE=1` in the environment of the process. Any other value, or no value, leaves the probe, its hooks, and its hotkeys out.
  - The hotkey registration is in `src/DrawEM.App/Infrastructure/Effects/EffectProbeHotkeys.cs` and the measurement hooks in `EffectProbeInputLoad.cs` next to it, because Presentation must not own global input. S4-06 removes both with the probe.
- [ ] [gate] After 100 effect cycles, the probe has no `CompositionTarget.Rendering` subscription or effect window left. Record process handle, GDI object, and USER object counts after warm-up and after the cycles; each final count is within 5% of its warm-up count.

## Validation

- Automated checks, 2026-10-08, commit `fc2ff1a`:
  - `dotnet build DrawEM.sln -warnaserror`: succeeded, no warnings.
  - `dotnet test DrawEM.sln --no-build --filter "FullyQualifiedName~EffectSurfaceCoordinateTests"`: 29 passed, 0 failed. The tests cover physical-to-DIP mapping of a monitor at negative desktop coordinates at 100%, 150%, and 200% scaling, points on the neighboring monitor, clipping at the right edge and the top-left corner, and invalid scales.
  - `dotnet test DrawEM.sln --no-build`: 501 passed, 0 failed, including the layer dependency tests.
- Published-app smoke check, 2026-10-08, the build above on the development PC (one 1920x1200 monitor at 100%, 120 Hz): one Ctrl+Alt+F9 invocation sent with `keybd_event` from a script. With commit `4898034`, the probe drew 240 frames in 2 seconds, its first drawing callback ended 62 ms after the hotkey (cold start, renderer estimate only), the window covered exactly the monitor, the foreground window did not change, and the process kept running. This is not a substitute for the desktop checks below.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.

## Run the probe

1. Close any running drawEM, then start the probe build from PowerShell:
   `$env:DRAWEM_EFFECT_PROBE = '1'; & '<publish directory>\DrawEM.App.exe'`
2. Press `Ctrl+Alt+F9` to show one effect on the monitor containing the cursor: a blue circle crosses the monitor in 2 seconds and touches its top and bottom edges, so clipping is visible. Pressing it again replaces the running effect.
3. Press `Ctrl+Alt+F10` and release all keys within 3 seconds to run the benchmark. Do not touch the mouse or keyboard for about 5 minutes: any key event ends the probe's draw mode. The benchmark runs 5 warm-up invocations. The process's first invocation is the cold start: the step 2 `Ctrl+Alt+F9` effect, or else the first warm-up. The report records it separately, excludes it from the warm results, and keeps it after later benchmarks in the same process; restart drawEM before each benchmark that needs its own cold start. After the warm-up, it runs 100 measured invocations with draw mode active. During each measured invocation, a background thread injects mouse moves along a 40 px circle and presses of an unassigned key (VK 0x97); the moves draw a stroke under the effect. Clear those strokes afterwards with `Ctrl+Alt+X`.
4. Read the report in `%LOCALAPPDATA%\drawEM\effect-probe\effect-probe-<time>.md`. It records the build, Windows, CPU, GPU, monitors and DPI scales, the timing method and its error, and pass/fail for each automated gate: all 5 warm-up and 100 measured invocations completed, render callback p95, hook delay p95 and maximum over at least 1,000 events of both types received while draw mode was active (any event after draw mode closed early fails the run), keyboard and mouse events received by both the probe hooks and the production hooks in a 1-second check after the last cycle, effect windows and rendering subscriptions left, handle, GDI, and USER counts against the warm-up counts, foreground changes, and window placement. Raw samples are in the `.csv` next to it. An invocation that renders no frame for 5 seconds, for example while the session is locked, ends as a failure. Single-invocation failures go to `effect-probe-errors.log` in the same directory.
5. Record the manual checks separately: visible order above strokes, click-through, typing and scrolling into another application, Alt+Tab and Win+Tab, monitor clipping and mixed DPI on two monitors, disconnect/reconnect during an effect, and a screen recording for stutter and missed strokes. Run the benchmark once per monitor configuration, starting with the cursor on the monitor to measure.

## Renderer decision

Complete this section after the Windows experiment; it is not a preselected decision.

**Sign-off owner:** drawEM project owner (`maxViolet`). Record approval here before S4-02.

- Chosen option and reason: pending.
- Evidence: reference PC, build identity, 100-invocation measurements, hook-delay samples, visible frame pacing, input/focus and mixed-DPI observations: pending.
- Dedicated WPF effect surface — chosen, rejected, or skipped, with reason: pending.
- Effects in the virtual-desktop drawing window — chosen, rejected, or skipped, with reason: pending.
- DirectComposition/Direct2D adapter — chosen, rejected, or skipped, with reason: pending.
- Decision on proceeding to S4-02, including any separate-thread or renderer proof, two-working-day fallback timeboxes, and the project owner's sign-off: pending. If no option meets the criteria, stop and revise the roadmap thresholds or scope before S4-02.
