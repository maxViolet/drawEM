using System.Diagnostics;

namespace DrawEM.Tests.Release;

// Setup.exe and the uninstaller start the app with these hook arguments. The app must exit at once,
// before WPF, the single-instance guard, settings, tray, or global hooks start.
public class VelopackHookExitTests
{
    private static readonly TimeSpan ExitTimeout = TimeSpan.FromSeconds(10);

    [Theory]
    [InlineData("--veloapp-install")]
    [InlineData("--veloapp-uninstall")]
    public void HookArgument_ExitsWithCodeZeroWithinTenSeconds(string hook)
    {
        var start = new ProcessStartInfo(Path.Combine(AppContext.BaseDirectory, "DrawEM.App.exe"))
        {
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        start.ArgumentList.Add(hook);
        start.ArgumentList.Add("0.0.0");

        using var process = Process.Start(start)!;
        var exited = process.WaitForExit(ExitTimeout);
        if (!exited)
        {
            process.Kill(entireProcessTree: true);
            process.WaitForExit();
        }

        Assert.True(exited, $"DrawEM.App.exe {hook} 0.0.0 still ran after {ExitTimeout.TotalSeconds} seconds.");
        Assert.Equal(0, process.ExitCode);
    }
}
