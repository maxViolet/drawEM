using System.Diagnostics;
using System.IO;
using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class LoggingSoundFailureReporterTests : IDisposable
{
    private static readonly SoundId Applause = new("applause");
    private static readonly SoundConfiguration Configuration = new(new Dictionary<SoundSlot, SoundAssignment>
    {
        [SoundSlot.Slot2] = new(Applause, @"C:\Sounds\applause.mp3"),
    });

    private readonly string directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void Report_AppendsSoundConfiguredPathAndReasonToLog()
    {
        var logPath = Path.Combine(directory, "sound.log");
        var reporter = new LoggingSoundFailureReporter(
            new SoundFailureLog(logPath).Append, Configuration, new ManualTimeProvider());

        reporter.Report(Applause, "No output device.");
        reporter.Dispose();

        var line = Assert.Single(File.ReadAllLines(logPath));
        Assert.EndsWith("\tslot=-\tsound=applause\tpath=C:\\Sounds\\applause.mp3\treason=No output device.", line);
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
            Configuration,
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
        var reporter = new LoggingSoundFailureReporter(_ => never.Wait(), Configuration, new ManualTimeProvider());
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
            Configuration,
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
