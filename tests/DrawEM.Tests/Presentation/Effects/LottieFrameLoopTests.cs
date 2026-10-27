using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

/// <summary>What one effect does on each WPF Rendering callback.</summary>
public sealed class LottieFrameLoopTests
{
    private readonly List<string> log = [];
    private TimeSpan elapsed;

    [Fact]
    public void Render_DrawsTheFrameAtTheElapsedTimeAfterFrameStarting()
    {
        var frames = Loop(duration: 2, loop: false);
        elapsed = TimeSpan.FromSeconds(1.5);

        frames.Render(TimeSpan.FromMilliseconds(16));

        Assert.Equal(["starting", "draw 1.5"], log);
    }

    [Fact]
    public void Render_DrawsOnlyOnceForOneRenderingTime()
    {
        var frames = Loop(duration: 2, loop: false);

        frames.Render(TimeSpan.FromMilliseconds(16));
        elapsed = TimeSpan.FromSeconds(1);
        frames.Render(TimeSpan.FromMilliseconds(16));
        frames.Render(TimeSpan.FromMilliseconds(33));

        Assert.Equal(["starting", "draw 0", "starting", "draw 1"], log);
    }

    [Fact]
    public void Render_LoopingEffectWrapsToFrameZero()
    {
        var frames = Loop(duration: 2, loop: true);
        elapsed = TimeSpan.FromSeconds(5.5);

        frames.Render(TimeSpan.FromMilliseconds(16));

        Assert.Equal(["starting", "draw 1.5"], log);
    }

    [Fact]
    public void Render_CompletesWithoutDrawingOnceTheEffectEnds()
    {
        var frames = Loop(duration: 2, loop: false);
        elapsed = TimeSpan.FromSeconds(2);

        frames.Render(TimeSpan.FromMilliseconds(16));

        Assert.Equal(["completed"], log);
    }

    [Fact]
    public void Render_LoopingEffectWithoutLengthFails()
    {
        var frames = Loop(duration: 0, loop: true);

        frames.Render(TimeSpan.FromMilliseconds(16));

        Assert.Equal(["failed: The effect has no length, so it cannot loop."], log);
    }

    [Fact]
    public void Render_FrameStartingThatThrowsFailsWithoutDrawing()
    {
        var frames = new LottieFrameLoop(
            TimeSpan.FromSeconds(2), false, () => elapsed,
            () => throw new InvalidOperationException("cursor lost"),
            seconds => log.Add(FormattableString.Invariant($"draw {seconds}")),
            () => log.Add("completed"),
            reason => log.Add($"failed: {reason}"));

        frames.Render(TimeSpan.FromMilliseconds(16));

        Assert.Equal(["failed: The effect could not be drawn: cursor lost"], log);
    }

    [Fact]
    public void Render_DrawThatThrowsFails()
    {
        var frames = new LottieFrameLoop(
            TimeSpan.FromSeconds(2), false, () => elapsed,
            () => log.Add("starting"),
            _ => throw new InvalidOperationException("no surface"),
            () => log.Add("completed"),
            reason => log.Add($"failed: {reason}"));

        frames.Render(TimeSpan.FromMilliseconds(16));

        Assert.Equal(["starting", "failed: The effect could not be drawn: no surface"], log);
    }

    private LottieFrameLoop Loop(double duration, bool loop) => new(
        TimeSpan.FromSeconds(duration), loop, () => elapsed,
        () => log.Add("starting"),
        seconds => log.Add(FormattableString.Invariant($"draw {seconds}")),
        () => log.Add("completed"),
        reason => log.Add($"failed: {reason}"));
}
