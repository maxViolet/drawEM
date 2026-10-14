using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

public sealed class LottiePlaybackTests
{
    [Theory]
    [InlineData(0, 2, false, 0, false)]
    [InlineData(1.5, 2, false, 1.5, false)]
    [InlineData(2, 2, false, 2, true)]
    [InlineData(5.5, 2, false, 5.5, true)]
    [InlineData(2, 2, true, 0, false)]
    [InlineData(5.5, 2, true, 1.5, false)]
    public void PlaybackPosition_LoopsFromFrameZeroOrCompletes(
        double elapsedSeconds,
        double durationSeconds,
        bool loop,
        double expectedSeconds,
        bool completed)
    {
        var result = LottiePlayback.PlaybackPosition(
            TimeSpan.FromSeconds(elapsedSeconds),
            TimeSpan.FromSeconds(durationSeconds),
            loop);

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), result.Position);
        Assert.Equal(completed, result.Completed);
    }
}
