# drawEM

drawEM is a Windows 10/11 x64 tray app for drawing temporary annotations over
other windows. It starts without a normal window and shows a transparent
overlay across the desktop.

## Use drawEM

1. Run `DrawEM.App.exe` from the published app directory (see
   [Build and run from source](#build-and-run-from-source)). The icon appears
   in the notification area.
2. Hold `Ctrl+Alt+Z` and move the pointer to draw. Release the keys to finish
   the stroke.
3. Press `Ctrl+Alt+X` to clear drawings on the monitor under the pointer.
4. Right-click the tray icon and choose `Exit` to close the app.

Drawings are OrangeRed (`#FF4500`) and 4 px thick. They remain visible while
you switch windows, until you clear them or exit. A stroke stays on the monitor
where it began. If the pointer crosses to another monitor while drawing, the
stroke ends; release and press `Ctrl+Alt+Z` again to draw there. Reaching a
screen edge with no monitor beyond it does not end the stroke.

While drawing, drawEM blocks pointer clicks, scrolling, and keyboard input
other than the draw shortcut from the app beneath the overlay. After the draw
shortcut is released, normal input passes through again.

## Show visual effects

1. Right-click the tray icon and choose `Settings…`.
2. On the `Actions · 8 slots` tab, set a slot's type to `Effect` and choose
   an effect:
   - `Confetti` (Monitor placement) covers the monitor under the pointer.
   - `Focus ring` (Cursor placement) appears around the pointer position at
     the moment you press the shortcut.
3. Bind a shortcut to the slot. Use `Sample` to preview the draft effect on the
   monitor with the pointer.
4. Choose `Save`. Shortcut changes do not apply until Save; `Cancel` discards
   them.
5. Press the slot's shortcut with any app focused. Effects do not take focus or
   block input to the app beneath them.

One effect plays at a time: a new effect replaces the running one. An effect
stops when it finishes (after at most 10 seconds), when the pointer moves to
another monitor, when you choose `Stop effects` in the tray menu, when you save
settings, and when displays change, the session locks, or the computer sleeps.
A stopped effect does not restart by itself. Effects and sound can play at the
same time. Effects appear above drawings and do not clear them; drawing still
works during an effect.

A full-display screen share shows effects. A share of one window may not
include them, because the effect is a separate window.

## Build and run from source

Publish the self-contained x64 app with the .NET SDK:

```powershell
dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64
```

Run `src\DrawEM.App\bin\Release\net8.0-windows\win-x64\publish\DrawEM.App.exe`.
Keep the entire publish directory together when copying the app to another
computer; the executable uses the other files in that directory.

To run the automated tests:

```powershell
dotnet test DrawEM.sln
```

The [manual smoke test](docs/v1/SMOKE-TEST.md) covers tray, input blocking, and
multi-monitor behavior that unit tests cannot establish.

## Continuous integration and releases

GitHub Actions builds the solution with warnings as errors, runs the tests, and
publishes the win-x64 app for every pull request to `main` and every push to
`main`. Changes that touch only `docs/` or Markdown files skip this run. A
separate check verifies branch names and pull request titles against
[AGENTS.md](AGENTS.md). The .NET SDK version is pinned in `global.json`.

To publish a release, push a version tag:

```powershell
git tag v0.3.0
git push origin v0.3.0
```

The release workflow runs the tests, publishes the app with the tag's version,
and attaches `drawEM-<version>-win-x64.zip` to a new GitHub Release.

## Current limits and plans

- Shortcuts, stroke color, and thickness are fixed. There is no eraser, undo,
  export, or Windows startup option.
- drawEM does not run elevated. Behavior over elevated apps, exclusive
  fullscreen games, and protected surfaces is not guaranteed.
- Only the built-in effects are available. Imported effects and video are
  planned for a later phase; see the
  [v4 visual effects roadmap](docs/v4/visual-effects/ROADMAP-v4.md).

The [v1 drawing roadmap](docs/v1/ROADMAP-v1.md) records the original work plan.
For contributors, the [architecture](docs/ARCHITECTURE.md) describes the
modules, test seams, and proposed screen-action model.
