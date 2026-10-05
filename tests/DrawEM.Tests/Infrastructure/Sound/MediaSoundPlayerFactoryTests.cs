using System.IO;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class MediaSoundPlayerFactoryTests
{
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");

    [Fact]
    public void MissingManagedCopy_IsRecoverableFailure()
    {
        var missing = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
        var settings = new ActiveSettings(TestSettings.WithSounds((1, Applause)), sound => Path.Combine(missing, sound.LibraryFileName));

        var exception = Assert.Throws<SoundPlaybackException>(
            () => new MediaSoundPlayerFactory().Create(settings.Current.CommandFor(Applause)));

        Assert.Equal("File not found.", exception.Message);
    }
}
