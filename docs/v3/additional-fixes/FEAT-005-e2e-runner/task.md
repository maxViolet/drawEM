# FEAT-005: assisted end-to-end runner for desktop checks

**Status:** planned. **Package:** [v3 additional fixes](../README.md).

## Current behavior

Desktop behavior is verified by hand from markdown checklists:

- [v1 smoke test](../../../v1/SMOKE-TEST.md): tray, input blocking, drawing,
  clear, monitors, exit.
- [v2 sound business tests](../../../v2/SOUND-BUSINESS-TESTS.md): scenarios
  that need audible output and global input.
- [v3 Step 8 acceptance](../../step-8-publish/acceptance.md): separate manual
  results for tray/settings, storage/media recovery, real keys and layouts,
  DPI/monitors, audible sound, and v1/v2 regressions.

`dotnet test DrawEM.sln` runs unit and architecture tests only. Nothing
launches the published `DrawEM.App.exe` or sends real input to it.
`tools/RenderLagHarness` uses a fake mouse source, not real input.

The published app cannot be started with known settings without overwriting
the user's own: `JsonSettingsStore.DefaultDirectory`,
`ManagedSoundLibrary.DefaultDirectory`, and `AppFailureLog.DefaultPath` are
fixed under `%LOCALAPPDATA%\drawEM`, and `App.xaml.cs` uses them directly.

## Required behavior

### Data directory override

When the `DRAWEM_DATA_DIR` environment variable is set to a non-empty path,
the app uses that directory instead of `%LOCALAPPDATA%\drawEM` for
`settings.json`, `sounds\`, and `logs\app.log`. All three locations derive
from one root resolved once in the composition root. Without the variable,
behavior is unchanged. The override adds no command-line argument and no UI.

The single-instance mutex is not affected: a test instance and a normal
instance never run together, because both would install the same global
hooks.

### Runner

`tools/E2ERunner` is a C# console app in `DrawEM.sln`. It has no project
reference to `DrawEM.App` and declares its own P/Invoke. It tests the
published executable as a black box.

- **Executable.** `--exe <path>` selects the app; the default is
  `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`.
  The runner never builds. If the executable is missing, it exits with an
  error that prints the `dotnet publish` command from the README. Results
  record the path, file version, SHA256, and file timestamp.
- **Isolation.** Each run creates `%TEMP%\drawEM-e2e\<run-id>\`, copies
  `tools/E2ERunner/fixtures/settings.json` into a data directory there, and
  starts the app with `DRAWEM_DATA_DIR` pointing at it. A scenario may supply
  its own settings file, including a deliberately corrupt one. The runner
  refuses to start while any `DrawEM.App.exe` process is running.
- **Input.** Scenarios send keyboard and mouse input with `SendInput`. The
  runner never uses the `KeyboardHookEvents.NeutralKeyTag` extra-info value.
- **Verdicts.** Each scenario ends as Pass, Fail, or Unverified with a reason.
  Checks are automatic where the runner can observe the result:
  - A target window owned by the runner (button, text box, scrollable list)
    counts clicks, characters, and scroll events that reach it.
  - Screen capture samples the area along the sent pointer path for the
    stroke color (`#FF4500`) within a color tolerance, to detect a drawn or
    cleared stroke.
  - The runner checks process exit and new error entries in `app.log`.

  Checks the runner cannot observe ask the operator
  `Pass? [y/n/s(kip)] note:`: tray icon, `Alt+Tab`/`Win+Tab`, audible sound,
  DPI appearance, and v3 settings-window steps. No UI Automation is used.
- **Environment requirements.** A scenario declares what it needs: monitor
  count, mixed DPI, audio output. The runner detects the environment
  (`EnumDisplayMonitors`, `GetDpiForMonitor`, default audio endpoint). An
  unmet requirement marks the scenario `Unverified: <reason>` without running
  it. The results header lists the detected environment.
- **Abort.** The runner installs its own low-level keyboard and mouse hooks.
  Any event without `LLKHF_INJECTED` / `LLMHF_INJECTED` while input is being
  sent aborts the run; operator prompts pause this watch. Every exit path,
  including abort and `Ctrl+C` (`Console.CancelKeyPress`), releases all keys
  and mouse buttons the runner pressed and stops the app. The interrupted
  scenario is recorded as `Unverified: aborted by user input`.
- **Results.** Every run writes a markdown summary, screenshots, a copy of
  `app.log`, and operator notes to the run directory and prints its path.
  `--report <path>` also writes the summary to that path, with screenshots
  copied next to it and linked relatively; acceptance runs use it to store
  results in a step folder.

### Scenario catalog

`docs/E2E-SCENARIOS.md` is the single source of truth for scenarios. Each
entry has a stable ID (`E2E-<GROUP>-NN`), intent, steps, expected result,
verdict method (automatic or operator), and requirements. Groups:

1. `INPUT`: click, type, and scroll are blocked while drawing and pass after
   release (v1 smoke checks 4–6, 8).
2. `DRAW`: stroke color and width; clear removes strokes only on the monitor
   under the cursor; no new stroke until the shortcut is pressed again
   (v1 smoke checks 3, 9).
3. `MONITOR`: a stroke stops on entering another monitor and continues past
   an outside edge ([FIX-006](../FIX-006-single-screen-edge/task.md)).
4. `SOUND`: v2 sound scenarios with `Ctrl+Alt+1`…`Ctrl+Alt+8`; the operator
   confirms audible output.
5. `TRAY`: hidden startup, tray icon, absence from `Alt+Tab`/`Win+Tab`, exit
   (v1 smoke checks 1, 2, 10).
6. `SETTINGS`: v3 settings window, shortcut capture, `Alt+Shift`/`Ctrl+Shift`
   layout-switch prevention, storage/media recovery. Operator-guided only:
   the runner prepares the data directory and sends keys, the operator
   judges.

A new `tools/E2ERunner.Tests` project fails when a catalog ID has no
scenario implementation or an implementation has no catalog ID. It also
covers runner logic that needs no desktop: color tolerance, requirement
matching, and report formatting. CI runs it through `dotnet test DrawEM.sln`;
CI never runs the runner itself.

## Delivery

Each part is a separate PR on its own branch:

1. `FEAT-005.1-data-dir-override`: `DRAWEM_DATA_DIR` and unit tests for root
   resolution with and without the variable.
2. `FEAT-005.2-e2e-runner`: runner, fixture, isolation, abort hooks,
   environment detection, results and `--report`, the catalog,
   `E2ERunner.Tests` with the drift check, and one scenario end to end:
   `E2E-DRAW-01` (draw a stroke, verify by pixel sampling).
3. `FEAT-005.3` onward: one scenario group per PR, in the order of the
   catalog groups above.

## Acceptance

- [ ] With `DRAWEM_DATA_DIR` set, the app reads and writes `settings.json`,
  `sounds\`, and `logs\app.log` only under that directory; without it,
  `%LOCALAPPDATA%\drawEM` is used as before. Unit tests cover both.
- [ ] The runner exits with an error when the executable is missing or a
  `DrawEM.App.exe` process is already running, and never builds the app.
- [ ] A run leaves `%LOCALAPPDATA%\drawEM` unchanged.
- [ ] Physical keyboard or mouse input during a run aborts it; afterwards no
  key or mouse button stays logically pressed and the app process has exited.
  The same holds after `Ctrl+C`.
- [ ] A scenario with an unmet requirement is reported as
  `Unverified: <reason>`, and the results header lists the detected
  environment.
- [ ] Results record executable path, file version, SHA256, and timestamp;
  `--report` writes the summary and linked screenshots to the given path.
- [ ] `E2E-DRAW-01` passes on a Windows desktop and fails when the stroke is
  not drawn.
- [ ] `dotnet test DrawEM.sln` fails when `docs/E2E-SCENARIOS.md` and the
  scenario implementations disagree on IDs.
- [ ] Every catalog group 1–6 has implemented scenarios, and one recorded run
  covers all of them, with unrun checks marked Unverified.
- [ ] README links `docs/E2E-SCENARIOS.md` and documents how to run the
  runner next to the existing smoke-test link.

## Validation

Not started.
