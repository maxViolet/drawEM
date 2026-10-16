using System.Windows;

namespace DrawEM.App.Presentation.Tray;

/// <summary>Where the tray menu opens: with a corner at the cursor, inside the monitor's work area.</summary>
public static class TrayMenuPlacement
{
    /// <summary>
    /// Opens the menu above and left of the cursor, as the taskbar is usually at the bottom right, then moves
    /// it into <paramref name="workArea"/>. All values share one unit.
    /// </summary>
    /// <returns>The menu's top-left corner.</returns>
    public static Point Place(Point cursor, Size menu, Rect workArea)
    {
        var left = cursor.X - menu.Width;
        var top = cursor.Y - menu.Height;
        if (left < workArea.Left)
        {
            left = cursor.X;
        }

        if (top < workArea.Top)
        {
            top = cursor.Y;
        }

        left = Math.Clamp(left, workArea.Left, Math.Max(workArea.Left, workArea.Right - menu.Width));
        top = Math.Clamp(top, workArea.Top, Math.Max(workArea.Top, workArea.Bottom - menu.Height));
        return new Point(left, top);
    }
}
