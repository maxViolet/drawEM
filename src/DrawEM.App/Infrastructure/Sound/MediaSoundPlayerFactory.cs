using System.IO;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Resolves a sound to its configured file and opens a <see cref="MediaSoundPlayer"/> for it.
/// Missing assignments, missing files, and engine errors become <see cref="SoundPlaybackException"/>.
/// </summary>
public sealed class MediaSoundPlayerFactory(SoundConfiguration configuration) : ISoundPlayerFactory
{
    public ISoundPlayer Create(SoundId sound)
    {
        var path = configuration.PathOf(sound)
            ?? throw new SoundPlaybackException("Sound is not assigned to a file.");

        if (!Path.IsPathFullyQualified(path))
        {
            throw new SoundPlaybackException("Path is not absolute.");
        }

        if (!File.Exists(path))
        {
            throw new SoundPlaybackException("File not found.");
        }

        return new MediaSoundPlayer(path);
    }
}
