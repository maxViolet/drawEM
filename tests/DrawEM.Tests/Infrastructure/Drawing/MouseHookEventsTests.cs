using System.Runtime.InteropServices;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure.Drawing;

namespace DrawEM.Tests.Infrastructure.Drawing;

/// <summary>
/// Raw low-level mouse hook arguments, as Windows passes them to the hook callback, routed through
/// <see cref="MouseHookEvents.HandleHook"/>.
/// </summary>
public class MouseHookEventsTests
{
    public static TheoryData<int> ButtonMessages => new()
    {
        NativeMethods.WM_LBUTTONDOWN, NativeMethods.WM_LBUTTONUP,
        NativeMethods.WM_RBUTTONDOWN, NativeMethods.WM_RBUTTONUP,
        NativeMethods.WM_MBUTTONDOWN, NativeMethods.WM_MBUTTONUP,
        NativeMethods.WM_XBUTTONDOWN, NativeMethods.WM_XBUTTONUP,
    };

    public static TheoryData<int> WheelMessages => new() { NativeMethods.WM_MOUSEWHEEL, NativeMethods.WM_MOUSEHWHEEL };

    [Fact]
    public void HandleHook_MoveReportsPointAndPassesOn()
    {
        var events = Suppressing(out _);
        var points = new List<ScreenPoint>();
        events.PointerMoved += points.Add;

        Assert.False(WithPoint(-5, 7, lParam => events.HandleHook(NativeMethods.HC_ACTION, NativeMethods.WM_MOUSEMOVE, lParam)));
        Assert.Equal([new ScreenPoint(-5, 7)], points);
    }

    [Theory]
    [MemberData(nameof(ButtonMessages))]
    public void HandleHook_ButtonReturnsButtonActivityDecision(int message)
    {
        var events = new MouseHookEvents();
        var asked = new List<string>();
        events.PointerButtonActivity += () => Ask(asked, "button", true);
        events.PointerWheelActivity += () => Ask(asked, "wheel", false);

        Assert.True(WithPoint(0, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, message, lParam)));
        Assert.Equal(["button"], asked);
    }

    [Theory]
    [MemberData(nameof(WheelMessages))]
    public void HandleHook_WheelReturnsWheelActivityDecision(int message)
    {
        var events = new MouseHookEvents();
        var asked = new List<string>();
        events.PointerButtonActivity += () => Ask(asked, "button", false);
        events.PointerWheelActivity += () => Ask(asked, "wheel", true);

        Assert.True(WithPoint(0, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, message, lParam)));
        Assert.Equal(["wheel"], asked);
    }

    [Theory]
    [InlineData(NativeMethods.WM_LBUTTONDOWN)]
    [InlineData(NativeMethods.WM_MOUSEWHEEL)]
    public void HandleHook_ActivityWithoutSubscriberPassesOn(int message)
    {
        var events = new MouseHookEvents();

        Assert.False(WithPoint(0, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, message, lParam)));
    }

    [Fact]
    public void HandleHook_ActivityThatDeclinesPassesOn()
    {
        var events = new MouseHookEvents();
        events.PointerButtonActivity += () => false;

        Assert.False(WithPoint(0, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, NativeMethods.WM_LBUTTONDOWN, lParam)));
    }

    [Fact]
    public void HandleHook_OtherMessagePassesOn()
    {
        const int WM_NCMOUSEMOVE = 0x00A0;
        var events = Suppressing(out var calls);
        var moves = 0;
        events.PointerMoved += _ => moves++;

        Assert.False(WithPoint(0, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, WM_NCMOUSEMOVE, lParam)));
        Assert.Equal(0, calls());
        Assert.Equal(0, moves);
    }

    [Fact]
    public void HandleHook_NegativeCodeIsPassedOnWithoutReadingTheEvent()
    {
        var events = Suppressing(out var calls);
        var moves = 0;
        events.PointerMoved += _ => moves++;

        Assert.False(events.HandleHook(-1, NativeMethods.WM_MOUSEMOVE, IntPtr.Zero));
        Assert.False(events.HandleHook(-1, NativeMethods.WM_LBUTTONDOWN, IntPtr.Zero));
        Assert.Equal(0, calls());
        Assert.Equal(0, moves);
    }

    [Fact]
    public void HandleHook_NegativeCodeWithWideParameterIsPassedOn()
    {
        var events = Suppressing(out var calls);

        Assert.False(events.HandleHook(-1, (IntPtr)(1L << 32), IntPtr.Zero));
        Assert.Equal(0, calls());
    }

    private static bool Ask(List<string> asked, string name, bool suppress)
    {
        asked.Add(name);
        return suppress;
    }

    private static MouseHookEvents Suppressing(out Func<int> calls)
    {
        var events = new MouseHookEvents();
        var count = 0;
        events.PointerButtonActivity += () => ++count > 0;
        events.PointerWheelActivity += () => ++count > 0;
        calls = () => count;
        return events;
    }

    private static bool WithPoint(int x, int y, Func<IntPtr, bool> handle)
    {
        var data = new NativeMethods.MSLLHOOKSTRUCT { pt = new NativeMethods.POINT { X = x, Y = y } };
        var lParam = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.MSLLHOOKSTRUCT>());
        try
        {
            Marshal.StructureToPtr(data, lParam, false);
            return handle(lParam);
        }
        finally
        {
            Marshal.FreeHGlobal(lParam);
        }
    }
}
