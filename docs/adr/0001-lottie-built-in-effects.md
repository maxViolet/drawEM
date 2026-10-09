---
status: accepted
---

# Built-in effects are Lottie files with a fixed placement

Built-in screen effects are authored in a visual animation editor, or selected from existing Lottie animations, and shipped as Lottie files, played with `SkiaSharp.Skottie` inside the existing WPF effect surface. Drawing effects in code with WPF `DrawingContext` cannot produce designer-quality motion, and Lottie has a mature .NET player, many editors, and a large free asset library. This adds drawEM's first media dependency, which the original v4 plan ruled out. Each effect has one fixed placement, Monitor or Cursor, chosen by its author rather than the user, so one file looks right without a variant per placement.

## Considered options

- **Code-drawn effects (WPF `DrawingContext`, shaders):** no dependency, but every effect is programmer work and the result looks programmer-made.
- **Rive:** its .NET runtime (`rive-sharp`) is experimental, and its strength, interactive state machines, does not apply to fire-and-forget effects.
- **User-chosen placement:** every effect would need a variant for each placement, and v4 ships no effect parameters.

## Consequences

- Skia renders on the CPU and WPF copies the bitmap each frame, so a monitor-sized effect may miss the 4 ms render target. The fallback order is half resolution for Monitor effects, then a roadmap revision to Cursor effects only for v4, confirmed by the project owner. A GPU renderer is outside v4.
- Placement is not saved in settings; it comes from the built-in catalog.
- User-imported Lottie files stay deferred: they need validation of unsupported Lottie features and a placement choice.
