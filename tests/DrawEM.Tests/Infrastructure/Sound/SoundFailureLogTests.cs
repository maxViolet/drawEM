using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class SoundFailureLogTests
{
    private static readonly DateTimeOffset FailureTime = new(2026, 9, 25, 14, 30, 5, TimeSpan.Zero);

    [Fact]
    public void DefaultPath_IsDrawEMSoundLogUnderCurrentUserLocalAppData()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.StartsWith(localAppData, SoundFailureLog.DefaultPath);
        Assert.EndsWith(@"\drawEM\logs\sound.log", SoundFailureLog.DefaultPath);
    }

    [Fact]
    public void FormatLine_ContainsTimeSoundPathAndReason()
    {
        var failure = new SoundFailure(FailureTime, new SoundId("applause"), @"C:\Sounds\applause.mp3", "File not found.");

        Assert.Equal(
            "2026-09-25T14:30:05.0000000+00:00\tsound=applause\tpath=C:\\Sounds\\applause.mp3\treason=File not found.",
            SoundFailureLog.FormatLine(failure));
    }

    [Fact]
    public void FormatLine_WritesUnknownDetailsAsDashAndFlattensSeparators()
    {
        var failure = new SoundFailure(FailureTime, null, null, "Decode\terror\r\nat frame 3.");

        Assert.Equal(
            "2026-09-25T14:30:05.0000000+00:00\tsound=-\tpath=-\treason=Decode error at frame 3.",
            SoundFailureLog.FormatLine(failure));
    }

    [Fact]
    public void Append_AddsOneLinePerFailure()
    {
        var directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
        var log = new SoundFailureLog(Path.Combine(directory, "logs", "sound.log"));
        var failure = new SoundFailure(FailureTime, new SoundId("applause"), null, "File not found.");

        try
        {
            log.Append(failure);
            log.Append(failure);

            Assert.Equal(
                [SoundFailureLog.FormatLine(failure), SoundFailureLog.FormatLine(failure)],
                File.ReadAllLines(Path.Combine(directory, "logs", "sound.log")));
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
        var log = new SoundFailureLog(Path.Combine(fileInPlaceOfDirectory, "logs", "sound.log"));

        try
        {
            var exception = Record.Exception(() =>
                log.Append(new SoundFailure(FailureTime, new SoundId("applause"), null, "Decode error.")));

            Assert.Null(exception);
        }
        finally
        {
            File.Delete(fileInPlaceOfDirectory);
        }
    }

    [Fact]
    public void Constructor_RejectsPathWithoutDirectory()
    {
        Assert.Throws<ArgumentException>(() => new SoundFailureLog(@"C:\"));
    }
}
