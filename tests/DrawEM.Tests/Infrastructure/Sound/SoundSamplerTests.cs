using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public sealed class SoundSamplerTests
{
    private static readonly SoundReference Horn = new("horn.wav", "Horn.wav");
    private static readonly SoundReference Bell = new("bell.mp3", "Bell.mp3");
    private readonly List<PlaySoundCommand> played = [];
    private int stops;
    private bool stopConfirmed = true;
    private readonly List<(PlaySoundCommand Command, string Reason)> logged = [];
    private readonly Queue<Action> ui = new();
    private readonly SampleFailureRouter router;
    private readonly SoundSampler sampler;

    public SoundSamplerTests()
    {
        router = new SampleFailureRouter(new RecordingReporter(logged), ui.Enqueue);
        var settings = new ActiveSettings(SettingsSnapshot.Default, sound => @"C:\library\" + sound.LibraryFileName);
        sampler = new SoundSampler(
            played.Add,
            () =>
            {
                stops++;
                return stopConfirmed;
            },
            settings.CommandFor,
            router);
    }

    [Fact]
    public void Sample_PlaysTheManagedCopyOnTheSoundChannel()
    {
        sampler.Sample(Horn, _ => { });

        Assert.Equal([new PlaySoundCommand(new SoundId("horn.wav"), @"C:\library\horn.wav")], played);
    }

    [Fact]
    public void SampleFailure_IsLoggedAndReportedOnTheUiThread()
    {
        var shown = new List<string>();
        sampler.Sample(Horn, shown.Add);

        router.Report(played[0], "file not found");

        Assert.Equal([(played[0], "file not found")], logged);
        Assert.Empty(shown);
        ui.Dequeue()();
        Assert.Equal(["file not found"], shown);
    }

    [Fact]
    public void ShortcutFailureForTheSameFile_IsOnlyLogged()
    {
        var shown = new List<string>();
        sampler.Sample(Horn, shown.Add);
        var shortcut = new PlaySoundCommand(new SoundId("horn.wav"), @"C:\library\horn.wav");

        router.Report(shortcut, "device busy");

        Assert.Single(logged);
        Assert.Empty(ui);
    }

    [Fact]
    public void EarlierSampleFailure_IsNotShownAfterANewerSample()
    {
        var first = new List<string>();
        var second = new List<string>();
        sampler.Sample(Horn, first.Add);
        sampler.Sample(Bell, second.Add);

        router.Report(played[0], "late failure");
        router.Report(played[1], "bad file");
        while (ui.TryDequeue(out var action))
        {
            action();
        }

        Assert.Empty(first);
        Assert.Equal(["bad file"], second);
        Assert.Equal(2, logged.Count);
    }

    [Fact]
    public void FailureQueuedBeforeANewerSample_IsNotShown()
    {
        var first = new List<string>();
        var second = new List<string>();
        sampler.Sample(Horn, first.Add);
        router.Report(played[0], "late failure");

        sampler.Sample(Bell, second.Add);
        ui.Dequeue()();

        Assert.Empty(first);
        Assert.Empty(second);
        Assert.Single(logged);
    }

    [Fact]
    public void StopSamples_AfterASample_StopsTheChannelOnceAndDropsQueuedFailures()
    {
        var shown = new List<string>();
        sampler.Sample(Horn, shown.Add);
        router.Report(played[0], "late failure");

        Assert.True(sampler.StopSamples());
        Assert.True(sampler.StopSamples());
        ui.Dequeue()();

        Assert.Equal(1, stops);
        Assert.Empty(shown);
    }

    [Fact]
    public void StopSamples_WithoutASample_LeavesTheChannelAlone()
    {
        stopConfirmed = false;

        Assert.True(sampler.StopSamples());

        Assert.Equal(0, stops);
    }

    [Fact]
    public void StopSamples_ReportsAnUnconfirmedStop()
    {
        stopConfirmed = false;
        sampler.Sample(Horn, _ => { });

        Assert.False(sampler.StopSamples());
    }

    private sealed class RecordingReporter(List<(PlaySoundCommand, string)> records) : ISoundFailureReporter
    {
        public void Report(PlaySoundCommand command, string reason) => records.Add((command, reason));
    }
}
