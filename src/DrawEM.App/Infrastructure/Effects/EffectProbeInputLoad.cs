using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DrawEM.App.Domain.Drawing;
using SharedNative = DrawEM.App.Infrastructure.NativeMethods;

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

    private static readonly int InputSize = Marshal.SizeOf<SharedNative.INPUT>();

    private readonly NativeMethods.LowLevelHookProc keyboardProc;
    private readonly NativeMethods.LowLevelHookProc mouseProc;
    private readonly AutoResetEvent received = new(false);

    /// <summary>Set by <see cref="Stop"/>, so the injector's pause ends at once instead of after a timer tick.</summary>
    private readonly ManualResetEvent stopping = new(false);

    private IntPtr keyboardHook;
    private IntPtr mouseHook;
    private Thread? injector;
    private long pendingTimestamp;
    private int pendingKind;
    private int keyboardReceived;
    private int mouseReceived;
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
        var desktop = VirtualDesktop.Read();
        stopping.Reset();
        injector = new Thread(() => Inject(circleCenter, desktop)) { IsBackground = true, Name = "drawEM effect probe input" };
        injector.Start();
    }

    /// <summary>
    /// Stops injecting. Returns the keyboard and mouse events the hooks received since <see cref="Start"/>,
    /// and the events no hook received within the timeout.
    /// </summary>
    public (int Keyboard, int Mouse, int Timeouts) Stop()
    {
        stopping.Set();
        received.Set();
        injector?.Join(ReceiptTimeout * 2);
        injector = null;
        Volatile.Write(ref pendingKind, NonePending);
        return (Interlocked.Exchange(ref keyboardReceived, 0),
            Interlocked.Exchange(ref mouseReceived, 0),
            Interlocked.Exchange(ref timeouts, 0));
    }

    public void Dispose()
    {
        Stop();
        Unhook(ref keyboardHook);
        Unhook(ref mouseHook);
    }

    private static void Unhook(ref IntPtr hook)
    {
        if (hook != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(hook);
            hook = IntPtr.Zero;
        }
    }

    private void Inject(ScreenPoint center, VirtualDesktop desktop)
    {
        var angle = 0d;
        var keyboard = false;
        while (!stopping.WaitOne(0))
        {
            keyboard = !keyboard;
            SharedNative.INPUT[] inputs;
            if (keyboard)
            {
                inputs =
                [
                    SharedNative.INPUT.Key(ProbeKey, keyUp: false, KeyboardHookEvents.NeutralKeyTag),
                    SharedNative.INPUT.Key(ProbeKey, keyUp: true, KeyboardHookEvents.NeutralKeyTag),
                ];
            }
            else
            {
                angle += CircleStep;
                inputs = [desktop.MoveTo(center.X + (int)Math.Round(CircleRadius * Math.Cos(angle)),
                    center.Y + (int)Math.Round(CircleRadius * Math.Sin(angle)))];
            }

            received.Reset();
            Volatile.Write(ref pendingTimestamp, Stopwatch.GetTimestamp());
            Volatile.Write(ref pendingKind, keyboard ? KeyboardPending : MousePending);
            var sent = SharedNative.SendInput((uint)inputs.Length, inputs, InputSize);
            if (sent == 0 || !received.WaitOne(ReceiptTimeout))
            {
                if (!stopping.WaitOne(0) && Interlocked.Exchange(ref pendingKind, NonePending) != NonePending)
                {
                    Interlocked.Increment(ref timeouts);
                }
            }

            stopping.WaitOne(PauseBetweenEvents);
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
        Interlocked.Increment(ref kind == KeyboardPending ? ref keyboardReceived : ref mouseReceived);
        DelayMeasured?.Invoke(kind == KeyboardPending, delayMs);
        received.Set();
    }

    /// <summary>The virtual desktop in physical pixels, read once per <see cref="Start"/>.</summary>
    private readonly record struct VirtualDesktop(int Left, int Top, int Width, int Height)
    {
        public static VirtualDesktop Read() => new(
            NativeMethods.GetSystemMetrics(NativeMethods.SM_XVIRTUALSCREEN),
            NativeMethods.GetSystemMetrics(NativeMethods.SM_YVIRTUALSCREEN),
            Math.Max(2, NativeMethods.GetSystemMetrics(NativeMethods.SM_CXVIRTUALSCREEN)),
            Math.Max(2, NativeMethods.GetSystemMetrics(NativeMethods.SM_CYVIRTUALSCREEN)));

        /// <summary>An absolute probe mouse move to a physical desktop point.</summary>
        public SharedNative.INPUT MoveTo(int x, int y) => new()
        {
            type = NativeMethods.INPUT_MOUSE,
            u = new SharedNative.InputUnion
            {
                mi = new SharedNative.MOUSEINPUT
                {
                    dx = (int)Math.Round((x - Left) * (double)NormalizedMaximum / (Width - 1)),
                    dy = (int)Math.Round((y - Top) * (double)NormalizedMaximum / (Height - 1)),
                    dwFlags = NativeMethods.MOUSEEVENTF_MOVE | NativeMethods.MOUSEEVENTF_ABSOLUTE
                        | NativeMethods.MOUSEEVENTF_VIRTUALDESK,
                    dwExtraInfo = (IntPtr)MouseTag,
                },
            },
        };
    }
}
