using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests.Application.Settings;

public class ActiveSettingsTests
{
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private static readonly SoundReference Drumroll = new("drumroll.mp3", "Drumroll.mp3");

    [Fact]
    public void Publish_ReplacesSnapshotAndSoundsInOneStep_AndLeavesPreviousConfigurationIntact()
    {
        var settings = new ActiveSettings(TestSettings.WithSounds((1, Applause)), ManagedPath);
        var previous = settings.Current;
        var saved = TestSettings.WithSounds((2, Drumroll));

        var published = settings.Publish(saved);

        Assert.Same(published, settings.Current);
        Assert.Same(saved, published.Snapshot);
        Assert.Equal(@"C:\library\drumroll.mp3", published.CommandFor(Drumroll).Path);
        Assert.Throws<ArgumentException>(() => published.CommandFor(Applause));
        Assert.Equal(@"C:\library\applause.wav", previous.CommandFor(Applause).Path);
    }

    [Fact]
    public void SoundKeptByPublish_BuildsTheSameCommand()
    {
        var settings = new ActiveSettings(TestSettings.WithSounds((1, Applause)), ManagedPath);
        var oldCommand = settings.Current.CommandFor(Applause);

        var published = settings.Publish(TestSettings.WithSounds((5, Applause), (2, Drumroll)));

        Assert.Equal(oldCommand, published.CommandFor(Applause));
        Assert.Equal(@"C:\library\applause.wav", oldCommand.Path);
    }

    [Fact]
    public void CommandFor_SoundNotInSnapshot_Throws()
    {
        var settings = new ActiveSettings(SettingsSnapshot.Default, ManagedPath);

        Assert.Throws<ArgumentException>(() => settings.Current.CommandFor(Applause));
    }

    private static string ManagedPath(SoundReference sound) => @"C:\library\" + sound.LibraryFileName;
}
