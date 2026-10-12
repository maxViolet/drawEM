using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Effects;

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

/// <summary>Reports a settable cursor position, or none while unavailable, and counts the reads.</summary>
internal sealed class SettableCursor : ICursorPositionSource
{
    public ScreenPoint Position { get; set; } = new(100, 100);

    public bool Available { get; set; } = true;

    public int Reads { get; private set; }

    public bool TryGetCurrentPosition(out ScreenPoint position)
    {
        Reads++;
        position = Position;
        return Available;
    }
}

/// <summary>Finds the first monitor that contains the point, counts the queries, and can swap the layout.</summary>
internal sealed class CountingMonitors(params MonitorBounds[] monitors) : IMonitorBoundsSource
{
    private FakeMonitorBoundsSource inner = new(monitors);

    public int Queries { get; private set; }

    public void Replace(params MonitorBounds[] layout) => inner = new FakeMonitorBoundsSource(layout);

    public bool TryGetBounds(ScreenPoint point, out MonitorBounds bounds)
    {
        Queries++;
        return inner.TryGetBounds(point, out bounds);
    }
}

/// <summary>Raises pointer moves and asks the button and wheel handlers whether to hide the event.</summary>
internal sealed class FakeMouseHookSource : IMouseHookSource
{
    public event Action<ScreenPoint>? PointerMoved;

    public event Func<bool>? PointerButtonActivity;

    public event Func<bool>? PointerWheelActivity;

    public void Move(ScreenPoint point) => PointerMoved?.Invoke(point);

    public bool ShouldSuppressPointerButton() =>
        PointerButtonActivity?
            .GetInvocationList()
            .Cast<Func<bool>>()
            .Any(handler => handler())
        ?? false;

    public bool ShouldSuppressPointerWheel() =>
        PointerWheelActivity?
            .GetInvocationList()
            .Cast<Func<bool>>()
            .Any(handler => handler())
        ?? false;
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

/// <summary>
/// Records effect starts as the shortcut path queues and dequeues them. Each enqueue returns
/// <see cref="NextEpoch"/>; <see cref="Started"/> holds every dequeued command that was not dropped.
/// </summary>
internal sealed class FakeEffectStartQueue : IEffectStartQueue
{
    public const long FirstEpoch = 10;

    public long NextEpoch { get; set; } = FirstEpoch;

    public List<ScreenPoint> Enqueued { get; } = [];

    public List<(long Epoch, StartEffectCommand? Command)> Dequeued { get; } = [];

    public List<StartEffectCommand> Started { get; } = [];

    public long Enqueue(ScreenPoint point)
    {
        Enqueued.Add(point);
        return NextEpoch;
    }

    public void Dequeue(long monitorEpoch, StartEffectCommand? command)
    {
        Dequeued.Add((monitorEpoch, command));
        if (command is not null)
        {
            Started.Add(command);
        }
    }
}
