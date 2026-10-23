using System.Diagnostics;
using System.IO;
using DrawEM.App.Application.Video;
using DrawEM.App.Infrastructure.Video;

namespace DrawEM.Tests.Infrastructure.Video;

public class Mp4VideoValidatorTests
{
    /// <summary>S5-01 gate: malformed and unsupported files are classified within 30 seconds each.</summary>
    private static readonly TimeSpan ValidationBudget = TimeSpan.FromSeconds(30);

    [Theory]
    [InlineData("h264-high-aac.mp4", "High", 320, 180, 25.0, "AAC-LC")]
    [InlineData("h264-main-aac-mono44k.mp4", "Main", 320, 180, 25.0, "AAC-LC")]
    [InlineData("h264-baseline-aac.mp4", "Constrained Baseline", 320, 180, 25.0, "AAC-LC")]
    [InlineData("h264-high-silent.mp4", "High", 320, 180, 25.0, null)]
    [InlineData("h264-high-1080p60-aac.mp4", "High", 1920, 1080, 60.0, "AAC-LC")]
    public void SupportedFixture_IsAcceptedWithItsProfile(
        string name, string profile, int width, int height, double frameRate, string? audioCodec)
    {
        var result = new Mp4VideoValidator().Validate(VideoFixtures.Path(name));

        Assert.True(result.IsSupported, result.Message);
        Assert.Null(result.Failure);
        var video = Assert.IsType<SupportedVideo>(result.Video);
        Assert.Equal("H.264", video.VideoCodec);
        Assert.Equal(profile, video.VideoProfile);
        Assert.Equal(width, video.Width);
        Assert.Equal(height, video.Height);
        Assert.Equal(frameRate, video.FrameRate, 2);
        Assert.Equal(audioCodec, video.AudioCodec);
    }

    [Theory]
    [InlineData("hevc-main-aac.mp4", VideoValidationFailure.UnsupportedVideo, "HEVC")]
    [InlineData("h264-high10-aac.mp4", VideoValidationFailure.UnsupportedVideo, "High 10")]
    [InlineData("h264-high-opus.mp4", VideoValidationFailure.UnsupportedAudio, "Opus")]
    [InlineData("aac-audio-only.mp4", VideoValidationFailure.NoVideoTrack, "no video track")]
    [InlineData("h264-high-aac-fragmented.mp4", VideoValidationFailure.UnsupportedContainer, "fragmented")]
    [InlineData("truncated-moov.mp4", VideoValidationFailure.Corrupt, "mdat")]
    [InlineData("not-mp4.mp4", VideoValidationFailure.NotMp4, "not an MP4")]
    public void RejectedFixture_NamesFailureClassAndCause_WithinBudget(
        string name, VideoValidationFailure failure, string cause)
    {
        var watch = Stopwatch.StartNew();
        var result = new Mp4VideoValidator().Validate(VideoFixtures.Path(name));
        watch.Stop();

        Assert.False(result.IsSupported);
        Assert.Equal(failure, result.Failure);
        Assert.Contains(cause, result.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Null(result.Video);
        Assert.True(watch.Elapsed < ValidationBudget, $"{name} took {watch.Elapsed}.");
    }

    [Fact]
    public void MissingFile_IsUnreadable()
    {
        var missing = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N") + ".mp4");

        var result = new Mp4VideoValidator().Validate(missing);

        Assert.Equal(VideoValidationFailure.Unreadable, result.Failure);
    }

    [Fact]
    public void FileLockedForWriting_IsStillReadable()
    {
        // A video still open in an editor or downloader is shared for reading; validation must not need exclusive access.
        var copy = VideoFixtures.CopyToTemp("h264-high-aac.mp4");
        try
        {
            using (new FileStream(copy, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                Assert.True(new Mp4VideoValidator().Validate(copy).IsSupported);
            }
        }
        finally
        {
            File.Delete(copy);
        }
    }
}
