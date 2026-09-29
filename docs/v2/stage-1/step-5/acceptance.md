# Step 5 acceptance: call microphone routing

**Status:** not run. See the [task](task.md) and [setup plan](plan.md).

## Setup identity

- [ ] Record the actual published executable, commit, and local fixture changes.
- [ ] Record Windows, microphone/headphones, mixer version/endpoint names, browser, and Meet account/filter availability.
- [ ] Meet noise cancellation is enabled and recorded in the evidence.
- [ ] Record routing settings and the second participant or receiving-device setup.

## Virtual microphone before Meet processing

- [ ] Local capture of the virtual recording endpoint contains physical microphone voice alone.
- [ ] The same endpoint contains drawEM WAV and MP3 alone, using headphones to prevent acoustic pickup.
- [ ] Simultaneous voice and drawEM are both present in the capture.
- [ ] Other applications, system sounds, and received call audio are excluded.
- [ ] Replacement, same-sound restart, natural completion, and the ten-second cap remain correct in the mix.

## Local monitoring and Google Meet

- [ ] drawEM is audible in headphones; the mixer does not return the user's own voice there.
- [ ] Meet uses the verified virtual recording endpoint as microphone and physical headphones as speakers.
- [ ] A remote participant hears voice with noise cancellation enabled; separately record drawEM as audible, attenuated, or removed.
- [ ] Meet mute silences outgoing voice and any drawEM audible before mute; local monitoring continues. Mark the effect comparison unobservable if Meet already removes that effect completely.
- [ ] Unmute restores the outgoing route; record voice and effect observations separately.
- [ ] Received call audio does not cause feedback through the outgoing mix.

## Completion rule

Upstream voice-plus-drawEM mixing, exclusion of unrelated audio, headphone
monitoring, shared Meet input, and voice mute/unmute checks must pass. Perform
and document the Meet call with noise cancellation enabled. For effects
audible before mute, their mute/unmute checks must also pass.

Meet attenuation or removal of non-speech effects is an accepted limitation
after upstream checks pass. A comparison made unobservable by Meet must be
recorded as such without a fabricated pass. Missing voice, incorrect upstream
routing, or an unperformed call does not meet acceptance.

Record results in `docs/v2/STAGE-1-STEP-5-ACCEPTANCE-RESULTS.md` after execution.
Leave checks open until evidence exists. These results do not replace or
close [step 4 acceptance](../step-4/acceptance.md).
