# v4 / FEAT-010: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Tests first.** Add `this-is-fine` to `SpecifiedSeconds` in
   `BuiltInEffectAnimationsTests` (2 s, or the length chosen for the open
   question) and an `[InlineData("this-is-fine", EffectPlacement.Monitor)]`
   case to `EffectCatalogTests`. Add a test that loads the file and checks the
   3840×1080 canvas. Run them: they fail because the catalog has no
   `this-is-fine` entry.
2. **Generator project.** Create `tools/ThisIsFineEffect/` (console,
   `net8.0`, no dependencies) and add it to the `tools` folder of
   `DrawEM.sln`. Constants at the top hold the canvas, flame heights, flame
   count, colors, length, frame rate, and random seed.
3. **Flames.** Build each tongue as three nested shape groups (outer, middle,
   core), each a closed Bézier path. Give every path 3–4 keyframes whose last
   keyframe repeats the first, so the loop is seamless. Offset each tongue's
   phase and vary its height between 8% and 20% of the canvas height.
4. **Output.** Write minified Lottie JSON to
   `src/DrawEM.App/Presentation/Effects/BuiltIn/this-is-fine.json`. Run the
   generator twice and confirm the two files are identical.
5. **Catalog.** Add the entry to `EffectCatalog.BuiltIn`: ID `this-is-fine`,
   name `This is fine`, Monitor, `this-is-fine.json`. The `*.json`
   `EmbeddedResource` glob already embeds the file.
6. **Sources.** Add a `this-is-fine.json` section to `SOURCES.md` (own work,
   generator path, date) and reword its intro paragraph.
7. **Visual check.** Run the app, bind the effect to a slot with Loop on,
   and check on a 16:9 monitor: the fire touches the bottom edge, reaches about
   20% of the height, loops without a visible jump, and plays smoothly. Check
   portrait or 21:9 if one is available.
8. Run `dotnet test DrawEM.sln` and `graphify update .`.
