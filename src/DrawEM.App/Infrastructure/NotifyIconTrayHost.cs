using System.Drawing;
using System.IO.Packaging;
using System.Windows.Forms;

namespace DrawEM.App.Infrastructure;

public sealed class NotifyIconTrayHost : ITrayHost
{
    /// <summary>The app icon, embedded as a WPF resource; the Settings window uses the same one.</summary>
    public static Uri IconUri
    {
        get
        {
            // Reading the scheme registers pack URIs, which otherwise happens only once a WPF Application exists.
            _ = PackUriHelper.UriSchemePack;
            return new Uri("pack://application:,,,/DrawEM.App;component/Assets/drawEM.ico");
        }
    }

    private readonly NotifyIcon notifyIcon;
    private readonly Icon icon;
    private readonly ITrayMenu menu;

    /// <param name="menu">Opens on a left or right click on the icon.</param>
    public NotifyIconTrayHost(ITrayMenu menu)
    {
        this.menu = menu;
        icon = LoadIcon();
        notifyIcon = new NotifyIcon
        {
            Icon = icon,
            Text = "drawEM",
            Visible = false,
        };
        notifyIcon.MouseUp += (_, e) => OnIconClick(e.Button);
        notifyIcon.BalloonTipClicked += (_, _) => UpdateRequested?.Invoke();
        menu.Chosen += OnChosen;
    }

    public event Action? ExitRequested;

    public event Action? SettingsRequested;

    public event Action? StopEffectsRequested;

    public event Action? UpdateRequested;

    public void Show() => notifyIcon.Visible = true;

    /// <summary>The balloon is the only one this tray shows, so a balloon click always means the update.</summary>
    public void ShowUpdateReady(string version)
    {
        menu.ShowUpdate(version);
        notifyIcon.ShowBalloonTip(
            10_000,
            "drawEM update ready",
            $"drawEM {version} is downloaded. Click here or choose Restart to update in the tray menu.",
            ToolTipIcon.Info);
    }

    /// <summary>A left click opens the same menu as a right click; Settings opens only from the menu.</summary>
    internal void OnIconClick(MouseButtons button)
    {
        if (button is MouseButtons.Left or MouseButtons.Right)
        {
            menu.Show();
        }
    }

    private void OnChosen(TrayMenuCommand command) => RequestFor(command)?.Invoke();

    private Action? RequestFor(TrayMenuCommand command) => command switch
    {
        TrayMenuCommand.Settings => SettingsRequested,
        TrayMenuCommand.StopEffects => StopEffectsRequested,
        TrayMenuCommand.Update => UpdateRequested,
        TrayMenuCommand.Exit => ExitRequested,
        _ => null,
    };

    public void Dispose()
    {
        menu.Chosen -= OnChosen;
        notifyIcon.Visible = false;
        notifyIcon.Dispose();
        icon.Dispose();
    }

    /// <summary>Loads the frame that fits the notification area at the current DPI.</summary>
    private static Icon LoadIcon()
    {
        using var stream = global::System.Windows.Application.GetResourceStream(IconUri).Stream;
        return new Icon(stream, SystemInformation.SmallIconSize);
    }
}
