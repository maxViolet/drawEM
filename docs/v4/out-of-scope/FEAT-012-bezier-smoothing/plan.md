# v4 / FEAT-012: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Domain.** Add `StrokeSmoothing { None, Bezier }` to
   `Domain/Drawing/DrawingStyle.cs` and a `Smoothing` property to
   `DrawingStyle`. The default style uses `Bezier`.
2. **Settings file.** Bump `SettingsJson.CurrentSchemaVersion` to 4. Write
   `drawing.smoothing` as `"none"` or `"bezier"`. Read schema 1–3 files with
   `bezier`. Reject any other value. Update
   `Infrastructure/Settings/README.md`.
3. **Geometry.** Add a pure helper that turns stroke points into pieces:
   the first straight piece from the first point to the first midpoint, one
   quadratic piece per inner point, and the open tail to the last point. Test
   it without WPF.
4. **Renderer.** In `StrokeRenderElement`, choose the drawing by the stroke's
   `Style.Smoothing`:
   - `None`: keep the current `DrawLine` path unchanged.
   - `Bezier`, completed stroke: one `StreamGeometry` with the pieces from
     step 3.
   - `Bezier`, active stroke: each update adds a visual with the new finished
     pieces. The tail is a separate visual, removed and drawn again on each
     update, and removed when the stroke ends.
   - Keep the reclassification of the active stroke into a completed one: the
     finished pieces stay, the tail becomes the final piece.
   - Draw every piece with the stroke's round-cap pen. Colors are opaque, so
     the overlap of pieces at a join is invisible and the incremental drawing
     matches one geometry. Test it with a pixel comparison on the existing
     `DeviceSurface` test helper.
   - Keep the fallback for a stroke whose last point was never seen as
     active: remove the active visuals and draw the full stroke.
   - Compare `tools/RenderLagHarness` runs with `None` and `Bezier`.
5. **Settings UI.** Add the `Developer settings` tab to `SettingsWindow.xaml`
   after `Actions · 8 slots`, with a `Bezier smoothing` check box bound to a
   `SettingsViewModel` property. The draft goes through `SettingsEditor` like
   color and width; Restore defaults sets it to checked.
6. **Docs.** Mark this task implemented in [out of scope](../README.md) and
   record the evidence in [acceptance criteria](acceptance.md).
7. Run `dotnet test DrawEM.sln` and `graphify update .`.
