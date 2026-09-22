namespace DrawEM.App.Infrastructure;

public interface IKeyboardHookSource
{
    event Action<int>? KeyDown;

    event Action<int>? KeyUp;

    event Func<int, bool>? KeySuppressionRequested;
}
