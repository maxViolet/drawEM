using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>Writes playback failures to <see cref="SoundFailureLog"/> with the sound's configured path.</summary>
public sealed class LoggingSoundFailureReporter(
    SoundFailureLog log,
    SoundConfiguration configuration,
    TimeProvider time) : ISoundFailureReporter
{
    public void Report(SoundId sound, string reason) =>
        log.Append(new SoundFailure(time.GetLocalNow(), null, sound, configuration.PathOf(sound), reason));
}
