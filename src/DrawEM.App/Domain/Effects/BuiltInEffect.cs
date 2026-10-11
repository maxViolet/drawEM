namespace DrawEM.App.Domain.Effects;

/// <summary>
/// One catalog entry. <see cref="ResourceName"/> names the embedded Lottie file that Presentation loads;
/// it is never a path.
/// </summary>
public sealed record BuiltInEffect(EffectId Id, string DisplayName, EffectPlacement Placement, string ResourceName);
