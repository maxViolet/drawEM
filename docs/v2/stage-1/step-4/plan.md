# Stage 1 / Step 4: implementation and verification plan

**Task:** [publish and verify](task.md). **Acceptance:** [criteria](acceptance.md).

1. Compose the Step 1 configuration and logger, Step 2 sound channel, and Step 3 shortcut routing in `src/DrawEM.App/App.xaml.cs`. Add sound disposal to normal tray exit, `OnExit`, and startup-failure cleanup. Use focused lifecycle tests for this wiring.
2. Close any running drawEM copy. Run `dotnet test DrawEM.sln`, then `dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64`. The existing profile publishes self-contained Windows x64 with `PublishSingleFile=false`.
3. Run the published executable on the user's Windows desktop. With another app focused, test assigned WAV and MP3 slots, replacement, same-sound restart, natural completion, and the ten-second cap. Use known files shorter and longer than ten seconds.
4. During a drawing stroke, trigger sound and check that the stroke continues while clicks, typing, and scrolling do not reach the underlying app. Move between monitors while sound plays and check that sound continues.
5. Request a missing file and an invalid file. Check the local log and verify drawEM still draws. Exit during playback and verify sound stops and the process exits. Repeat every item of the [v1 smoke test](../../../v1/SMOKE-TEST.md).
6. Write a separate Stage 1 results file in `docs/v2/` with date, Windows version, executable path, monitor count and scaling, test media duration, automated command/results, manual pass/fail evidence, and any untested cases. Do not mark manual checks passed from unit-test output.

If a manual check fails, record the failure, fix the behavior, and repeat the affected check before acceptance.
