namespace DrawEM.App.Application.Video;

/// <summary>Why a file cannot be imported as a video action.</summary>
public enum VideoValidationFailure
{
    /// <summary>The file cannot be opened or read.</summary>
    Unreadable,

    /// <summary>The file is not an MP4 (ISO base media) file.</summary>
    NotMp4,

    /// <summary>The MP4 structure is damaged or incomplete.</summary>
    Corrupt,

    /// <summary>A valid MP4 layout outside the supported profile, such as a fragmented movie.</summary>
    UnsupportedContainer,

    /// <summary>The MP4 has no video track.</summary>
    NoVideoTrack,

    /// <summary>The video codec, profile, frame size, frame rate, or track count is outside the supported profile.</summary>
    UnsupportedVideo,

    /// <summary>The audio codec, channel count, sample rate, or track count is outside the supported profile.</summary>
    UnsupportedAudio,
}

/// <summary>An accepted file's playback profile.</summary>
/// <param name="AudioCodec"><c>null</c> for a silent video.</param>
public sealed record SupportedVideo(
    string VideoCodec, string VideoProfile, int Width, int Height, double FrameRate, string? AudioCodec)
{
    public override string ToString() =>
        $"{VideoCodec} {VideoProfile}, {Width}×{Height}, {FrameRate:0.##} fps, " +
        (AudioCodec is null ? "no audio" : $"{AudioCodec} audio");
}

/// <summary>
/// The outcome of checking one file against the supported MP4 profile. <see cref="Message"/> is a
/// sentence a user can read: the accepted profile, or what was rejected and why.
/// </summary>
public sealed class VideoValidationResult
{
    private VideoValidationResult(VideoValidationFailure? failure, string message, SupportedVideo? video)
    {
        Failure = failure;
        Message = message;
        Video = video;
    }

    public bool IsSupported => Failure is null;

    public VideoValidationFailure? Failure { get; }

    public string Message { get; }

    /// <summary>The accepted profile; <c>null</c> when rejected.</summary>
    public SupportedVideo? Video { get; }

    public static VideoValidationResult Supported(SupportedVideo video) => new(null, video.ToString(), video);

    public static VideoValidationResult Rejected(VideoValidationFailure failure, string message) => new(failure, message, null);

    public override string ToString() => IsSupported ? $"Supported: {Message}" : $"{Failure}: {Message}";
}
