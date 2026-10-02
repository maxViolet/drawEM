namespace DrawEM.App.Infrastructure;

/// <summary>
/// Routes low-level keyboard events to one synchronous decision handler. Events that carry
/// <see cref="NeutralKeyTag"/> are drawEM's own neutral keys: they pass through without invoking it.
/// </summary>
public sealed class KeyboardHookEvents : IKeyboardHookSource
{
    /// <summary>The extra-info value that marks drawEM's injected neutral key events.</summary>
    public const nuint NeutralKeyTag = 0x6472_454D;

    private Func<int, KeyDirection, bool>? handleKey;

    public void SetKeyHandler(Func<int, KeyDirection, bool> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        if (handleKey is not null)
        {
            throw new InvalidOperationException("The keyboard hook already has a key handler.");
        }

        handleKey = handler;
    }

    /// <returns><c>true</c> to hide the event from the focused application.</returns>
    public bool Handle(int vkCode, KeyDirection direction, nuint extraInfo)
    {
        if (extraInfo == NeutralKeyTag)
        {
            return false;
        }

        return handleKey?.Invoke(vkCode, direction) ?? false;
    }
}
