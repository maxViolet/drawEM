# Stage 1 / Step 1: implementation plan

**Task:** [command and code configuration](task.md). **Acceptance:** [criteria](acceptance.md).

1. Define a sound identifier and play command in `src/DrawEM.App/Application/Sound/`. Keep file paths, playback, WPF, and hook details out of the command.
2. Define the eight fixed sound slots and one slot-to-identifier/path mapping in `src/DrawEM.App/Infrastructure/Sound/`. Allow empty assignments and document where a developer sets local paths before publishing a build.
3. Define failure logging at `%LOCALAPPDATA%\drawEM\logs\sound.log`. Record timestamp, slot or sound identifier, configured path, and failure reason; logging failure must not end the tray application.
4. Use one red-to-green cycle per observable mapping behavior: assigned slot resolves to one command; unassigned slot resolves to none. Test through the command-resolution boundary without audio hardware.

No sound playback or keyboard-hook integration is required for this step.
