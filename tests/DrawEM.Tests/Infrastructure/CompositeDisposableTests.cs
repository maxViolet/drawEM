using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class CompositeDisposableTests
{
    [Fact]
    public void Dispose_ReleasesEveryResourceInOrder()
    {
        var calls = new List<string>();
        var resources = new CompositeDisposable(
            new FakeDisposable(calls, "keyboard"),
            new FakeDisposable(calls, "sound"));

        resources.Dispose();
        resources.Dispose();

        Assert.Equal(["keyboard", "sound"], calls);
    }

    [Fact]
    public void Dispose_WhenOneResourceThrows_ReleasesTheRestThenRethrows()
    {
        var calls = new List<string>();
        var failure = new InvalidOperationException("Sound stop failed.");
        var resources = new CompositeDisposable(
            new FakeDisposable(calls, "keyboard"),
            new FakeDisposable(calls, "sound", failure),
            new FakeDisposable(calls, "sound log"));

        var thrown = Assert.Throws<InvalidOperationException>(resources.Dispose);

        Assert.Same(failure, thrown);
        Assert.Equal(["keyboard", "sound", "sound log"], calls);
    }

    [Fact]
    public void Dispose_WhenSeveralResourcesThrow_ReportsEveryFailure()
    {
        var calls = new List<string>();
        var first = new InvalidOperationException("Keyboard hook failed.");
        var second = new InvalidOperationException("Sound stop failed.");
        var resources = new CompositeDisposable(
            new FakeDisposable(calls, "keyboard", first),
            new FakeDisposable(calls, "sound", second));

        var thrown = Assert.Throws<AggregateException>(resources.Dispose);

        Assert.Equal([first, second], thrown.InnerExceptions);
        Assert.Equal(["keyboard", "sound"], calls);
    }

    private sealed class FakeDisposable(List<string> calls, string name, Exception? failure = null) : IDisposable
    {
        public void Dispose()
        {
            calls.Add(name);
            if (failure is not null)
            {
                throw failure;
            }
        }
    }
}
