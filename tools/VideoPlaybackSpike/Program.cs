using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DrawEM.App.Application.Video;
using DrawEM.App.Infrastructure.Video;
using Microsoft.Win32;
using NAudio.Wave;

namespace VideoPlaybackSpike;

/// <summary>
/// S5-01 Windows playback spike. For every fixture: validates it, plays it unmuted through
/// <see cref="MediaVideoPlayer"/> in a 640×360 topmost window, samples the screen under the window twice, records
/// the default render device through WASAPI loopback and looks for the fixtures' 440 Hz tone, waits for the
/// natural end or a failure, disposes the player, and deletes the private copy to prove the file was released.
/// Prints a Markdown report. Usage: VideoPlaybackSpike [report.md]
/// </summary>
internal static class Program
{
    private const double ToneHz = 440;
    private static readonly TimeSpan EventWait = TimeSpan.FromSeconds(15);
    private static readonly Rect Surface = new(100, 100, 640, 360);

    [STAThread]
    private static int Main(string[] args)
    {
        var app = new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
        var exitCode = 0;
        app.Startup += async (_, _) =>
        {
            try
            {
                var report = await RunAsync();
                Console.Out.Write(report);
                if (args.Length > 0)
                {
                    File.WriteAllText(args[0], report, new UTF8Encoding(false));
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
                exitCode = 1;
            }
            finally
            {
                app.Shutdown();
            }
        };
        app.Run();
        return exitCode;
    }

    private static async Task<string> RunAsync()
    {
        var report = new StringBuilder();
        report.AppendLine("# S5-01 Windows playback spike");
        report.AppendLine();
        report.AppendLine($"- Run (UTC): {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
        report.AppendLine($"- Windows: {WindowsIdentity()}");
        report.AppendLine($"- App build: DrawEM.App {AppBuild()}");
        report.AppendLine($"- Playback path: WPF System.Windows.Media.MediaPlayer via DrawEM.App MediaVideoPlayer");
        report.AppendLine($"- Audio endpoint: {AudioEndpoint()}");
        report.AppendLine();
        report.AppendLine("| Fixture | SHA-256 (first 12) | Validation | Validate ms | Open ms | Opened (video/audio, size) | Screen under window (lit a/b, changed) | Loopback 440 Hz level (playing / before) | End | Released |");
        report.AppendLine("|---|---|---|---|---|---|---|---|---|---|");

        var directory = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Video");
        foreach (var fixture in Directory.GetFiles(directory, "*.mp4").OrderBy(name => name, StringComparer.Ordinal))
        {
            report.AppendLine(await RunFixtureAsync(fixture));
        }

        return report.ToString();
    }

    private static async Task<string> RunFixtureAsync(string fixture)
    {
        var name = Path.GetFileName(fixture);
        var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(fixture))).ToLowerInvariant();
        var copy = Path.Combine(Path.GetTempPath(), $"drawEM-spike-{Guid.NewGuid():N}-{name}");
        File.Copy(fixture, copy);

        var validationWatch = Stopwatch.StartNew();
        var validation = new Mp4VideoValidator().Validate(copy);
        var validateMs = validationWatch.Elapsed.TotalMilliseconds;

        var window = new Window
        {
            WindowStyle = WindowStyle.None,
            ResizeMode = ResizeMode.NoResize,
            Topmost = true,
            ShowActivated = false,
            ShowInTaskbar = false,
            Background = Brushes.Black,
            Left = Surface.Left,
            Top = Surface.Top,
            Width = Surface.Width,
            Height = Surface.Height,
        };
        var image = new Image { Stretch = Stretch.Fill };
        window.Content = image;
        window.Show();

        // Let the previous fixture's audio drain from the endpoint (Bluetooth adds latency) before the baseline.
        await Task.Delay(1500);
        using var audio = new ToneProbe();
        audio.Start();
        await Task.Delay(500);
        var toneBefore = audio.Take();

        VideoOpened? opened = null;
        double? openMs = null;
        var openedSignal = new TaskCompletionSource();
        var end = new TaskCompletionSource<string>();
        var watch = Stopwatch.StartNew();
        var player = new MediaVideoPlayer(copy);
        image.Source = new DrawingImage(player.CreateDrawing(new Rect(0, 0, Surface.Width, Surface.Height)));
        player.Opened += info =>
        {
            opened = info;
            openMs = watch.Elapsed.TotalMilliseconds;
            openedSignal.TrySetResult();
        };
        player.Completed += () => end.TrySetResult("natural end");
        player.Failed += failure => end.TrySetResult($"failed: {failure.Message}");
        player.Play();

        // Sample the screen twice while playing; the testsrc2 pattern moves between the samples.
        var frames = "-";
        if (await Task.WhenAny(openedSignal.Task, end.Task, Task.Delay(EventWait)) == openedSignal.Task)
        {
            await Task.Delay(500);
            if (!end.Task.IsCompleted)
            {
                var a = ScreenSample.Capture(window);
                await Task.Delay(300);
                var b = ScreenSample.Capture(window);
                frames = $"lit {a.LitFraction:P0}/{b.LitFraction:P0}, changed {ScreenSample.ChangedFraction(a, b):P0}";
            }
        }

        var finished = await Task.WhenAny(end.Task, Task.Delay(EventWait));
        var outcome = finished == end.Task ? end.Task.Result : $"no end or failure within {EventWait.TotalSeconds:0} s";
        var playedMs = watch.Elapsed.TotalMilliseconds;
        await Task.Delay(500);
        var toneDuring = audio.Take();

        player.Dispose();
        window.Close();
        string released;
        try
        {
            File.Delete(copy);
            released = File.Exists(copy) ? "no" : "yes";
        }
        catch (IOException exception)
        {
            released = $"no: {exception.Message}";
        }

        var openedText = opened is null
            ? "-"
            : $"{(opened.HasVideo ? "video" : "no video")}/{(opened.HasAudio ? "audio" : "no audio")}, {opened.Width}×{opened.Height}";
        var validationText = validation.IsSupported ? $"supported: {validation.Message}" : $"{validation.Failure}: {validation.Message}";
        return $"| {name} | {hash[..12]} | {Escape(validationText)} | {validateMs:0.0} | " +
               $"{(openMs is { } ms ? ms.ToString("0") : "-")} | {openedText} | {frames} | {toneDuring} / {toneBefore} | " +
               $"{Escape(outcome)} ({playedMs:0} ms) | {released} |";
    }

    private static string Escape(string text) => text.Replace("|", "\\|", StringComparison.Ordinal);

    private static string WindowsIdentity()
    {
        using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
        var build = int.TryParse(key?.GetValue("CurrentBuild") as string, out var number) ? number : 0;
        var product = build >= 22000 ? "Windows 11" : "Windows 10";
        return $"{product} {key?.GetValue("EditionID")} {key?.GetValue("DisplayVersion")}, build {build}.{key?.GetValue("UBR")}, {Environment.OSVersion.Version}";
    }

    private static string AppBuild()
    {
        var assembly = typeof(MediaVideoPlayer).Assembly;
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        var configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
        return $"{version} ({configuration})";
    }

    private static string AudioEndpoint()
    {
        using var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
        using var device = enumerator.GetDefaultAudioEndpoint(NAudio.CoreAudioApi.DataFlow.Render, NAudio.CoreAudioApi.Role.Multimedia);
        return $"{device.FriendlyName}, volume {device.AudioEndpointVolume.MasterVolumeLevelScalar:P0}, muted {device.AudioEndpointVolume.Mute}";
    }

    /// <summary>What the default render device played since the previous <see cref="ToneProbe.Take"/>.</summary>
    /// <param name="ToneDbfs">Level of the fixtures' 440 Hz tone (see <see cref="Goertzel.ToneDbfs"/>).</param>
    /// <param name="PeakDbfs">Loudest 100 ms block of any sound; -inf for digital silence.</param>
    private readonly record struct AudioLevel(double ToneDbfs, double PeakDbfs)
    {
        public override string ToString() =>
            double.IsNegativeInfinity(PeakDbfs) ? "silent" : $"{ToneDbfs:0} dBFS (all sound {PeakDbfs:0})";
    }

    /// <summary>Records the default render device in loopback, first channel only.</summary>
    private sealed class ToneProbe : IDisposable
    {
        private readonly WasapiLoopbackCapture capture = new();
        private readonly object gate = new();
        private readonly List<float> samples = [];

        public ToneProbe()
        {
            capture.DataAvailable += (_, e) =>
            {
                var format = capture.WaveFormat;
                var frame = format.BlockAlign;
                lock (gate)
                {
                    for (var at = 0; at + frame <= e.BytesRecorded; at += frame)
                    {
                        samples.Add(format.Encoding == WaveFormatEncoding.IeeeFloat || format.BitsPerSample == 32
                            ? BitConverter.ToSingle(e.Buffer, at)
                            : BitConverter.ToInt16(e.Buffer, at) / 32768f);
                    }
                }
            };
        }

        public void Start() => capture.StartRecording();

        public AudioLevel Take()
        {
            float[] recorded;
            lock (gate)
            {
                recorded = [.. samples];
                samples.Clear();
            }

            var rate = capture.WaveFormat.SampleRate;
            var block = rate / 10;
            var peak = double.NegativeInfinity;
            for (var start = 0; start + block <= recorded.Length; start += block)
            {
                var energy = 0.0;
                for (var i = start; i < start + block; i++)
                {
                    energy += recorded[i] * recorded[i];
                }

                if (energy > 0)
                {
                    peak = Math.Max(peak, 10 * Math.Log10(energy / block));
                }
            }

            return new AudioLevel(Goertzel.ToneDbfs(recorded, rate, ToneHz), peak);
        }

        public void Dispose()
        {
            capture.StopRecording();
            capture.Dispose();
        }
    }
}
