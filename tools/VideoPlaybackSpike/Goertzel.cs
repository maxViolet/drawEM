namespace VideoPlaybackSpike;

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
