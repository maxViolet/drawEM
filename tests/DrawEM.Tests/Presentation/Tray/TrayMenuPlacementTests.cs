using System.Windows;
using DrawEM.App.Presentation.Tray;

namespace DrawEM.Tests.Presentation.Tray;

public class TrayMenuPlacementTests
{
    private static readonly Size Menu = new(224, 140);

    /// <summary>A 1920×1080 screen with a 48-unit taskbar at the bottom.</summary>
    private static readonly Rect BottomTaskbar = new(0, 0, 1920, 1032);

    [Fact]
    public void Place_AtABottomTaskbar_OpensAboveAndLeftOfTheCursor()
    {
        var position = TrayMenuPlacement.Place(new Point(1800, 1050), Menu, BottomTaskbar);

        Assert.Equal(new Point(1800 - 224, 1032 - 140), position);
    }

    [Fact]
    public void Place_AtATopTaskbarOnTheLeft_OpensBelowAndRightOfTheCursor()
    {
        var workArea = new Rect(0, 48, 1920, 1032);

        var position = TrayMenuPlacement.Place(new Point(10, 20), Menu, workArea);

        Assert.Equal(new Point(10, 48), position);
    }

    [Fact]
    public void Place_InsideTheWorkArea_KeepsTheCornerAtTheCursor()
    {
        var position = TrayMenuPlacement.Place(new Point(900, 600), Menu, BottomTaskbar);

        Assert.Equal(new Point(900 - 224, 600 - 140), position);
    }

    [Fact]
    public void Place_OnASecondMonitor_StaysInsideItsWorkArea()
    {
        var right = new Rect(1920, 0, 1280, 984);

        var position = TrayMenuPlacement.Place(new Point(3190, 1000), Menu, right);

        Assert.Equal(new Point(3190 - 224, 984 - 140), position);
    }
}
