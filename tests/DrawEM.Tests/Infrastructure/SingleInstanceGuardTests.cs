using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class SingleInstanceGuardTests
{
    private readonly string name = @"Local\drawEM-tests-" + Guid.NewGuid().ToString("N");

    [Fact]
    public void DefaultName_IsGlobalAndScopedToCurrentUser()
    {
        var sid = System.Security.Principal.WindowsIdentity.GetCurrent().User!.Value;

        Assert.Equal($@"Global\drawEM-{sid}", SingleInstanceGuard.DefaultName);
    }

    [Fact]
    public void TryAcquire_WhileAnotherInstanceHoldsName_ReturnsNull()
    {
        using var first = SingleInstanceGuard.TryAcquire(name);

        var second = OnOtherThread(() => SingleInstanceGuard.TryAcquire(name));

        Assert.NotNull(first);
        Assert.Null(second);
    }

    [Fact]
    public void TryAcquire_AfterFirstInstanceExits_Succeeds()
    {
        SingleInstanceGuard.TryAcquire(name)!.Dispose();

        var acquired = OnOtherThread(() =>
        {
            using var guard = SingleInstanceGuard.TryAcquire(name);
            return guard is not null;
        });

        Assert.True(acquired);
    }

    [Fact]
    public void TryAcquire_AfterInstanceDiedHoldingName_TakesItOver()
    {
        // A thread that ends while owning the mutex abandons it, as a killed process does.
        var abandonedGuard = OnOtherThread(() => SingleInstanceGuard.TryAcquire(name));
        Assert.NotNull(abandonedGuard);

        SingleInstanceGuard? guard = null;
        try
        {
            // A zero-timeout wait can race with the mutex becoming abandoned after Thread.Join.
            Assert.True(SpinWait.SpinUntil(
                () => (guard = SingleInstanceGuard.TryAcquire(name)) is not null,
                TimeSpan.FromSeconds(1)));
        }
        finally
        {
            guard?.Dispose();
            GC.KeepAlive(abandonedGuard);
        }
    }

    [Fact]
    public void Dispose_Twice_DoesNotThrow()
    {
        var guard = SingleInstanceGuard.TryAcquire(name)!;

        guard.Dispose();
        guard.Dispose();
    }

    private static T OnOtherThread<T>(Func<T> action)
    {
        T result = default!;
        var thread = new Thread(() => result = action());
        thread.Start();
        thread.Join();
        return result;
    }
}
