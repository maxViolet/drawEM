using System.Globalization;
using System.IO;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Appends effect failures to a local text log, one tab-separated line per failure.
/// <see cref="Append"/> never throws: a log that cannot be written must not end drawing or the tray app.
/// </summary>
public sealed class EffectFailureLog
{
    private readonly TextFileLogSink sink;

    /// <exception cref="ArgumentException"><paramref name="path"/> has no parent directory.</exception>
    public EffectFailureLog(string path)
    {
        sink = new TextFileLogSink(path);
    }

    /// <summary><c>%LOCALAPPDATA%\drawEM\logs\effect.log</c> for the current Windows user.</summary>
    public static string DefaultPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "drawEM", "logs", "effect.log");

    public static string FormatLine(EffectFailure failure) => string.Join('\t',
        failure.Time.ToString("O", CultureInfo.InvariantCulture),
        "effect=" + failure.Effect.Value,
        "instance=" + failure.Instance.ToString(CultureInfo.InvariantCulture),
        "reason=" + failure.Reason.ReplaceLineEndings(" ").Replace('\t', ' '));

    public void Append(EffectFailure failure) => sink.Append(FormatLine(failure));
}
