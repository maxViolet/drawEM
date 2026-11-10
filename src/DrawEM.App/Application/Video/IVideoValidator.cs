namespace DrawEM.App.Application.Video;

/// <summary>Checks a local file against the supported MP4 profile before it is imported.</summary>
public interface IVideoValidator
{
    /// <summary>Reads only the file's headers. Never throws for a bad or missing file; the result says why.</summary>
    VideoValidationResult Validate(string path);
}
