# drawEM

drawEM is a Windows 10/11 x64 tray app for drawing temporary annotations over
other windows. It starts without a normal window and shows a transparent
overlay across the desktop.

## Use drawEM

1. Run `DrawEM.App.exe` from the published app directory (see
   [Build and run from source](#build-and-run-from-source)). The icon appears
   in the notification area.
2. Hold the draw shortcut (default `Ctrl+Alt+Z`) and move the pointer to
   draw. Release the keys to finish the stroke.
3. Press the clear shortcut (default `Ctrl+Alt+X`) to clear drawings on the
   monitor under the pointer.
4. Click the tray icon and choose `Exit` to close the app.

By default, drawings are OrangeRed (`#FF4500`) and 4 px thick. To change the
color, line width, or the draw and clear shortcuts, open `Settings…` from the
tray menu, use the `Drawing` tab, and choose `Save`.

Drawings remain visible while you switch windows, until you clear them, save
settings, or exit. A stroke stays on the monitor where it began. If the pointer
crosses to another monitor while drawing, the stroke ends; release and press
the draw shortcut again to draw there. Reaching a screen edge with no monitor
beyond it does not end the stroke.

A successful Save in Settings clears all drawings on every monitor and stops
any playing sound or effect. If Save fails, drawings, sound, and effects stay
as they were.

While drawing, drawEM blocks pointer clicks, scrolling, and keyboard input
other than the draw shortcut from the app beneath the overlay. After the draw
shortcut is released, normal input passes through again.

## Play sounds

1. Click the tray icon and choose `Settings…`.
2. On the `Actions · 8 slots` tab, set a slot's type to `Sound`.
3. Choose `Choose file…` and pick a WAV or MP3 file.
4. Keep the proposed `Ctrl+Alt+<N>` shortcut, where `<N>` is the slot number,
   or click it to capture another one. Use `Sample` to hear the draft sound.
5. Choose `Save`.

One sound plays at a time: a new sound replaces the playing one. Playback
stops after at most 10 seconds.

## Show visual effects

1. Click the tray icon and choose `Settings…`.
2. On the `Actions · 8 slots` tab, set a slot's type to `Effect` and choose
   an effect:
   - `Confetti` (Monitor placement) covers the monitor under the pointer.
   - `Focus ring` (Cursor placement) appears around the pointer position at
     the moment you press the shortcut.
3. Set Loop if the animation should repeat until stopped. Otherwise, choose a
   Max length from 1–60 seconds or select No limit to let the file finish.
4. Keep the proposed `Ctrl+Alt+<N>` shortcut, where `<N>` is the slot number,
   or click it to capture another one. Use `Sample` to preview the draft effect
   on the monitor with the pointer. A looping Sample changes the button to
   `Stop`.
5. Choose `Save`. Shortcut changes do not apply until Save; `Cancel` discards
   them.
6. Press the slot's shortcut with any app focused. Effects are designed not to
   take focus or block input to the app beneath them; this is not yet verified
   in the published app.

One effect plays at a time: a new effect replaces the running one. With Loop
off, an effect stops when its file finishes or its Max length passes, whichever
happens first. No limit lets the file finish. With Loop on, the file restarts
from frame 0 and the same shortcut stops it instead of restarting it.

An effect also stops when the pointer moves to another monitor, when you choose
`Stop effects` in the tray menu, when you save settings, and when displays
change, the session locks, or the computer sleeps. A stopped effect does not
restart by itself. Effects and sound can play at the same time. Effects appear
above drawings and do not clear them; drawing still works during an effect.

A full-display screen share is expected to show effects, but this is not yet
verified on a receiving device. A share of one window may not include them,
because the effect is a separate window.

## Return to drawEM v3

The first v4 Save converts `%LOCALAPPDATA%\drawEM\settings.json` to a format
v3 cannot read and keeps the v3 file as `settings.schema1-backup*.json`. To go
back to v3:

1. Exit drawEM from the tray icon.
2. Copy the whole `%LOCALAPPDATA%\drawEM\sounds` folder to a backup folder
   outside `%LOCALAPPDATA%\drawEM`. v3 deletes sound copies its settings do not
   use, including ones the v4 settings still use.
3. Rename `settings.json` to `settings.v4.json`.
4. Copy the newest `settings.schema1-backup*.json` to `settings.json`.
5. Start v3.

See [Rolling back to a v3 drawEM](src/DrawEM.App/Infrastructure/Settings/README.md#rolling-back-to-a-v3-drawem)
for the exact backup to choose, how to restore sounds, and how to upgrade
again.

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

To prepare a release, push a `vX.Y.Z` version tag:

```powershell
git tag v0.3.0
git push origin v0.3.0
```

The release workflow rejects any other tag form, runs the tests, publishes the
self-contained app with the tag's version, checks the publish output, and
attaches `drawEM-<version>-win-x64.zip` to a draft GitHub Release with
generated notes. Inspect the draft's notes and assets on GitHub, then publish
it by hand; the workflow never publishes a release.

## Current limits and plans

- There is no eraser, undo, export, or Windows startup option.
- drawEM does not run elevated. Behavior over elevated apps, exclusive
  fullscreen games, and protected surfaces is not guaranteed.
- Only the built-in effects are available. Imported effects and video are
  planned for a later phase; see the
  [v4 visual effects roadmap](docs/v4/visual-effects/ROADMAP-v4.md).

The [v1 drawing roadmap](docs/v1/ROADMAP-v1.md) records the original work plan.
For contributors, the [architecture](docs/ARCHITECTURE.md) describes the
modules, test seams, and proposed screen-action model.
