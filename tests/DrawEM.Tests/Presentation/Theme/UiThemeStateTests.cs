using DrawEM.App.Presentation.Theme;

namespace DrawEM.Tests.Presentation.Theme;

public class UiThemeStateTests
{
    [Theory]
    [InlineData(1, UiTheme.Light)]
    [InlineData(0, UiTheme.Dark)]
    [InlineData(null, UiTheme.Dark)]
    public void Default_FollowsTheWindowsAppTheme(int? appsUseLightTheme, UiTheme expected)
    {
        Assert.Equal(expected, UiThemeState.FromAppsUseLightTheme(appsUseLightTheme).Current);
    }

    [Fact]
    public void Toggle_SwitchesTheThemeAndRaisesOneChange()
    {
        var state = new UiThemeState(UiTheme.Dark);
        var changes = new List<UiTheme>();
        state.Changed += () => changes.Add(state.Current);

        state.Toggle();
        state.Toggle();

        Assert.Equal([UiTheme.Light, UiTheme.Dark], changes);
    }
}
