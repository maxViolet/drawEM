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
}
