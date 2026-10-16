using System.Reflection;
using DrawEM.App.Infrastructure.Settings;

namespace DrawEM.Tests.Release;

// Velopack installs to %LocalAppData%\<packId> and removes that whole folder on uninstall, so the ID must
// never name the settings folder or one of its ancestors.
public class VelopackPackageIdentityTests
{
    private static readonly string LocalAppData =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    [Fact]
    public void PackageId_IsDrawEMDesktop()
    {
        Assert.Equal("drawEM.Desktop", PackageId());
    }

    [Fact]
    public void PackageId_DiffersFromSettingsFolderName_IgnoringCase()
    {
        Assert.False(string.Equals("drawEM", PackageId(), StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void InstallDirectory_IsNeitherTheSettingsDirectoryNorItsAncestor()
    {
        var install = Path.Combine(LocalAppData, PackageId());

        Assert.False(
            IsSameOrAncestor(install, JsonSettingsStore.DefaultDirectory),
            $"Install directory {install} contains the settings directory {JsonSettingsStore.DefaultDirectory}.");
    }

    [Theory]
    [InlineData(@"C:\Users\a\AppData\Local\drawEM", @"C:\Users\a\AppData\Local\drawEM\settings", true)]
    [InlineData(@"C:\Users\a\AppData\Local\DRAWEM", @"C:\Users\a\AppData\Local\drawEM", true)]
    [InlineData(@"C:\Users\a\AppData\Local\", @"C:\Users\a\AppData\Local\drawEM", true)]
    [InlineData(@"C:\Users\a\AppData\Local\drawEM.Desktop", @"C:\Users\a\AppData\Local\drawEM", false)]
    [InlineData(@"C:\Users\a\AppData\Local\draw", @"C:\Users\a\AppData\Local\drawEM", false)]
    public void IsSameOrAncestor_ComparesWholeSegmentsIgnoringCase(string candidate, string path, bool expected)
    {
        Assert.Equal(expected, IsSameOrAncestor(candidate, path));
    }

    private static string PackageId() =>
        typeof(DrawEM.App.App).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .SingleOrDefault(attribute => attribute.Key == "VelopackPackId")?.Value
        ?? throw new InvalidOperationException("DrawEM.App has no VelopackPackId assembly metadata.");

    // A plain StartsWith would place drawEM.Desktop under drawEM; comparing whole segments does not.
    private static bool IsSameOrAncestor(string candidate, string path)
    {
        var ancestor = Segments(candidate);
        var descendant = Segments(path);
        return ancestor.Length <= descendant.Length
            && ancestor.Zip(descendant).All(pair => string.Equals(pair.First, pair.Second, StringComparison.OrdinalIgnoreCase));
    }

    private static string[] Segments(string path) =>
        Path.GetFullPath(path).Split(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries);
}
