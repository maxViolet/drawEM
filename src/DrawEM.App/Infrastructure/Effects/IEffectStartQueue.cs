using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// The effect start path for shortcuts and Settings samples. A shortcut press enqueues its start inside the hook
/// callback and dequeues it later on the dispatcher; a sample enqueues and dequeues in one button click. Each
/// start is dequeued once, whether or not it still starts. Call both members on the UI thread.
/// </summary>
public interface IEffectStartQueue
{
    /// <summary>
    /// Called with the cursor point captured for the start: inside the hook callback at key-down, or at a
    /// Sample click.
    /// </summary>
    /// <returns>The monitor epoch the start belongs to; pass it to <see cref="Dequeue"/>.</returns>
    long Enqueue(ScreenPoint point);

    /// <summary>
    /// Called on the UI thread exactly once for each <see cref="Enqueue"/>. Starts <paramref name="command"/>
    /// only while <paramref name="monitorEpoch"/> is still current.
    /// </summary>
    /// <param name="command">The start, or <c>null</c> when it was dropped (a Save before a shortcut start was dispatched, or no monitor at the point).</param>
    void Dequeue(long monitorEpoch, StartEffectCommand? command);
}
