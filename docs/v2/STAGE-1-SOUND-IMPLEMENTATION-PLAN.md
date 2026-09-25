# Stage 1 implementation plan: sound command

**Status:** Step 1 implemented; Steps 2 through 4 not started.

**Source:** [ROADMAP-v2.md](ROADMAP-v2.md), Stage 1, and the proposed screen-action model in [ARCHITECTURE.md](../ARCHITECTURE.md).

Each step has a task description, implementation plan, and acceptance criteria:

| Step | Task | Plan | Acceptance |
| --- | --- | --- | --- |
| 1. Command and code configuration | [Task](stage-1/step-1/task.md) | [Plan](stage-1/step-1/plan.md) | [Acceptance](stage-1/step-1/acceptance.md) |
| 2. Global sound channel | [Task](stage-1/step-2/task.md) | [Plan](stage-1/step-2/plan.md) | [Acceptance](stage-1/step-2/acceptance.md) |
| 3. Sound shortcuts | [Task](stage-1/step-3/task.md) | [Plan](stage-1/step-3/plan.md) | [Acceptance](stage-1/step-3/acceptance.md) |
| 4. Publish and verify | [Task](stage-1/step-4/task.md) | [Plan](stage-1/step-4/plan.md) | [Acceptance](stage-1/step-4/acceptance.md) |

Use a red-to-green TDD cycle for testable behavior in steps 1 through 3: one failing behavior test, the minimum implementation, then the next behavior. Step 4 records automated results and separate manual acceptance evidence. Passing unit tests does not complete manual acceptance.

## Goal and scope

Play a code-configured WAV or MP3 file through one global sound channel when an assigned `Ctrl+Alt+1` through `Ctrl+Alt+8` shortcut is pressed. Sound remains independent of drawing and of the monitor under the cursor. Preserve `Ctrl+Alt+Z` drawing, `Ctrl+Alt+X` monitor-scoped clearing, and input suppression while drawing.

Each sound plays once, ending at the file's natural end or ten seconds after playback starts, whichever comes first. A new request stops and replaces the active sound; requesting the same sound again restarts it. A held shortcut starts only once and can start again after release and another press.

Settings UI, media import and library, `Sample`, video, effects, and editable drawing shortcuts belong to later stages.

## 1. Define the command and configuration

1. Add a sound identifier and play command in `src/DrawEM.App/Application/Sound/`. Keep these types independent of WPF, Win32, paths, and monitor state.
2. Add one code-owned mapping in `src/DrawEM.App/Infrastructure/Sound/` from the eight sound slots to local external file paths. An unassigned slot produces no command. Document where to edit the assignments for a local build. Do not copy files into the application or a managed library.
3. Define playback-failure logging under `%LOCALAPPDATA%\drawEM\logs\sound.log`. Record time, requested slot or sound identifier, configured path, and failure reason. A logging failure must not terminate drawing or the tray app.
4. Test command resolution and unassigned slots without WPF controls or real audio hardware.

**Done when:** one mapping owns the slot assignments, and the command can be tested independently of Windows playback.

## 2. Implement the global sound channel

1. Add a playback port and a Windows adapter in `src/DrawEM.App/Infrastructure/Sound/` that can start and stop WAV and MP3 files and report natural completion and playback failure. Keep the playback engine behind the port so controller tests need no audio device.
2. Add a controller in `src/DrawEM.App/Application/Sound/` that owns at most one active player. Stop and dispose the previous player before starting another request, including a request for the same sound.
3. Start a new ten-second deadline for every playback attempt. On natural completion or timeout, stop and dispose the active player. Identify each attempt so a completion, failure, or timeout from an older attempt cannot stop its replacement.
4. Treat missing files, invalid media, decode errors, and output-device failures as recoverable playback failures. Log them locally and keep drawing and the tray app running.
5. Dispose the active player and cancel its deadline on application exit.

**Done when:** fake-player tests cover replacement, same-sound restart, natural completion, ten-second timeout, stale callbacks, recoverable failure, and disposal on exit.

## 3. Connect the shortcuts

1. Extend `src/DrawEM.App/Infrastructure/VirtualKeys.cs` and `src/DrawEM.App/Infrastructure/Drawing/GlobalShortcutAdapter.cs` to recognize assigned `Ctrl+Alt+1` through `Ctrl+Alt+8` slots using the existing keyboard hook. Detect the first key-down of each press; ignore repeated key-down events until release.
2. Resolve a slot to one sound command and queue playback outside the keyboard-hook callback through the existing Dispatcher path. Keep sound handling independent of cursor position and monitor bounds.
3. Allow sound commands during an active drawing stroke. Preserve the existing synchronous drawing input gate and suppression for the application underneath the overlay. Pressing a sound shortcut must not end the stroke or change `Ctrl+Alt+X` clearing behavior.
4. Test first press, auto-repeat, release and repress, unassigned slots, queued playback, and playback during an active stroke. Retain the existing drawing shortcut tests.

**Done when:** each assigned shortcut starts exactly one sound per press and drawing behavior remains unchanged.

## 4. Wire lifecycle, publish, and verify

1. Create the sound configuration, adapter, logger, and controller in `src/DrawEM.App/App.xaml.cs`. Include sound disposal in normal tray exit and startup-failure cleanup.
2. Run `dotnet test DrawEM.sln`. Publish the self-contained Windows x64 app with `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64` after closing any running copy. Use the existing profile's `PublishSingleFile=false` setting.
3. On the user's Windows desktop, manually test configured WAV and MP3 shortcuts while another app has focus: audible start, replacement, same-sound restart, natural completion, and the ten-second cutoff.
4. While drawing, start a sound and confirm the stroke continues and the underlying app still receives no click, scroll, or typed input. Move the cursor between monitors and confirm sound continues.
5. Test a missing path and invalid media file, inspect the local log, and confirm drawEM remains usable. Exit while sound plays and confirm playback stops. Repeat the [v1 smoke test](../v1/SMOKE-TEST.md).
6. Record the tested executable, Windows setup, sound files, automated results, manual pass/fail results, and any limitations in a separate Stage 1 acceptance-results document.

**Done when:** the published app passes the sound checks and v1 drawing checks on the tested Windows setup. Unit tests do not establish audible output, global hook behavior, input suppression, or multi-monitor behavior.

## Expected file scope

| Area | Expected changes |
| --- | --- |
| `src/DrawEM.App/Application/Sound/` | Sound command, controller, playback port |
| `src/DrawEM.App/Infrastructure/Sound/` | Code assignments, Windows player, local logger |
| `src/DrawEM.App/Infrastructure/VirtualKeys.cs` | Sound-slot key codes |
| `src/DrawEM.App/Infrastructure/Drawing/GlobalShortcutAdapter.cs` | Sound shortcut recognition and dispatch |
| `src/DrawEM.App/App.xaml.cs` | Composition and disposal |
| `tests/DrawEM.Tests/Application/Sound/` | Controller behavior with fake playback |
| `tests/DrawEM.Tests/Infrastructure/Drawing/` | Shortcut behavior and drawing regressions |
| `docs/v2/` | Stage 1 acceptance results after the manual run |

No production change is required in `Presentation/Sound/` for this stage.
