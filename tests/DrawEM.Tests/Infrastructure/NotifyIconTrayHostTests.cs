using System.Windows.Forms;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class NotifyIconTrayHostTests
{
    [Fact]
    public void LeftClickMenu_FindsTheNotifyIconsOwnMenuDisplay()
    {
        // A left click shows the tray menu through the method a right click uses; a WinForms update could
        // rename that private method, and this test then fails instead of the click silently doing nothing.
        Assert.True(NotifyIconTrayHost.CanShowMenuOnLeftClick);
    }

    [Fact]
    public void ShowMenu_WithoutTheNotifyIconsOwnMenuDisplay_ShowsTheMenuAtTheCursor()
    {
        StaThread.Run(() =>
        {
            using var menu = new ContextMenuStrip();
            menu.Items.Add("Exit");
            using var notifyIcon = new NotifyIcon { ContextMenuStrip = menu };

            NotifyIconTrayHost.ShowMenu(notifyIcon, showContextMenu: null);

            Assert.True(menu.Visible);
            menu.Close();
        });
    }
}
