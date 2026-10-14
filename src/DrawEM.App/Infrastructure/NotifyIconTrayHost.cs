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

    public NotifyIconTrayHost()
    {
        icon = LoadIcon();
        var settingsItem = new ToolStripMenuItem("Settings…") { Font = new Font(SystemFonts.MenuFont!, FontStyle.Bold) };
        settingsItem.Click += (_, _) => SettingsRequested?.Invoke();
        var stopEffectsItem = new ToolStripMenuItem("Stop effects");
        stopEffectsItem.Click += (_, _) => StopEffectsRequested?.Invoke();
        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) => ExitRequested?.Invoke();

        notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = new ContextMenuStrip(),
            Icon = icon,
            Text = "drawEM",
            Visible = false,
        };
        notifyIcon.ContextMenuStrip.Items.Add(settingsItem);
        notifyIcon.ContextMenuStrip.Items.Add(stopEffectsItem);
        notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
        notifyIcon.ContextMenuStrip.Items.Add(exitItem);
        // One left click opens Settings; a right click shows the menu.
        notifyIcon.MouseClick += OnMouseClick;
    }

    public event Action? ExitRequested;

    public event Action? SettingsRequested;

    public event Action? StopEffectsRequested;

    public void Show() => notifyIcon.Visible = true;

    private void OnMouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            SettingsRequested?.Invoke();
        }
    }

    public void Dispose()
    {
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
