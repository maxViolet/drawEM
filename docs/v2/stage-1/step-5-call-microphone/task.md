# Step 5: route sound into the call microphone

**Status:** proposed; setup and manual acceptance have not run.

**Source:** [Stage 1 roadmap](../../ROADMAP-v2.md#5-route-sound-into-the-call-microphone).

## Task

Use Voicemeeter Standard as an external mixer so the selected call microphone
contains physical microphone voice and drawEM playback. Verify this in Google
Meet with headphones, without acoustic pickup from speakers.

The user hears drawEM in headphones without monitoring their own voice.
Other applications, system sounds, and received call audio stay outside the
outgoing mix. Meet microphone mute silences both outgoing sources.

Keep Meet noise cancellation enabled. Voice and drawEM share the same input;
there is no separate priority assigned to either source. Meet can attenuate
or remove non-speech sounds. This is an accepted interoperability limitation
after the mixed microphone signal before Meet processing has been verified.
Record actual call behavior; remote audibility of every effect is not guaranteed.

## Scope and dependencies

- Use the current published drawEM build and existing WAV/MP3 shortcuts.
- Install and configure Voicemeeter Standard during execution of this step.
- Validate Windows per-app output routing with the current WPF player.
- Configure Google Meet and perform separate upstream and call checks.
- No custom virtual audio driver, internal mixer, or settings UI is planned.

Depends on sound playback and shortcuts from steps 1–3 and an identified
published executable from [step 4](../step-4-publish/task.md). This step does not
close step 4's outstanding manual checks.

## Documents

- [Setup and verification plan](plan.md)
- [Acceptance criteria](acceptance.md)
