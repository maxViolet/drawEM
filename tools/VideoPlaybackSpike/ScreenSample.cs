using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;

namespace VideoPlaybackSpike;

/// <summary>Device pixels of the screen under a window, captured with GDI after composition.</summary>
internal sealed class ScreenSample
{
    private readonly int[] pixels;

    private ScreenSample(int[] pixels)
    {
        this.pixels = pixels;
    }

    /// <summary>Share of pixels brighter than near-black: the window background is black until a frame is drawn.</summary>
    public double LitFraction => pixels.Count(pixel => Brightness(pixel) > 24) / (double)pixels.Length;

    public static ScreenSample Capture(Window window)
    {
        var device = PresentationSource.FromVisual(window)!.CompositionTarget!.TransformToDevice;
        var topLeft = device.Transform(new System.Windows.Point(window.Left, window.Top));
        var size = device.Transform(new System.Windows.Point(window.ActualWidth, window.ActualHeight));
        var width = (int)size.X;
        var height = (int)size.Y;

        using var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.CopyFromScreen((int)topLeft.X, (int)topLeft.Y, 0, 0, new System.Drawing.Size(width, height));
        }

        var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        try
        {
            var pixels = new int[width * height];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
            return new ScreenSample(pixels);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    /// <summary>Share of pixels whose brightness differs by more than compression noise.</summary>
    public static double ChangedFraction(ScreenSample a, ScreenSample b) =>
        a.pixels.Zip(b.pixels).Count(pair => Math.Abs(Brightness(pair.First) - Brightness(pair.Second)) > 24) / (double)a.pixels.Length;

    private static int Brightness(int argb) => (((argb >> 16) & 0xFF) + ((argb >> 8) & 0xFF) + (argb & 0xFF)) / 3;
}

/// <summary>Single-frequency level by the Goertzel algorithm.</summary>
internal static class Goertzel
{
    /// <summary>
    /// The amplitude of <paramref name="frequency"/> in <paramref name="samples"/>, in dB relative to full scale:
    /// the 75th percentile over 100 ms blocks, so a tone that fills a quarter of the recording still shows.
    /// A 100 ms block has 10 Hz bins, so 440 Hz falls on a bin exactly. Other sound at other frequencies
    /// barely moves it. Negative infinity for an empty recording.
    /// </summary>
    public static double ToneDbfs(float[] samples, int sampleRate, double frequency)
    {
        var block = sampleRate / 10;
        var coefficient = 2 * Math.Cos(2 * Math.PI * frequency / sampleRate);
        var levels = new List<double>();
        for (var start = 0; start + block <= samples.Length; start += block)
        {
            double previous = 0, beforePrevious = 0;
            for (var i = start; i < start + block; i++)
            {
                var current = samples[i] + coefficient * previous - beforePrevious;
                beforePrevious = previous;
                previous = current;
            }

            // A sine of amplitude A on the bin gives power (A·N/2)².
            var power = previous * previous + beforePrevious * beforePrevious - coefficient * previous * beforePrevious;
            var amplitude = 2 * Math.Sqrt(Math.Max(0, power)) / block;
            levels.Add(20 * Math.Log10(amplitude));
        }

        if (levels.Count == 0)
        {
            return double.NegativeInfinity;
        }

        levels.Sort();
        return levels[(int)(0.75 * (levels.Count - 1))];
    }
}
