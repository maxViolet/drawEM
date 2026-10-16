namespace DrawEM.App.Infrastructure;

/// <summary>A command in the tray menu.</summary>
public enum TrayMenuCommand
{
    Settings,
    StopEffects,

    /// <summary>Restart to apply a downloaded update.</summary>
    Update,
    Exit,
}

/// <summary>The menu that a click on the tray icon opens.</summary>
public interface ITrayMenu
{
    /// <summary>A command was chosen; the menu has closed by then.</summary>
    event Action<TrayMenuCommand>? Chosen;

    /// <summary>Opens the menu next to the cursor.</summary>
    void Show();

    /// <summary>Shows the restart-to-update command for <paramref name="version"/>, above Exit, from now on.</summary>
    void ShowUpdate(string version);
}
