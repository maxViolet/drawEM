using System.Globalization;
using System.IO;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Appends sound failures to a local text log, one tab-separated line per failure.
/// <see cref="Append"/> never throws: a log that cannot be written must not end drawing or the tray app.
/// </summary>
public sealed class SoundFailureLog
{
    private const string Unknown = "-";

    private readonly TextFileLogSink sink;

    /// <exception cref="ArgumentException"><paramref name="path"/> has no parent directory.</exception>
    public SoundFailureLog(string path)
    {
        sink = new TextFileLogSink(path);
    }

    /// <summary><c>%LOCALAPPDATA%\drawEM\logs\sound.log</c> for the current Windows user.</summary>
    public static string DefaultPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "drawEM", "logs", "sound.log");

    public static string FormatLine(SoundFailure failure) => string.Join('\t',
        failure.Time.ToString("O", CultureInfo.InvariantCulture),
        "slot=" + FieldValue(failure.Slot is { } slot ? ((int)slot).ToString(CultureInfo.InvariantCulture) : null),
        "sound=" + FieldValue(failure.Sound?.Value),
        "path=" + FieldValue(failure.Path),
        "reason=" + FieldValue(failure.Reason));

    public void Append(SoundFailure failure) => sink.Append(FormatLine(failure));

    private static string FieldValue(string? value) =>
        string.IsNullOrEmpty(value) ? Unknown : value.ReplaceLineEndings(" ").Replace('\t', ' ');
}
