using System.Windows.Interop;
using System.Windows;
using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

/// <summary>Shows real effect windows on a monitor rectangle far off-screen, then releases them.</summary>
[Collection(WpfCollection.Name)]
public sealed class LottiePlaybackWindowTests
{
    private static readonly MonitorBounds OffScreen = new(-20000, -20000, -19600, -19700);

    [Theory]
    [InlineData(EffectPlacement.Monitor)]
    [InlineData(EffectPlacement.Cursor)]
    public void Start_ShowsOneEffectWindowAndDisposeClosesIt(EffectPlacement placement)
    {
        StaThread.Run(() =>
        {
            using var animation = Animation();

            var playback = LottiePlayback.Start(Command(placement), animation, () => { }, () => { });

            var window = Assert.Single(EffectWindows());
            Assert.True(window.IsVisible);
            playback.Dispose();
            Assert.Empty(EffectWindows());
            playback.Dispose();
        });
    }

    [Fact]
    public void Start_UnknownPlacementFailsAndLeavesNoWindow()
    {
        StaThread.Run(() =>
        {
            using var animation = Animation();

            var failure = Assert.Throws<EffectPlaybackException>(
                () => LottiePlayback.Start(Command((EffectPlacement)99), animation, () => { }, () => { }));

            Assert.StartsWith("The effect window could not be shown: ", failure.Message);
            Assert.Empty(EffectWindows());
        });
    }

    private static SkiaSharp.Skottie.Animation Animation()
    {
        Assert.True(EffectCatalog.TryFind(new EffectId("focus-ring"), out var effect));
        return BuiltInEffectAnimations.Load(effect);
    }

    private static StartEffectCommand Command(EffectPlacement placement) => new(
        1, new EffectId("focus-ring"), placement, OffScreen, new ScreenPoint(-19800, -19850), false, null, 1, 1);

    private static EffectSurfaceWindow[] EffectWindows() =>
        PresentationSource.CurrentSources.OfType<HwndSource>()
            .Select(source => source.RootVisual)
            .OfType<EffectSurfaceWindow>()
            .ToArray();
}
