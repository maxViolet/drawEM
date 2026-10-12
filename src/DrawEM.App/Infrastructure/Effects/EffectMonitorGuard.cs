using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Stops the effect when the cursor enters another monitor, whether drawing is active or not. While an effect
/// runs or a start is queued, each pointer move from the hook and each animation frame's cursor check is tested
/// against the cached bounds of the cursor's monitor. Only a point outside them costs a monitor lookup, and
/// only entering another monitor queues UI work. A point outside every monitor is an outside desktop edge, not
/// another monitor. With nothing running or queued, pointer moves cost two field reads.
/// </summary>
/// <remarks>
/// The monitor epoch advances on each transition and each display topology change, so a monitor is identified
/// by its bounds within one topology, not by bounds alone. Every effect and queued start carries the epoch of
/// its press; a later epoch stops it or drops it. Because the epoch only grows, A -> B -> A movement still
/// stops an effect started on A, however late the UI runs the stop. Call every member on the UI thread, which
/// also runs the low-level hooks.
/// </remarks>
public sealed class EffectMonitorGuard : IEffectStartQueue
{
    private readonly IMonitorBoundsSource monitors;
    private readonly ICursorPositionSource cursor;
    private readonly EffectChannelController channel;
    private readonly Action<Action> dispatch;
    private long epoch;
    private int queuedStarts;

    /// <summary>
    /// The press point that started observing; its monitor is looked up only when first needed, and again after
    /// a display change.
    /// </summary>
    private ScreenPoint anchor;

    private bool monitorKnown;

    /// <summary>The cursor's monitor, or <c>null</c> when the anchor lies outside every monitor.</summary>
    private MonitorBounds? monitor;

    public EffectMonitorGuard(
        IMouseHookSource mouse,
        IDisplayChangeSource displays,
        IMonitorBoundsSource monitors,
        ICursorPositionSource cursor,
        EffectChannelController channel,
        Action<Action> dispatch)
    {
        this.monitors = monitors;
        this.cursor = cursor;
        this.channel = channel;
        this.dispatch = dispatch;
        mouse.PointerMoved += OnPointerMoved;
        displays.DisplayChanged += OnDisplayChanged;
    }

    /// <remarks>
    /// With nothing running or queued, does no monitor lookup: the press point becomes the cursor's last known
    /// position, resolved only when first needed. Otherwise the press point is checked like a pointer move, so
    /// a press on another monitor stops the running effect even when the hook reported no move there, and the
    /// new start gets the epoch after that transition.
    /// </remarks>
    public long Enqueue(ScreenPoint point)
    {
        if (IsObserving())
        {
            Observe(point);
        }
        else
        {
            anchor = point;
            monitorKnown = false;
        }

        queuedStarts++;
        return epoch;
    }

    public EffectInstance? Dequeue(long monitorEpoch, StartEffectCommand? command)
    {
        queuedStarts--;
        return command is not null && monitorEpoch == epoch ? channel.Start(command) : null;
    }

    /// <summary>
    /// The per-frame backstop for movement the hook did not report. Reads the cursor only while an effect
    /// runs or a start is queued.
    /// </summary>
    public void CheckCursor()
    {
        if (IsObserving() && cursor.TryGetCurrentPosition(out var point))
        {
            Observe(point);
        }
    }

    private void OnPointerMoved(ScreenPoint point)
    {
        if (IsObserving())
        {
            Observe(point);
        }
    }

    private bool IsObserving() => queuedStarts > 0 || channel.ActiveInstance is not null;

    private void Observe(ScreenPoint point)
    {
        if (!monitorKnown)
        {
            monitor = monitors.TryGetBounds(anchor, out var pressed) ? pressed : null;
            monitorKnown = true;
        }

        if (monitor is { } current && current.Contains(point))
        {
            return;
        }

        if (!monitors.TryGetBounds(point, out var entered))
        {
            return;
        }

        monitor = entered;
        var reached = ++epoch;
        dispatch(() => channel.StopStartedBefore(reached));
    }

    /// <summary>
    /// Stops the effect and invalidates queued starts. The next effect gets a new window placed on the new
    /// layout, and the cursor's monitor is looked up again.
    /// </summary>
    private void OnDisplayChanged()
    {
        monitorKnown = false;
        channel.StopStartedBefore(++epoch);
    }
}
