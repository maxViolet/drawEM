using System.IO;
using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class MediaSoundPlayerFactoryTests
{
    private static readonly SoundId Applause = new("applause");

    [Fact]
    public void UnassignedSound_IsRecoverableFailure()
    {
        var factory = new MediaSoundPlayerFactory(new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>()));

        var exception = Assert.Throws<SoundPlaybackException>(() => factory.Create(Applause));

        Assert.Equal("Sound is not assigned to a file.", exception.Message);
    }

    [Fact]
    public void MissingFile_IsRecoverableFailure()
    {
        var missing = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N") + ".wav");
        var factory = new MediaSoundPlayerFactory(Configure(missing));

        var exception = Assert.Throws<SoundPlaybackException>(() => factory.Create(Applause));

        Assert.Equal("File not found.", exception.Message);
    }

    [Fact]
    public void RelativePath_IsRecoverableFailure()
    {
        var factory = new MediaSoundPlayerFactory(Configure(@"Sounds\applause.wav"));

        var exception = Assert.Throws<SoundPlaybackException>(() => factory.Create(Applause));

        Assert.Equal("Path is not absolute.", exception.Message);
    }

    private static SoundConfiguration Configure(string path) =>
        new(new Dictionary<SoundSlot, SoundAssignment> { [SoundSlot.Slot1] = new(Applause, path) });
}
