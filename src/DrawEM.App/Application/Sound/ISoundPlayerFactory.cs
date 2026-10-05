namespace DrawEM.App.Application.Sound;

/// <summary>Opens a player for one requested sound. The implementation owns the playback engine.</summary>
public interface ISoundPlayerFactory
{
    /// <summary>Opens <see cref="PlaySoundCommand.Path"/>; never resolves the sound against other settings.</summary>
    /// <exception cref="SoundPlaybackException">The file cannot be opened.</exception>
    ISoundPlayer Create(PlaySoundCommand command);
}
