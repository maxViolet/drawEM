using System.Threading;

namespace DrawEM.App.Infrastructure;

public sealed class DrawingModeInputGate
{
    /// <summary>Flag value for an inactive gate. Interlocked has no bool overload, so the flag is an int.</summary>
    private const int Inactive = 0;

    /// <summary>Flag value for an active gate.</summary>
    private const int Active = 1;

    private int active = Inactive;

    public bool IsActive => Volatile.Read(ref active) == Active;

    public void SetActive(bool isActive) => Interlocked.Exchange(ref active, isActive ? Active : Inactive);
}
