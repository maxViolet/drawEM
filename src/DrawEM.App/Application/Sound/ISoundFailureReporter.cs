namespace DrawEM.App.Application.Sound;

/// <summary>
/// Records a recoverable playback failure. Called on the sound thread after the failed player is
/// released. Must not throw and must not block on I/O.
/// </summary>
public interface ISoundFailureReporter
{
    void Report(SoundId sound, string reason);
}
