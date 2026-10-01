using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class AppFailureLogTests
{
    private static readonly DateTimeOffset FailureTime = new(2026, 9, 28, 20, 30, 5, TimeSpan.Zero);

    [Fact]
    public async Task StuckWriter_DoesNotBlockAppendOrHoldDisposeBeyondOneSecond()
    {
        using var writing = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        using var finished = new ManualResetEventSlim();
        var log = new AppFailureLog(_ =>
        {
            writing.Set();
            release.Wait();
            finished.Set();
        });
        var append = Task.Factory.StartNew(() => log.Append("exit", new InvalidOperationException("Failed.")), TaskCreationOptions.LongRunning);
        Task? dispose = null;

        try
        {
            Assert.True(writing.Wait(TimeSpan.FromSeconds(5)));
            await append.WaitAsync(TimeSpan.FromMilliseconds(500));
            dispose = Task.Factory.StartNew(log.Dispose, TaskCreationOptions.LongRunning);
            await dispose.WaitAsync(TimeSpan.FromMilliseconds(1500));
        }
        finally
        {
            release.Set();
            await append.WaitAsync(TimeSpan.FromSeconds(5));
            Assert.True(finished.Wait(TimeSpan.FromSeconds(5)));
            if (dispose is not null)
            {
                await dispose.WaitAsync(TimeSpan.FromSeconds(5));
            }
        }
    }

    [Fact]
    public void DefaultPath_IsDrawEMAppLogUnderCurrentUserLocalAppData()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.StartsWith(localAppData, AppFailureLog.DefaultPath);
        Assert.EndsWith(@"\drawEM\logs\app.log", AppFailureLog.DefaultPath);
    }

    [Fact]
    public void FormatLine_ContainsTimeOperationAndError()
    {
        var failure = new InvalidOperationException("Sound stop failed.");

        Assert.Equal(
            "2026-09-28T20:30:05.0000000+00:00\toperation=exit\terror=System.InvalidOperationException: Sound stop failed.",
            AppFailureLog.FormatLine(FailureTime, "exit", failure));
    }

    [Fact]
    public void FormatLine_ListsEveryInnerErrorAndFlattensSeparators()
    {
        var failure = new AggregateException(
            new InvalidOperationException("Overlay\tclose\r\nfailed."),
            new IOException("Tray failed."));

        Assert.Equal(
            "2026-09-28T20:30:05.0000000+00:00\toperation=exit\terror=System.InvalidOperationException: Overlay close failed. | System.IO.IOException: Tray failed.",
            AppFailureLog.FormatLine(FailureTime, "exit", failure));
    }

    [Fact]
    public void Append_AddsOneLinePerFailure()
    {
        var directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
        var log = new AppFailureLog(Path.Combine(directory, "logs", "app.log"));

        try
        {
            log.Append("exit", new InvalidOperationException("Sound stop failed."));
            log.Append("startup cleanup", new InvalidOperationException("Hook release failed."));
            log.Dispose();

            var lines = File.ReadAllLines(Path.Combine(directory, "logs", "app.log"));
            Assert.Equal(2, lines.Length);
            Assert.EndsWith("\toperation=exit\terror=System.InvalidOperationException: Sound stop failed.", lines[0]);
            Assert.EndsWith("\toperation=startup cleanup\terror=System.InvalidOperationException: Hook release failed.", lines[1]);
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
        var log = new AppFailureLog(Path.Combine(fileInPlaceOfDirectory, "logs", "app.log"));

        try
        {
            var exception = Record.Exception(() => log.Append("exit", new InvalidOperationException("Failed.")));

            Assert.Null(exception);
            log.Dispose();
        }
        finally
        {
            File.Delete(fileInPlaceOfDirectory);
        }
    }
}
