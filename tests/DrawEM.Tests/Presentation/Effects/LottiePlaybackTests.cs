using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

public sealed class LottiePlaybackTests
{
    [Theory]
    [InlineData(0, 2, false, 0, "Playing")]
    [InlineData(1.5, 2, false, 1.5, "Playing")]
    [InlineData(2, 2, false, 2, "Completed")]
    [InlineData(5.5, 2, false, 5.5, "Completed")]
    [InlineData(2, 2, true, 0, "Playing")]
    [InlineData(5.5, 2, true, 1.5, "Playing")]
    [InlineData(0, 0, false, 0, "Completed")]
    [InlineData(0, 0, true, 0, "CannotLoop")]
    [InlineData(3, 0, true, 3, "CannotLoop")]
    public void PlaybackPosition_LoopsFromFrameZeroOrEnds(
        double elapsedSeconds,
        double durationSeconds,
        bool loop,
        double expectedSeconds,
        string state)
    {
        var result = LottiePlayback.PlaybackPosition(
            TimeSpan.FromSeconds(elapsedSeconds),
            TimeSpan.FromSeconds(durationSeconds),
            loop);

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), result.Position);
        Assert.Equal(state, result.State.ToString());
    }
}
