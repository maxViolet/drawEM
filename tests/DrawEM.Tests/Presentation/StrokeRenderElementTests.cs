using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawEM.App.Application;
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
                    new Stroke([new ScreenPoint(10, 10)], DrawingColor.OrangeRed, 4),
                    true,
                    0),
                new PhysicalToLocalTransform(0, 0, Matrix.Identity));

            var bitmap = new RenderTargetBitmap(20, 20, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(element);
            var pixels = new byte[20 * 20 * 4];
            bitmap.CopyPixels(pixels, 20 * 4, 0);

            var offset = ((10 * 20) + 10) * 4;
            Assert.Equal((byte)0, pixels[offset]);
            Assert.Equal((byte)69, pixels[offset + 1]);
            Assert.Equal((byte)255, pixels[offset + 2]);
            Assert.Equal((byte)255, pixels[offset + 3]);
        });
    }

    [Fact]
    public void UpdateState_GrowingActiveStroke_RendersEachIncrementalSegment()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);

            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(2, 10)], DrawingColor.OrangeRed, 4), true, 0),
                transform);
            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(2, 10), new ScreenPoint(10, 10)], DrawingColor.OrangeRed, 4), true, 0),
                transform);
            element.UpdateState(
                new DrawingState(
                    [],
                    new Stroke([new ScreenPoint(2, 10), new ScreenPoint(10, 10), new ScreenPoint(17, 10)], DrawingColor.OrangeRed, 4),
                    true,
                    0),
                transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 17, 10));
            Assert.Equal((byte)255, PixelAlphaAt(element, 10, 10));
        });
    }

    [Fact]
    public void UpdateState_StrokeCompletesThenNewOneStarts_KeepsCompletedPixelsVisible()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var completed = new Stroke([new ScreenPoint(1, 1), new ScreenPoint(5, 1)], DrawingColor.OrangeRed, 4);

            element.UpdateState(new DrawingState([], completed, true, 0), transform);
            element.UpdateState(new DrawingState([completed], null, false, 0), transform);
            element.UpdateState(
                new DrawingState([completed], new Stroke([new ScreenPoint(15, 15)], DrawingColor.OrangeRed, 4), true, 0),
                transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 5, 1));
            Assert.Equal((byte)255, PixelAlphaAt(element, 15, 15));
        });
    }

    [Fact]
    public void UpdateState_FewerCompletedStrokesThanBefore_ResetsAndDropsOldPixels()
    {
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var completed = new Stroke([new ScreenPoint(1, 1), new ScreenPoint(5, 1)], DrawingColor.OrangeRed, 4);

            element.UpdateState(new DrawingState([], completed, true, 0), transform);
            element.UpdateState(new DrawingState([completed], null, false, 0), transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 5, 1));

            element.UpdateState(new DrawingState([], null, false, 1), transform);

            Assert.Equal((byte)0, PixelAlphaAt(element, 5, 1));
        });
    }

    [Fact]
    public void UpdateState_CoalescedClearThenNewStrokeOfSameLength_DropsOldAndDrawsNew()
    {
        // Regression for a coalesced pendingState update that folds clear -> new
        // stroke -> end into one call: CompletedStrokes.Count is unchanged (1 -> 1),
        // so only Generation distinguishes this from "nothing happened".
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var oldStroke = new Stroke([new ScreenPoint(1, 1), new ScreenPoint(5, 1)], DrawingColor.OrangeRed, 4);
            var newStroke = new Stroke([new ScreenPoint(12, 12), new ScreenPoint(16, 12)], DrawingColor.OrangeRed, 4);

            element.UpdateState(new DrawingState([oldStroke], null, false, 0), transform);
            Assert.Equal((byte)255, PixelAlphaAt(element, 5, 1));

            element.UpdateState(new DrawingState([newStroke], null, false, 1), transform);

            Assert.Equal((byte)0, PixelAlphaAt(element, 5, 1));
            Assert.Equal((byte)255, PixelAlphaAt(element, 16, 12));
        });
    }

    [Fact]
    public void UpdateState_StrokeEndsWithUnseenPoint_DropsStalePartialVisualAndDrawsFullStroke()
    {
        // Regression for a coalesced update where the active stroke gained a point the
        // renderer never saw as "active" (e.g. a move batched together with the End),
        // so the match heuristic in UpdateState cannot recognize the completed stroke
        // as a continuation of what it already drew.
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);

            element.UpdateState(
                new DrawingState([], new Stroke([new ScreenPoint(2, 2), new ScreenPoint(6, 2)], DrawingColor.OrangeRed, 4), true, 0),
                transform);

            var completedWithExtraPoint = new Stroke(
                [new ScreenPoint(2, 2), new ScreenPoint(6, 2), new ScreenPoint(10, 2)],
                DrawingColor.OrangeRed,
                4);
            element.UpdateState(new DrawingState([completedWithExtraPoint], null, false, 0), transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 10, 2));

            // A brand-new stroke starting at the same first point must not be treated
            // as a continuation of the stale (and now removed) tracking above.
            element.UpdateState(
                new DrawingState(
                    [completedWithExtraPoint],
                    new Stroke([new ScreenPoint(2, 2)], DrawingColor.OrangeRed, 4),
                    true,
                    0),
                transform);
            element.UpdateState(
                new DrawingState(
                    [completedWithExtraPoint],
                    new Stroke([new ScreenPoint(2, 2), new ScreenPoint(2, 15)], DrawingColor.OrangeRed, 4),
                    true,
                    0),
                transform);

            Assert.Equal((byte)255, PixelAlphaAt(element, 2, 15));
        });
    }

    [Fact]
    public void ClearDuringFirstActiveStroke_DropsPixelsThroughRealController()
    {
        // Regression: CompletedStrokes.Count stays 0 -> 0 when a clear happens before the
        // first stroke ever completes, so only DrawingState.Generation can signal the
        // reset. Wires a real DrawingSessionController to a real StrokeRenderElement, the
        // same way OverlayWindowAdapter/OverlayWindow.Render do, instead of hand-built
        // DrawingState values.
        RunOnStaThread(() =>
        {
            var element = new StrokeRenderElement { Width = 20, Height = 20 };
            element.Measure(new Size(20, 20));
            element.Arrange(new Rect(0, 0, 20, 20));
            var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);
            var controller = new DrawingSessionController();
            controller.StateChanged += state => element.UpdateState(state, transform);

            controller.EnterDrawMode(new ScreenPoint(5, 5));
            controller.Move(new ScreenPoint(15, 5));

            Assert.Equal((byte)255, PixelAlphaAt(element, 15, 5));
            Assert.Empty(controller.CompletedStrokes);

            controller.ClearAndExitDrawMode();

            Assert.Equal((byte)0, PixelAlphaAt(element, 15, 5));
            Assert.Empty(controller.CompletedStrokes);
        });
    }

    private static byte PixelAlphaAt(StrokeRenderElement element, int x, int y)
    {
        var bitmap = new RenderTargetBitmap(20, 20, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(element);
        var pixels = new byte[20 * 20 * 4];
        bitmap.CopyPixels(pixels, 20 * 4, 0);
        return pixels[(((y * 20) + x) * 4) + 3];
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
