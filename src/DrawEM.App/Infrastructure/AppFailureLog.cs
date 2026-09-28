using System.Globalization;
using System.IO;

namespace DrawEM.App.Infrastructure;

/// <summary>
/// Appends application lifecycle failures, such as cleanup errors at exit, to a local text log,
/// one tab-separated line per failure. <see cref="Append"/> never throws: a log that cannot be
/// written must not stop the application from exiting.
/// </summary>
public sealed class AppFailureLog
{
    private readonly object gate = new();
    private readonly string path;
    private readonly string directory;

    /// <exception cref="ArgumentException"><paramref name="path"/> has no parent directory.</exception>
    public AppFailureLog(string path)
    {
        this.path = Path.GetFullPath(path);
        directory = Path.GetDirectoryName(this.path)
            ?? throw new ArgumentException("Log path must name a file inside a directory.", nameof(path));
    }

    /// <summary><c>%LOCALAPPDATA%\drawEM\logs\app.log</c> for the current Windows user.</summary>
    public static string DefaultPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "drawEM", "logs", "app.log");

    public static string FormatLine(DateTimeOffset time, string operation, Exception exception) => string.Join('\t',
        time.ToString("O", CultureInfo.InvariantCulture),
        "operation=" + Flatten(operation),
        "error=" + string.Join(" | ", Errors(exception).Select(error => Flatten($"{error.GetType()}: {error.Message}"))));

    public void Append(string operation, Exception exception)
    {
        var line = FormatLine(DateTimeOffset.Now, operation, exception) + Environment.NewLine;

        try
        {
            lock (gate)
            {
                Directory.CreateDirectory(directory);
                File.AppendAllText(path, line);
            }
        }
        catch (Exception writeFailure) when (writeFailure is IOException or UnauthorizedAccessException)
        {
            // Logging is best effort; the application is already on its way out.
        }
    }

    private static IEnumerable<Exception> Errors(Exception exception) =>
        exception is AggregateException aggregate ? aggregate.Flatten().InnerExceptions : [exception];

    private static string Flatten(string value) => value.ReplaceLineEndings(" ").Replace('\t', ' ');
}
