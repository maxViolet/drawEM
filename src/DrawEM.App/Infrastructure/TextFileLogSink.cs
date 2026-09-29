using System.IO;

namespace DrawEM.App.Infrastructure;

/// <summary>Best-effort, synchronized line append shared by application and sound logs.</summary>
internal sealed class TextFileLogSink
{
    private readonly object gate = new();
    private readonly string path;
    private readonly string directory;

    public TextFileLogSink(string path)
    {
        this.path = Path.GetFullPath(path);
        directory = Path.GetDirectoryName(this.path)
            ?? throw new ArgumentException("Log path must name a file inside a directory.", nameof(path));
    }

    public void Append(string line)
    {
        try
        {
            lock (gate)
            {
                Directory.CreateDirectory(directory);
                File.AppendAllText(path, line + Environment.NewLine);
            }
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            // Logging is best effort; failure must not interrupt playback or exit.
        }
    }
}
