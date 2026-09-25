namespace DrawEM.App.Application.Sound;

/// <summary>Records a recoverable playback failure. Must not throw.</summary>
public interface ISoundFailureReporter
{
    void Report(SoundId sound, string reason);
}
