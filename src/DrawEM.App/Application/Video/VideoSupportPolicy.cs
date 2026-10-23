namespace DrawEM.App.Application.Video;

/// <summary>
/// The MP4 profile S5-01 proved on Windows through WPF <c>MediaPlayer</c>
/// (docs/v5/step-01-video-decoder-and-formats/acceptance.md). Every limit here is the edge of the tested
/// fixture matrix, not of what Windows might play:
/// <list type="bullet">
/// <item>a non-fragmented ISO MP4 (any major brand except QuickTime <c>qt  </c>) whose tracks each have one
/// sample description (the infrastructure probe rejects more before this policy runs);</item>
/// <item>exactly one H.264 <c>avc1</c> video track: Constrained Baseline, Main, or High (8-bit 4:2:0),
/// at most 1920×1080 (landscape) and 60 fps;</item>
/// <item>no audio, or exactly one AAC-LC <c>mp4a</c> track, mono or stereo, at 44.1 or 48 kHz.</item>
/// </list>
/// </summary>
public static class VideoSupportPolicy
{
    public const int MaxWidth = 1920;
    public const int MaxHeight = 1080;
    public const int MaxFrameRate = 60;

    private const int ProfileBaseline = 66;
    private const int ProfileMain = 77;
    private const int ProfileHigh = 100;
    private const int Mpeg4Audio = 0x40;
    private const int AacLc = 2;

    private const string VideoLimits =
        "drawEM plays H.264 Constrained Baseline, Main, or High video up to 1920×1080 at 60 fps.";

    private const string AudioLimits =
        "drawEM plays AAC-LC audio in mono or stereo at 44.1 or 48 kHz, or video without sound.";

    public static VideoValidationResult Evaluate(Mp4MediaInfo info)
    {
        if (info.MajorBrand == "qt  ")
        {
            return Reject(VideoValidationFailure.NotMp4, "The file is a QuickTime movie, not an MP4 video.");
        }

        if (info.IsFragmented)
        {
            return Reject(VideoValidationFailure.UnsupportedContainer, "Fragmented MP4 files are not supported.");
        }

        if (info.VideoTracks.Count == 0)
        {
            return Reject(VideoValidationFailure.NoVideoTrack, "The MP4 file has no video track.");
        }

        if (info.VideoTracks.Count > 1)
        {
            return Reject(VideoValidationFailure.UnsupportedVideo,
                $"The MP4 file has {info.VideoTracks.Count} video tracks; drawEM plays files with one.");
        }

        if (info.AudioTracks.Count > 1)
        {
            return Reject(VideoValidationFailure.UnsupportedAudio,
                $"The MP4 file has {info.AudioTracks.Count} audio tracks; drawEM plays files with at most one.");
        }

        var video = info.VideoTracks[0];
        if (CheckVideo(video) is { } videoFailure)
        {
            return videoFailure;
        }

        string? audioCodec = null;
        if (info.AudioTracks.Count == 1)
        {
            var audio = info.AudioTracks[0];
            if (CheckAudio(audio) is { } audioFailure)
            {
                return audioFailure;
            }

            audioCodec = AudioName(audio);
        }

        return VideoValidationResult.Supported(new SupportedVideo(
            "H.264", ProfileName(video.Avc!), video.Width, video.Height, video.FrameRate, audioCodec));
    }

    private static VideoValidationResult? CheckVideo(Mp4VideoTrack video)
    {
        if (video.SampleEntry != "avc1")
        {
            return Reject(VideoValidationFailure.UnsupportedVideo,
                $"The video codec {VideoName(video.SampleEntry)} is not supported. {VideoLimits}");
        }

        if (video.Avc is not { } avc)
        {
            return Reject(VideoValidationFailure.Corrupt, "The H.264 track has no avcC decoder configuration.");
        }

        var supportedProfile = avc.Profile switch
        {
            ProfileBaseline => avc.ConstraintSet1,
            ProfileMain or ProfileHigh => true,
            _ => false,
        };
        if (!supportedProfile)
        {
            return Reject(VideoValidationFailure.UnsupportedVideo,
                $"H.264 {ProfileName(avc)} video (profile {avc.Profile}) is not supported. {VideoLimits}");
        }

        if (video.Width > MaxWidth || video.Height > MaxHeight)
        {
            return Reject(VideoValidationFailure.UnsupportedVideo,
                $"Video frames of {video.Width}×{video.Height} are larger than {MaxWidth}×{MaxHeight}. {VideoLimits}");
        }

        if (video.FrameRate <= 0)
        {
            return Reject(VideoValidationFailure.Corrupt, "The video track has no frames.");
        }

        // One decimal absorbs the rounding of an average taken from integer timestamps (60 fps may read 60.02).
        if (Math.Round(video.FrameRate, 1) > MaxFrameRate)
        {
            return Reject(VideoValidationFailure.UnsupportedVideo,
                $"A frame rate of {video.FrameRate:0.##} fps is above {MaxFrameRate} fps. {VideoLimits}");
        }

        return null;
    }

    private static VideoValidationResult? CheckAudio(Mp4AudioTrack audio)
    {
        var name = AudioName(audio);
        if (name != "AAC-LC")
        {
            return Reject(VideoValidationFailure.UnsupportedAudio,
                $"The audio codec {name} is not supported. {AudioLimits}");
        }

        if (audio.Channels is not (1 or 2) || audio.SampleRate is not (44100 or 48000))
        {
            return Reject(VideoValidationFailure.UnsupportedAudio,
                $"AAC-LC audio with {audio.Channels} channels at {audio.SampleRate} Hz is not supported. {AudioLimits}");
        }

        return null;
    }

    private static string ProfileName(AvcConfiguration avc) => avc.Profile switch
    {
        ProfileBaseline => avc.ConstraintSet1 ? "Constrained Baseline" : "Baseline",
        ProfileMain => "Main",
        88 => "Extended",
        ProfileHigh => "High",
        110 => "High 10",
        122 => "High 4:2:2",
        244 => "High 4:4:4 Predictive",
        44 => "CAVLC 4:4:4 Intra",
        _ => $"profile {avc.Profile}",
    };

    private static string VideoName(string sampleEntry) => sampleEntry switch
    {
        "avc1" => "H.264",
        "avc3" => "H.264 (avc3)",
        "hvc1" or "hev1" => "HEVC",
        "av01" => "AV1",
        "vp09" => "VP9",
        "vp08" => "VP8",
        "mp4v" => "MPEG-4 Part 2",
        "encv" => "encrypted video",
        _ => $"'{sampleEntry}'",
    };

    private static string AudioName(Mp4AudioTrack audio) => audio.SampleEntry switch
    {
        "mp4a" => audio.ObjectTypeIndication switch
        {
            Mpeg4Audio => audio.AudioObjectType switch
            {
                AacLc => "AAC-LC",
                1 => "AAC Main",
                5 => "HE-AAC",
                29 => "HE-AAC v2",
                { } other => $"MPEG-4 audio object type {other}",
                null => "MPEG-4 audio without a decoder configuration",
            },
            0x66 or 0x67 or 0x68 => "MPEG-2 AAC",
            0x69 or 0x6B => "MP3",
            { } other => $"MPEG-4 object type 0x{other:X2}",
            null => "'mp4a' without esds",
        },
        "Opus" => "Opus",
        "ac-3" => "AC-3",
        "ec-3" => "E-AC-3",
        "fLaC" => "FLAC",
        "alac" => "ALAC",
        "enca" => "encrypted audio",
        _ => $"'{audio.SampleEntry}'",
    };

    private static VideoValidationResult Reject(VideoValidationFailure failure, string message) =>
        VideoValidationResult.Rejected(failure, message);
}
