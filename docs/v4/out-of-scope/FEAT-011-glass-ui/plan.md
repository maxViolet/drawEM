# v4 / FEAT-011: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Theme state.** Add `Presentation/Theme/UiThemeState`: the current
   `UiTheme` (dark or light), its default from the `AppsUseLightTheme` value,
   `Toggle()`, and a `Changed` event. One instance is shared by the Settings
   window and the tray menu.
2. **Brushes and styles.** Add `Presentation/Theme/GlassDark.xaml` and
   `GlassLight.xaml` with the same brush keys, and `GlassStyles.xaml` with
   styles for buttons, the primary button, text boxes, tabs, combo boxes,
   check boxes, repeat buttons, scroll bars, and the theme switch. Styles use
   the brushes through `DynamicResource`, so a theme change swaps one
   dictionary.
3. **Backdrop.** Add `Presentation/Theme/GlassBackdrop`: `IsSupported(Version)`
   for build 22621 and later, and `Apply(Window, UiTheme)`, which sets
   `DWMWA_SYSTEMBACKDROP_TYPE` to Acrylic, `DWMWA_USE_IMMERSIVE_DARK_MODE`,
   and round corners. Without support the window keeps an opaque background.
4. **Settings window.** Use `WindowChrome` with a custom title bar that holds
   the icon, the title, and the theme switch. Restyle both tabs and the
   footer with the glass styles. Keep every name, binding, and handler.
5. **Tray menu.** Add `Infrastructure/ITrayMenu`. `NotifyIconTrayHost` drops
   its `ContextMenuStrip` and shows an `ITrayMenu` on a left or right click.
   Add `Presentation/Tray/TrayMenuWindow` that implements it, and
   `TrayMenuPlacement`, which places the menu next to the cursor inside the
   work area. Allow `ITrayMenu` in the Presentation layer rule.
6. **Composition.** `App` creates one `UiThemeState` and passes it to the tray
   menu and every Settings window.
7. Run `dotnet test DrawEM.sln` and `graphify update .`.
