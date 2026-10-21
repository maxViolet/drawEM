using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class InstanceActivationTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);
    private readonly string name = @"Local\drawEM-activation-tests-" + Guid.NewGuid().ToString("N");

    [Fact]
    public void DefaultName_IsGlobalAndScopedToCurrentUser()
    {
        var sid = System.Security.Principal.WindowsIdentity.GetCurrent().User!.Value;

        Assert.Equal($@"Global\drawEM-activate-{sid}", InstanceActivation.DefaultName);
    }

    [Fact]
    public void Signal_WhileRunningInstanceListens_ReachesIt()
    {
        using var activated = new SemaphoreSlim(0);
        using var listener = InstanceActivation.Listen(name, () => activated.Release());

        var sent = InstanceActivation.Signal(name);

        Assert.True(sent);
        Assert.True(activated.Wait(Timeout));
    }

    [Fact]
    public void Signal_Repeated_ReachesRunningInstanceEachTime()
    {
        using var activated = new SemaphoreSlim(0);
        using var listener = InstanceActivation.Listen(name, () => activated.Release());

        Assert.True(InstanceActivation.Signal(name));
        Assert.True(activated.Wait(Timeout));
        Assert.True(InstanceActivation.Signal(name));
        Assert.True(activated.Wait(Timeout));
    }

    [Fact]
    public void Signal_WithoutListener_ReturnsFalse()
    {
        Assert.False(InstanceActivation.Signal(name));
    }

    [Fact]
    public void Signal_AfterListenerDisposed_ReturnsFalse()
    {
        // The thread pool releases the event asynchronously unless Dispose waits for it; load it to catch that race.
        Parallel.For(0, 500, i =>
        {
            var attemptName = $"{name}-{i}";
            InstanceActivation.Listen(attemptName, () => { }).Dispose();

            Assert.False(InstanceActivation.Signal(attemptName), $"attempt {i}");
        });
    }

    [Fact]
    public void Dispose_Twice_DoesNotThrow()
    {
        var listener = InstanceActivation.Listen(name, () => { });

        listener.Dispose();
        listener.Dispose();
    }
}
