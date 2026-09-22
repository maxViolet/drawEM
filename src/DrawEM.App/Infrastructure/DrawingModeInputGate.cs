using System.Threading;

namespace DrawEM.App.Infrastructure;

public sealed class DrawingModeInputGate
{
    private int active;

    public bool IsActive => Volatile.Read(ref active) == 1;

    public void SetActive(bool isActive) => Interlocked.Exchange(ref active, isActive ? 1 : 0);
}
