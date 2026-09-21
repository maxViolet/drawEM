using System.Drawing;
using System.Windows.Forms;

namespace DrawEM.App.Infrastructure;

public sealed class NotifyIconTrayHost : ITrayHost
{
    private readonly NotifyIcon notifyIcon;

    public NotifyIconTrayHost()
    {
        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) => ExitRequested?.Invoke();

        notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = new ContextMenuStrip(),
            Icon = SystemIcons.Application,
            Text = "drawEM",
            Visible = false,
        };
        notifyIcon.ContextMenuStrip.Items.Add(exitItem);
    }

    public event Action? ExitRequested;

    public void Show() => notifyIcon.Visible = true;

    public void Dispose()
    {
        notifyIcon.Visible = false;
        notifyIcon.Dispose();
    }
}
