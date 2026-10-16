# v4 / FEAT-011: glass design for Settings and the tray menu

**Status:** implemented. **Source:** [v4 out of scope](../README.md).

See [implementation plan](plan.md) and [acceptance criteria](acceptance.md).
Prototypes: [Settings window](../../../design/settings-glass.html),
[tray menu](../../../design/tray-menu-glass.html).

## Previous behavior

- The Settings window uses the default WPF look on a white background.
- The tray menu is a WinForms `ContextMenuStrip` in the system style.
- Neither has a dark theme.

## Required behavior

- The Settings window and the tray menu use a glass style: a blurred
  translucent background, an inner glow, and a faint light border
  (about 10% white).
- On Windows 11 22H2 (build 22621) and later, the blur is the system Acrylic
  backdrop. On earlier builds, the same layout uses an opaque tinted
  background.
- The Settings title bar has a dark/light theme switch in its upper right
  corner, left of the system window buttons.
- On start, the theme follows the Windows app theme
  (`AppsUseLightTheme`); if the value is missing, the theme is dark.
- The switch changes the Settings window and the tray menu at once. The
  choice lasts until the app exits; it is not saved to the settings file.
- The tray menu keeps its commands and order: **Settings…** (bold),
  **Stop effects**, a separator, **Exit**. A left or right click on the tray
  icon opens it next to the cursor, inside the work area of that monitor.
  A click outside the menu or Escape closes it.
- Save, Cancel, Restore defaults, shortcut capture, slots, and every other
  Settings behavior stay as they were.
