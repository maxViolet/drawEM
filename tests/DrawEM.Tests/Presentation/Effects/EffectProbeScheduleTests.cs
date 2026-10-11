using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

public class EffectProbeScheduleTests
{
    [Theory]
    [InlineData(3.0, 5)] // Monitor probe animation
    [InlineData(2.0, 8)] // Cursor probe animation
    [InlineData(10.0, 1)] // Longer than the budget allows: still one measured invocation
    public void MeasuredInvocations_FitTheRunIntoItsBudget(double animationSeconds, int expected)
    {
        Assert.Equal(expected, EffectSurfaceProbe.MeasuredInvocationsFor(TimeSpan.FromSeconds(animationSeconds)));
    }

    [Theory]
    [InlineData(3.0)]
    [InlineData(2.0)]
    public void PlannedRun_TakesAtMostTheBudget(double animationSeconds)
    {
        var duration = TimeSpan.FromSeconds(animationSeconds);

        Assert.True(EffectSurfaceProbe.PlannedRunTime(duration) <= EffectSurfaceProbe.RunBudget);
    }
}
