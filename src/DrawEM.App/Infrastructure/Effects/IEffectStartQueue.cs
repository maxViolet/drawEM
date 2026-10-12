using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// The effect side of the shortcut path. A press enqueues its start inside the hook callback and dequeues it
/// once on the dispatcher, whether or not it still starts. Call both members on the UI thread.
/// </summary>
public interface IEffectStartQueue
{
    /// <summary>Called inside the hook callback at key-down, with the cursor point captured there.</summary>
    /// <returns>The monitor epoch the start belongs to; pass it to <see cref="Dequeue"/>.</returns>
    long Enqueue(ScreenPoint point);

    /// <summary>
    /// Called on the dispatcher exactly once for each <see cref="Enqueue"/>. Starts <paramref name="command"/>
    /// only while <paramref name="monitorEpoch"/> is still current.
    /// </summary>
    /// <param name="command">The start, or <c>null</c> when it was dropped (a Save, or no monitor at the point).</param>
    /// <returns>The started instance, or <c>null</c> when nothing started.</returns>
    EffectInstance? Dequeue(long monitorEpoch, StartEffectCommand? command);
}
