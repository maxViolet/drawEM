using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// One sound failure. <see cref="Sound"/> and <see cref="Path"/> are <c>null</c> when the failure is not
/// about one sound or one copy.
/// </summary>
public sealed record SoundFailure(
    DateTimeOffset Time,
    SoundId? Sound,
    string? Path,
    string Reason);
