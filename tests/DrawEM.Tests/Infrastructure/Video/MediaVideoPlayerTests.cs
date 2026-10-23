using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using DrawEM.App.Application.Video;
using DrawEM.App.Infrastructure.Video;

namespace DrawEM.Tests.Infrastructure.Video;

/// <summary>
/// Plays the checked-in fixtures through the Windows media stack that WPF <c>MediaPlayer</c> uses. The tests
/// mute the player; audible audio and rendered frames are checked by tools/VideoPlaybackSpike.
/// </summary>
[Collection(WpfCollection.Name)]
[Trait("Category", "WindowsMedia")]
public class MediaVideoPlayerTests
{
    private static readonly TimeSpan EventWait = TimeSpan.FromSeconds(15);

    [Theory]
    [InlineData("h264-high-aac.mp4", 320, 180, true)]
    [InlineData("h264-main-aac-mono44k.mp4", 320, 180, true)]
    [InlineData("h264-baseline-aac.mp4", 320, 180, true)]
    [InlineData("h264-high-silent.mp4", 320, 180, false)]
    [InlineData("h264-high-1080p60-aac.mp4", 1920, 1080, true)]
    public void SupportedFixture_OpensPlaysToNaturalEndAndReleasesFile(string name, int width, int height, bool hasAudio)
    {
        var copy = VideoFixtures.CopyToTemp(name);
        try
        {
            var run = PlayOnStaThread(copy);

            Assert.Null(run.Failure);
            Assert.Null(run.Unhandled);
            var opened = Assert.IsType<VideoOpened>(run.Opened);
            Assert.True(opened.HasVideo);
            Assert.Equal(hasAudio, opened.HasAudio);
            Assert.Equal(width, opened.Width);
            Assert.Equal(height, opened.Height);
            Assert.True(run.Completed, $"{name} did not reach its natural end within {EventWait}.");

            File.Delete(copy);
            Assert.False(File.Exists(copy));
        }
        finally
        {
            File.Delete(copy);
        }
    }

    [Fact]
    public void FileThatPassedValidation_FailsWhenPlaybackOpensIt_ReportsAndReleases()
    {
        // The avcC header says High, so validation accepts the file; the SPS says High 10, which the Windows
        // H.264 decoder rejects with 0xC00D11B1 when playback opens the file.
        var copy = VideoFixtures.CopyToTemp("h264-high10-labeled-high.mp4");
        try
        {
            Assert.True(new Mp4VideoValidator().Validate(copy).IsSupported);

            var run = PlayOnStaThread(copy);

            Assert.Null(run.Unhandled);
            Assert.False(run.Completed);
            var failure = Assert.IsType<VideoPlaybackFailure>(run.Failure);
            Assert.Equal(unchecked((int)0xC00D11B1), failure.HResult);
            Assert.Contains("0xC00D11B1", failure.Message);

            File.Delete(copy);
            Assert.False(File.Exists(copy));
        }
        finally
        {
            File.Delete(copy);
        }
    }

    [Fact]
    public void FileRemovedAfterValidation_ReportsFailureInsteadOfThrowing()
    {
        var copy = VideoFixtures.CopyToTemp("h264-high-aac.mp4");
        Assert.True(new Mp4VideoValidator().Validate(copy).IsSupported);
        File.Delete(copy);

        var run = PlayOnStaThread(copy);

        Assert.Null(run.Unhandled);
        Assert.False(run.Completed);
        Assert.NotNull(run.Failure);
    }

    [Fact]
    public void Dispose_DuringPlayback_StopsAndRaisesNothingLater()
    {
        var copy = VideoFixtures.CopyToTemp("h264-high-aac.mp4");
        try
        {
            var run = PlayOnStaThread(copy, disposeWhenOpened: true);

            Assert.NotNull(run.Opened);
            Assert.False(run.Completed);
            Assert.Null(run.Failure);
            Assert.Null(run.Unhandled);

            File.Delete(copy);
            Assert.False(File.Exists(copy));
        }
        finally
        {
            File.Delete(copy);
        }
    }

    private sealed class Run
    {
        public VideoOpened? Opened { get; set; }

        public bool Completed { get; set; }

        public VideoPlaybackFailure? Failure { get; set; }

        public Exception? Unhandled { get; set; }
    }

    /// <summary>
    /// Opens and plays <paramref name="path"/> on a new STA thread, pumps its Dispatcher until the player
    /// completes or fails, disposes it, and keeps pumping briefly so a late event would be seen. With
    /// <paramref name="disposeWhenOpened"/>, disposes as soon as the player opens, then pumps past the end.
    /// </summary>
    private static Run PlayOnStaThread(string path, bool disposeWhenOpened = false)
    {
        var run = new Run();
        StaThread.Run(() =>
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            dispatcher.UnhandledException += (_, e) =>
            {
                run.Unhandled = e.Exception;
                e.Handled = true;
            };

            var player = new MediaVideoPlayer(path, muted: true);
            var disposed = false;
            player.Opened += opened =>
            {
                run.Opened = opened;
                if (disposeWhenOpened)
                {
                    player.Dispose();
                    disposed = true;
                }
            };
            player.Completed += () => run.Completed = true;
            player.Failed += failure => run.Failure = failure;
            player.Play();

            Pump(() => run.Completed || run.Failure is not null || disposed, EventWait);
            player.Dispose();
            Pump(() => false, disposeWhenOpened ? TimeSpan.FromSeconds(3) : TimeSpan.FromMilliseconds(500));
            dispatcher.InvokeShutdown();
        });
        return run;
    }

    private static void Pump(Func<bool> done, TimeSpan timeout)
    {
        var watch = Stopwatch.StartNew();
        var frame = new DispatcherFrame();
        var timer = new DispatcherTimer(
            TimeSpan.FromMilliseconds(20),
            DispatcherPriority.Background,
            (_, _) => frame.Continue = !done() && watch.Elapsed < timeout,
            Dispatcher.CurrentDispatcher);
        Dispatcher.PushFrame(frame);
        timer.Stop();
    }
}
