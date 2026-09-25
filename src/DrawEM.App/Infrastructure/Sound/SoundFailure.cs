using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// One failed sound request. <see cref="Slot"/>, <see cref="Sound"/>, and <see cref="Path"/> are
/// <c>null</c> when the failure happened before that detail was known.
/// </summary>
public sealed record SoundFailure(
    DateTimeOffset Time,
    SoundSlot? Slot,
    SoundId? Sound,
    string? Path,
    string Reason);
