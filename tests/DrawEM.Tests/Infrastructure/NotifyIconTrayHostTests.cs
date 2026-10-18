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

    [Fact]
    public void UpdateItem_IsHiddenAtFirst()
    {
        StaThread.Run(() =>
        {
            using var host = new NotifyIconTrayHost();

            Assert.False(host.UpdateItem.Available);
        });
    }

    [Fact]
    public void ShowUpdateReady_ShowsTheVersionAboveExit_AndAClickRequestsTheUpdate()
    {
        StaThread.Run(() =>
        {
            using var host = new NotifyIconTrayHost();
            var requests = 0;
            host.UpdateRequested += () => requests++;

            host.ShowUpdateReady("1.2.3");
            host.UpdateItem.PerformClick();

            var items = host.Menu.Items.Cast<ToolStripItem>().ToList();
            Assert.True(host.UpdateItem.Available);
            Assert.Equal("Restart to update to 1.2.3", host.UpdateItem.Text);
            Assert.True(items.IndexOf(host.UpdateItem) < items.FindIndex(item => item.Text == "Exit"));
            Assert.Equal(1, requests);
        });
    }
}
