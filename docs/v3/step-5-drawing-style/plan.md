# v3 / Step 5: implementation plan

**Task:** [configurable drawing style](task.md). **Acceptance:** [criteria](acceptance.md).

1. Pass the active snapshot's color and physical width to
   `Application/Drawing/DrawingSessionController.cs` when a new stroke begins.
   Preserve those values in the stroke; changing settings never recolors a
   completed stroke.
2. Update `Presentation/Drawing/StrokeRenderElement.cs` so both pen thickness
   and dot diameter use the stroke monitor's DPI conversion. Account for
   mixed-DPI monitors and avoid treating the overlay's one
   `TransformFromDevice` matrix as the DPI of every stroke.
   **Implemented differently:** the overlay is one Per-Monitor-V2 window, so
   that one matrix is the correct conversion on every monitor. See the
   [acceptance validation](acceptance.md#validation).
3. Test rendered line and dot dimensions at 100% and 150%, including strokes
   on monitors with different DPI. Test arbitrary valid HEX color and the
   1- and 20-pixel width boundaries; record any rasterization tolerance.
