using DrawEM.App.Domain.Effects;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>One effect failure: the effect, the channel's instance number, and why it failed.</summary>
public sealed record EffectFailure(DateTimeOffset Time, EffectId Effect, long Instance, string Reason);
