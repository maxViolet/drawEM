using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

/// <summary>Reports one fixed cursor position.</summary>
internal sealed class FakeCursorPositionSource(ScreenPoint point) : ICursorPositionSource
{
    public bool TryGetCurrentPosition(out ScreenPoint position)
    {
        position = point;
        return true;
    }
}

/// <summary>Finds the first monitor that contains the point.</summary>
internal sealed class FakeMonitorBoundsSource(params MonitorBounds[] monitors) : IMonitorBoundsSource
{
    public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
    {
        foreach (var monitor in monitors)
        {
            if (monitor.Contains(point))
            {
                bounds = monitor;
                return true;
            }
        }

        bounds = default;
        return false;
    }
}

/// <summary>Delivers key events to the installed handler; emitting a neutral key does nothing.</summary>
internal sealed class FakeKeyboardHookSource : IKeyboardHookSource, INeutralKeyEmitter
{
    private Func<int, KeyDirection, bool>? handleKey;

    public void SetKeyHandler(Func<int, KeyDirection, bool> handler) => handleKey = handler;

    /// <returns>Whether the handler hid the key-down.</returns>
    public bool PressKey(int vkCode) => handleKey?.Invoke(vkCode, KeyDirection.Down) ?? false;

    /// <returns>Whether the handler hid the key-up.</returns>
    public bool ReleaseKey(int vkCode) => handleKey?.Invoke(vkCode, KeyDirection.Up) ?? false;

    public void Press(params int[] keys)
    {
        foreach (var key in keys)
        {
            PressKey(key);
        }
    }

    public void Release(params int[] keys)
    {
        foreach (var key in keys)
        {
            ReleaseKey(key);
        }
    }

    public void EmitNeutralKey()
    {
    }
}
