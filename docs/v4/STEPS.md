# v4: simple implementation steps

**Status:** S4-01 is done by owner decision, with its measurement gates unverified; later steps are pending.
**Contract:** [v4 roadmap](ROADMAP-v4.md). Its product rules, technical boundaries,
and acceptance requirements apply to every step below.

Build built-in effects first. Imported video comes later. Follow these steps
in order; each step assumes the previous ones are complete. Keep incomplete
paths internal until their settings and lifecycle integration is ready.

## Part 1: make effects work

### S4-01: prove the effect window

**Do:** Create a small experiment that shows an animated shape in a transparent
window on the monitor containing the cursor. Use the real drawing overlay
alongside it. The experiment is the only step that precedes the production
controller and may use temporary invocation code. A development-only trigger
may also be used through S4-03 in a locally published Windows x64 probe build,
then removed when shortcuts arrive in S4-06. The proposed probe lives in
`src/DrawEM.App/Presentation/Effects/EffectSurfaceProbe.cs`; `App.xaml.cs`
enables its trigger only with `DRAWEM_EFFECT_PROBE=1`.

**Done when:** On Windows, the shape appears above drawings without stealing
focus or blocking clicks in another application. Record monitor clipping,
mixed-DPI behavior, task-switcher behavior, hook delivery delay during drawing,
and preliminary animation measurements. Record whether WPF is suitable before
continuing; the first-appearance release target requires the later shortcut path.

### S4-02: control one running effect

**Do:** Add an application controller with Start and Stop operations, a unique
ID for each invocation, a clock, and a replaceable rendering port. Starting an
effect replaces the previous one. Add completion, failure, and the ten-second cap.

**Done when:** Tests with a fake renderer and clock show that replacement works,
Stop is safe to repeat, and an old completion or timeout cannot stop a new effect.
Every ending path releases the instance and its resources.

### S4-03: add the two animations

**Do:** Implement confetti (3 seconds) and a focus ring (2 seconds) behind the
rendering port. The ring stays at the cursor position captured when invoked.
Use elapsed time and bounded drawing data; disconnect the frame callback when idle.

**Done when:** Both effects finish and disappear, can be replaced midway, and
leave no render subscription running. Check their appearance and drawing/audio
coexistence on Windows. Keep failures local to the effect.

### S4-04: add effects to the action model

**Do:** Add EffectAction and stable preset IDs to the existing eight-slot model.
Keep one action and one shortcut per occupied slot. Validate known presets and
shortcut conflicts through the existing snapshot validation.

**Done when:** Model tests accept both presets, reject unsupported effect IDs,
and detect conflicts with sound, drawing, and clear shortcuts. Existing sound
slots retain their behavior.

## Part 2: connect effects to the app

### S4-05: save and load effect assignments

**Do:** Add schema 2 with an effect action payload. Read schema 1 without changing
its sound assignments. Back up the old settings before the first migration Save;
write the new format only on explicit Save.

**Done when:** Round-trip, migration, and failure tests preserve all slots and
recoverable files. Unsupported types/IDs produce a useful error. Loading never
rewrites settings. Document how to restore the backup for an older app version.

### S4-06: launch effects from shortcuts

**Do:** Extend prepared slot commands and bindings to dispatch Sound or Effect.
Capture the invocation cursor position and configuration generation. Keep
rendering outside the hook callback and preserve existing capture/suppression rules.

**Done when:** A configured effect starts once per press; holding the shortcut
does not repeat it, and a fresh press restarts it. Sound remains independent.
Tests cover dispatch, captured position, stale configuration requests, AltGr,
capture mode, and paired key suppression.

### S4-07: stop on monitor changes

**Do:** Observe monitor transitions even when drawing is inactive. Stop the
effect when the cursor enters another monitor. Preserve transition ordering
and invalidate queued starts that belong to an earlier monitor epoch. Add a
display-change listener, monitor identity/topology generation, surface rebuild,
and an active-frame cursor check. Avoid per-move UI dispatch.

**Done when:** A -> B -> A movement stops the original effect without restarting
it. Movement inside one monitor and reaching an outside desktop edge do not
stop it. Test queued-start races and topology changes; confirm the behavior on
two monitors.

### S4-08: select an effect in Settings

**Do:** Add Sound/Effect selection and an effect preset list to existing Actions
rows. Preserve shortcuts when changing type; propose the slot default when
filling an empty slot. Clearing a slot clears its binding.

**Done when:** Draft selection and validation work without changing active
assignments. Cancelling a Sound -> Effect edit preserves the saved sound and
its managed file. Use the model/storage from earlier steps; successful runtime
application is completed in S4-10.

## Part 3: finish the user flow and release

### S4-09: preview with Sample

**Do:** Route the selected draft preset through the same effect controller as
shortcuts. Associate each Sample with its editor and instance ID. Preview on
the monitor containing the cursor without saving.

**Done when:** Sample plays at normal desktop size and keeps Settings focused.
Closing/cancelling the editor stops its own current Sample. It cannot stop a
newer shortcut effect or another Sample. Missing/conflicting draft shortcuts
do not prevent preview. Test these ownership cases explicitly.

### S4-10: apply settings safely

**Do:** Extend the successful Save sequence to invalidate queued effect starts
and remove the running effect before publishing the new snapshot and bindings.
Retain the existing sound start-hold and drawing interruption behavior.

**Done when:** A user can select, save, invoke, and reload an effect assignment.
No pre-Save effect starts afterwards. Validation or persistence failure leaves
the previous runtime configuration and actions intact. Tests cover queued
invocations, active Samples, and sound-library cleanup after type changes.

### S4-11: connect stop and lifecycle events

**Do:** Add tray Stop effects. Connect the S4-07 display-change handling to
final cleanup, plus lock, suspend, shutdown,
and partial startup failure to effect cancellation and surface cleanup. Report
Sample failures in Settings and runtime failures through local logging.

**Done when:** Each event cancels pending starts and removes visible effects;
unlock/resume never restarts them. Stop effects preserves global sound and
drawings. Repeated cleanup is safe and leaves no effect windows or callbacks.
Test event handling and separately exercise real desktop lifecycle events.

### S4-12: publish and verify

**Do:** Run the automated suite, publish Windows x64, and execute the roadmap's
desktop/performance acceptance checks. Update user instructions for assignment,
Sample, monitor switching, Stop effects, and settings rollback.

**Done when:** Evidence identifies the exact published build and tested hardware.
Drawing, sound, shortcuts, focus, mixed DPI, screen sharing, and lifecycle checks
are recorded separately from automated results. The 100-invocation latency and
resource and hook-delay checks meet the roadmap targets. Mark unrun cases
unverified. Complete
the v3 Step 8 baseline before releasing v4.

**First action:** start S4-01 with one animated shape and the existing drawing
overlay on the target Windows desktop. Estimated experiment time: 1-2 engineering days.
