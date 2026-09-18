# drawEM

Windows 10/11 x64 desktop utility for drawing temporary annotations over every
monitor.

## First-version behavior

- Starts hidden in the system tray and has an `Exit` command.
- Shows a transparent overlay across the virtual desktop.
- While `Ctrl+Alt+Z` is held, mouse movement draws a 4 px orange stroke.
  The overlay intercepts mouse input during drawing, so the application below
  it receives no clicks.
- `Ctrl+Alt+X` clears every stroke.
- Strokes remain visible while switching windows and monitors, until cleared.

## Deliberately out of scope

- Configurable shortcuts
- Color or thickness controls
- Eraser, undo, export, startup with Windows
- Administrator elevation and support over elevated apps

## Delivery target

One self-contained WPF `.exe`, built with C#, with no installer required.

## Development method

Work in small vertical TDD slices: define one business behavior at a public
seam, write its failing test, implement the smallest change that passes it,
then continue. The ordered plan is in [docs/ROADMAP.md](docs/ROADMAP.md).
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
  Application/     # drawing-session use cases
  Domain/          # strokes, points, and drawing state
  Infrastructure/  # Windows shortcuts and tray adapters
  Presentation/    # WPF overlay and rendering adapters
tests/DrawEM.Tests/
  Application/     # controller behavior tests
  Infrastructure/  # adapter contract tests
  Presentation/    # overlay behavior tests
```

The folders are intentionally empty until the test seams are confirmed.
