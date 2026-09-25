# Stage 1 / Step 4: acceptance criteria

**Task:** [publish and verify](task.md). **Plan:** [implementation and verification](plan.md).

## Automated gate

- [ ] `dotnet test DrawEM.sln` passes, including Step 1–3 behavior and lifecycle tests.
- [ ] The self-contained Windows x64 publish succeeds with the existing profile and `PublishSingleFile=false`.

## Manual gate on the user's Windows desktop

- [ ] With another app focused, assigned WAV and MP3 shortcuts produce audible playback once per press, without auto-repeat.
- [ ] Another sound replaces the current sound; invoking the same sound again audibly restarts it.
- [ ] A short file ends naturally; a file longer than ten seconds stops by the ten-second deadline.
- [ ] Sound continues while the cursor moves between monitors.
- [ ] Sound can start during an active drawing stroke without ending it; click, typing, and scroll input remain blocked from the underlying app until drawing ends.
- [ ] Missing and invalid files produce local log records without stopping drawEM or drawing.
- [ ] Tray Exit during playback stops sound, releases resources, and ends the process.
- [ ] All applicable [v1 smoke-test checks](../../../v1/SMOKE-TEST.md) pass on the tested setup.

Record each result and its evidence in a separate Stage 1 acceptance-results file. Stage 1 is complete only when both gates pass; unit tests alone cannot establish audible output or global input behavior.
