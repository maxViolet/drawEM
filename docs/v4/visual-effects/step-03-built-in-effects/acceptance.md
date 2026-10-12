# v4 / S4-03: acceptance criteria

**Task:** [play built-in Lottie effects](task.md). **Plan:** [implementation](plan.md).

## S4-03.1: speed gate

- [x] The probe's measured render path plays a Lottie file through the Skottie player, not the S4-01 code-drawn shape, and its report names the placement and window bounds.
- [x] Skia draws inside the measured `CompositionTarget.Rendering` handler into the effect's own `WriteableBitmap`; `SKElement` is not used.
- [x] Render callback p95, frame interval p95, dropped frames, hook delay p95 and maximum, and resource counts are recorded for a Monitor effect and a Cursor effect against the roadmap targets. See [S4-03.1 results](#s4-031-results).
- [x] `LayerDependencyTests` passes with `SkiaSharp` allowed in Presentation only.
- [x] If the Monitor effect missed a target, the half-resolution result is recorded; if that also missed, the roadmap is revised to Cursor effects only with the project owner's confirmation before S4-03 continues. Monitor missed render callback p95 (4.082 ms); half resolution passed it (1.613 ms). Frame interval is covered by the revision below, not by the Cursor-only fallback.
- [x] If the Cursor effect missed a target, S4-03 stops: no fallback exists, because the Cursor window is already the smallest surface. The result goes to the project owner, who revises the roadmap targets or scope; the revision and the owner's confirmation are recorded before S4-03 continues. Cursor missed only the frame-interval targets; the owner revised them on 2026-10-12 (see [revision](#target-revision)).
- [x] S4-03 starts only when each placement meets every target, or when the recorded roadmap revision with the owner's confirmation covers each missed target.

## S4-03: built-in effects

- [x] Confetti covers the monitor and focus ring appears centered on the captured cursor point; both start, animate once, and leave no residual pixels or active frame subscription. Desktop screenshots, see [desktop checks](#published-app-desktop-checks). The frame subscription is released on completion, failure, and Dispose by code (`LottiePlayback.Unsubscribe`); the S4-03.1 probe measured 0 subscriptions left with the same pattern. It is not counted in the production build.
- [x] The focus ring stays at its captured point while the pointer moves within the monitor, and is clipped, not shifted, at each monitor edge. Checked on screen at the right edge, the bottom-right corner, and the top-left corner, and with the pointer moved away during the ring; bottom and left edges alone are covered by `EffectCanvasPlacementTests` only.
- [x] Replacement across placements and Stop clear old visuals; animation failures clear partial visuals and leave drawing and sound usable. Confetti -> focus ring replacement checked on screen. Stop, failure, and release paths: `EffectChannelControllerTests` with a fake surface; `LottiePlayback` raises `Failed` and the channel disposes the window. A real render failure was not provoked on screen.
- [x] Automated tests check that every built-in file loads, lasts no more than 10 seconds and no more than its specified length (confetti 3 s, focus ring 2 s), and has a source and license record; and check Cursor surface bounds, clipping, negative coordinates, and 100%/150%/200% scaling, and Monitor cover scaling for 16:9, 16:10, 21:9, and portrait monitors.
- [ ] Windows visual checks with the development-only trigger are recorded separately, including that neither effect flashes more than three times in any one second. Shape and placement checks ran through real shortcuts (the probe trigger is removed). Unverified: watching each effect for flashes. File analysis by the sourcing check found peak mean luminance 1% (confetti) and one 300 px cyan pulse per play (focus ring).
- [x] Carried over from [S4-06](../step-06-effect-shortcuts/acceptance.md#scope): `App.xaml.cs` passes `EffectChannelController.Start` to `GlobalShortcutAdapter`, and a bound shortcut shows each effect on screen, with a Cursor effect centered on the key-down cursor point.
- [x] Carried over from S4-06: the S4-01 probe is removed. `EffectSurfaceProbe.cs`, `EffectProbeHotkeys.cs`, `EffectProbeInputLoad.cs`, and the `DRAWEM_EFFECT_PROBE` wiring in `App.xaml.cs` no longer exist, and a search of `src/` for `DRAWEM_EFFECT_PROBE` finds nothing.

## Implementation

- `Presentation/Effects/BuiltIn/`: `confetti.json` (3240x3240, 2.75 s) and `focus-ring.json` (300x300, 1.02 s) from LottieFiles, unchanged, embedded under their catalog resource names. `SOURCES.md` records source, author, and license (Lottie Simple License; the files keep that license, not the drawEM license).
- `Presentation/Effects/BuiltInEffectAnimations.cs`: parses every catalog file once at startup. A file that fails to parse disables only its effect: `Show` throws `EffectPlaybackException` and the channel logs it.
- `Presentation/Effects/EffectRenderLayout.cs`: window bounds, bitmap size, and canvas destination per placement. Monitor renders at half resolution (`MonitorRenderScale = 0.5`), the fallback the owner chose after S4-03.1.
- `Presentation/Effects/LottieEffectSurface.cs`, `LottiePlayback.cs`: the `IEffectSurface` implementation. One `EffectSurfaceWindow` per instance; each `Rendering` callback draws the Skottie frame for the monotonic time since start into the bitmap; the callback is subscribed only while playing; `Completed` at the animation's end, `Failed` on a draw error.
- `Infrastructure/Effects/EffectFailureLog.cs`, `LoggingEffectFailureReporter.cs`: failures go to `%LOCALAPPDATA%\drawEM\logs\effect.log` on the background log writer, like sound failures.
- `App.xaml.cs`: composes the channel with the surface and passes `Start` to `GlobalShortcutAdapter`; the tray cleanup stops effects, then releases animations and the log.
- Removed: the S4-01/S4-03.1 probe (`EffectSurfaceProbe`, `EffectProbeMode`, `EffectProbeAnimations`, probe animations, `EffectProbeHotkeys`, `EffectProbeInputLoad`, `Infrastructure/Effects/NativeMethods.cs`, `tools/EffectProbeAnimations`, `Win32KeyboardHookSource.EventObserved`, probe-only native imports and window members) and its tests.

## Validation

- S4-03.1 automated checks, 2026-10-10, branch `S4-03.1-lottie-spike`:
  - `dotnet build DrawEM.sln -warnaserror`: succeeded, no warnings.
  - `dotnet test DrawEM.sln --no-build`: 555 passed, 0 failed. New tests cover cover scaling for 16:9, 16:10, 21:9, and portrait monitors; Cursor bounds at 100%/125%/150%/200%, clipping at the bottom-right edge and at the top-left corner of a monitor at negative coordinates; the probe mode variable and the Cursor mode of `Ctrl+Alt+F11`; parsing of both probe animations, one per placement; and `SkiaSharp` allowed in Presentation only.
  - `dotnet publish src/DrawEM.App -p:PublishProfile=win-x64`: succeeded; `libSkiaSharp.dll` is in the output.
- S4-03.1 benchmarks: run 2026-10-12; see [S4-03.1 results](#s4-031-results).
- S4-03 automated checks, 2026-10-12, branch `S4-03-built-in-effects`:
  - Test first: `BuiltInEffectAnimationsTests`, `EffectRenderLayoutTests`, and `EffectFailureLogTests` first failed to build for the expected reason: `BuiltInEffectAnimations`, `EffectRenderLayout`, `EffectFailure`, and `EffectFailureLog` did not exist.
  - `dotnet build DrawEM.sln -warnaserror`: succeeded, no warnings.
  - `dotnet test DrawEM.sln`: 593 passed, 0 failed (the removed probe tests no longer count).
- Published-app desktop checks: see below.

## S4-03.1 results

Run 2026-10-12 by the agent with the owner's permission, probe build `1.0.0+00a639f` (SHA-256 `D00AAB5D…CB3D2`), on Windows 10 Pro 26H2 build 26300, Intel Core Ultra 7 155H, Intel Arc and NVIDIA RTX 4070 Laptop GPUs, one monitor (0, 0, 1920, 1200) at 100% and 240 Hz. Hotkeys were sent with `keybd_event`; the probe's own input load ran as documented. Reports: `%LOCALAPPDATA%\drawEM\effect-probe\effect-probe-20261012-090737.md` (monitor), `-090826.md` (cursor), `-090957.md` (monitor-half).

| Mode | Render callback p95 | Frame interval p95 | Max interval | Dropped (> 1.5 intervals at 240 Hz) | Hook delay p95 / max | First appearance p95 |
|---|---|---|---|---|---|---|
| Monitor | 4.082 ms (fail) | 12.997 ms (3.12 intervals) | 23.5 ms | 54.8 % | 1.365 / 55.4 ms | 74.0 ms |
| Monitor, half resolution | 1.613 ms | 13.269 ms (3.19 intervals) | 17.6 ms | 59.4 % | 0.997 / 45.0 ms | 54.5 ms |
| Cursor (240x240) | 0.478 ms | 10.213 ms (2.45 intervals) | 19.8 ms | 38.8 % | 0.224 / 36.8 ms | 38.8 ms |

All other gates passed in every mode: hook events received, 0 windows and 0 rendering subscriptions left, handle/GDI/USER counts within 5 %, foreground unchanged, window bounds exact, 0 failures. An earlier Monitor run on 2026-10-11 (build `7658bef`, same PC) gave the same picture: render p95 4.413 ms, frame interval p95 3.29 intervals.

### Target revision

Decided by the project owner on 2026-10-12:

- Frame interval: p95 within 25 ms, and a frame is dropped when its interval exceeds 25 ms (at most 1 %), instead of 1.5 display refresh intervals. Rationale: the Cursor effect drew in 0.26 ms median and still missed at 240 Hz, so the refresh-relative limit measured WPF composition rate, not effect cost. 25 ms equals the old limit at 60 Hz. Under it every mode passes with 0 dropped frames (largest interval 23.5 ms).
- Monitor effects render at half resolution, the pre-approved fallback, which brings render callback p95 under 4 ms. No contract change.

## Published-app desktop checks

2026-10-12, published build of branch `S4-03-built-in-effects` (`dotnet publish src/DrawEM.App -p:PublishProfile=win-x64`), same PC, one monitor at 100%. Effects were bound to Ctrl+Alt+1 (confetti) and Ctrl+Alt+2 (focus ring) in a temporary schema 2 settings file; the owner's settings were restored afterwards (SHA-256 unchanged). Keys and cursor moves were sent with `keybd_event` and `SetCursorPos`; screens were captured with `Graphics.CopyFromScreen`.

- Confetti covers the monitor above other windows at 0.5 s; at 3.6 s nothing remains.
- Focus ring at (1000, 300): disc at 0.35 s, ring at 0.55 s, centered on the point.
- Pointer moved from (700, 400) to (1300, 800) 150 ms after the press: the ring stayed at (700, 400).
- Ring at (1910, 600), (1910, 1190), and (5, 5): clipped at the right edge, the bottom-right corner over the taskbar, and the top-left corner; the canvas was not shifted.
- Confetti, then focus ring 0.6 s later: the confetti was gone and the ring was shown; 1.5 s later nothing remained.
- In one capture, a Task Manager window in front covered the ring at (600, 500). The cause (Task Manager's always-on-top or elevation) was not investigated.
- Unverified: flashing, drawing and sound alongside an effect, mixed-DPI and second-monitor appearance, and 100 restart cycles (S4-12).
