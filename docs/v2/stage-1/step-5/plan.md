# Step 5 plan: external microphone mixing

**Status:** approved approach; execution and manual verification pending.

**Task:** [Route sound into the call microphone](task.md).

## Routing

Use Voicemeeter Standard's physical input for the microphone and virtual
playback input for drawEM. Send both to its virtual recording output (bus B).
Send only drawEM to the headphone output (bus A).

| Source | Destination | Headphone monitoring | Outgoing microphone |
| --- | --- | --- | --- |
| Physical microphone | Voicemeeter hardware input | Off (A off) | On (B on) |
| drawEM | Voicemeeter virtual playback input | On (A on) | On (B on) |
| Meet received audio | Physical headphones | Direct | Excluded |
| Other apps and system sounds | Physical headphones | Direct | Excluded |

Keep Windows default playback on physical headphones. Select Voicemeeter's
virtual recording endpoint as the Meet microphone. Record actual endpoint
names from the installed version; playback and recording endpoints differ.

The vendor's [microphone and PC sound setup guide](https://voicemeeter.com/first-steps-connect-your-mic-and-mix-your-voice-with-any-pc-sound/)
describes physical/virtual inputs and A/B routing. This plan narrows that
setup to drawEM through per-app output assignment.

## Prerequisites

1. Identify the published executable, commit, and local sound-assignment
   changes. Have short WAV/MP3 files and a file longer than ten seconds.
2. Have a physical microphone, headphones, administrator access for mixer
   installation, and an opportunity to reboot.
3. Have Google Meet and a second participant or separate receiving device
   with headphones. Record browser and account details.
4. Check noise cancellation is available and enabled for the tested Meet
   account. If unavailable, record the unmet prerequisite rather than claim
   the agreed enabled-filter scenario was tested.

Voicemeeter is donationware. Installation requires administrator access and
a reboot; record its version and any licensing prompts. Use the
[official download and installation instructions](https://vb-audio.com/Voicemeeter/).

## Setup procedure

1. Record existing Windows default playback/recording devices, drawEM
   per-app output, and Meet input/output selections for restoration.
2. Install Voicemeeter Standard from the official vendor and reboot as
   instructed. Start it and record its version and endpoint names.
3. Set the mixer headphone output to physical headphones. Select the physical
   microphone on a hardware input; enable B and disable A there.
4. Enable A and B for the virtual playback input. Keep unrelated inputs
   disconnected or excluded from B.
5. Play a drawEM sound so its audio session appears in Windows Volume mixer.
   Assign that app's output to the Voicemeeter virtual playback endpoint.
   Keep default output, browser output, and other apps on physical headphones.
   Restart drawEM if needed and verify the assignment takes effect. See
   Microsoft's [per-app output selection instructions](https://support.microsoft.com/en-au/windows/hardware/audio/fix-app-audio-not-working-while-system-sounds-work-in-windows).
6. In Meet select the virtual recording endpoint as microphone and physical
   headphones as speakers. Keep noise cancellation enabled. Follow Google's
   [audio-device selection instructions](https://support.google.com/meet/answer/10409699?hl=en).

Per-app routing of the current drawEM player is a validation gate, not a
proven capability. If it fails, record the result and revise this plan before
implementing an adapter or switching the default output to mix all PC audio.

## Verification and evidence

1. Capture the virtual recording endpoint locally, before Meet processing.
   Check voice alone, drawEM alone, and simultaneous voice and drawEM using
   WAV and MP3. Check other app audio and received call audio are excluded.
2. Check drawEM is audible in headphones and the mixer does not return the
   user's own voice. Use headphones to prevent acoustic pickup.
3. Join a Meet call with the selected virtual microphone and enabled noise
   cancellation. Record remote voice/effect observations separately from
   the upstream recording.
4. Mute in Meet while speaking and playing drawEM. Check outgoing voice and
   any otherwise audible drawEM stop while local drawEM monitoring continues.
   Unmute and check the outgoing route resumes. If Meet already removes an
   effect completely, mark its audible mute comparison unobservable. Record
   shared input selection and voice mute evidence separately.
5. Check replacement, same-sound restart, natural completion, and the
   ten-second cap in the upstream mix. Step 4 owns the full drawing, input,
   lifecycle, and multi-monitor regression gate.

Google documents that [noise cancellation filters non-speech sounds](https://support.google.com/meet/answer/9919960?hl=en-GB).
Keeping it enabled is the agreed configuration. Suppression or attenuation
after a verified upstream mix is an accepted limitation. Missing upstream
drawEM, feedback, or unrelated audio in the mix is a setup failure.

Create `docs/v2/STAGE-1-STEP-5-ACCEPTANCE-RESULTS.md` after the manual run.
Record executable identity, local fixture differences, Windows/device
details, mixer/browser versions, Meet filter state, routing selections,
upstream recordings, remote observations, mute evidence, and limitations.
Label checks passed, failed, blocked, or unobservable. Unit tests, publish
success, and mixer meters alone do not establish manual acceptance.

## Restore the previous setup

1. Restore Meet and Windows device selections and drawEM per-app output from
   the recorded settings.
2. Stop Voicemeeter once no selected route depends on it.
3. If removing the mixer, follow vendor uninstall and reboot instructions.

## File and implementation scope

This step owns its task, plan, acceptance, roadmap/aggregate-plan links, and
future manual results document. No production code change or driver
development is planned. Installation and device changes are execution work;
creating these documents does not perform that setup.
