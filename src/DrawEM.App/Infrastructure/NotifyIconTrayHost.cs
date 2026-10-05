using System.Drawing;
using System.Windows.Forms;

namespace DrawEM.App.Infrastructure;

public sealed class NotifyIconTrayHost : ITrayHost
{
    private readonly NotifyIcon notifyIcon;

    public NotifyIconTrayHost()
    {
        var settingsItem = new ToolStripMenuItem("Settings…") { Font = new Font(SystemFonts.MenuFont!, FontStyle.Bold) };
        settingsItem.Click += (_, _) => SettingsRequested?.Invoke();
        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) => ExitRequested?.Invoke();

        notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = new ContextMenuStrip(),
            Icon = SystemIcons.Application,
            Text = "drawEM",
            Visible = false,
        };
        notifyIcon.ContextMenuStrip.Items.Add(settingsItem);
        notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
        notifyIcon.ContextMenuStrip.Items.Add(exitItem);
        notifyIcon.DoubleClick += (_, _) => SettingsRequested?.Invoke();
    }

    public event Action? ExitRequested;

    public event Action? SettingsRequested;

    public void Show() => notifyIcon.Visible = true;

    public void Dispose()
    {
        notifyIcon.Visible = false;
        notifyIcon.Dispose();
    }
}
