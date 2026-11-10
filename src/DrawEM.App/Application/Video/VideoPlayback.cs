namespace DrawEM.App.Application.Video;

/// <summary>What the media engine reported when it opened a file.</summary>
/// <param name="Duration"><c>null</c> when the engine reports no fixed duration.</param>
public sealed record VideoOpened(bool HasVideo, bool HasAudio, int Width, int Height, TimeSpan? Duration);

/// <summary>
/// The media engine could not open or decode a file. Raised as an event, never thrown from a callback, so
/// a failed video cannot end drawEM. <see cref="HResult"/> is the engine's error code, for example
/// <c>0xC00D11B1</c> when the Windows decoder rejects the stream.
/// </summary>
public sealed record VideoPlaybackFailure(string Message, int HResult);

/// <summary>A video player cannot be created, controlled, or released. Recoverable.</summary>
public sealed class VideoPlaybackException : Exception
{
    public VideoPlaybackException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
