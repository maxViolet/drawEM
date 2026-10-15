using System.Drawing;
using System.IO.Packaging;
using System.Reflection;
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
        // A left click shows the same menu as a right click; Settings opens only from the menu.
        notifyIcon.MouseUp += OnMouseUp;
    }

    /// <summary>
    /// NotifyIcon's own menu display, which a right click uses. It brings the menu to the foreground, so the
    /// menu closes on a click elsewhere; <see cref="ContextMenuStrip.Show()"/> alone does not.
    /// </summary>
    private static readonly MethodInfo? ShowContextMenu =
        typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes);

    /// <summary>Whether a left click can open the tray menu, as a right click does.</summary>
    public static bool CanShowMenuOnLeftClick => ShowContextMenu is not null;

    public event Action? ExitRequested;

    public event Action? SettingsRequested;

    public event Action? StopEffectsRequested;

    public void Show() => notifyIcon.Visible = true;

    private void OnMouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ShowMenu(notifyIcon, ShowContextMenu);
        }
    }

    /// <summary>
    /// Shows the tray menu through <paramref name="showContextMenu"/>, or at the cursor if a WinForms update
    /// removed that method, so a left click still opens the menu.
    /// </summary>
    public static void ShowMenu(NotifyIcon notifyIcon, MethodInfo? showContextMenu)
    {
        if (showContextMenu is not null)
        {
            showContextMenu.Invoke(notifyIcon, null);
        }
        else
        {
            notifyIcon.ContextMenuStrip?.Show(Cursor.Position);
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
