using DrawEM.App.Domain;

namespace DrawEM.App.Infrastructure;

public interface IMouseHookSource
{
    event Action<ScreenPoint>? PointerMoved;

    event Func<bool>? PointerButtonActivity;

    event Func<bool>? PointerWheelActivity;
}
