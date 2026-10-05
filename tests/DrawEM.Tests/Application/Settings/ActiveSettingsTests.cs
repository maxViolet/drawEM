using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests.Application.Settings;

public class ActiveSettingsTests
{
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");

    [Fact]
    public void CommandFor_NamesSoundByLibraryFileAndCarriesItsManagedCopy_AcrossPublish()
    {
        var settings = new ActiveSettings(TestSettings.WithSounds((1, Applause)), ManagedPath);
        var before = settings.CommandFor(Applause);

        settings.Publish(SettingsSnapshot.Default);

        Assert.Equal(new PlaySoundCommand(new SoundId("applause.wav"), @"C:\library\applause.wav"), before);
        Assert.Equal(before, settings.CommandFor(Applause));
        Assert.Same(SettingsSnapshot.Default, settings.Current);
    }

    private static string ManagedPath(SoundReference sound) => @"C:\library\" + sound.LibraryFileName;
}
