namespace DrawEM.App.Application.Sound;

/// <summary>
/// Request to play one configured sound on the global sound channel. <see cref="Path"/> is the managed copy
/// resolved from the configuration that built the request's shortcut binding, so a request queued before a
/// Save plays and reports that copy, never one resolved under a newer configuration.
/// </summary>
public sealed record PlaySoundCommand(SoundId Sound, string Path);
