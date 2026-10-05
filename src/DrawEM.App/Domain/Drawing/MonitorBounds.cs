namespace DrawEM.App.Domain.Drawing;

// Physical desktop coordinates; Right and Bottom are exclusive.
public readonly record struct MonitorBounds(int Left, int Top, int Right, int Bottom)
{
    public bool Contains(ScreenPoint point) =>
        point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;

    /// <summary>Returns the nearest point inside these bounds.</summary>
    public ScreenPoint Clamp(ScreenPoint point) =>
        new(Math.Clamp(point.X, Left, Right - 1), Math.Clamp(point.Y, Top, Bottom - 1));
}
