using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DrawEM.App.Domain.Drawing;

namespace DrawEM.App.Infrastructure.Effects;

/// <summary>
/// Temporary S4-01 measurement load, removed with the effect probe. While started, a background thread
/// injects one event at a time, alternating a mouse move along a small circle and a press of an unassigned
/// key, and times it until a low-level hook on the UI thread receives it. These hooks are installed after
/// the production hooks, so they run first in the chain on the same thread and see the same delivery delay.
/// </summary>
/// <remarks>
/// Probe keys carry <see cref="KeyboardHookEvents.NeutralKeyTag"/>, so the shortcut handler ignores them.
/// Probe mouse moves are ordinary moves for the drawing hook: in draw mode they draw a stroke.
/// </remarks>
public sealed class EffectProbeInputLoad : IDisposable
{
    /// <summary>An unassigned virtual-key code, distinct from the neutral key the shortcut handler emits.</summary>
    public const int ProbeKey = 0x97;

    /// <summary>The extra-info value that marks the probe's injected mouse moves.</summary>
    public const nuint MouseTag = 0x6472_4550;

    private const int NonePending = 0;
    private const int KeyboardPending = 1;
    private const int MousePending = 2;

    /// <summary>Radius in physical pixels of the circle the injected mouse moves follow.</summary>
    private const int CircleRadius = 40;

    /// <summary>Angle in radians between consecutive injected mouse positions.</summary>
    private const double CircleStep = 0.25;

    /// <summary>Largest normalized coordinate of an absolute SendInput mouse move.</summary>
    private const int NormalizedMaximum = 65535;

    /// <summary>An event the hook has not received within this time counts as a timeout.</summary>
    private static readonly TimeSpan ReceiptTimeout = TimeSpan.FromSeconds(1);

    /// <summary>Pause after each received event. About 100 events per second with the sleep granularity.</summary>
    private static readonly TimeSpan PauseBetweenEvents = TimeSpan.FromMilliseconds(5);

    private readonly NativeMethods.LowLevelHookProc keyboardProc;
    private readonly NativeMethods.LowLevelHookProc mouseProc;
    private readonly AutoResetEvent received = new(false);
    private IntPtr keyboardHook;
    private IntPtr mouseHook;
    private Thread? injector;
    private volatile bool running;
    private long pendingTimestamp;
    private int pendingKind;
    private int timeouts;

    /// <summary>Installs the measuring hooks on the calling thread, which must run a message loop.</summary>
    public EffectProbeInputLoad()
    {
        keyboardProc = KeyboardCallback;
        mouseProc = MouseCallback;
        var module = NativeMethods.GetModuleHandle(null);
        keyboardHook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, keyboardProc, module, NativeMethods.AllThreads);
        mouseHook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, mouseProc, module, NativeMethods.AllThreads);
        if (keyboardHook == IntPtr.Zero || mouseHook == IntPtr.Zero)
        {
            Dispose();
            throw new InvalidOperationException("Failed to register the effect probe input hooks.");
        }
    }

    /// <summary>
    /// One injected event reached the hook: whether it was a key, and the delay in milliseconds.
    /// Raised on the hook thread.
    /// </summary>
    public event Action<bool, double>? DelayMeasured;

    /// <summary>Starts injecting events, moving the pointer around <paramref name="center"/>.</summary>
    public void Start(ScreenPoint center, MonitorBounds monitor)
    {
        Stop();
        var circleCenter = new ScreenPoint(
            Math.Clamp(center.X, monitor.Left + CircleRadius, monitor.Right - 1 - CircleRadius),
            Math.Clamp(center.Y, monitor.Top + CircleRadius, monitor.Bottom - 1 - CircleRadius));
        running = true;
        injector = new Thread(() => Inject(circleCenter)) { IsBackground = true, Name = "drawEM effect probe input" };
        injector.Start();
    }

    /// <summary>Stops injecting and returns how many events no hook received within the timeout.</summary>
    public int Stop()
    {
        running = false;
        received.Set();
        injector?.Join(ReceiptTimeout * 2);
        injector = null;
        Volatile.Write(ref pendingKind, NonePending);
        return Interlocked.Exchange(ref timeouts, 0);
    }

    public void Dispose()
    {
        Stop();
        if (keyboardHook != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(keyboardHook);
            keyboardHook = IntPtr.Zero;
        }

        if (mouseHook != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(mouseHook);
            mouseHook = IntPtr.Zero;
        }
    }

    private void Inject(ScreenPoint center)
    {
        var angle = 0d;
        var keyboard = false;
        while (running)
        {
            keyboard = !keyboard;
            NativeMethods.INPUT[] inputs;
            if (keyboard)
            {
                inputs = [Key(keyUp: false), Key(keyUp: true)];
            }
            else
            {
                angle += CircleStep;
                inputs = [MoveTo(center.X + (int)Math.Round(CircleRadius * Math.Cos(angle)),
                    center.Y + (int)Math.Round(CircleRadius * Math.Sin(angle)))];
            }

            received.Reset();
            Volatile.Write(ref pendingTimestamp, Stopwatch.GetTimestamp());
            Volatile.Write(ref pendingKind, keyboard ? KeyboardPending : MousePending);
            var sent = NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<NativeMethods.INPUT>());
            if (sent == 0 || !received.WaitOne(ReceiptTimeout))
            {
                if (running && Interlocked.Exchange(ref pendingKind, NonePending) != NonePending)
                {
                    Interlocked.Increment(ref timeouts);
                }
            }

            Thread.Sleep(PauseBetweenEvents);
        }
    }

    private IntPtr KeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= NativeMethods.HC_ACTION)
        {
            var now = Stopwatch.GetTimestamp();
            var message = wParam.ToInt32();
            var data = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
            if (data.vkCode == ProbeKey
                && (nuint)data.dwExtraInfo == KeyboardHookEvents.NeutralKeyTag
                && message is NativeMethods.WM_KEYDOWN or NativeMethods.WM_SYSKEYDOWN)
            {
                Receive(KeyboardPending, now);
            }
        }

        return NativeMethods.CallNextHookEx(keyboardHook, nCode, wParam, lParam);
    }

    private IntPtr MouseCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= NativeMethods.HC_ACTION && wParam.ToInt32() == NativeMethods.WM_MOUSEMOVE)
        {
            var now = Stopwatch.GetTimestamp();
            var data = Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam);
            if ((nuint)data.dwExtraInfo == MouseTag)
            {
                Receive(MousePending, now);
            }
        }

        return NativeMethods.CallNextHookEx(mouseHook, nCode, wParam, lParam);
    }

    private void Receive(int kind, long now)
    {
        if (Interlocked.CompareExchange(ref pendingKind, NonePending, kind) != kind)
        {
            return;
        }

        var delayMs = (now - Volatile.Read(ref pendingTimestamp)) * 1000d / Stopwatch.Frequency;
        DelayMeasured?.Invoke(kind == KeyboardPending, delayMs);
        received.Set();
    }

    private static NativeMethods.INPUT Key(bool keyUp) => new()
    {
        type = NativeMethods.INPUT_KEYBOARD,
        u = new NativeMethods.InputUnion
        {
            ki = new NativeMethods.KEYBDINPUT
            {
                wVk = ProbeKey,
                dwFlags = keyUp ? NativeMethods.KEYEVENTF_KEYUP : 0,
                dwExtraInfo = (IntPtr)KeyboardHookEvents.NeutralKeyTag,
            },
        },
    };

    private static NativeMethods.INPUT MoveTo(int x, int y)
    {
        var left = NativeMethods.GetSystemMetrics(NativeMethods.SM_XVIRTUALSCREEN);
        var top = NativeMethods.GetSystemMetrics(NativeMethods.SM_YVIRTUALSCREEN);
        var width = Math.Max(2, NativeMethods.GetSystemMetrics(NativeMethods.SM_CXVIRTUALSCREEN));
        var height = Math.Max(2, NativeMethods.GetSystemMetrics(NativeMethods.SM_CYVIRTUALSCREEN));
        return new NativeMethods.INPUT
        {
            type = NativeMethods.INPUT_MOUSE,
            u = new NativeMethods.InputUnion
            {
                mi = new NativeMethods.MOUSEINPUT
                {
                    dx = (int)Math.Round((x - left) * (double)NormalizedMaximum / (width - 1)),
                    dy = (int)Math.Round((y - top) * (double)NormalizedMaximum / (height - 1)),
                    dwFlags = NativeMethods.MOUSEEVENTF_MOVE | NativeMethods.MOUSEEVENTF_ABSOLUTE
                        | NativeMethods.MOUSEEVENTF_VIRTUALDESK,
                    dwExtraInfo = (IntPtr)MouseTag,
                },
            },
        };
    }
}
