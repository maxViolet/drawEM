using DrawEM.App.Infrastructure.Updates;

namespace DrawEM.Tests.Infrastructure.Updates;

public class UpdateFailureLogTests
{
    private static readonly DateTimeOffset FailureTime = new(2026, 10, 16, 9, 30, 5, TimeSpan.Zero);

    [Fact]
    public void DefaultPath_IsDrawEMUpdateLogUnderCurrentUserLocalAppData()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.StartsWith(localAppData, UpdateFailureLog.DefaultPath);
        Assert.EndsWith(@"\drawEM\logs\update.log", UpdateFailureLog.DefaultPath);
    }

    [Fact]
    public void FormatLine_ContainsTimeOperationAndFlattenedReason()
    {
        var failure = new UpdateFailure(FailureTime, "check", "Request\tfailed\r\nwith 403.");

        Assert.Equal(
            "2026-10-16T09:30:05.0000000+00:00\toperation=check\treason=Request failed with 403.",
            UpdateFailureLog.FormatLine(failure));
    }

    [Fact]
    public void Append_AddsOneLinePerFailure()
    {
        var directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
        var log = new UpdateFailureLog(Path.Combine(directory, "logs", "update.log"));
        var failure = new UpdateFailure(FailureTime, "apply", "Updater not found.");

        try
        {
            log.Append(failure);
            log.Append(failure);

            Assert.Equal(
                [UpdateFailureLog.FormatLine(failure), UpdateFailureLog.FormatLine(failure)],
                File.ReadAllLines(Path.Combine(directory, "logs", "update.log")));
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
        var log = new UpdateFailureLog(Path.Combine(fileInPlaceOfDirectory, "logs", "update.log"));

        try
        {
            var exception = Record.Exception(() => log.Append(new UpdateFailure(FailureTime, "check", "No network.")));

            Assert.Null(exception);
        }
        finally
        {
            File.Delete(fileInPlaceOfDirectory);
        }
    }

    [Fact]
    public void Reporter_WritesReportedFailureWithReportTime()
    {
        var records = new List<UpdateFailure>();
        var time = new ManualTimeProvider();
        var reporter = new LoggingUpdateFailureReporter(records.Add, time);

        reporter.Report("check", "No network.");
        reporter.Dispose();

        var record = Assert.Single(records);
        Assert.Equal(time.GetLocalNow(), record.Time);
        Assert.Equal("check", record.Operation);
        Assert.Equal("No network.", record.Reason);
    }
}
