# drawEM

drawEM is a Windows 10/11 x64 tray app for drawing temporary annotations over
other windows. It starts without a normal window and shows a transparent
overlay across the desktop.

## Use drawEM

1. Run `DrawEM.App.exe` from the published app directory. The icon appears in
   the notification area.
2. Hold `Ctrl+Alt+Z` and move the pointer to draw. Release the keys to finish
   the stroke.
3. Press `Ctrl+Alt+X` to clear drawings on the monitor under the pointer.
4. Choose `Exit` from the tray icon menu to close the app.

Drawings are OrangeRed (`#FF4500`) and 4 px thick. They remain visible while
you switch windows, until you clear them or exit. A stroke stays on the monitor
where it began. If the pointer crosses to another monitor while drawing, the
stroke ends; release and press `Ctrl+Alt+Z` again to draw there.

While drawing, drawEM blocks pointer clicks, scrolling, and other keyboard
input from the app beneath the overlay. After the draw shortcut is released,
normal input passes through again.

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

The [manual smoke test](docs/SMOKE-TEST.md) covers tray, input blocking, and
multi-monitor behavior that unit tests cannot establish.

## Current limits and plans

- Shortcuts, stroke color, and thickness are fixed. There is no eraser, undo,
  export, or Windows startup option.
- drawEM does not run elevated. Behavior over elevated apps, exclusive
  fullscreen games, and protected surfaces is not guaranteed.
- Sound playback, settings UI, video, and screen effects are planned; none is
  implemented yet. See the [v2 roadmap](docs/ROADMAP-v2.md).

The [v1 drawing roadmap](docs/ROADMAP-v1.md) records the original work plan.
The [architecture](docs/ARCHITECTURE.md) describes the modules and proposed
screen-action model.
