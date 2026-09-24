# Step 7: acceptance pass plan

## Goal

Produce release evidence that the published drawEM executable delivers the
first-version behavior on the available Windows hardware. This step validates
the assembled application; it does not add product features or treat Win32
interactions as unit-test coverage.

## Scope

- Run the automated test suite and publish the self-contained `win-x64` build.
- Execute the manual acceptance checks against the published executable.
- Record the device context and the result of every check.
- Capture a reproducible failure before planning any corrective code change.

## Out of scope

- New shortcuts, settings, drawing tools, installer work, or elevation.
- Pixel-perfect comparison, automated input injection, or unit tests that
  claim to prove notification-area, hook, or click-suppression behavior.
- Fixing a failure during this acceptance pass without a separate approved
  plan.

## Preconditions

- Windows 10 or 11 x64 with permission to run the published executable.
- At least one ordinary underlying application with a button or other visible
  click action. Use a second monitor when one is available.
- A clean working tree, so acceptance evidence is not mixed with unrelated
  changes.

## Execution sequence

1. Run the automated gate.

   ```powershell
   dotnet test DrawEM.sln --no-restore
   dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64 --no-restore
   ```

   Stop here if either command fails. Record the command, exit code, and first
   actionable error; do not call the acceptance pass successful.

2. Run `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`.
   Confirm that no normal application window appears and the drawEM tray icon
   is visible.

3. Hold `Ctrl+Alt+Z`, move the pointer, and release the shortcut. Confirm a
   persistent orange 4 px stroke remains visible.

4. With `Ctrl+Alt+Z` held, click the prepared underlying control. Confirm its
   action does not execute. Release the shortcut and click the same control;
   confirm its action executes.

5. Switch windows. When more than one monitor is available, move between
   monitors and repeat the draw action. Confirm existing strokes remain
   visible throughout.

6. Press `Ctrl+Alt+X`. Confirm every completed stroke disappears.

7. Choose `Exit` from the tray menu. Confirm the overlay disappears, the
   shortcuts no longer affect the underlying application, and
   `DrawEM.App.exe` is absent from Task Manager.

8. Copy the results template in `../v1/SMOKE-TEST.md` into
   `S07-ACCEPTANCE-RESULTS.md` and complete every field. Use `Not tested`
   when hardware or an environment constraint prevents a check; do not record
   it as a pass.

## Evidence and decision rules

- **Pass:** each automated command succeeds and every applicable manual check
  passes.
- **Conditional pass:** applicable checks pass and each unavailable check is
  recorded as `Not tested` with its reason. This is not equivalent to complete
  multi-monitor or mixed-DPI coverage.
- **Fail:** an automated command fails or any applicable manual check fails.
  Preserve the observed behavior and stop before changing source code.

Record Windows version, monitor count, per-monitor scaling, executable path,
test and publish commands, and the check result. The accepted product
boundaries remain: behavior over elevated applications, exclusive fullscreen
games, protected surfaces, and monitor hot-plug is not guaranteed.

## Files expected to change

| File | Change |
| --- | --- |
| `S07-acceptance-pass-plan.md` | Add this procedure and evidence criteria. |
| `../v1/SMOKE-TEST.md` | Add the window and multi-monitor persistence check and a reusable results template. |
| `S07-ACCEPTANCE-RESULTS.md` | Create only when the acceptance run is executed. |

No application or test source file is expected to change in Step 7.

## Done when

- Automated test and publish results are recorded.
- Every manual check is recorded as `Pass`, `Fail`, or `Not tested`.
- A complete pass has no failed or untested applicable checks.
- Any failure has a concise reproduction record and a separate proposed fix
  plan before implementation begins.
