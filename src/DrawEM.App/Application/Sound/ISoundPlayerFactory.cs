namespace DrawEM.App.Application.Sound;

/// <summary>Opens a player for one configured sound. The implementation owns path lookup and the playback engine.</summary>
public interface ISoundPlayerFactory
{
    /// <exception cref="SoundPlaybackException">The sound has no path, or its file cannot be opened.</exception>
    ISoundPlayer Create(SoundId sound);
}
