namespace DrawEM.App.Infrastructure;

/// <summary>Whether a keyboard event presses (including auto-repeat) or releases a key.</summary>
public enum KeyDirection
{
    Down,
    Up,
}

public interface IKeyboardHookSource
{
    /// <summary>
    /// Installs the sole synchronous key decision handler. Its return value determines whether the
    /// current event is hidden from the focused application.
    /// </summary>
    void SetKeyHandler(Func<int, KeyDirection, bool> handleKey);
}
