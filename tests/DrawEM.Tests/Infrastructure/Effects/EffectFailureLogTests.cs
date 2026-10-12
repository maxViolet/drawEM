using DrawEM.App.Domain.Effects;
using DrawEM.App.Infrastructure.Effects;

namespace DrawEM.Tests.Infrastructure.Effects;

public class EffectFailureLogTests
{
    private static readonly DateTimeOffset FailureTime = new(2026, 10, 12, 9, 30, 5, TimeSpan.Zero);

    [Fact]
    public void DefaultPath_IsDrawEMEffectLogUnderCurrentUserLocalAppData()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.StartsWith(localAppData, EffectFailureLog.DefaultPath);
        Assert.EndsWith(@"\drawEM\logs\effect.log", EffectFailureLog.DefaultPath);
    }

    [Fact]
    public void FormatLine_ContainsTimeEffectInstanceAndFlattenedReason()
    {
        var failure = new EffectFailure(FailureTime, new EffectId("confetti"), 7, "Render\tfailed\r\nat frame 3.");

        Assert.Equal(
            "2026-10-12T09:30:05.0000000+00:00\teffect=confetti\tinstance=7\treason=Render failed at frame 3.",
            EffectFailureLog.FormatLine(failure));
    }

    [Fact]
    public void Append_AddsOneLinePerFailure()
    {
        var directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
        var log = new EffectFailureLog(Path.Combine(directory, "logs", "effect.log"));
        var failure = new EffectFailure(FailureTime, new EffectId("focus-ring"), 3, "Window could not be shown.");

        try
        {
            log.Append(failure);
            log.Append(failure);

            Assert.Equal(
                [EffectFailureLog.FormatLine(failure), EffectFailureLog.FormatLine(failure)],
                File.ReadAllLines(Path.Combine(directory, "logs", "effect.log")));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void Append_WhenLogCannotBeWritten_DoesNotThrow()
    {
        var fileInPlaceOfDirectory = Path.GetTempFileName();
        var log = new EffectFailureLog(Path.Combine(fileInPlaceOfDirectory, "logs", "effect.log"));

        try
        {
            var exception = Record.Exception(() =>
                log.Append(new EffectFailure(FailureTime, new EffectId("confetti"), 1, "Render failed.")));

            Assert.Null(exception);
        }
        finally
        {
            File.Delete(fileInPlaceOfDirectory);
        }
    }
}
