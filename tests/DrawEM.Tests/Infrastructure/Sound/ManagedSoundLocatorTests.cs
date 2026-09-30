using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class ManagedSoundLocatorTests
{
    private const string Library = @"C:\Users\test\AppData\Local\drawEM\media";

    [Fact]
    public void DefaultDirectory_IsMediaUnderCurrentUserDrawEMFolder()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.Equal(Path.Combine(localAppData, "drawEM", "media"), ManagedSoundLocator.DefaultDirectory);
    }

    [Theory]
    [InlineData("3f2a9c.wav")]
    [InlineData("3f2a9c.mp3")]
    [InlineData("applause")]
    public void TryResolve_MapsIdToFileDirectlyInsideLibrary(string id)
    {
        var locator = new ManagedSoundLocator(Library);

        Assert.True(locator.TryResolve(new ManagedSoundId(id), out var path));
        Assert.Equal(Path.Combine(Library, id), path);
    }

    [Theory]
    [InlineData(@"..\escape.wav")]
    [InlineData("../escape.wav")]
    [InlineData(@"sub\a.wav")]
    [InlineData(@"C:\Windows\Media\Alarm07.wav")]
    [InlineData("C:a.wav")]
    [InlineData("..")]
    [InlineData(".")]
    [InlineData("a.wav.")]
    [InlineData(" a.wav")]
    [InlineData("a.wav ")]
    [InlineData("a|b.wav")]
    [InlineData("CON")]
    [InlineData("nul.wav")]
    [InlineData("COM1.mp3")]
    public void TryResolve_RejectsIdsThatAreNotPlainLibraryFileNames(string id)
    {
        var locator = new ManagedSoundLocator(Library);

        Assert.False(locator.TryResolve(new ManagedSoundId(id), out var path));
        Assert.Null(path);
    }
}
