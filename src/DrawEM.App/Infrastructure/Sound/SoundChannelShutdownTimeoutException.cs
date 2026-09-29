namespace DrawEM.App.Infrastructure.Sound;

/// <summary>The sound thread did not finish within its shutdown budget; process exit must stop it.</summary>
public sealed class SoundChannelShutdownTimeoutException()
    : TimeoutException("The sound thread did not stop within two seconds.");
