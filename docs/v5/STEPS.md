# drawEM v5: steps

Follow these steps in order. Every step has `task.md`, `plan.md`, and
`acceptance.md`, as described in the [package structure](README.md#package-structure).

| ID | Outcome | Status | Done when |
|---|---|---|---|
| [S5-01](step-01-video-decoder-and-formats/task.md) | Prove the Windows playback path and choose the supported MP4 codec profiles | Planned | The supported profile and a positive/negative fixture matrix are recorded; all fixtures produce their expected result in the target Windows environment. |
| [S5-02](step-02-video-action-and-library/task.md) | Add video actions, persistence, managed import, settings, and Sample | Planned | Settings round-trip video actions and existing sound/effect data; import, assignment, save, cancel, and last-reference cleanup pass their gates. |
| [S5-03](step-03-fullscreen-video-playback/task.md) | Play a video on the active monitor with the agreed frame, layer, input, and audio behavior | Planned | Automated playback/surface gates pass, and the Windows surface check confirms full-monitor bounds, aspect fit, layer order, input pass-through, and audible embedded audio. |
| [S5-04](step-04-video-controls-and-lifecycle/task.md) | Connect shortcuts, tray stop, clear, monitor changes, lock/sleep, and shutdown | Planned | Each event in the playback table has an automated or explicitly manual result; stopping leaves sound and drawings unaffected except for the specified clear command. |
| [S5-05](step-05-video-publish/task.md) | Verify the published application and record release evidence | Planned | The exact published artifact passes the acceptance matrix, including OBS Display Capture and repeated start/stop resource checks; unavailable manual checks remain Unverified. |
