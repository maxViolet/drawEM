# Stage 1 / Step 2: implementation plan

**Task:** [global sound channel](task.md). **Acceptance:** [criteria](acceptance.md).

1. Define a playback port for starting, stopping, disposing, and reporting completion or failure. Put controller behavior in `src/DrawEM.App/Application/Sound/`; implement WAV and MP3 playback in `src/DrawEM.App/Infrastructure/Sound/`.
2. Add a controller that owns at most one playback attempt. On every request, stop and dispose the old attempt before starting the new one. Treat the same identifier as a restart.
3. Give each attempt its own ten-second deadline and generation token. Complete or time out only the current generation; cancel obsolete deadlines and ignore callbacks from disposed attempts.
4. Handle missing paths and media or device failures as recoverable events. Write the Step 1 failure record; keep drawing and tray lifecycle independent of sound failure.
5. Develop through red-to-green behavior slices using a fake player and controlled clock: first start, replacement, same-sound restart, natural completion, deadline, stale callbacks, failure, and exit disposal. Verify each behavior through the controller's public command and observable playback state or fake-player calls.

Real audible playback is verified in [Step 4](../step-4-publish/task.md).
