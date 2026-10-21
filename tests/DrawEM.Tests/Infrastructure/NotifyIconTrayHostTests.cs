using System.Windows.Forms;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

[Collection(WpfCollection.Name)]
public class NotifyIconTrayHostTests
{
    [Theory]
    [InlineData(MouseButtons.Left)]
    [InlineData(MouseButtons.Right)]
    public void Click_OnTheIcon_ShowsTheMenu(MouseButtons button)
    {
        var menu = new FakeTrayMenu();
        using var host = new NotifyIconTrayHost(menu);

        host.OnIconClick(button);

        Assert.Equal(1, menu.Shown);
    }

    [Fact]
    public void Click_WithTheMiddleButton_DoesNothing()
    {
        var menu = new FakeTrayMenu();
        using var host = new NotifyIconTrayHost(menu);

        host.OnIconClick(MouseButtons.Middle);

        Assert.Equal(0, menu.Shown);
    }

    [Fact]
    public void MenuCommands_RaiseTheTrayCommands()
    {
        var menu = new FakeTrayMenu();
        using var host = new NotifyIconTrayHost(menu);
        var commands = new List<string>();
        host.SettingsRequested += () => commands.Add("settings");
        host.StopEffectsRequested += () => commands.Add("stop effects");
        host.UpdateRequested += () => commands.Add("update");
        host.ExitRequested += () => commands.Add("exit");

        menu.Choose(TrayMenuCommand.Settings);
        menu.Choose(TrayMenuCommand.StopEffects);
        menu.Choose(TrayMenuCommand.Update);
        menu.Choose(TrayMenuCommand.Exit);

        Assert.Equal(["settings", "stop effects", "update", "exit"], commands);
    }

    [Fact]
    public void ShowUpdateReady_ShowsTheUpdateInTheMenu()
    {
        StaThread.Run(() =>
        {
            var menu = new FakeTrayMenu();
            using var host = new NotifyIconTrayHost(menu);

            host.ShowUpdateReady("1.2.3");

            Assert.Equal(["1.2.3"], menu.Updates);
        });
    }

    private sealed class FakeTrayMenu : ITrayMenu
    {
        public int Shown { get; private set; }

        public List<string> Updates { get; } = [];

        public event Action<TrayMenuCommand>? Chosen;

        public void Show() => Shown++;

        public void ShowUpdate(string version) => Updates.Add(version);

        public void Choose(TrayMenuCommand command) => Chosen?.Invoke(command);
    }
}
