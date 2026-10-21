using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DrawEM.App.Infrastructure;
using DrawEM.App.Presentation.Theme;
using DrawEM.App.Presentation.Tray;

namespace DrawEM.Tests.Presentation.Tray;

[Collection(WpfCollection.Name)]
public class TrayMenuWindowTests
{
    [Theory]
    [InlineData("SettingsItem", TrayMenuCommand.Settings)]
    [InlineData("StopEffectsItem", TrayMenuCommand.StopEffects)]
    [InlineData("ExitItem", TrayMenuCommand.Exit)]
    public void Item_RaisesItsCommandAndClosesTheMenu(string item, TrayMenuCommand expected)
    {
        StaThread.Run(() =>
        {
            var menu = Open(new UiThemeState(UiTheme.Dark));
            var chosen = new List<TrayMenuCommand>();
            menu.Chosen += chosen.Add;

            Click((Button)menu.FindName(item));

            Assert.Equal([expected], chosen);
            Assert.False(menu.IsVisible);
            menu.Close();
        });
    }

    [Fact]
    public void UpdateItem_IsHiddenUntilAnUpdateIsReady_ThenShowsTheVersionAboveExit()
    {
        StaThread.Run(() =>
        {
            var menu = Open(new UiThemeState(UiTheme.Dark));
            var update = (Button)menu.FindName("UpdateItem");
            var items = (Panel)update.Parent;
            var chosen = new List<TrayMenuCommand>();
            menu.Chosen += chosen.Add;
            Assert.Equal(Visibility.Collapsed, update.Visibility);

            ((ITrayMenu)menu).ShowUpdate("1.2.3");
            Click(update);

            Assert.Equal(Visibility.Visible, update.Visibility);
            Assert.Equal("Restart to update to 1.2.3", ((TextBlock)menu.FindName("UpdateText")).Text);
            Assert.True(items.Children.IndexOf(update) < items.Children.IndexOf((UIElement)menu.FindName("ExitItem")));
            Assert.Equal([TrayMenuCommand.Update], chosen);
            menu.Close();
        });
    }

    [Fact]
    public void Escape_ClosesTheMenuWithoutACommand()
    {
        StaThread.Run(() =>
        {
            var menu = Open(new UiThemeState(UiTheme.Dark));
            var chosen = new List<TrayMenuCommand>();
            menu.Chosen += chosen.Add;

            menu.RaiseEvent(new KeyEventArgs(
                Keyboard.PrimaryDevice, PresentationSource.FromVisual(menu)!, 0, Key.Escape)
            {
                RoutedEvent = Keyboard.PreviewKeyDownEvent,
            });

            Assert.Empty(chosen);
            Assert.False(menu.IsVisible);
            menu.Close();
        });
    }

    [Fact]
    public void ThemeChange_SwapsTheMenuBrushes()
    {
        StaThread.Run(() =>
        {
            var theme = new UiThemeState(UiTheme.Dark);
            var menu = Open(theme);
            var dark = Foreground(menu);

            theme.Toggle();
            Pump();

            Assert.NotEqual(dark, Foreground(menu));
            menu.Close();
        });
    }

    private static TrayMenuWindow Open(UiThemeState theme)
    {
        var menu = new TrayMenuWindow(theme) { ShowActivated = false };
        menu.ShowAt(new Point(-20000, -20000), new Rect(-20500, -20500, 1000, 1000));
        Pump();
        return menu;
    }

    private static Color Foreground(TrayMenuWindow menu) =>
        ((SolidColorBrush)((TextBlock)menu.FindName("SettingsText")).Foreground).Color;

    private static void Click(Button button) =>
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));

    private static void Pump() =>
        Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
}
