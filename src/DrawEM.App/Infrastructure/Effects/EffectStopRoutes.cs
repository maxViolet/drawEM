namespace DrawEM.App.Infrastructure.Effects;

/// <summary>Routes the app events that stop effects without ending the app to <see cref="EffectMonitorGuard"/>.</summary>
public static class EffectStopRoutes
{
    /// <summary>Tray Stop effects, a session lock, and a system suspend each stop the effect and drop queued starts.</summary>
    public static void Connect(TrayApplication tray, ISessionInterruptionSource sessions, EffectMonitorGuard guard)
    {
        tray.StopEffectsRequested += guard.StopEffects;
        sessions.Interrupted += guard.StopEffects;
    }
}
