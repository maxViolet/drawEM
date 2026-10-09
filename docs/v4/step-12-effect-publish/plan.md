# v4 / S4-12: implementation plan

**Task:** [publish and verify on Windows](task.md). **Acceptance:** [criteria](acceptance.md).

1. Run the automated suite and build/publish the self-contained Windows x64 app; record commands, results, commit, executable version and hash.
2. Exercise Settings draft/Save/Cancel/Sample, tray Stop, effect shortcuts, drawing and sound coexistence, monitor switching, focus/input pass-through, mixed DPI, Cursor effects at each monitor edge, and display/session lifecycle.
3. Measure 100 warm starts through the real shortcut path, cold starts separately, callback cost, visible pacing, hook delivery delay during animation and drawing, and resource counts after 100 replacement cycles on a recorded reference PC, separately for Monitor and Cursor effects. Define first appearance as hook event timestamp to first presented effect frame and record the method/error.
4. Check full-display screen sharing from a receiving device and separately record window-only sharing. Update user instructions and mark any unrun check unverified.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
