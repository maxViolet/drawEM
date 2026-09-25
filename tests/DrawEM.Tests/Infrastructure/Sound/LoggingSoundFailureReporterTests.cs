using System.IO;
using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class LoggingSoundFailureReporterTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void Report_AppendsSoundConfiguredPathAndReason()
    {
        var sound = new SoundId("applause");
        var logPath = Path.Combine(directory, "sound.log");
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot2] = new(sound, @"C:\Sounds\applause.mp3"),
        });
        var reporter = new LoggingSoundFailureReporter(new SoundFailureLog(logPath), configuration, new ManualTimeProvider());

        reporter.Report(sound, "No output device.");

        var line = Assert.Single(File.ReadAllLines(logPath));
        Assert.EndsWith("\tslot=-\tsound=applause\tpath=C:\\Sounds\\applause.mp3\treason=No output device.", line);
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
