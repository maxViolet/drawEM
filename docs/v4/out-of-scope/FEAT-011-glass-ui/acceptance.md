# v4 / FEAT-011: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [x] The default theme is light when `AppsUseLightTheme` is 1, and dark when
  it is 0 or missing.
- [x] The theme switch toggles the theme and raises one change.
- [x] The Acrylic backdrop is used only on build 22621 and later.
- [x] The tray menu opens next to the cursor and stays inside the work area
  at each screen corner.
- [x] A left or right click on the tray icon opens the tray menu.
- [x] Settings…, Stop effects, and Exit in the tray menu raise the same
  commands as before and close the menu.
- [x] The Settings window shows both tabs without binding errors, at the same
  1.2 scale and work-area height as before.
- [x] The theme switch in the Settings window changes its brushes and the
  tray menu's brushes.
- [x] Manual: on Windows 11, the Settings window and the tray menu show a
  blurred background, an inner glow, and a faint border, in both themes
  (screenshots on build 26300).
- [ ] Manual: on Windows 10, both show an opaque background and stay readable.
- [ ] Manual: a click outside the tray menu closes it.
- [x] `dotnet test DrawEM.sln` passes.
