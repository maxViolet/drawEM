namespace DrawEM.App.Infrastructure;

/// <summary>Whether a keyboard event presses (including auto-repeat) or releases a key.</summary>
public enum KeyDirection
{
    Down,
    Up,
}

public interface IKeyboardHookSource
{
    event Action<int>? KeyDown;

    event Action<int>? KeyUp;

    /// <summary>
    /// Asks whether to hide a key event from the focused application. Raised after
    /// <see cref="KeyDown"/> for a key-down and before <see cref="KeyUp"/> for a key-up.
    /// </summary>
    event Func<int, KeyDirection, bool>? KeySuppressionRequested;

    /// <summary>
    /// Queues one key-down and key-up of <see cref="VirtualKeys.Neutral"/> behind the event being handled.
    /// The source never raises events for its own neutral keys and never suppresses them.
    /// </summary>
    void EmitNeutralKey();
}
