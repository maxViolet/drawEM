using DrawEM.App.Application.Input;

namespace DrawEM.App.Infrastructure;

public interface IKeyboardHookSource
{
    /// <summary>
    /// Raised inside the hook callback for every key-down (including auto-repeat) and key-up, with the
    /// virtual-key code. Returns <c>true</c> to hide the event from the focused application; the answer is
    /// needed before the callback returns.
    /// </summary>
    event Func<int, KeyDirection, bool>? KeyEvent;
}
