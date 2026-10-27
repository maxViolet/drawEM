using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Presentation.Drawing;

namespace DrawEM.Tests.Presentation.Drawing;

/// <summary>
/// <see cref="OverlayWindow.Render"/> on and off the UI thread, on a window that is never shown. Without a
/// window handle, points map as if the window started at the screen origin.
/// </summary>
[Collection(WpfCollection.Name)]
public sealed class OverlayWindowRenderTests
{
    private const int Size = 20;
    private static readonly DrawingState Empty = new([], null, false, 0);
    private static readonly DrawingState Dot = new(
        [new Stroke([new ScreenPoint(10, 10)], SettingsSnapshot.Default.Style, new MonitorBounds(0, 0, Size, Size))],
        null,
        false,
        0);

    [Fact]
    public void Render_OnTheUiThreadDrawsAtOnce()
    {
        StaThread.Run(() =>
        {
            var (window, element) = NewWindow();

            window.Render(Dot);

            Assert.True(AlphaAt(element, 10, 10) > 0);
            window.Close();
        });
    }

    [Fact]
    public void Render_FromAnotherThreadDrawsTheLatestStateOnTheNextRenderPass()
    {
        StaThread.Run(() =>
        {
            var (window, element) = NewWindow();

            Task.Run(() =>
            {
                window.Render(Empty);
                window.Render(Dot);
            }).Wait();

            Assert.Equal(0, AlphaAt(element, 10, 10));
            Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.Background);
            Assert.True(AlphaAt(element, 10, 10) > 0);
            window.Close();
        });
    }

    private static (OverlayWindow Window, UIElement Element) NewWindow()
    {
        var window = new OverlayWindow();
        var element = (FrameworkElement)window.Content;
        element.Width = Size;
        element.Height = Size;
        element.Measure(new Size(Size, Size));
        element.Arrange(new Rect(0, 0, Size, Size));
        return (window, element);
    }

    private static byte AlphaAt(UIElement element, int x, int y)
    {
        var bitmap = new RenderTargetBitmap(Size, Size, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(element);
        var pixel = new byte[4];
        bitmap.CopyPixels(new Int32Rect(x, y, 1, 1), pixel, 4, 0);
        return pixel[3];
    }
}
