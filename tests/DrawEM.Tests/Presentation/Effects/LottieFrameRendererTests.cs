using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Presentation.Effects;
using SkiaSharp;

namespace DrawEM.Tests.Presentation.Effects;

/// <summary>Draws real built-in animation frames into a bitmap, as an effect window shows them.</summary>
[Collection(WpfCollection.Name)]
public sealed class LottieFrameRendererTests
{
    private const int Size = 128;

    [Fact]
    public void Draw_PaintsTheFrameIntoTheBitmapAndClearsTheLastOne()
    {
        StaThread.Run(() =>
        {
            Assert.True(EffectCatalog.TryFind(new EffectId("focus-ring"), out var effect));
            using var animation = BuiltInEffectAnimations.Load(effect);
            var bitmap = new WriteableBitmap(Size, Size, 96, 96, PixelFormats.Pbgra32, null);
            using var renderer = new LottieFrameRenderer(animation, bitmap, SKRect.Create(0, 0, Size, Size));

            renderer.Draw(animation.Duration.TotalSeconds / 2);
            var first = Pixels(bitmap);
            renderer.Draw(animation.Duration.TotalSeconds / 4);
            var second = Pixels(bitmap);

            Assert.Contains(first, alpha => alpha > 0);
            Assert.Contains(second, alpha => alpha > 0);
            Assert.NotEqual(first, second);
        });
    }

    [Fact]
    public void Draw_AnEmptyDestinationLeavesTheBitmapTransparent()
    {
        StaThread.Run(() =>
        {
            Assert.True(EffectCatalog.TryFind(new EffectId("focus-ring"), out var effect));
            using var animation = BuiltInEffectAnimations.Load(effect);
            var bitmap = new WriteableBitmap(Size, Size, 96, 96, PixelFormats.Pbgra32, null);
            using var renderer = new LottieFrameRenderer(animation, bitmap, SKRect.Empty);

            renderer.Draw(animation.Duration.TotalSeconds / 2);

            Assert.All(Pixels(bitmap), alpha => Assert.Equal(0, alpha));
        });
    }

    /// <summary>The alpha byte of every pixel.</summary>
    private static byte[] Pixels(WriteableBitmap bitmap)
    {
        var bytes = new byte[bitmap.PixelWidth * bitmap.PixelHeight * 4];
        bitmap.CopyPixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), bytes, bitmap.PixelWidth * 4, 0);
        return bytes.Where((_, index) => index % 4 == 3).ToArray();
    }
}
