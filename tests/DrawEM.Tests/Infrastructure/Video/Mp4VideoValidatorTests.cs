using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Text;
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

    [Fact]
    public void TrackWithSecondSampleDescription_IsUnsupportedContainer()
    {
        // Only the first description is checked against the profile; samples may switch to a later one,
        // so a supported first entry must not stand for the whole track.
        var path = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N") + ".mp4");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, WithFirstSampleDescriptionTwice(File.ReadAllBytes(VideoFixtures.Path("h264-high-aac.mp4"))));
        try
        {
            var result = new Mp4VideoValidator().Validate(path);

            Assert.Equal(VideoValidationFailure.UnsupportedContainer, result.Failure);
            Assert.Contains("sample descriptions", result.Message);
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Copies the first <c>stsd</c> entry in the file after itself, raising the entry count and every enclosing
    /// box size. The fixture keeps <c>moov</c> after <c>mdat</c>, so no chunk offset moves.
    /// </summary>
    private static byte[] WithFirstSampleDescriptionTwice(byte[] file)
    {
        var enclosing = new List<int>();
        Assert.True(FindStsd(file, 0, file.Length, enclosing));
        var stsd = enclosing[^1];
        var entry = stsd + 16;
        var entrySize = (int)BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(entry));

        var patched = new byte[file.Length + entrySize];
        file.AsSpan(0, entry + entrySize).CopyTo(patched);
        file.AsSpan(entry, entrySize).CopyTo(patched.AsSpan(entry + entrySize));
        file.AsSpan(entry + entrySize).CopyTo(patched.AsSpan(entry + 2 * entrySize));
        BinaryPrimitives.WriteUInt32BigEndian(patched.AsSpan(stsd + 12), BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(stsd + 12)) + 1);
        foreach (var box in enclosing)
        {
            BinaryPrimitives.WriteUInt32BigEndian(patched.AsSpan(box), BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(box)) + (uint)entrySize);
        }

        return patched;
    }

    /// <summary>Collects the offsets of the boxes from the top level down to the first <c>stsd</c>, inclusive.</summary>
    private static bool FindStsd(byte[] file, int start, int end, List<int> enclosing)
    {
        for (var at = start; at + 8 <= end;)
        {
            var size = (int)BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(at));
            var type = Encoding.ASCII.GetString(file, at + 4, 4);
            enclosing.Add(at);
            if (type == "stsd"
                || (type is "moov" or "trak" or "mdia" or "minf" or "stbl" && FindStsd(file, at + 8, at + size, enclosing)))
            {
                return true;
            }

            enclosing.RemoveAt(enclosing.Count - 1);
            at += size;
        }

        return false;
    }
}
