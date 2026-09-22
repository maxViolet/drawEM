using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawEM.App.Domain;
using DrawEM.App.Presentation;

namespace DrawEM.Tests.Presentation;

public class StrokeRenderElementTests
{
    [Fact]
    public void UpdateState_RendersSinglePointStroke()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement
            {
                Width = 20,
                Height = 20,
            };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            element.UpdateState(
                new DrawingState(
                    [],
                    new Stroke([new ScreenPoint(10, 10)], DrawingColor.Orange, 4),
                    true),
                new PhysicalToLocalTransform(0, 0, Matrix.Identity));

            var bitmap = new RenderTargetBitmap(20, 20, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(element);
            var pixels = new byte[20 * 20 * 4];
            bitmap.CopyPixels(pixels, 20 * 4, 0);

            var offset = ((10 * 20) + 10) * 4;
            Assert.Equal((byte)0, pixels[offset]);
            Assert.Equal((byte)165, pixels[offset + 1]);
            Assert.Equal((byte)255, pixels[offset + 2]);
            Assert.Equal((byte)255, pixels[offset + 3]);
        });
    }

    private static void RunOnStaThread(Action action)
    {
        Exception? exception = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception caught)
            {
                exception = caught;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (exception is not null)
        {
            throw new InvalidOperationException("The STA render assertion failed.", exception);
        }
    }
}
