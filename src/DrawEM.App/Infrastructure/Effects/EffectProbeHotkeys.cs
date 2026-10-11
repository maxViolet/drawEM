using System.ComponentModel;
using System.Windows.Interop;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Temporary S4-01 trigger, enabled only with <c>DRAWEM_EFFECT_PROBE=1</c>: Ctrl+Alt+F9 shows one probe
/// effect, Ctrl+Alt+F10 runs the probe benchmark in the startup mode, and Ctrl+Alt+F11 runs it for a
/// Cursor effect. Registered for the UI thread, outside the low-level
/// hooks and the shortcut settings. S4-06 removes it when effects get their own shortcuts.
/// </summary>
public sealed class EffectProbeHotkeys : IDisposable
{
    private const int InvokeKey = 0x78; // F9

    private const int BenchmarkKey = 0x79; // F10

    private const int CursorBenchmarkKey = 0x7A; // F11

    private const int InvokeId = 1;
    private const int BenchmarkId = 2;
    private const int CursorBenchmarkId = 3;
    private const uint Modifiers = NativeMethods.MOD_CONTROL | NativeMethods.MOD_ALT | NativeMethods.MOD_NOREPEAT;

    private readonly Action invoke;
    private readonly Action runBenchmark;
    private readonly Action runCursorBenchmark;
    private bool disposed;

    public EffectProbeHotkeys(Action invoke, Action runBenchmark, Action runCursorBenchmark)
    {
        this.invoke = invoke;
        this.runBenchmark = runBenchmark;
        this.runCursorBenchmark = runCursorBenchmark;
        ComponentDispatcher.ThreadFilterMessage += OnThreadMessage;
        try
        {
            Register(InvokeId, InvokeKey, "Ctrl+Alt+F9");
            Register(BenchmarkId, BenchmarkKey, "Ctrl+Alt+F10");
            Register(CursorBenchmarkId, CursorBenchmarkKey, "Ctrl+Alt+F11");
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        ComponentDispatcher.ThreadFilterMessage -= OnThreadMessage;
        NativeMethods.UnregisterHotKey(IntPtr.Zero, InvokeId);
        NativeMethods.UnregisterHotKey(IntPtr.Zero, BenchmarkId);
        NativeMethods.UnregisterHotKey(IntPtr.Zero, CursorBenchmarkId);
    }

    private static void Register(int id, int key, string name)
    {
        if (!NativeMethods.RegisterHotKey(IntPtr.Zero, id, Modifiers, (uint)key))
        {
            throw new InvalidOperationException(
                $"The effect probe could not register {name}; another application may use it.",
                new Win32Exception());
        }
    }

    private void OnThreadMessage(ref MSG message, ref bool handled)
    {
        // A hotkey registered without a window is posted to the thread with no window handle.
        if (handled || message.hwnd != IntPtr.Zero || message.message != NativeMethods.WM_HOTKEY)
        {
            return;
        }

        switch ((int)message.wParam)
        {
            case InvokeId:
                handled = true;
                invoke();
                break;
            case BenchmarkId:
                handled = true;
                runBenchmark();
                break;
            case CursorBenchmarkId:
                handled = true;
                runCursorBenchmark();
                break;
        }
    }
}
