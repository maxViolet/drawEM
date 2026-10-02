namespace DrawEM.App.Infrastructure;

/// <summary>
/// Turns low-level keyboard events into <see cref="IKeyboardHookSource"/> events. Events that carry
/// <see cref="NeutralKeyTag"/> are drawEM's own neutral keys: they raise nothing and always pass through.
/// </summary>
/// <param name="sendNeutralKey">Injects the neutral key-down and key-up tagged with <see cref="NeutralKeyTag"/>.</param>
public sealed class KeyboardHookEvents(Action sendNeutralKey) : IKeyboardHookSource
{
    /// <summary>The extra-info value that marks drawEM's injected neutral key events.</summary>
    public const nuint NeutralKeyTag = 0x6472_454D;

    public event Action<int>? KeyDown;

    public event Action<int>? KeyUp;

    public event Func<int, KeyDirection, bool>? KeySuppressionRequested;

    public void EmitNeutralKey() => sendNeutralKey();

    /// <returns><c>true</c> to hide the event from the focused application.</returns>
    public bool Handle(int vkCode, KeyDirection direction, nuint extraInfo)
    {
        if (extraInfo == NeutralKeyTag)
        {
            return false;
        }

        if (direction == KeyDirection.Down)
        {
            KeyDown?.Invoke(vkCode);
            return ShouldSuppressKey(vkCode, direction);
        }

        var suppress = ShouldSuppressKey(vkCode, direction);
        KeyUp?.Invoke(vkCode);
        return suppress;
    }

    private bool ShouldSuppressKey(int vkCode, KeyDirection direction) =>
        KeySuppressionRequested?.Invoke(vkCode, direction) ?? false;
}
