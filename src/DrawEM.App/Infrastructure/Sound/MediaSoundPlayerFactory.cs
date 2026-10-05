using System.IO;
using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Opens a <see cref="MediaSoundPlayer"/> for the managed copy a request carries. A missing copy and engine
/// errors become <see cref="SoundPlaybackException"/>.
/// </summary>
public sealed class MediaSoundPlayerFactory : ISoundPlayerFactory
{
    public ISoundPlayer Create(PlaySoundCommand command)
    {
        if (!File.Exists(command.Path))
        {
            throw new SoundPlaybackException("File not found.");
        }

        return new MediaSoundPlayer(command.Path);
    }
}
