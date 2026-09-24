# drawEM

Windows 10/11 x64 desktop utility for drawing temporary annotations over every
monitor.

## Current behavior

- Starts hidden in the system tray and has an `Exit` command.
- Shows a transparent overlay across the virtual desktop.
- While `Ctrl+Alt+Z` is held, mouse movement draws a 4 px OrangeRed
  (#FF4500) stroke.
  The stroke is confined to the monitor under the cursor when drawing starts.
  Moving to another monitor ends the stroke; release and press the shortcut
  again to draw on the other monitor.
  Global hooks suppress pointer buttons, scrolling, and other keyboard input
  during drawing, so the application below receives neither clicks, scrolling,
  nor typed symbols.
- `Ctrl+Alt+X` clears strokes only on the monitor under the cursor and exits draw mode.
- Strokes remain visible while switching windows and monitors, until cleared.

## Deliberately out of scope

- Configurable shortcuts
- Color or thickness controls
- Eraser, undo, export, startup with Windows
- Administrator elevation and support over elevated apps

## Delivery target

One self-contained WPF application, launched through `DrawEM.App.exe`, with no
installer required. The publish directory contains the executable and its
runtime files.

## Development method

Work in small vertical TDD slices: define one business behavior at a public
seam, write its failing test, implement the smallest change that passes it,
then continue. The drawing plan is in [docs/ROADMAP-v1.md](docs/ROADMAP-v1.md).
The sound extension plan is in [docs/ROADMAP-v2.md](docs/ROADMAP-v2.md).
The module boundaries and runtime data flow are in
[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).

## Publish and smoke test

Publish a self-contained x64 executable with:

```powershell
dotnet publish .\src\DrawEM.App\DrawEM.App.csproj -c Release -p:PublishProfile=win-x64
```

Run the manual checks in [docs/SMOKE-TEST.md](docs/SMOKE-TEST.md) against the
published executable.

## Project layout

```text
src/DrawEM.App/
  Application/{Drawing,Sound}/     # use cases and sound placeholder
  Domain/{Drawing,Sound}/          # drawing rules and sound placeholder
  Infrastructure/{Drawing,Sound}/  # drawing adapters and sound placeholder
  Infrastructure/                 # shared keyboard hook and tray lifecycle
  Presentation/{Drawing,Sound}/    # drawing overlay and sound UI placeholder
tests/DrawEM.Tests/
  Application/Drawing/             # drawing-controller behavior tests
  Infrastructure/Drawing/          # drawing input adapter tests
  Presentation/Drawing/            # drawing overlay tests
  Infrastructure/                  # shared lifecycle tests
```

`App.xaml.cs` remains the composition root. Sound has no implementation yet.
