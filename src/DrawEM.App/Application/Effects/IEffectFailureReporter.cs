namespace DrawEM.App.Application.Effects;

/// <summary>
/// Records a recoverable effect failure. Called on the controller thread after the failed playback is
/// released. Must not throw and must not block on I/O.
/// </summary>
public interface IEffectFailureReporter
{
    void Report(EffectInstance instance, string reason);
}
