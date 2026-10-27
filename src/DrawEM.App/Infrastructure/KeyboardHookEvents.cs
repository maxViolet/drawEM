using System.Runtime.InteropServices;

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

    /// <summary>
    /// Routes the raw arguments of a low-level keyboard hook callback. A negative <paramref name="nCode"/> and a
    /// message that is not a key press or release pass through unread.
    /// </summary>
    /// <returns><c>true</c> to hide the event from the focused application.</returns>
    internal bool HandleHook(int nCode, int message, IntPtr lParam)
    {
        if (nCode < NativeMethods.HC_ACTION || Direction(message) is not { } direction)
        {
            return false;
        }

        var data = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
        return Handle((int)data.vkCode, direction, (nuint)data.dwExtraInfo);
    }

    private static KeyDirection? Direction(int message) => message switch
    {
        NativeMethods.WM_KEYDOWN or NativeMethods.WM_SYSKEYDOWN => KeyDirection.Down,
        NativeMethods.WM_KEYUP or NativeMethods.WM_SYSKEYUP => KeyDirection.Up,
        _ => null,
    };
}
