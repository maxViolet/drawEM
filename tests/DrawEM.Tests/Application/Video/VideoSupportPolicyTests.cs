using DrawEM.App.Application.Video;

namespace DrawEM.Tests.Application.Video;

public class VideoSupportPolicyTests
{
    private const int High = 100;
    private const int Baseline = 66;
    private const int ConstraintSet1 = 0x40;
    private const int Mpeg4Audio = 0x40;
    private const int AacLc = 2;

    private static readonly Mp4VideoTrack H264High1080p60 = new("avc1", 1920, 1080, 60.0, new AvcConfiguration(High, 0, 42));
    private static readonly Mp4AudioTrack AacStereo48k = new("mp4a", 2, 48000, Mpeg4Audio, AacLc);

    [Fact]
    public void LargestSupportedVideoWithAudio_IsAccepted()
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60], [AacStereo48k]));

        Assert.True(result.IsSupported, result.Message);
        Assert.Equal(new SupportedVideo("H.264", "High", 1920, 1080, 60.0, "AAC-LC"), result.Video);
    }

    [Theory]
    [InlineData(1921, 1080)]
    [InlineData(1920, 1081)]
    [InlineData(1080, 1920)]
    public void FrameLargerThan1920x1080_IsUnsupportedVideo(int width, int height)
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60 with { Width = width, Height = height }], []));

        Assert.Equal(VideoValidationFailure.UnsupportedVideo, result.Failure);
        Assert.Contains($"{width}×{height}", result.Message);
    }

    [Theory]
    [InlineData(59.94, true)]
    [InlineData(60.0, true)]
    [InlineData(60.5, false)]
    [InlineData(120.0, false)]
    public void FrameRateAbove60_IsUnsupportedVideo(double frameRate, bool supported)
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60 with { FrameRate = frameRate }], []));

        Assert.Equal(supported, result.IsSupported);
        if (!supported)
        {
            Assert.Equal(VideoValidationFailure.UnsupportedVideo, result.Failure);
        }
    }

    [Theory]
    [InlineData(Baseline, ConstraintSet1, true, "Constrained Baseline")]
    [InlineData(Baseline, 0, false, "Baseline")]
    [InlineData(77, 0, true, "Main")]
    [InlineData(88, 0, false, "Extended")]
    [InlineData(High, 0, true, "High")]
    [InlineData(110, 0, false, "High 10")]
    [InlineData(122, 0, false, "High 4:2:2")]
    [InlineData(244, 0, false, "High 4:4:4")]
    public void H264Profiles_OnlyConstrainedBaselineMainAndHighAreSupported(
        int profile, int compatibility, bool supported, string name)
    {
        var track = H264High1080p60 with { Avc = new AvcConfiguration(profile, compatibility, 30) };

        var result = VideoSupportPolicy.Evaluate(Info([track], []));

        Assert.Equal(supported, result.IsSupported);
        Assert.Contains(name, supported ? result.Video!.VideoProfile : result.Message);
    }

    [Theory]
    [InlineData("avc3", "H.264 (avc3)")]
    [InlineData("hev1", "HEVC")]
    [InlineData("av01", "AV1")]
    [InlineData("vp09", "VP9")]
    [InlineData("encv", "encrypted")]
    [InlineData("xyz1", "xyz1")]
    public void OtherVideoSampleEntry_IsUnsupportedVideoAndNamed(string sampleEntry, string name)
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60 with { SampleEntry = sampleEntry, Avc = null }], []));

        Assert.Equal(VideoValidationFailure.UnsupportedVideo, result.Failure);
        Assert.Contains(name, result.Message);
    }

    [Fact]
    public void TwoVideoTracks_AreUnsupportedVideo()
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60, H264High1080p60], []));

        Assert.Equal(VideoValidationFailure.UnsupportedVideo, result.Failure);
    }

    [Theory]
    [InlineData(1, 44100, true)]
    [InlineData(2, 48000, true)]
    [InlineData(3, 48000, false)]
    [InlineData(6, 48000, false)]
    [InlineData(2, 32000, false)]
    [InlineData(2, 96000, false)]
    public void AacLc_IsSupportedOnlyAsMonoOrStereoAt44100Or48000(int channels, int sampleRate, bool supported)
    {
        var audio = AacStereo48k with { Channels = channels, SampleRate = sampleRate };

        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60], [audio]));

        Assert.Equal(supported, result.IsSupported);
        if (!supported)
        {
            Assert.Equal(VideoValidationFailure.UnsupportedAudio, result.Failure);
        }
    }

    [Theory]
    [InlineData("mp4a", Mpeg4Audio, 5, "HE-AAC")]
    [InlineData("mp4a", Mpeg4Audio, 29, "HE-AAC v2")]
    [InlineData("mp4a", 0x6B, null, "MP3")]
    [InlineData("Opus", null, null, "Opus")]
    [InlineData("ac-3", null, null, "AC-3")]
    [InlineData("ec-3", null, null, "E-AC-3")]
    [InlineData("fLaC", null, null, "FLAC")]
    public void OtherAudioCodec_IsUnsupportedAudioAndNamed(
        string sampleEntry, int? objectType, int? audioObjectType, string name)
    {
        var audio = new Mp4AudioTrack(sampleEntry, 2, 48000, objectType, audioObjectType);

        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60], [audio]));

        Assert.Equal(VideoValidationFailure.UnsupportedAudio, result.Failure);
        Assert.Contains(name, result.Message);
    }

    [Fact]
    public void TwoAudioTracks_AreUnsupportedAudio()
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60], [AacStereo48k, AacStereo48k]));

        Assert.Equal(VideoValidationFailure.UnsupportedAudio, result.Failure);
    }

    [Fact]
    public void NoTracks_IsNoVideoTrack()
    {
        var result = VideoSupportPolicy.Evaluate(Info([], []));

        Assert.Equal(VideoValidationFailure.NoVideoTrack, result.Failure);
    }

    [Fact]
    public void QuickTimeBrand_IsNotMp4()
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60], []) with { MajorBrand = "qt  " });

        Assert.Equal(VideoValidationFailure.NotMp4, result.Failure);
    }

    [Fact]
    public void FragmentedMovie_IsUnsupportedContainer()
    {
        var result = VideoSupportPolicy.Evaluate(Info([H264High1080p60], []) with { IsFragmented = true });

        Assert.Equal(VideoValidationFailure.UnsupportedContainer, result.Failure);
    }

    private static Mp4MediaInfo Info(Mp4VideoTrack[] video, Mp4AudioTrack[] audio) =>
        new("isom", IsFragmented: false, video, audio);
}
