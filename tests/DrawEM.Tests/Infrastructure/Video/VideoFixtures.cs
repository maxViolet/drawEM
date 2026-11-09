using System.IO;

namespace DrawEM.Tests.Infrastructure.Video;

/// <summary>The checked-in MP4 files from tools/video-fixtures/New-VideoFixtures.ps1, copied to the test output.</summary>
internal static class VideoFixtures
{
    public static string Path(string name) => System.IO.Path.Combine(AppContext.BaseDirectory, "Fixtures", "Video", name);

    /// <summary>A private copy the test may delete; the caller deletes it.</summary>
    public static string CopyToTemp(string name)
    {
        var directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "drawEM-tests");
        Directory.CreateDirectory(directory);
        var copy = System.IO.Path.Combine(directory, Guid.NewGuid().ToString("N") + "-" + name);
        File.Copy(Path(name), copy);
        return copy;
    }
}
