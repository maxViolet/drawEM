namespace DrawEM.App.Application.Sound;

/// <summary>
/// Request to play one configured sound on the global sound channel.
/// The playback side looks up the file path; the command only names the sound.
/// </summary>
public sealed record PlaySoundCommand(SoundId Sound);
