using System.Globalization;
using System.IO;

namespace DrawEM.App.Infrastructure.Updates;

/// <summary>
/// Appends update failures to a local text log, one tab-separated line per failure.
/// <see cref="Append"/> never throws: a log that cannot be written must not end the tray app.
/// </summary>
public sealed class UpdateFailureLog
{
    private readonly TextFileLogSink sink;

    /// <exception cref="ArgumentException"><paramref name="path"/> has no parent directory.</exception>
    public UpdateFailureLog(string path)
    {
        sink = new TextFileLogSink(path);
    }

    /// <summary><c>%LOCALAPPDATA%\drawEM\logs\update.log</c> for the current Windows user.</summary>
    public static string DefaultPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "drawEM", "logs", "update.log");

    public static string FormatLine(UpdateFailure failure) => string.Join('\t',
        failure.Time.ToString("O", CultureInfo.InvariantCulture),
        "operation=" + Flatten(failure.Operation),
        "reason=" + Flatten(failure.Reason));

    public void Append(UpdateFailure failure) => sink.Append(FormatLine(failure));

    private static string Flatten(string value) => value.ReplaceLineEndings(" ").Replace('\t', ' ');
}
