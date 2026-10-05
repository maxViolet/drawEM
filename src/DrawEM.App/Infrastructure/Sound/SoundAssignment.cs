using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// A v2 code-owned sound: its identifier and an external local WAV or MP3 path. Only the retired
/// <c>SoundAssignments</c> file uses it; the running app plays managed copies named by saved settings.
/// </summary>
public sealed record SoundAssignment(SoundId Id, string Path);
