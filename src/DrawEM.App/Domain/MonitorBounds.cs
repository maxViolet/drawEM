namespace DrawEM.App.Domain;

// Physical desktop coordinates; Right and Bottom are exclusive.
public readonly record struct MonitorBounds(int Left, int Top, int Right, int Bottom)
{
    public bool Contains(ScreenPoint point) =>
        point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;
}
