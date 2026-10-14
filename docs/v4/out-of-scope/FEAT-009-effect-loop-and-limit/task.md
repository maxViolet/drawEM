# v4 / FEAT-009: loop an effect and set its length limit per slot

**Status:** planned. **Source:** [v4 out of scope](../README.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).

## Current behavior

- An effect plays its Lottie file once and never loops
  ([v4 roadmap](../../visual-effects/ROADMAP-v4.md#effect-content-rules)).
- `EffectChannelController.MaxDuration` stops every effect 10 seconds after it
  starts. The limit is a constant; the Settings window does not show it.
- Pressing the shortcut of the running effect again stops it and restarts it
  from the beginning. Sample behaves the same way.

## Required behavior

Each effect slot gets two settings, shown on a second line under the effect
choice. Sound and empty slots do not show them.

```
F1 [Effect v][Confetti  v][Clear]  [Ctrl+1]  [Sample]
   [ ] Loop
   Max length: [-][10 s][+]  [ ] No limit
```

| Setting | Values | Default |
|---|---|---|
| Loop | on / off | off |
| Max length | 1–60 seconds, step 1 second | 10 seconds |
| No limit | on / off | off |

- The length stepper works like the line-width stepper (FEAT-006): a button is
  disabled only at its limit.
- While Loop is on, Max length and No limit are disabled and have no effect.
- While No limit is on, the Max length stepper is disabled.

### Playback

| Slot setting | Effect ends when |
|---|---|
| Loop off, limit N | The file ends or N seconds pass, whichever comes first |
| Loop off, No limit | The file ends |
| Loop on | Never by itself: the animation restarts from frame 0 at its end |

| Event | Loop off | Loop on |
|---|---|---|
| Shortcut pressed while the slot's effect is running | Restart from the beginning (unchanged) | Stop the effect |
| Shortcut pressed while nothing or another effect is running | Start (unchanged) | Start |
| Sample pressed | Play once from the beginning (unchanged) | Start; the button text changes to `Stop`, and pressing it stops the effect |

All other stop rules stay as they are: another effect started, Tray > Stop
effects, the cursor entering another monitor, a display topology change,
session lock, suspend, Settings Save, Settings Cancel/close (for a Sample), and
app exit.

### Settings file

Schema version 3 adds two fields to an effect action:

```json
{ "type": "effect", "effectId": "confetti", "shortcut": "Ctrl+Alt+2", "loop": false, "maxSeconds": 10 }
```

- `loop`: boolean.
- `maxSeconds`: integer 1–60, or `null` for No limit.
- Schema 1 and 2 files load with `loop: false` and `maxSeconds: 10`, so saved
  slots keep today's behavior.
- A file with `maxSeconds` outside 1–60 fails validation like any other
  invalid value.

### Documentation

- The [v4 roadmap](../../visual-effects/ROADMAP-v4.md) rule "plays once and
  never loops" changes to describe the per-slot Loop and Max length settings.
  The test that fails on a built-in file longer than 10 seconds stays.
- `Infrastructure/Settings/README.md` documents schema version 3.
