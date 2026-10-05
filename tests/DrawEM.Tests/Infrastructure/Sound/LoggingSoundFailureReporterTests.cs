using System.Diagnostics;
using System.IO;
using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class LoggingSoundFailureReporterTests : IDisposable
{
    private static readonly PlaySoundCommand Applause = new(new SoundId("applause.mp3"), @"C:\Sounds\applause.mp3");

    private readonly string directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void Report_AppendsSoundManagedCopyAndReasonToLog()
    {
        var logPath = Path.Combine(directory, "sound.log");
        var reporter = new LoggingSoundFailureReporter(new SoundFailureLog(logPath).Append, new ManualTimeProvider());

        reporter.Report(Applause, "No output device.");
        reporter.Dispose();

        var line = Assert.Single(File.ReadAllLines(logPath));
        Assert.EndsWith("\tsound=applause.mp3\tpath=C:\\Sounds\\applause.mp3\treason=No output device.", line);
    }

    [Fact]
    public void ReportCleanup_AppendsCopyAndReasonWithoutSound()
    {
        var logPath = Path.Combine(directory, "sound.log");
        var reporter = new LoggingSoundFailureReporter(new SoundFailureLog(logPath).Append, new ManualTimeProvider());

        reporter.ReportCleanup(@"C:\Sounds\orphan.wav", "In use.");
        reporter.Dispose();

        var line = Assert.Single(File.ReadAllLines(logPath));
        Assert.EndsWith("\tsound=-\tpath=C:\\Sounds\\orphan.wav\treason=In use.", line);
    }

    [Fact]
    public void Report_ReturnsBeforeSlowLogWrites_AndKeepsReportTime()
    {
        using var release = new ManualResetEventSlim();
        var written = new List<SoundFailure>();
        var time = new ManualTimeProvider();
        var reporter = new LoggingSoundFailureReporter(
            failure =>
            {
                release.Wait();
                written.Add(failure);
            },
            time);
        var reportedAt = time.GetLocalNow();

        reporter.Report(Applause, "Decode error.");
        time.Advance(TimeSpan.FromMinutes(1));

        Assert.Empty(written);
        release.Set();
        reporter.Dispose();
        Assert.Equal(reportedAt, Assert.Single(written).Time);
    }

    [Fact]
    public void Dispose_WaitsForStuckLogOnlyUpToDrainTimeout()
    {
        using var never = new ManualResetEventSlim();
        var reporter = new LoggingSoundFailureReporter(_ => never.Wait(), new ManualTimeProvider());
        reporter.Report(Applause, "Decode error.");
        var watch = Stopwatch.StartNew();

        reporter.Dispose();

        Assert.True(watch.Elapsed < LoggingSoundFailureReporter.DrainTimeout + TimeSpan.FromSeconds(2));
        never.Set();
    }

    [Fact]
    public void LogThatThrows_DoesNotStopLaterRecords()
    {
        var written = new List<string>();
        var reporter = new LoggingSoundFailureReporter(
            failure =>
            {
                if (failure.Reason == "first")
                {
                    throw new InvalidOperationException("Log broke.");
                }

                written.Add(failure.Reason);
            },
            new ManualTimeProvider());

        reporter.Report(Applause, "first");
        reporter.Report(Applause, "second");
        reporter.Dispose();

        Assert.Equal(["second"], written);
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
