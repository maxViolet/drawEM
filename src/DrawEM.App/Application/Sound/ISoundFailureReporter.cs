namespace DrawEM.App.Application.Sound;

/// <summary>
/// Records a recoverable playback failure. Called on the sound thread after the failed player is
/// released. Must not throw and must not block on I/O.
/// </summary>
public interface ISoundFailureReporter
{
    /// <param name="command">The failed request; its <see cref="PlaySoundCommand.Path"/> names the file.</param>
    void Report(PlaySoundCommand command, string reason);
}
