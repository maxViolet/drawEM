using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public sealed class CleanupStepsTests
{
    [Fact]
    public void RunAll_RunsEveryStepInOrder()
    {
        var ran = new List<int>();

        CleanupSteps.RunAll([() => ran.Add(1), () => ran.Add(2), () => ran.Add(3)]);

        Assert.Equal([1, 2, 3], ran);
    }

    [Fact]
    public void RunAll_RethrowsOneFailureAsIsAfterTheLastStep()
    {
        var ran = new List<int>();
        var failure = new InvalidOperationException("close failed");

        var thrown = Assert.Throws<InvalidOperationException>(
            () => CleanupSteps.RunAll([() => ran.Add(1), () => throw failure, () => ran.Add(3)]));

        Assert.Same(failure, thrown);
        Assert.Equal([1, 3], ran);
    }

    [Fact]
    public void RunAll_ThrowsSeveralFailuresTogetherInOrder()
    {
        var first = new InvalidOperationException("first");
        var second = new ArgumentException("second");

        var thrown = Assert.Throws<AggregateException>(
            () => CleanupSteps.RunAll([() => throw first, () => throw second]));

        Assert.Equal([first, second], thrown.InnerExceptions);
    }

    [Fact]
    public void Failures_RunsEveryStepAndReturnsEachFailureInOrder()
    {
        var ran = new List<int>();
        var first = new InvalidOperationException("first");
        var second = new ArgumentException("second");

        var failures = CleanupSteps.Failures([() => throw first, () => ran.Add(2), () => throw second]);

        Assert.Equal([first, second], failures);
        Assert.Equal([2], ran);
    }

    [Fact]
    public void Failures_IsEmptyWhenEveryStepSucceeds()
    {
        Assert.Empty(CleanupSteps.Failures([() => { }]));
    }
}
