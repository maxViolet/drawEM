using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using DrawEM.App.Application;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;
using DrawEM.App.Presentation;

namespace RenderLagHarness;

/// <summary>
/// Synthetic harness for F03: reproduces adapter -> Dispatcher -> controller -> renderer
/// chain with a fake mouse source and measures dispatch queue age and per-event redraw
/// cost under controlled event rates. See docs/steps/F03-render-lag-investigation-plan.md
/// step 3. Not a replacement for a real Win32/screen measurement.
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        var rates = new[] { 125, 500, 1000 };
        var scenarios = new (string Name, Func<ScenarioSeed> Seed)[]
        {
            ("empty-canvas", () => new ScenarioSeed(0, 0)),
            ("long-completed-line", () => new ScenarioSeed(1, 1000)),
            ("many-completed-strokes", () => new ScenarioSeed(20, 50)),
        };

        var results = new List<RunResult>();
        RunResult? worst = null;
        List<Sample>? worstSamples = null;

        // Cold start: first run of the process, before any JIT warmup on this code path.
        var coldSeed = scenarios[0].Seed();
        var (cold, coldSamples) = RunScenario("empty-canvas [cold]", coldSeed, rate: 500, eventCount: 500);
        results.Add(cold);
        Console.WriteLine(cold.Summarize());
        worst = cold;
        worstSamples = coldSamples;

        foreach (var (name, seed) in scenarios)
        {
            foreach (var rate in rates)
            {
                var eventCount = rate; // ~1 nominal second of events per run, budget-capped below
                var (result, sampleList) = RunScenario(name, seed(), rate, eventCount, warmup: true);
                results.Add(result);
                Console.WriteLine(result.Summarize());

                if (result.QueueAgeMax > worst.QueueAgeMax)
                {
                    worst = result;
                    worstSamples = sampleList;
                }
            }
        }

        WriteMarkdownReport(results, worst);
        if (worstSamples is not null)
        {
            WriteTraceCsv(worst, worstSamples);
        }

        Console.WriteLine();
        Console.WriteLine("Report: docs/steps/F03-render-lag-harness-results.md");
        Console.WriteLine("Worst-case trace: docs/steps/F03-render-lag-harness-trace.csv");
    }

    private static (RunResult Result, List<Sample> Samples) RunScenario(
        string name,
        ScenarioSeed seed,
        int rate,
        int eventCount,
        bool warmup = false)
    {
        (RunResult Result, List<Sample> Samples)? outcome = null;
        var thread = new Thread(() =>
        {
            outcome = RunOnDispatcherThread(name, seed, rate, eventCount, warmup);
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        return outcome!.Value;
    }

    private static (RunResult Result, List<Sample> Samples) RunOnDispatcherThread(
        string name,
        ScenarioSeed seed,
        int rate,
        int eventCount,
        bool warmup)
    {
        var dispatcher = Dispatcher.CurrentDispatcher;
        var controller = new DrawingSessionController();
        var renderElement = new StrokeRenderElement();
        var inputGate = new DrawingModeInputGate();
        var mouseSource = new FakeMouseHookSource();
        var transform = new PhysicalToLocalTransform(0, 0, Matrix.Identity);

        controller.StateChanged += state => renderElement.UpdateState(state, transform);

        SeedCompletedStrokes(controller, seed);

        var startPoint = new ScreenPoint(0, 0);
        inputGate.SetActive(true);
        controller.EnterDrawMode(startPoint);

        var pendingNormalOps = 0;
        var maxPendingNormalOps = 0;
        dispatcher.Hooks.OperationPosted += (_, e) =>
        {
            if (e.Operation.Priority == DispatcherPriority.Normal)
            {
                var value = Interlocked.Increment(ref pendingNormalOps);
                if (value > maxPendingNormalOps)
                {
                    maxPendingNormalOps = value;
                }
            }
        };
        dispatcher.Hooks.OperationCompleted += (_, e) =>
        {
            if (e.Operation.Priority == DispatcherPriority.Normal)
            {
                Interlocked.Decrement(ref pendingNormalOps);
            }
        };

        var samples = new List<Sample>(eventCount);
        var sequence = 0;

        Action<Action> instrumentedDispatch = action =>
        {
            var postedAt = Stopwatch.GetTimestamp();
            var index = sequence++;
            dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                var startedAt = Stopwatch.GetTimestamp();
                action();
                var finishedAt = Stopwatch.GetTimestamp();
                samples.Add(new Sample(index, postedAt, startedAt, finishedAt));
            }));
        };

        _ = new GlobalMouseInputAdapter(mouseSource, controller, inputGate, instrumentedDispatch);

        var intervalTicks = Stopwatch.Frequency / rate;
        var runStart = Stopwatch.GetTimestamp();

        for (var i = 0; i < eventCount; i++)
        {
            var targetTicks = runStart + (i * intervalTicks);
            while (Stopwatch.GetTimestamp() < targetTicks)
            {
                // Busy-pace to the target tick; DispatcherTimer granularity is too coarse
                // at 500-1000 events/sec (see plan step 3: fixed injection rates).
            }

            var point = new ScreenPoint(i % 4000, (i / 4000) % 4000);
            dispatcher.Invoke(() => mouseSource.RaiseMove(point), DispatcherPriority.Send);
        }

        // Let remaining queued Normal-priority work finish; a single Background-priority
        // Invoke waits for everything queued ahead of it to drain first. Bounded so a
        // pathological backlog (see hypothesis 1/2) cannot hang the harness itself.
        using (var drainTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(8)))
        {
            try
            {
                dispatcher.Invoke(() => { }, DispatcherPriority.Background, drainTimeout.Token);
            }
            catch (OperationCanceledException)
            {
                // Backlog did not drain within budget; samples collected so far still stand.
            }
        }

        var runEnd = Stopwatch.GetTimestamp();

        var result = BuildResult(name, rate, eventCount, samples, maxPendingNormalOps, runStart, runEnd, warmup);
        return (result, samples);
    }

    private static void SeedCompletedStrokes(DrawingSessionController controller, ScenarioSeed seed)
    {
        for (var s = 0; s < seed.StrokeCount; s++)
        {
            controller.Start(new ScreenPoint(0, s));
            for (var p = 1; p < seed.PointsPerStroke; p++)
            {
                controller.Move(new ScreenPoint(p % 4000, s));
            }

            controller.End();
        }
    }

    private static RunResult BuildResult(
        string name,
        int rate,
        int eventCount,
        List<Sample> samples,
        int maxPendingNormalOps,
        long runStart,
        long runEnd,
        bool warmup)
    {
        var tickToMs = 1000.0 / Stopwatch.Frequency;
        var queueAges = samples.Select(s => (s.StartedAt - s.PostedAt) * tickToMs).OrderBy(v => v).ToArray();
        var processing = samples.Select(s => (s.FinishedAt - s.StartedAt) * tickToMs).OrderBy(v => v).ToArray();

        double firstTenthAvg = 0;
        double lastTenthAvg = 0;
        if (processing.Length >= 10)
        {
            var bySequence = samples.OrderBy(s => s.Index).Select(s => (s.FinishedAt - s.StartedAt) * tickToMs).ToArray();
            var tenth = Math.Max(1, bySequence.Length / 10);
            firstTenthAvg = bySequence.Take(tenth).Average();
            lastTenthAvg = bySequence.TakeLast(tenth).Average();
        }

        return new RunResult(
            name,
            rate,
            eventCount,
            samples.Count,
            (runEnd - runStart) * tickToMs,
            Percentile(queueAges, 0.50),
            Percentile(queueAges, 0.95),
            Percentile(queueAges, 0.99),
            queueAges.Length == 0 ? 0 : queueAges[^1],
            Percentile(processing, 0.50),
            Percentile(processing, 0.95),
            Percentile(processing, 0.99),
            processing.Length == 0 ? 0 : processing[^1],
            maxPendingNormalOps,
            firstTenthAvg,
            lastTenthAvg,
            warmup);
    }

    private static double Percentile(double[] sorted, double p)
    {
        if (sorted.Length == 0)
        {
            return 0;
        }

        var index = (int)Math.Ceiling(p * sorted.Length) - 1;
        return sorted[Math.Clamp(index, 0, sorted.Length - 1)];
    }

    private static void WriteMarkdownReport(List<RunResult> results, RunResult? worst)
    {
        var directory = FindDocsStepsDirectory();
        var path = Path.Combine(directory, "F03-render-lag-harness-results.md");

        var lines = new List<string>
        {
            "# F03: результаты синтетического harness",
            "",
            $"Сгенерировано: {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Инструмент: tools/RenderLagHarness.",
            "Запуск: `dotnet run --project tools/RenderLagHarness -c Release`.",
            "",
            "Ограничение: harness воспроизводит цепочку adapter -> Dispatcher -> controller -> " +
            "renderer синтетически, без реального Win32-хука и композиции окна. Он не заменяет " +
            "измерение на реальной сборке (план, шаг 1/3).",
            "",
            "| Сценарий | Rate (ev/s) | События | Длительность (мс) | Queue age p50/p95/p99/max (мс) | Обработка p50/p95/p99/max (мс) | Max pending ops | Рост обработки (первые 10% -> последние 10%, мс) |",
            "| --- | --- | --- | --- | --- | --- | --- | --- |",
        };

        foreach (var r in results)
        {
            lines.Add(
                $"| {r.Scenario}{(r.Warmup ? "" : " [cold]")} | {r.Rate} | {r.Completed}/{r.Requested} | " +
                $"{r.DurationMs:F1} | {r.QueueAgeP50:F2}/{r.QueueAgeP95:F2}/{r.QueueAgeP99:F2}/{r.QueueAgeMax:F2} | " +
                $"{r.ProcessingP50:F2}/{r.ProcessingP95:F2}/{r.ProcessingP99:F2}/{r.ProcessingMax:F2} | " +
                $"{r.MaxPendingOps} | {r.FirstTenthAvgMs:F3} -> {r.LastTenthAvgMs:F3} |");
        }

        lines.Add("");
        if (worst is not null)
        {
            lines.Add($"Худший случай по max queue age: `{worst.Scenario}` @ {worst.Rate} ev/s, " +
                      $"max queue age {worst.QueueAgeMax:F2} мс, max pending ops {worst.MaxPendingOps}. " +
                      "Полная трасса: F03-render-lag-harness-trace.csv.");
        }

        File.WriteAllLines(path, lines);
    }

    private static void WriteTraceCsv(RunResult worst, List<Sample> samples)
    {
        var directory = FindDocsStepsDirectory();
        var path = Path.Combine(directory, "F03-render-lag-harness-trace.csv");

        var tickToMs = 1000.0 / Stopwatch.Frequency;
        var lines = new List<string> { "index,queue_age_ms,processing_ms" };
        lines.AddRange(samples
            .OrderBy(s => s.Index)
            .Select(s => $"{s.Index},{(s.StartedAt - s.PostedAt) * tickToMs:F4},{(s.FinishedAt - s.StartedAt) * tickToMs:F4}"));

        File.WriteAllLines(path, lines);
        Console.WriteLine($"Worst-case run: {worst.Scenario} @ {worst.Rate} ev/s ({samples.Count} samples).");
    }

    private static string FindDocsStepsDirectory()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "DrawEM.sln")))
        {
            dir = dir.Parent;
        }

        if (dir is null)
        {
            throw new InvalidOperationException("Could not locate repository root (DrawEM.sln) from harness output directory.");
        }

        var docsSteps = Path.Combine(dir.FullName, "docs", "steps");
        Directory.CreateDirectory(docsSteps);
        return docsSteps;
    }

    private sealed class FakeMouseHookSource : IMouseHookSource
    {
        public event Action<ScreenPoint>? PointerMoved;

        public event Func<bool>? PointerButtonActivity;

        public event Func<bool>? PointerWheelActivity;

        public void RaiseMove(ScreenPoint point) => PointerMoved?.Invoke(point);
    }

    private readonly record struct ScenarioSeed(int StrokeCount, int PointsPerStroke);

    private readonly record struct Sample(int Index, long PostedAt, long StartedAt, long FinishedAt);

    private sealed record RunResult(
        string Scenario,
        int Rate,
        int Requested,
        int Completed,
        double DurationMs,
        double QueueAgeP50,
        double QueueAgeP95,
        double QueueAgeP99,
        double QueueAgeMax,
        double ProcessingP50,
        double ProcessingP95,
        double ProcessingP99,
        double ProcessingMax,
        int MaxPendingOps,
        double FirstTenthAvgMs,
        double LastTenthAvgMs,
        bool Warmup)
    {
        public string Summarize() =>
            string.Format(
                CultureInfo.InvariantCulture,
                "{0,-24} rate={1,5} ev/s completed={2}/{3} queueAge(p50/p95/p99/max)={4:F2}/{5:F2}/{6:F2}/{7:F2}ms " +
                "processing(p50/p95/p99/max)={8:F2}/{9:F2}/{10:F2}/{11:F2}ms maxPending={12} growth(first10%->last10%)={13:F3}->{14:F3}ms",
                Scenario,
                Rate,
                Completed,
                Requested,
                QueueAgeP50,
                QueueAgeP95,
                QueueAgeP99,
                QueueAgeMax,
                ProcessingP50,
                ProcessingP95,
                ProcessingP99,
                ProcessingMax,
                MaxPendingOps,
                FirstTenthAvgMs,
                LastTenthAvgMs);
    }
}
