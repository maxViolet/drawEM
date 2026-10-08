using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using DrawEM.App.Domain.Drawing;
using Microsoft.Win32;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;

namespace DrawEM.App.Presentation.Effects;

/// <summary>Probe access to cursor, monitors, drawing, and the hook measurement load. See <see cref="EffectSurfaceProbe"/>.</summary>
/// <param name="CursorPosition">The current cursor position, or <c>null</c> when it is unavailable.</param>
/// <param name="MonitorAt">The monitor containing a point, or <c>null</c> outside every monitor.</param>
/// <param name="BeginDrawing">Opens draw mode with a stroke starting at the point, as the draw shortcut does.</param>
/// <param name="EndDrawing">Closes draw mode and completes the stroke.</param>
/// <param name="IsDrawing">Whether the drawing input gate is open.</param>
/// <param name="StartInputLoad">Starts injecting measured keyboard and mouse events around the point.</param>
/// <param name="StopInputLoad">Stops the injection; returns the number of events no hook received in time.</param>
/// <param name="ReportDirectory">Where benchmark reports are written.</param>
public sealed record EffectProbePorts(
    Func<ScreenPoint?> CursorPosition,
    Func<ScreenPoint, MonitorBounds?> MonitorAt,
    Action<ScreenPoint, MonitorBounds> BeginDrawing,
    Action EndDrawing,
    Func<bool> IsDrawing,
    Action<ScreenPoint, MonitorBounds> StartInputLoad,
    Func<int> StopInputLoad,
    string ReportDirectory);

/// <summary>
/// Temporary S4-01 experiment (docs/v4/step-01-effect-surface): shows an animated shape in an
/// <see cref="EffectSurfaceWindow"/> on the monitor containing the cursor, above the drawing overlay, and
/// measures the WPF renderer. Enabled only with <c>DRAWEM_EFFECT_PROBE=1</c>; S4-06 removes it with its trigger.
/// <list type="bullet">
/// <item><see cref="Invoke"/> shows one effect, replacing a running one.</item>
/// <item><see cref="RunBenchmark"/> runs warm-up and 100 measured invocations with drawing active and
/// injected input, then writes a report with the automated gate results.</item>
/// </list>
/// All members run on the UI thread.
/// </summary>
public sealed class EffectSurfaceProbe : IDisposable
{
    public const string EnableVariable = "DRAWEM_EFFECT_PROBE";

    /// <summary>Invocations before the resource baseline; the process's first one is recorded as cold.</summary>
    public const int WarmUpInvocations = 5;

    public const int MeasuredInvocations = 100;

    // Automated thresholds from the S4-01 acceptance criteria.
    private const double RenderCallbackP95LimitMs = 4;
    private const double HookDelayP95LimitMs = 25;
    private const double HookDelayMaxLimitMs = 100;
    private const int MinimumHookEvents = 1000;
    private const double ResourceGrowthLimit = 0.05;
    private const double FirstAppearanceTargetMs = 100;

    /// <summary>A frame interval this many times the median counts as a long frame in the pacing record.</summary>
    private const double LongFrameFactor = 1.5;

    private const double ShapeDiameter = 160;
    private const double OutlineThickness = 4;

    private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(2);

    /// <summary>Time to release the benchmark shortcut: a key event closes the draw mode the benchmark opens.</summary>
    private static readonly TimeSpan KeyReleaseDelay = TimeSpan.FromSeconds(3);

    private static readonly TimeSpan PauseBetweenInvocations = TimeSpan.FromMilliseconds(250);
    private static readonly TimeSpan IdleSettleDelay = TimeSpan.FromSeconds(1);

    private static readonly Brush ShapeFill = Freeze(new SolidColorBrush(Color.FromArgb(0xC0, 0x1E, 0x90, 0xFF)));
    private static readonly Pen ShapeOutline = Freeze(new Pen(Brushes.White, OutlineThickness));

    private readonly EffectProbePorts ports;
    private ProbeInvocation? current;
    private BenchmarkRecord? benchmark;
    private bool coldRecorded;
    private bool benchmarkRunning;
    private bool disposed;

    public EffectSurfaceProbe(EffectProbePorts ports)
    {
        this.ports = ports;
    }

    public static string DefaultReportDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "drawEM", "effect-probe");

    /// <summary>Effect windows that are created and not yet closed.</summary>
    public int OpenWindows { get; private set; }

    /// <summary>Live <see cref="CompositionTarget.Rendering"/> subscriptions.</summary>
    public int RenderingSubscriptions { get; private set; }

    public static bool IsEnabled(string? value) => value == "1";

    /// <summary>Shows one effect on the monitor containing the cursor. Ignored while the benchmark runs.</summary>
    public async void Invoke()
    {
        if (disposed || benchmarkRunning)
        {
            return;
        }

        var trigger = Stopwatch.GetTimestamp();
        if (TargetAtCursor() is not { } target)
        {
            AppendError("No monitor contains the cursor; the effect was not shown.");
            return;
        }

        var result = await Start(target.Monitor, trigger, invocationIndex: 0);
        if (result.Failure is not null)
        {
            AppendError($"Effect on monitor {result.Monitor} failed: {result.Failure}");
        }
    }

    /// <summary>Runs the 100-invocation benchmark and writes its report. Ignored while one is running.</summary>
    public async void RunBenchmark()
    {
        if (disposed || benchmarkRunning)
        {
            return;
        }

        benchmarkRunning = true;
        current?.Finish();
        var record = new BenchmarkRecord(DateTimeOffset.Now);
        try
        {
            await Task.Delay(KeyReleaseDelay);
            for (var i = 1; i <= WarmUpInvocations && !disposed; i++)
            {
                if (TargetAtCursor() is { } target)
                {
                    record.Add(await Start(target.Monitor, Stopwatch.GetTimestamp(), -i));
                }

                await Task.Delay(PauseBetweenInvocations);
            }

            await Task.Delay(IdleSettleDelay);
            record.WarmUpResources = ResourceCounts.Read();

            for (var i = 1; i <= MeasuredInvocations && !disposed; i++)
            {
                if (TargetAtCursor() is not { } target)
                {
                    record.SkippedNoMonitor++;
                    continue;
                }

                var trigger = Stopwatch.GetTimestamp();
                benchmark = record;
                record.CurrentInvocation = i;
                ports.BeginDrawing(target.Cursor, target.Monitor);
                ports.StartInputLoad(target.Cursor, target.Monitor);
                try
                {
                    record.Add(await Start(target.Monitor, trigger, i));
                }
                finally
                {
                    benchmark = null;
                    record.InputTimeouts += ports.StopInputLoad();
                    ports.EndDrawing();
                }

                await Task.Delay(PauseBetweenInvocations);
            }

            await Task.Delay(IdleSettleDelay);
            record.FinalResources = ResourceCounts.Read();
            record.FinalOpenWindows = OpenWindows;
            record.FinalRenderingSubscriptions = RenderingSubscriptions;
        }
        catch (Exception exception)
        {
            record.Failure = exception;
        }
        finally
        {
            benchmarkRunning = false;
        }

        WriteReport(record);
    }

    /// <summary>Records one hook event-to-callback delay while a measured invocation runs.</summary>
    public void RecordHookDelay(bool keyboard, double delayMs) =>
        benchmark?.AddHookDelay(keyboard, delayMs, ports.IsDrawing());

    public void Dispose()
    {
        disposed = true;
        current?.Finish();
    }

    private (MonitorBounds Monitor, ScreenPoint Cursor)? TargetAtCursor() =>
        ports.CursorPosition() is { } cursor && ports.MonitorAt(cursor) is { } monitor ? (monitor, cursor) : null;

    private Task<InvocationResult> Start(MonitorBounds monitor, long trigger, int invocationIndex)
    {
        current?.Finish();
        var invocation = new ProbeInvocation(this, monitor, trigger, invocationIndex, cold: !coldRecorded);
        coldRecorded = true;
        current = invocation;
        invocation.Start();
        return invocation.Completion;
    }

    private void WriteReport(BenchmarkRecord record)
    {
        try
        {
            Directory.CreateDirectory(ports.ReportDirectory);
            var stamp = record.Started.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            var samplesPath = Path.Combine(ports.ReportDirectory, $"effect-probe-{stamp}.csv");
            var reportPath = Path.Combine(ports.ReportDirectory, $"effect-probe-{stamp}.md");
            File.WriteAllText(samplesPath, record.SamplesCsv());
            File.WriteAllText(reportPath, record.Report(samplesPath));
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Trace.TraceError($"drawEM effect probe report could not be written: {exception}");
        }
    }

    /// <summary>Records a single-invocation failure, which has no report of its own.</summary>
    private void AppendError(string message)
    {
        try
        {
            Directory.CreateDirectory(ports.ReportDirectory);
            File.AppendAllText(
                Path.Combine(ports.ReportDirectory, "effect-probe-errors.log"),
                $"{DateTimeOffset.Now:O} {message}{Environment.NewLine}");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Trace.TraceError($"drawEM effect probe error could not be written: {message}");
        }
    }

    private static T Freeze<T>(T freezable)
        where T : Freezable
    {
        freezable.Freeze();
        return freezable;
    }

    private static double ToMilliseconds(long ticks) => ticks * 1000d / Stopwatch.Frequency;

    private static void DrawShape(DrawingContext context, EffectSurfaceLayout layout, double progress)
    {
        // Crosses the monitor left to right and reaches its top and bottom edges, so clipping is visible.
        var size = layout.LocalSize;
        var center = new Point(
            size.Width * progress,
            (size.Height / 2) - ((size.Height / 2) * Math.Sin(2 * Math.PI * progress)));
        context.DrawEllipse(ShapeFill, ShapeOutline, center, ShapeDiameter / 2, ShapeDiameter / 2);
    }

    /// <summary>One shown effect: its window, its rendering subscription, and its measurements.</summary>
    private sealed class ProbeInvocation
    {
        private readonly EffectSurfaceProbe owner;
        private readonly MonitorBounds monitor;
        private readonly long trigger;
        private readonly TaskCompletionSource<InvocationResult> completion = new();
        private readonly InvocationResult result;
        private EffectSurfaceWindow? window;
        private IntPtr foregroundBefore;
        private TimeSpan lastRenderingTime = TimeSpan.MinValue;
        private long lastFrame;
        private bool subscribed;
        private bool finished;

        public ProbeInvocation(EffectSurfaceProbe owner, MonitorBounds monitor, long trigger, int index, bool cold)
        {
            this.owner = owner;
            this.monitor = monitor;
            this.trigger = trigger;
            result = new InvocationResult(index, cold, monitor);
        }

        public Task<InvocationResult> Completion => completion.Task;

        public void Start()
        {
            try
            {
                foregroundBefore = NativeMethods.GetForegroundWindow();
                window = new EffectSurfaceWindow(monitor);
                owner.OpenWindows++;
                window.Closed += (_, _) => owner.OpenWindows--;
                window.ShowOnMonitor();
                result.DpiScale = window.Layout.DpiScale;
                result.PlacementMatches = window.PlacementMatchesMonitor;
                result.ForegroundUnchanged = NativeMethods.GetForegroundWindow() == foregroundBefore;
                CompositionTarget.Rendering += OnRendering;
                subscribed = true;
                owner.RenderingSubscriptions++;
            }
            catch (Exception exception)
            {
                result.Failure = exception;
                Finish();
            }
        }

        public void Finish()
        {
            if (finished)
            {
                return;
            }

            finished = true;
            if (owner.current == this)
            {
                owner.current = null;
            }

            if (window is not null)
            {
                result.ForegroundUnchanged &= NativeMethods.GetForegroundWindow() == foregroundBefore;
                window.Close();
                window = null;
            }

            if (subscribed)
            {
                CompositionTarget.Rendering -= OnRendering;
                subscribed = false;
                owner.RenderingSubscriptions--;
            }

            completion.TrySetResult(result);
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            // Rendering can be raised more than once for one frame; only the first call draws.
            var renderingTime = ((RenderingEventArgs)e).RenderingTime;
            if (finished || renderingTime == lastRenderingTime)
            {
                return;
            }

            lastRenderingTime = renderingTime;
            var callbackStart = Stopwatch.GetTimestamp();
            var progress = ToMilliseconds(callbackStart - trigger) / EffectDuration.TotalMilliseconds;
            if (progress >= 1)
            {
                Finish();
                return;
            }

            try
            {
                window!.Draw((context, layout) => DrawShape(context, layout, progress));
            }
            catch (Exception exception)
            {
                result.Failure = exception;
                Finish();
                return;
            }

            var callbackEnd = Stopwatch.GetTimestamp();
            result.CallbackMs.Add(ToMilliseconds(callbackEnd - callbackStart));
            if (lastFrame != 0)
            {
                result.FrameIntervalMs.Add(ToMilliseconds(callbackStart - lastFrame));
            }
            else
            {
                result.FirstAppearanceMs = ToMilliseconds(callbackEnd - trigger);
            }

            lastFrame = callbackStart;
            result.FrameCount++;
        }
    }

    private sealed class InvocationResult(int index, bool cold, MonitorBounds monitor)
    {
        /// <summary>Negative for warm-up invocations, 0 for a single invocation, 1-100 when measured.</summary>
        public int Index { get; } = index;

        public bool Cold { get; } = cold;

        public MonitorBounds Monitor { get; } = monitor;

        public double DpiScale { get; set; }

        public bool PlacementMatches { get; set; }

        public bool ForegroundUnchanged { get; set; }

        /// <summary>Trigger to the end of the first rendering callback that drew the shape.</summary>
        public double? FirstAppearanceMs { get; set; }

        public int FrameCount { get; set; }

        public List<double> CallbackMs { get; } = [];

        public List<double> FrameIntervalMs { get; } = [];

        public Exception? Failure { get; set; }
    }

    private readonly record struct ResourceCounts(int Handles, uint GdiObjects, uint UserObjects)
    {
        public static ResourceCounts Read()
        {
            using var process = Process.GetCurrentProcess();
            var self = NativeMethods.GetCurrentProcess();
            return new ResourceCounts(
                process.HandleCount,
                NativeMethods.GetGuiResources(self, NativeMethods.GR_GDIOBJECTS),
                NativeMethods.GetGuiResources(self, NativeMethods.GR_USEROBJECTS));
        }
    }

    private readonly record struct HookDelay(int Invocation, bool Keyboard, double DelayMs, bool Drawing);

    private sealed class BenchmarkRecord(DateTimeOffset started)
    {
        private readonly List<InvocationResult> invocations = [];
        private readonly List<HookDelay> hookDelays = [];

        public DateTimeOffset Started { get; } = started;

        public int CurrentInvocation { get; set; }

        public int SkippedNoMonitor { get; set; }

        public int InputTimeouts { get; set; }

        public ResourceCounts? WarmUpResources { get; set; }

        public ResourceCounts? FinalResources { get; set; }

        public int FinalOpenWindows { get; set; } = -1;

        public int FinalRenderingSubscriptions { get; set; } = -1;

        public Exception? Failure { get; set; }

        private IEnumerable<InvocationResult> Measured => invocations.Where(invocation => invocation.Index > 0);

        public void Add(InvocationResult invocation) => invocations.Add(invocation);

        public void AddHookDelay(bool keyboard, double delayMs, bool drawing) =>
            hookDelays.Add(new HookDelay(CurrentInvocation, keyboard, delayMs, drawing));

        public string SamplesCsv()
        {
            var csv = new StringBuilder("kind,invocation,cold,value_ms,drawing\n");
            foreach (var invocation in invocations)
            {
                var cold = invocation.Cold ? 1 : 0;
                if (invocation.FirstAppearanceMs is { } appearance)
                {
                    csv.Append(Invariant($"first-appearance,{invocation.Index},{cold},{appearance:F3},\n"));
                }

                foreach (var value in invocation.CallbackMs)
                {
                    csv.Append(Invariant($"render-callback,{invocation.Index},{cold},{value:F3},\n"));
                }

                foreach (var value in invocation.FrameIntervalMs)
                {
                    csv.Append(Invariant($"frame-interval,{invocation.Index},{cold},{value:F3},\n"));
                }
            }

            foreach (var delay in hookDelays)
            {
                var kind = delay.Keyboard ? "hook-keyboard" : "hook-mouse";
                csv.Append(Invariant($"{kind},{delay.Invocation},0,{delay.DelayMs:F3},{(delay.Drawing ? 1 : 0)}\n"));
            }

            return csv.ToString();
        }

        public string Report(string samplesPath)
        {
            var measured = Measured.ToList();
            var callbacks = measured.SelectMany(invocation => invocation.CallbackMs).ToList();
            var intervals = measured.SelectMany(invocation => invocation.FrameIntervalMs).ToList();
            var appearances = measured.Where(invocation => invocation.FirstAppearanceMs is not null)
                .Select(invocation => invocation.FirstAppearanceMs!.Value).ToList();
            var delays = hookDelays.Select(delay => delay.DelayMs).ToList();
            var keyboardCount = hookDelays.Count(delay => delay.Keyboard);
            var mouseCount = hookDelays.Count - keyboardCount;
            var drawingCount = hookDelays.Count(delay => delay.Drawing);
            var medianInterval = Percentile(intervals, 0.5);
            var longFrames = medianInterval is { } median ? intervals.Count(value => value > median * LongFrameFactor) : 0;
            var cold = invocations.FirstOrDefault(invocation => invocation.Cold);
            var failures = invocations.Where(invocation => invocation.Failure is not null).ToList();

            var report = new StringBuilder();
            report.AppendLine("# drawEM S4-01 effect probe report");
            report.AppendLine();
            report.AppendLine(Invariant($"Started: {Started:yyyy-MM-dd HH:mm:ss zzz}. Raw samples: `{samplesPath}`."));
            report.AppendLine();
            report.AppendLine("Automated results only. Visible rendering, click-through, focus, task switching, clipping on");
            report.AppendLine("neighbor monitors, stutter in a recording, and missed strokes remain manual checks.");
            report.AppendLine();

            report.AppendLine("## Environment");
            report.AppendLine();
            foreach (var line in EnvironmentLines())
            {
                report.AppendLine($"- {line}");
            }

            report.AppendLine("- Monitors used (physical bounds @ DPI scale): " + string.Join("; ", invocations
                .Select(invocation => Invariant($"({invocation.Monitor.Left}, {invocation.Monitor.Top}, {invocation.Monitor.Right}, {invocation.Monitor.Bottom}) @ {invocation.DpiScale:0.##}"))
                .Distinct()));
            report.AppendLine("- Timing: `Stopwatch` (QueryPerformanceCounter, " +
                Invariant($"{1e9 / Stopwatch.Frequency:0.#} ns resolution)."));
            report.AppendLine("  - Render callback: duration of the probe's `CompositionTarget.Rendering` handler; WPF's own");
            report.AppendLine("    composition and presentation time is not included.");
            report.AppendLine("  - First appearance: trigger to the end of the first drawing callback. The frame is presented");
            report.AppendLine("    later, so add up to one display refresh interval (16.7 ms at 60 Hz) of error.");
            report.AppendLine("  - Hook delay: `SendInput` from a background thread to the probe's low-level hook callback on the");
            report.AppendLine("    UI thread, which runs before the production hooks on the same thread. Error below 0.1 ms.");
            report.AppendLine();

            report.AppendLine("## Automated gates");
            report.AppendLine();
            report.AppendLine("| Check | Measured | Limit | Result |");
            report.AppendLine("|---|---|---|---|");
            var callbackP95 = Percentile(callbacks, 0.95);
            Gate(report, "Measured invocations completed", Invariant($"{measured.Count}"), Invariant($"{MeasuredInvocations}"),
                measured.Count == MeasuredInvocations && SkippedNoMonitor == 0);
            Gate(report, "Render callback p95", Ms(callbackP95), Invariant($"<= {RenderCallbackP95LimitMs} ms"),
                callbackP95 <= RenderCallbackP95LimitMs);
            var delayP95 = Percentile(delays, 0.95);
            var delayMax = delays.Count > 0 ? delays.Max() : (double?)null;
            Gate(report, "Hook delay p95", Ms(delayP95), Invariant($"<= {HookDelayP95LimitMs} ms"), delayP95 <= HookDelayP95LimitMs);
            Gate(report, "Hook delay max", Ms(delayMax), Invariant($"<= {HookDelayMaxLimitMs} ms"),
                delayMax <= HookDelayMaxLimitMs && InputTimeouts == 0);
            Gate(report, "Hook events (keyboard / mouse / while drawing)",
                Invariant($"{hookDelays.Count} ({keyboardCount} / {mouseCount} / {drawingCount})"),
                Invariant($">= {MinimumHookEvents}, both types"),
                hookDelays.Count >= MinimumHookEvents && keyboardCount > 0 && mouseCount > 0);
            Gate(report, "Injected events never received", Invariant($"{InputTimeouts}"), "0", InputTimeouts == 0);
            Gate(report, "Effect windows left", Invariant($"{FinalOpenWindows}"), "0", FinalOpenWindows == 0);
            Gate(report, "Rendering subscriptions left", Invariant($"{FinalRenderingSubscriptions}"), "0",
                FinalRenderingSubscriptions == 0);
            ResourceGate(report, "Process handles", counts => counts.Handles);
            ResourceGate(report, "GDI objects", counts => counts.GdiObjects);
            ResourceGate(report, "USER objects", counts => counts.UserObjects);
            var foregroundChanged = invocations.Count(invocation => !invocation.ForegroundUnchanged);
            Gate(report, "Invocations that changed the foreground window", Invariant($"{foregroundChanged}"), "0",
                foregroundChanged == 0);
            var misplaced = invocations.Count(invocation => !invocation.PlacementMatches);
            Gate(report, "Windows not covering exactly the target monitor", Invariant($"{misplaced}"), "0", misplaced == 0);
            Gate(report, "Invocation failures", Invariant($"{failures.Count}"), "0", failures.Count == 0 && Failure is null);
            report.AppendLine();

            report.AppendLine("## Records");
            report.AppendLine();
            report.AppendLine(Invariant($"- First appearance, {appearances.Count} warm invocations (estimate against the {FirstAppearanceTargetMs} ms release target, not a gate): ") +
                Invariant($"median {Ms(Percentile(appearances, 0.5))}, p95 {Ms(Percentile(appearances, 0.95))}, max {Ms(appearances.Count > 0 ? appearances.Max() : null)}."));
            report.AppendLine(cold is null
                ? "- Cold start: the process had shown an effect before the benchmark; restart drawEM to record one."
                : Invariant($"- Cold start (invocation {cold.Index}): first appearance {Ms(cold.FirstAppearanceMs)}, ") +
                  Invariant($"render callback max {Ms(cold.CallbackMs.Count > 0 ? cold.CallbackMs.Max() : null)}."));
            report.AppendLine(Invariant($"- Render callbacks: {callbacks.Count} samples; median {Ms(Percentile(callbacks, 0.5))}, ") +
                Invariant($"p99 {Ms(Percentile(callbacks, 0.99))}, max {Ms(callbacks.Count > 0 ? callbacks.Max() : null)}."));
            report.AppendLine(Invariant($"- Frame pacing: {intervals.Count} intervals; median {Ms(medianInterval)}, p95 {Ms(Percentile(intervals, 0.95))}, ") +
                Invariant($"max {Ms(intervals.Count > 0 ? intervals.Max() : null)}; {longFrames} intervals over {LongFrameFactor}x the median."));
            report.AppendLine(Invariant($"- Hook delay: median {Ms(Percentile(delays, 0.5))}, p99 {Ms(Percentile(delays, 0.99))}."));
            report.AppendLine(Invariant($"- Resources after warm-up: {Describe(WarmUpResources)}; after the cycles: {Describe(FinalResources)}."));
            if (Failure is not null)
            {
                report.AppendLine($"- Benchmark failure: {Failure}");
            }

            foreach (var failed in failures)
            {
                report.AppendLine($"- Invocation {failed.Index} failure: {failed.Failure}");
            }

            return report.ToString();
        }

        private void ResourceGate(StringBuilder report, string name, Func<ResourceCounts, double> count)
        {
            if (WarmUpResources is not { } before || FinalResources is not { } after)
            {
                Gate(report, name, "not measured", "within 5% of warm-up", false);
                return;
            }

            var start = count(before);
            var end = count(after);
            Gate(report, name, Invariant($"{start} -> {end}"), "within 5% of warm-up",
                Math.Abs(end - start) <= start * ResourceGrowthLimit);
        }

        private static void Gate(StringBuilder report, string check, string measured, string limit, bool passed) =>
            report.AppendLine($"| {check} | {measured} | {limit} | {(passed ? "pass" : "FAIL")} |");

        private static string Describe(ResourceCounts? counts) => counts is { } value
            ? Invariant($"{value.Handles} handles, {value.GdiObjects} GDI, {value.UserObjects} USER")
            : "not measured";

        private static string Ms(double? value) => value is { } ms ? Invariant($"{ms:0.###} ms") : "n/a";

        /// <summary>Nearest-rank percentile, or <c>null</c> without samples.</summary>
        private static double? Percentile(List<double> values, double fraction)
        {
            if (values.Count == 0)
            {
                return null;
            }

            var sorted = values.Order().ToList();
            var rank = (int)Math.Ceiling(fraction * sorted.Count);
            return sorted[Math.Clamp(rank - 1, 0, sorted.Count - 1)];
        }

        private static IEnumerable<string> EnvironmentLines()
        {
            var path = Environment.ProcessPath;
            yield return $"Executable: `{path}`";
            var assembly = Assembly.GetEntryAssembly();
            yield return "Version: " + (assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? assembly?.GetName().Version?.ToString() ?? "unknown");
            yield return "Executable SHA-256: " + (path is null ? "unknown" : HashFile(path));
            yield return $"{EnableVariable}: {Environment.GetEnvironmentVariable(EnableVariable)}";
            yield return "Windows: " + ReadWindowsVersion();
            yield return "CPU: " + (ReadRegistry(
                @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString")?.Trim() ?? "unknown");
            yield return "GPU: " + ReadGpuNames();
            yield return Invariant($"Logical processors: {Environment.ProcessorCount}");
        }

        private static string HashFile(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                return Convert.ToHexString(SHA256.HashData(stream));
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                return $"unreadable ({exception.Message})";
            }
        }

        private static string ReadWindowsVersion()
        {
            const string key = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            return $"{ReadRegistry(key, "ProductName")} {ReadRegistry(key, "DisplayVersion")}, " +
                $"build {ReadRegistry(key, "CurrentBuild")}.{ReadRegistry(key, "UBR")} ({Environment.OSVersion.Version})";
        }

        private static string ReadGpuNames()
        {
            // Display adapter device class; one numbered subkey per adapter driver.
            const string displayClass = @"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}";
            var names = Enumerable.Range(0, 8)
                .Select(index => ReadRegistry(Invariant($@"{displayClass}\{index:0000}"), "DriverDesc"))
                .OfType<string>()
                .Distinct()
                .ToList();
            return names.Count == 0 ? "unknown" : string.Join("; ", names);
        }

        private static string? ReadRegistry(string key, string name)
        {
            try
            {
                using var subKey = Registry.LocalMachine.OpenSubKey(key);
                return subKey?.GetValue(name)?.ToString();
            }
            catch (Exception exception) when (exception is System.Security.SecurityException or UnauthorizedAccessException)
            {
                return null;
            }
        }

        private static string Invariant(FormattableString value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
