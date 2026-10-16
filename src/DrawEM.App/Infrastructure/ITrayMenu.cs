namespace DrawEM.App.Infrastructure;

/// <summary>A command in the tray menu.</summary>
public enum TrayMenuCommand
{
    Settings,
    StopEffects,
    Exit,
}

/// <summary>The menu that a click on the tray icon opens.</summary>
public interface ITrayMenu
{
    /// <summary>A command was chosen; the menu has closed by then.</summary>
    event Action<TrayMenuCommand>? Chosen;

    /// <summary>Opens the menu next to the cursor.</summary>
    void Show();
}
