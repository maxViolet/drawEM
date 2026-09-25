using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// One sound assigned to a slot: its identifier and the external local WAV or MP3 path.
/// The file stays where it is; drawEM does not import or copy it.
/// </summary>
public sealed record SoundAssignment(SoundId Id, string Path);
