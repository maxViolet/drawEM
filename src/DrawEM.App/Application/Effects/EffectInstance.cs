namespace DrawEM.App.Application.Effects;

/// <summary>Identity of one effect invocation. Unique within one effect channel.</summary>
public readonly record struct EffectInstanceId(long Value);

/// <summary>One invocation of one effect: its identity, its request, and when the channel started it.</summary>
public sealed record EffectInstance(EffectInstanceId Id, StartEffectCommand Command, DateTimeOffset StartedAt);
