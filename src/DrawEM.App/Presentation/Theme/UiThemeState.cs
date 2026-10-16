using Microsoft.Win32;

namespace DrawEM.App.Presentation.Theme;

public enum UiTheme
{
    Dark,
    Light,
}

/// <summary>
/// The theme of the Settings window and the tray menu, shared by both. It lasts until the app exits and is
/// not saved to the settings file.
/// </summary>
public sealed class UiThemeState(UiTheme initial)
{
    private const string PersonalizeKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

    public UiTheme Current { get; private set; } = initial;

    /// <summary>Raised after <see cref="Current"/> changes.</summary>
    public event Action? Changed;

    /// <summary>Starts from the Windows app theme.</summary>
    public static UiThemeState FromSystem()
    {
        using var key = Registry.CurrentUser.OpenSubKey(PersonalizeKey);
        return FromAppsUseLightTheme(key?.GetValue("AppsUseLightTheme") as int?);
    }

    /// <param name="appsUseLightTheme">The Windows <c>AppsUseLightTheme</c> value; missing means dark.</param>
    public static UiThemeState FromAppsUseLightTheme(int? appsUseLightTheme) =>
        new(appsUseLightTheme == 1 ? UiTheme.Light : UiTheme.Dark);

    public void Toggle()
    {
        Current = Current == UiTheme.Dark ? UiTheme.Light : UiTheme.Dark;
        Changed?.Invoke();
    }
}
