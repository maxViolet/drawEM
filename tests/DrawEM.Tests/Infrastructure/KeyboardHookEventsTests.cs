using System.Runtime.InteropServices;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

/// <summary>
/// Raw low-level keyboard hook arguments, as Windows passes them to the hook callback, routed through
/// <see cref="KeyboardHookEvents.HandleHook"/>.
/// </summary>
public class KeyboardHookEventsTests
{
    private const int Z = 0x5A;

    [Theory]
    [InlineData(NativeMethods.WM_KEYDOWN, KeyDirection.Down)]
    [InlineData(NativeMethods.WM_SYSKEYDOWN, KeyDirection.Down)]
    [InlineData(NativeMethods.WM_KEYUP, KeyDirection.Up)]
    [InlineData(NativeMethods.WM_SYSKEYUP, KeyDirection.Up)]
    public void HandleHook_PassesKeyAndDirectionAndReturnsHandlerDecision(int message, KeyDirection expected)
    {
        var events = new KeyboardHookEvents();
        var seen = new List<(int, KeyDirection)>();
        events.SetKeyHandler((key, direction) =>
        {
            seen.Add((key, direction));
            return true;
        });

        var suppressed = WithKey(Z, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, message, lParam));

        Assert.True(suppressed);
        Assert.Equal([(Z, expected)], seen);
    }

    [Fact]
    public void HandleHook_NegativeCodeIsPassedOnWithoutReadingTheEvent()
    {
        var events = new KeyboardHookEvents();
        var calls = 0;
        events.SetKeyHandler((_, _) => ++calls > 0);

        Assert.False(events.HandleHook(-1, NativeMethods.WM_KEYDOWN, IntPtr.Zero));
        Assert.Equal(0, calls);
    }

    [Fact]
    public void HandleHook_NegativeCodeWithWideParameterIsPassedOn()
    {
        var events = new KeyboardHookEvents();
        events.SetKeyHandler((_, _) => true);

        Assert.False(events.HandleHook(-1, (IntPtr)(1L << 32), IntPtr.Zero));
    }

    [Fact]
    public void HandleHook_OtherMessageIsPassedOn()
    {
        const int WM_CHAR = 0x0102;
        var events = new KeyboardHookEvents();
        var calls = 0;
        events.SetKeyHandler((_, _) => ++calls > 0);

        Assert.False(WithKey(Z, 0, lParam => events.HandleHook(NativeMethods.HC_ACTION, WM_CHAR, lParam)));
        Assert.Equal(0, calls);
    }

    [Fact]
    public void HandleHook_NeutralKeyIsPassedOnWithoutInvokingHandler()
    {
        var events = new KeyboardHookEvents();
        var calls = 0;
        events.SetKeyHandler((_, _) => ++calls > 0);

        var suppressed = WithKey(
            Z,
            KeyboardHookEvents.NeutralKeyTag,
            lParam => events.HandleHook(NativeMethods.HC_ACTION, NativeMethods.WM_KEYDOWN, lParam));

        Assert.False(suppressed);
        Assert.Equal(0, calls);
    }

    private static bool WithKey(int vkCode, nuint extraInfo, Func<IntPtr, bool> handle)
    {
        var data = new NativeMethods.KBDLLHOOKSTRUCT { vkCode = (uint)vkCode, dwExtraInfo = (IntPtr)extraInfo };
        var lParam = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.KBDLLHOOKSTRUCT>());
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
