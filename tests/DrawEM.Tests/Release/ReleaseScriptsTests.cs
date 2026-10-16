using System.Diagnostics;
using System.IO.Compression;

namespace DrawEM.Tests.Release;

// The release workflow runs these scripts; the tests run the same files, copied next to the test assembly.
public class ReleaseScriptsTests : IDisposable
{
    private static readonly string ScriptDirectory = Path.Combine(AppContext.BaseDirectory, "release");
    private readonly string _workDirectory = Directory.CreateTempSubdirectory("drawem-release-").FullName;

    public void Dispose() => Directory.Delete(_workDirectory, recursive: true);

    [Theory]
    [InlineData("v1.2.3", "1.2.3")]
    [InlineData("v0.1.0", "0.1.0")]
    [InlineData("v10.20.30", "10.20.30")]
    public void ReleaseVersion_FromSemverTag_IsTagWithoutPrefix(string tag, string expected)
    {
        var result = RunScript("Get-ReleaseVersion.ps1", "-Tag", tag);

        Assert.True(result.ExitCode == 0, result.Output);
        Assert.Equal(expected, result.StdOut.Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData("1.2.3")]
    [InlineData("V1.2.3")]
    [InlineData("v1.2")]
    [InlineData("v1.2.3.4")]
    [InlineData("v1.2.3-beta")]
    [InlineData("v01.2.3")]
    [InlineData("v1.2.3 ")]
    [InlineData("refs/tags/v1.2.3")]
    public void ReleaseVersion_FromMalformedTag_Fails(string tag)
    {
        var result = RunScript("Get-ReleaseVersion.ps1", "-Tag", tag);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Equal("", result.StdOut.Trim());
    }

    [Fact]
    public void ReleasePackage_FromMatchingPublish_ZipsWholeDirectoryUnderVersionedName()
    {
        var version = CreatePublishDirectory(out var publishDirectory);
        Directory.CreateDirectory(Path.Combine(publishDirectory, "licenses"));
        File.WriteAllText(Path.Combine(publishDirectory, "licenses", "notice.md"), "notice");
        var outputDirectory = Path.Combine(_workDirectory, "out");

        var result = RunPackage(publishDirectory, version, outputDirectory);

        Assert.True(result.ExitCode == 0, result.Output);
        var zipPath = Path.Combine(outputDirectory, $"drawEM-{version}-win-x64.zip");
        Assert.Equal(zipPath, result.StdOut.Trim());
        using var zip = ZipFile.OpenRead(zipPath);
        var entries = zip.Entries.Where(e => e.Name != "").Select(e => e.FullName.Replace('\\', '/')).Order();
        var files = Directory.EnumerateFiles(publishDirectory, "*", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(publishDirectory, f).Replace('\\', '/')).Order();
        Assert.Equal(files, entries);
    }

    [Fact]
    public void ReleasePackage_WhenExecutableVersionDiffersFromTag_FailsWithoutZip()
    {
        CreatePublishDirectory(out var publishDirectory);
        var outputDirectory = Path.Combine(_workDirectory, "out");

        var result = RunPackage(publishDirectory, "98.76.54", outputDirectory);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains("version", result.StdErr, StringComparison.OrdinalIgnoreCase);
        Assert.False(File.Exists(Path.Combine(outputDirectory, "drawEM-98.76.54-win-x64.zip")));
    }

    [Theory]
    [InlineData("coreclr.dll")] // framework-dependent publish has no runtime
    [InlineData("hostfxr.dll")] // framework-dependent publish has no host resolver
    [InlineData("DrawEM.App.dll")] // single-file publish bundles the app assembly into the exe
    public void ReleasePackage_WhenPublishIsNotSelfContainedMultiFile_Fails(string missingFile)
    {
        var version = CreatePublishDirectory(out var publishDirectory);
        File.Delete(Path.Combine(publishDirectory, missingFile));
        var outputDirectory = Path.Combine(_workDirectory, "out");

        var result = RunPackage(publishDirectory, version, outputDirectory);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains(missingFile, result.StdErr);
        Assert.False(Directory.Exists(outputDirectory) && Directory.EnumerateFiles(outputDirectory).Any());
    }

    [Fact]
    public void ReleasePackage_WithMalformedVersion_Fails()
    {
        CreatePublishDirectory(out var publishDirectory);

        var result = RunPackage(publishDirectory, "1.2", Path.Combine(_workDirectory, "out"));

        Assert.NotEqual(0, result.ExitCode);
    }

    [Fact]
    public void Sha256Sums_ForFilesInReverseOrder_WritesSortedHashLinesWithLfAndNoBom()
    {
        var first = WriteFile("b-second.zip", "second");
        var second = WriteFile("a-first.exe", "first");
        var manifest = Path.Combine(_workDirectory, "out", "SHA256SUMS.txt");

        var result = RunScript("Write-Sha256Sums.ps1", "-Path", $"{first},{second}", "-OutputPath", manifest);

        Assert.True(result.ExitCode == 0, result.Output);
        var bytes = File.ReadAllBytes(manifest);
        Assert.False(bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF);
        Assert.Equal(
            $"{Sha256(second)}  a-first.exe\n{Sha256(first)}  b-second.zip\n",
            System.Text.Encoding.UTF8.GetString(bytes));
    }

    [Fact]
    public void Sha256Sums_WhenOutputIsAnInput_FailsWithoutManifest()
    {
        var input = WriteFile("drawEM.zip", "zip");
        var manifest = Path.Combine(_workDirectory, "SHA256SUMS.txt");
        File.WriteAllText(manifest, "old");

        var result = RunScript("Write-Sha256Sums.ps1", "-Path", $"{input},{manifest}", "-OutputPath", manifest);

        Assert.NotEqual(0, result.ExitCode);
        Assert.Equal("old", File.ReadAllText(manifest));
    }

    [Fact]
    public void Sha256Sums_WithDuplicateFileNames_Fails()
    {
        var first = WriteFile("drawEM.zip", "one");
        Directory.CreateDirectory(Path.Combine(_workDirectory, "other"));
        var second = WriteFile(Path.Combine("other", "drawEM.zip"), "two");
        var manifest = Path.Combine(_workDirectory, "SHA256SUMS.txt");

        var result = RunScript("Write-Sha256Sums.ps1", "-Path", $"{first},{second}", "-OutputPath", manifest);

        Assert.NotEqual(0, result.ExitCode);
        Assert.False(File.Exists(manifest));
    }

    [Fact]
    public void Sha256Sums_WithMissingInput_Fails()
    {
        var manifest = Path.Combine(_workDirectory, "SHA256SUMS.txt");

        var result = RunScript("Write-Sha256Sums.ps1",
            "-Path", Path.Combine(_workDirectory, "missing.zip"), "-OutputPath", manifest);

        Assert.NotEqual(0, result.ExitCode);
        Assert.False(File.Exists(manifest));
    }

    [Theory]
    [InlineData("1.2.3")]
    [InlineData("0.0.1-ci.42")]
    public void VelopackPackage_RenamesSetupAndListsTheThreeDownloads(string version)
    {
        var publishDirectory = Path.Combine(_workDirectory, "publish");
        Directory.CreateDirectory(publishDirectory);
        var outputDirectory = Path.Combine(_workDirectory, "velopack");
        var argumentsFile = Path.Combine(_workDirectory, "vpk-arguments.txt");

        var result = RunVelopackPackage(publishDirectory, version, outputDirectory, FakeVpk(argumentsFile));

        Assert.True(result.ExitCode == 0, result.Output);
        Assert.Equal(
            [
                Path.Combine(outputDirectory, $"drawEM-{version}-win-x64-Setup.exe"),
                Path.Combine(outputDirectory, $"drawEM.Desktop-{version}-full.nupkg"),
                Path.Combine(outputDirectory, "releases.win.json"),
            ],
            result.StdOut.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        Assert.False(File.Exists(Path.Combine(outputDirectory, "drawEM.Desktop-win-Setup.exe")));
        Assert.Equal(
            [
                "pack", "--packId", "drawEM.Desktop", "--packVersion", version,
                "--packDir", publishDirectory, "--mainExe", "DrawEM.App.exe", "--packTitle", "drawEM",
                "--icon", Path.Combine(_workDirectory, "drawEM.ico"),
                "--runtime", "win-x64", "--channel", "win", "--noPortable", "--outputDir", outputDirectory,
            ],
            File.ReadAllLines(argumentsFile));
    }

    [Fact]
    public void VelopackPackage_WhenPackFails_Fails()
    {
        var publishDirectory = Path.Combine(_workDirectory, "publish");
        Directory.CreateDirectory(publishDirectory);

        var result = RunVelopackPackage(
            publishDirectory, "1.2.3", Path.Combine(_workDirectory, "velopack"), FakeVpk(exitCode: 3));

        Assert.NotEqual(0, result.ExitCode);
        Assert.Equal("", result.StdOut.Trim());
    }

    [Theory]
    [InlineData("drawEM.Desktop-win-Setup.exe")]
    [InlineData("drawEM.Desktop-1.2.3-full.nupkg")]
    [InlineData("releases.win.json")]
    public void VelopackPackage_WhenAnExpectedFileIsMissing_FailsNamingIt(string missing)
    {
        var publishDirectory = Path.Combine(_workDirectory, "publish");
        Directory.CreateDirectory(publishDirectory);

        var result = RunVelopackPackage(
            publishDirectory, "1.2.3", Path.Combine(_workDirectory, "velopack"), FakeVpk(skip: missing));

        Assert.NotEqual(0, result.ExitCode);
        Assert.Contains(missing, result.StdErr);
        Assert.Equal("", result.StdOut.Trim());
    }

    [Fact]
    public void VelopackPackage_WithNonEmptyOutputDirectory_FailsWithoutPacking()
    {
        var publishDirectory = Path.Combine(_workDirectory, "publish");
        Directory.CreateDirectory(publishDirectory);
        var outputDirectory = Path.Combine(_workDirectory, "velopack");
        Directory.CreateDirectory(outputDirectory);
        File.WriteAllText(Path.Combine(outputDirectory, "stale.nupkg"), "");
        var argumentsFile = Path.Combine(_workDirectory, "vpk-arguments.txt");

        var result = RunVelopackPackage(publishDirectory, "1.2.3", outputDirectory, FakeVpk(argumentsFile));

        Assert.NotEqual(0, result.ExitCode);
        Assert.False(File.Exists(argumentsFile));
    }

    [Theory]
    [InlineData("1.2")]
    [InlineData("v1.2.3")]
    [InlineData("1.2.3-")]
    public void VelopackPackage_WithMalformedVersion_Fails(string version)
    {
        var publishDirectory = Path.Combine(_workDirectory, "publish");
        Directory.CreateDirectory(publishDirectory);
        var argumentsFile = Path.Combine(_workDirectory, "vpk-arguments.txt");

        var result = RunVelopackPackage(
            publishDirectory, version, Path.Combine(_workDirectory, "velopack"), FakeVpk(argumentsFile));

        Assert.NotEqual(0, result.ExitCode);
        Assert.False(File.Exists(argumentsFile));
    }

    // Stands in for "dotnet vpk": records its arguments, one per line, and writes the files vpk pack writes.
    private string FakeVpk(string? argumentsFile = null, int exitCode = 0, string? skip = null)
    {
        var path = Path.Combine(_workDirectory, "fake-vpk.ps1");
        File.WriteAllText(path, $$"""
            $arguments = $args
            if ('{{argumentsFile}}' -ne '') { [IO.File]::WriteAllLines('{{argumentsFile}}', [string[]] $arguments) }
            if ({{exitCode}} -ne 0) { exit {{exitCode}} }
            $id = $arguments[[array]::IndexOf($arguments, '--packId') + 1]
            $version = $arguments[[array]::IndexOf($arguments, '--packVersion') + 1]
            $output = $arguments[[array]::IndexOf($arguments, '--outputDir') + 1]
            New-Item -ItemType Directory -Path $output -Force | Out-Null
            foreach ($name in "$id-win-Setup.exe", "$id-$version-full.nupkg", 'releases.win.json', 'RELEASES', 'assets.win.json') {
                if ($name -ne '{{skip}}') { Set-Content -LiteralPath (Join-Path $output $name) -Value $name }
            }
            exit 0
            """);
        return path;
    }

    private ScriptResult RunVelopackPackage(
        string publishDirectory, string version, string outputDirectory, string packCommand) =>
        RunScript("New-VelopackPackage.ps1",
            "-PublishDirectory", publishDirectory, "-Version", version, "-OutputDirectory", outputDirectory,
            "-PackId", "drawEM.Desktop", "-Icon", Path.Combine(_workDirectory, "drawEM.ico"),
            "-PackCommand", packCommand);

    private string WriteFile(string name, string content)
    {
        var path = Path.Combine(_workDirectory, name);
        File.WriteAllText(path, content);
        return path;
    }

    private static string Sha256(string path) =>
        Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

    // Builds a stand-in publish directory whose executable carries the version resource of the built app assembly.
    private string CreatePublishDirectory(out string publishDirectory)
    {
        publishDirectory = Path.Combine(_workDirectory, "publish");
        Directory.CreateDirectory(publishDirectory);
        var appAssembly = typeof(DrawEM.App.App).Assembly.Location;
        File.Copy(appAssembly, Path.Combine(publishDirectory, "DrawEM.App.exe"));
        File.Copy(appAssembly, Path.Combine(publishDirectory, "DrawEM.App.dll"));
        File.WriteAllText(Path.Combine(publishDirectory, "coreclr.dll"), "");
        File.WriteAllText(Path.Combine(publishDirectory, "hostfxr.dll"), "");
        var info = FileVersionInfo.GetVersionInfo(appAssembly);
        return $"{info.FileMajorPart}.{info.FileMinorPart}.{info.FileBuildPart}";
    }

    private static ScriptResult RunPackage(string publishDirectory, string version, string outputDirectory) =>
        RunScript("New-ReleasePackage.ps1",
            "-PublishDirectory", publishDirectory, "-Version", version, "-OutputDirectory", outputDirectory);

    private static ScriptResult RunScript(string script, params string[] arguments)
    {
        var start = new ProcessStartInfo(PowerShellExecutable())
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (var argument in new[] { "-NoProfile", "-NonInteractive", "-ExecutionPolicy", "Bypass", "-File", Path.Combine(ScriptDirectory, script) })
        {
            start.ArgumentList.Add(argument);
        }
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        using var process = Process.Start(start)!;
        var stdErr = process.StandardError.ReadToEndAsync();
        var stdOut = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return new ScriptResult(process.ExitCode, stdOut, stdErr.Result);
    }

    // The release workflow uses PowerShell 7; Windows PowerShell 5.1 is the fallback on machines without it.
    private static string PowerShellExecutable()
    {
        var path = Environment.GetEnvironmentVariable("PATH") ?? "";
        var hasPwsh = path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Any(directory => File.Exists(Path.Combine(directory, "pwsh.exe")));
        return hasPwsh ? "pwsh.exe" : "powershell.exe";
    }

    private sealed record ScriptResult(int ExitCode, string StdOut, string StdErr)
    {
        public string Output => $"exit {ExitCode}\nstdout:\n{StdOut}\nstderr:\n{StdErr}";
    }
}
