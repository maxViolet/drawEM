# v4: simple implementation steps

**Status:** S4-01 is done by owner decision, with its measurement gates unverified. S4-02 is done; later steps are pending.
**Contract:** [v4 roadmap](ROADMAP-v4.md). Its product rules, technical boundaries,
and acceptance requirements apply to every step below. Terms follow the
[glossary](../../../CONTEXT.md).

Build built-in Lottie effects with two placements: **Monitor** (covers the
monitor) and **Cursor** (fixed size, centered on the captured cursor point).
Imported effects and video come later. Follow these steps in order; each step
assumes the previous ones are complete. Keep incomplete paths internal until
their settings and lifecycle integration is ready.

## Part 1: make effects work

### S4-01: prove the effect window (done)

**Do:** Create a small experiment that shows an animated shape in a transparent
window on the monitor containing the cursor, alongside the real drawing overlay.
A development-only trigger in
`src/DrawEM.App/Presentation/Effects/EffectSurfaceProbe.cs`, enabled only with
`DRAWEM_EFFECT_PROBE=1`, may be used through S4-03 and is removed in S4-06.

**Done when:** Done on 2026-10-09 by owner decision: the dedicated WPF effect
surface is chosen. Its measurement gates are unverified; S4-03.1 and S4-12 measure them.

### S4-02: control one running effect (done)

**Do:** Add an application controller with Start and Stop operations, a unique
ID for each invocation, a clock, and a replaceable rendering port. An invocation
carries its effect ID and placement. Starting an effect replaces the previous
one, whatever either placement is. Add completion, failure, and the ten-second cap.

**Done when:** Tests with a fake renderer and clock show that replacement works,
including Monitor -> Cursor and Cursor -> Monitor, Stop is safe to repeat, and an
old completion or timeout cannot stop a new effect. Every ending path releases
the instance and its resources.

### S4-03.1: prove Lottie speed for both placements

**Do:** Add `SkiaSharp.Skottie` and allow `SkiaSharp` in Presentation only in
the layer dependency test. Change the S4-01 probe so its `Rendering` handler
draws a Lottie frame with Skia into the effect's own `WriteableBitmap`, not
through `SKElement`. Add a cursor-centered, clipped window next to the
monitor-sized one. Run the benchmark once per placement; the report names the placement.

**Done when:** Render callback p95, frame interval p95, dropped frames, and hook
delay are recorded for each placement against the roadmap targets. If Monitor
misses a target, measure half resolution.
If that also misses, stop and revise the roadmap to Cursor effects only, with
owner confirmation, before S4-03. Estimated time: half a day.

### S4-03: play the built-in effects

**Do:** Build the Monitor surface (cover scaling) and the Cursor surface (canvas
size in DIP, centered on the captured point, clipped to the monitor) behind the
rendering port. Add the built-in catalog with confetti (Monitor) and focus ring
(Cursor) as embedded Lottie files selected from LottieFiles, each with its
recorded source and license. Play each file once, from elapsed time; disconnect the frame callback when idle.

**Done when:** Both effects finish and disappear, can be replaced midway, and
leave no render subscription running. Tests check that every built-in file loads,
lasts no more than 10 seconds and no more than its specified length (confetti
3 s, focus ring 2 s), and has a license record, and check Cursor surface
bounds and clipping. Check appearance at monitor edges, no rapid flashing, and drawing/audio
coexistence on Windows. Keep failures local to the effect.

### S4-04: add effects to the action model

**Do:** Add EffectAction with a stable effect ID to the existing eight-slot model.
Keep one action and one shortcut per occupied slot. Validate known effect IDs
against the catalog and shortcut conflicts through the existing snapshot validation.

**Done when:** Model tests accept both effects, reject unsupported effect IDs,
and detect conflicts with sound, drawing, and clear shortcuts. Existing sound
slots retain their behavior. Placement is not part of the action.

## Part 2: connect effects to the app

### S4-05: save and load effect assignments

**Do:** Add schema 2 with an effect action payload holding only the effect ID.
Read schema 1 without changing its sound assignments. Back up the old settings
before the first migration Save; write the new format only on explicit Save.

**Done when:** Round-trip, migration, and failure tests preserve all slots and
recoverable files. Unsupported types/IDs produce a useful error. Loading never
rewrites settings. Document how to restore the backup for an older app version.

### S4-06: launch effects from shortcuts

**Do:** Extend prepared slot commands and bindings to dispatch Sound or Effect.
Capture the invocation cursor position and configuration generation. Keep
rendering outside the hook callback and preserve existing capture/suppression
rules. Remove the S4-01 probe trigger.

**Done when:** A configured effect of either placement starts once per press;
holding the shortcut does not repeat it, and a fresh press restarts it. A Cursor
effect appears at the cursor point captured at key-down. Sound remains independent.
Tests cover dispatch, captured position, stale configuration requests, AltGr,
capture mode, and paired key suppression.

### S4-07: stop on monitor changes

**Do:** Observe monitor transitions even when drawing is inactive. Stop the
effect, of either placement, when the cursor enters another monitor. Preserve
transition ordering and invalidate queued starts that belong to an earlier
monitor epoch. Add a display-change listener, monitor identity/topology
generation, surface rebuild, and an active-frame cursor check. Avoid per-move UI dispatch.

**Done when:** A -> B -> A movement stops the original effect without restarting
it. Movement inside one monitor and reaching an outside desktop edge do not
stop it; a Cursor effect does not move. Test queued-start races and topology
changes; confirm the behavior on two monitors.

### S4-08: select an effect in Settings

**Do:** Add Sound/Effect selection and an effect list, grouped under Monitor
and Cursor, to existing Actions rows. Preserve shortcuts when changing type;
propose the slot default when filling an empty slot. Clearing a slot clears its binding.

**Done when:** Draft selection and validation work without changing active
assignments. The list shows each effect under its placement. Cancelling a
Sound -> Effect edit preserves the saved sound and its managed file. Successful
runtime application is completed in S4-10.

## Part 3: finish the user flow and release

### S4-09: preview with Sample

**Do:** Route the selected draft effect through the same effect controller as
shortcuts. Associate each Sample with its editor and instance ID. Preview on
the monitor containing the cursor without saving.

**Done when:** Sample plays at normal desktop size and placement, and keeps
Settings focused: a Monitor effect covers the monitor and a Cursor effect
appears around the cursor. Closing/cancelling the editor stops its own current
Sample. It cannot stop a newer shortcut effect or another Sample.
Missing/conflicting draft shortcuts do not prevent preview. Test these ownership cases.

### S4-10: apply settings safely

**Do:** Extend the successful Save sequence to invalidate queued effect starts
and remove the running effect before publishing the new snapshot and bindings.
Retain the existing sound start-hold and drawing interruption behavior.

**Done when:** A user can select, save, invoke, and reload an effect assignment
of either placement. No pre-Save effect starts afterwards. Validation or
persistence failure leaves the previous runtime configuration and actions intact.
Tests cover queued invocations, active Samples, and sound-library cleanup after type changes.

### S4-11: connect stop and lifecycle events

**Do:** Add tray Stop effects. Connect the S4-07 display-change handling to
final cleanup, plus lock, suspend, shutdown, and partial startup failure to
effect cancellation and surface cleanup for both surfaces. Report Sample
failures in Settings and runtime failures through local logging.

**Done when:** Each event cancels pending starts and removes visible effects;
unlock/resume never restarts them. Stop effects preserves global sound and
drawings. Repeated cleanup is safe and leaves no effect windows or callbacks.
Test event handling and separately exercise real desktop lifecycle events.

### S4-12: publish and verify

**Do:** Run the automated suite, publish Windows x64, and execute the roadmap's
desktop/performance acceptance checks for both placements. Update user
instructions for assignment, placement, Sample, monitor switching, Stop effects,
and settings rollback.

**Done when:** Evidence identifies the exact published build and tested hardware.
Drawing, sound, shortcuts, focus, mixed DPI, Cursor effects at monitor edges,
screen sharing, and lifecycle checks are recorded separately from automated
results. The 100-invocation latency, resource, and hook-delay checks meet the
roadmap targets for each placement. Mark unrun cases unverified. Complete the
v3 Step 8 baseline before releasing v4.

**First action:** start S4-03.1: add `SkiaSharp.Skottie` and draw one Lottie frame in the S4-01 probe.
Estimated time: half a day.
