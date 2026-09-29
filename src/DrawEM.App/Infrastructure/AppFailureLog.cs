using System.Globalization;
using System.IO;

namespace DrawEM.App.Infrastructure;

/// <summary>
/// Queues lifecycle failures for a background writer. File I/O never holds the caller;
/// disposal drains for at most one second and may leave unwritten records behind.
/// </summary>
public sealed class AppFailureLog : IDisposable
{
    public static readonly TimeSpan DrainTimeout = BackgroundLogWriter<string>.DrainTimeout;
    private readonly BackgroundLogWriter<string> writer;

    /// <exception cref="ArgumentException"><paramref name="path"/> has no parent directory.</exception>
    public AppFailureLog(string path) : this(new TextFileLogSink(path).Append)
    {
    }

    /// <param name="writeLine">Appends a formatted line on the background writer.</param>
    public AppFailureLog(Action<string> writeLine) => writer = new BackgroundLogWriter<string>(writeLine);

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
        writer.Enqueue(FormatLine(DateTimeOffset.Now, operation, exception));
    }

    public void Dispose() => writer.Dispose();

    private static IEnumerable<Exception> Errors(Exception exception) =>
        exception is AggregateException aggregate ? aggregate.Flatten().InnerExceptions : [exception];

    private static string Flatten(string value) => value.ReplaceLineEndings(" ").Replace('\t', ' ');
}
