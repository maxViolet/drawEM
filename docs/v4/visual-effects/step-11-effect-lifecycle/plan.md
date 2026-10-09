# v4 / S4-11: implementation plan

**Task:** [stop effects through app lifecycle](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add tray Stop effects and route it to controller cancellation without clearing drawings or stopping sound.
2. Connect S4-07 display-change handling to lifecycle cleanup. Handle session lock, suspend, app exit, and partial startup failure. Invalidate pending starts before destroying the surface.
3. Detach rendering callbacks, close windows, and log runtime failures; keep Sample failures visible in Settings.
4. Make cleanup safe to repeat and ensure unlock/resume does not replay an interrupted effect.

Record implementation evidence in [acceptance](acceptance.md) when this step is executed.
