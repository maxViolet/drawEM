namespace DrawEM.App.Application.Video;

/// <summary>
/// What an MP4 header declares: the <c>ftyp</c> major brand, whether the movie is fragmented
/// (<c>mvex</c> in <c>moov</c>), and every video and audio track in file order. Other track kinds are omitted.
/// </summary>
public sealed record Mp4MediaInfo(
    string MajorBrand,
    bool IsFragmented,
    IReadOnlyList<Mp4VideoTrack> VideoTracks,
    IReadOnlyList<Mp4AudioTrack> AudioTracks);

/// <param name="SampleEntry">The four-character code of the first <c>stsd</c> entry, for example <c>avc1</c>.</param>
/// <param name="Width">Coded width from the visual sample entry.</param>
/// <param name="Height">Coded height from the visual sample entry.</param>
/// <param name="FrameRate">Average frames per second: samples × media timescale ÷ media duration; 0 when unknown.</param>
/// <param name="Avc">The <c>avcC</c> header for H.264 entries; <c>null</c> otherwise or when absent.</param>
public sealed record Mp4VideoTrack(string SampleEntry, int Width, int Height, double FrameRate, AvcConfiguration? Avc);

/// <summary>The first bytes of an <c>avcC</c> box (ISO/IEC 14496-15).</summary>
/// <param name="Profile">AVCProfileIndication, the H.264 <c>profile_idc</c>.</param>
/// <param name="ProfileCompatibility">The constraint-set flags byte.</param>
/// <param name="Level">AVCLevelIndication, the H.264 <c>level_idc</c>.</param>
public sealed record AvcConfiguration(int Profile, int ProfileCompatibility, int Level)
{
    /// <summary><c>constraint_set1_flag</c>: with profile 66 it marks Constrained Baseline.</summary>
    public bool ConstraintSet1 => (ProfileCompatibility & 0x40) != 0;
}

/// <param name="SampleEntry">The four-character code of the first <c>stsd</c> entry, for example <c>mp4a</c>.</param>
/// <param name="ObjectTypeIndication">The <c>esds</c> objectTypeIndication; <c>null</c> without <c>esds</c>.</param>
/// <param name="AudioObjectType">The AudioSpecificConfig audioObjectType for MPEG-4 audio; otherwise <c>null</c>.</param>
public sealed record Mp4AudioTrack(
    string SampleEntry, int Channels, int SampleRate, int? ObjectTypeIndication, int? AudioObjectType);
