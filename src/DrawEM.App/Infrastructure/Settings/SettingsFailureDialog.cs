using DrawEM.App.Application.Settings;

namespace DrawEM.App.Infrastructure.Settings;

/// <summary>
/// Shows the user why saved settings were not used and where the damaged bytes were kept, or why a Save
/// failed.
/// </summary>
public sealed class SettingsFailureDialog : ISettingsFailureReporter
{
    private readonly Action<string> show;

    /// <param name="show">Displays the message, for example in a message box.</param>
    public SettingsFailureDialog(Action<string> show)
    {
        ArgumentNullException.ThrowIfNull(show);
        this.show = show;
    }

    public void SettingsUnreadable(string reason, string? recoveryCopy) => show(Message(reason, recoveryCopy));

    public void SettingsNotSaved(string reason) => show(NotSavedMessage(reason));

    public static string Message(string reason, string? recoveryCopy) =>
        $"drawEM could not use its saved settings and started with defaults.{Environment.NewLine}{reason}" +
        (recoveryCopy is null ? "" : $"{Environment.NewLine}A copy of the damaged file was kept at {recoveryCopy}");

    public static string NotSavedMessage(string reason) =>
        $"drawEM could not save the settings and keeps using the previous ones.{Environment.NewLine}{reason}";
}
