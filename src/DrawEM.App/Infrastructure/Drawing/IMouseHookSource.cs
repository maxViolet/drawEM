using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Drawing;

public interface IMouseHookSource
{
    event Action<ScreenPoint>? PointerMoved;

    event Func<bool>? PointerButtonActivity;

    event Func<bool>? PointerWheelActivity;
}
