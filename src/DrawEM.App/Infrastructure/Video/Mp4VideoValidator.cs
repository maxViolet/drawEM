using System.IO;
using System.Security;
using DrawEM.App.Application.Video;

namespace DrawEM.App.Infrastructure.Video;

/// <summary>
/// Reads a file's MP4 headers with <see cref="Mp4Probe"/> and applies <see cref="VideoSupportPolicy"/>.
/// It reads headers only, so a damaged or unsupported file is classified in milliseconds, far inside the
/// 30-second S5-01 budget. The file is opened read-only and shared with writers, so a file another program
/// still holds open can be checked.
/// </summary>
public sealed class Mp4VideoValidator : IVideoValidator
{
    public VideoValidationResult Validate(string path)
    {
        Mp4MediaInfo info;
        try
        {
            using var stream = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, 4096, FileOptions.RandomAccess);
            info = Mp4Probe.Read(stream);
        }
        catch (Mp4FormatException exception)
        {
            return VideoValidationResult.Rejected(exception.Failure, exception.Message);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException
            or ArgumentException or NotSupportedException or SecurityException)
        {
            return VideoValidationResult.Rejected(
                VideoValidationFailure.Unreadable, $"The file cannot be read: {exception.Message}");
        }

        return VideoSupportPolicy.Evaluate(info);
    }
}
