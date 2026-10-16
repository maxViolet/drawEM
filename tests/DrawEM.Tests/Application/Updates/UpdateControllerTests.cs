using DrawEM.App.Application.Updates;

namespace DrawEM.Tests.Application.Updates;

public class UpdateControllerTests
{
    private static readonly TimeSpan Day = TimeSpan.FromHours(24);

    [Fact]
    public void Start_WhenNotInstalled_MakesNoCheckAndCreatesNoTimer()
    {
        var fixture = new Fixture(installed: false);

        fixture.Controller.Start();
        fixture.Time.Advance(Day * 2);

        Assert.Equal(0, fixture.Service.CheckCount);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
        Assert.Empty(fixture.Log);
    }

    [Fact]
    public void Start_ChecksOnce_ThenExactlyEvery24Hours()
    {
        var fixture = new Fixture();

        fixture.Controller.Start();
        Assert.Equal(1, fixture.Service.CheckCount);
        fixture.Service.Complete(null);

        fixture.Time.Advance(Day - TimeSpan.FromSeconds(1));
        Assert.Equal(1, fixture.Service.CheckCount);

        fixture.Time.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(2, fixture.Service.CheckCount);
        fixture.Service.Complete(null);

        fixture.Time.Advance(Day - TimeSpan.FromSeconds(1));
        Assert.Equal(2, fixture.Service.CheckCount);

        fixture.Time.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(3, fixture.Service.CheckCount);
    }

    [Fact]
    public void Start_Twice_StartsOnlyOneSchedule()
    {
        var fixture = new Fixture();

        fixture.Controller.Start();
        fixture.Controller.Start();

        Assert.Equal(1, fixture.Service.CheckCount);
        Assert.Equal(1, fixture.Time.ActiveTimerCount);
    }

    [Fact]
    public void TimerTick_WhileCheckStillRuns_StartsNoSecondCheck()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();

        fixture.Time.Advance(Day);

        Assert.Equal(1, fixture.Service.CheckCount);

        fixture.Service.Complete(null);
        fixture.Time.Advance(Day);

        Assert.Equal(2, fixture.Service.CheckCount);
    }

    [Fact]
    public void DownloadedVersion_NotifiesOncePerVersion()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();

        fixture.Service.Complete("1.2.3");
        fixture.Time.Advance(Day);
        fixture.Service.Complete("1.2.3");
        fixture.Time.Advance(Day);
        fixture.Service.Complete("1.2.4");

        Assert.Equal(["ready 1.2.3", "ready 1.2.4"], fixture.Log);
    }

    [Fact]
    public void PendingVersionAtStartup_NotifiesWithoutChecking()
    {
        var fixture = new Fixture(pendingVersion: "1.2.3");

        fixture.Controller.Start();

        Assert.Equal(["ready 1.2.3"], fixture.Log);
        Assert.Equal(0, fixture.Service.CheckCount);
    }

    [Fact]
    public void PendingVersionAtStartup_FoundAgainLater_DoesNotNotifyAgain()
    {
        var fixture = new Fixture(pendingVersion: "1.2.3");
        fixture.Controller.Start();

        fixture.Time.Advance(Day);
        fixture.Service.Complete("1.2.3");

        Assert.Equal(1, fixture.Service.CheckCount);
        Assert.Equal(["ready 1.2.3"], fixture.Log);
    }

    [Fact]
    public void RequestUpdate_WithoutReadyVersion_AsksNothing()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();

        fixture.Controller.RequestUpdate();

        Assert.Empty(fixture.Log);
    }

    [Fact]
    public void ConsentNo_AppliesNothing_AndALaterRequestAsksAgain()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();
        fixture.Service.Complete("1.2.3");
        fixture.Consent = false;

        fixture.Controller.RequestUpdate();
        fixture.Controller.RequestUpdate();

        Assert.Equal(["ready 1.2.3", "confirm 1.2.3", "confirm 1.2.3"], fixture.Log);
    }

    [Fact]
    public void ConsentYes_AppliesAfterExit_ThenExits_Once()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();
        fixture.Service.Complete("1.2.3");
        fixture.Consent = true;

        fixture.Controller.RequestUpdate();
        fixture.Controller.RequestUpdate();

        Assert.Equal(["ready 1.2.3", "confirm 1.2.3", "apply", "exit"], fixture.Log);
    }

    [Fact]
    public void RequestWhileConfirmationIsOpen_AsksNothing_AndAppliesOnce()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();
        fixture.Service.Complete("1.2.3");
        fixture.Consent = true;
        fixture.WhileConfirming = () =>
        {
            fixture.WhileConfirming = null;
            fixture.Controller.RequestUpdate();
        };

        fixture.Controller.RequestUpdate();

        Assert.Equal(["ready 1.2.3", "confirm 1.2.3", "apply", "exit"], fixture.Log);
    }

    [Fact]
    public void ApplyFailure_IsReported_AndTheAppKeepsRunning()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();
        fixture.Service.Complete("1.2.3");
        fixture.Service.ApplyFailure = new InvalidOperationException("Updater not found.");
        fixture.Consent = true;

        fixture.Controller.RequestUpdate();

        Assert.Equal(["ready 1.2.3", "confirm 1.2.3", "apply", "failure apply: Updater not found."], fixture.Log);
    }

    [Fact]
    public void CheckThrowing_IsReported_AndTheNextTickChecksAgain()
    {
        var fixture = new Fixture();
        fixture.Service.CheckFailure = new InvalidOperationException("No network.");

        fixture.Controller.Start();

        Assert.Equal(["failure check: No network."], fixture.Log);

        fixture.Service.CheckFailure = null;
        fixture.Time.Advance(Day);

        Assert.Equal(2, fixture.Service.CheckCount);
    }

    [Fact]
    public void CheckFaulting_IsReported_AndTheNextTickChecksAgain()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();

        fixture.Service.Fail(new HttpRequestException("Rate limit exceeded."));

        Assert.Equal(["failure check: Rate limit exceeded."], fixture.Log);

        fixture.Time.Advance(Day);
        fixture.Service.Complete("1.2.3");

        Assert.Equal(2, fixture.Service.CheckCount);
        Assert.Equal(["failure check: Rate limit exceeded.", "ready 1.2.3"], fixture.Log);
    }

    [Fact]
    public void Dispose_StopsTimer_CancelsCheck_AndIgnoresLateCompletion()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();
        var token = fixture.Service.LastToken;

        fixture.Controller.Dispose();
        fixture.Service.Complete("1.2.3");
        fixture.Time.Advance(Day * 2);
        fixture.Controller.RequestUpdate();

        Assert.True(token.IsCancellationRequested);
        Assert.Equal(0, fixture.Time.ActiveTimerCount);
        Assert.Equal(1, fixture.Service.CheckCount);
        Assert.Empty(fixture.Log);
    }

    [Fact]
    public void Dispose_IgnoresLateFailure()
    {
        var fixture = new Fixture();
        fixture.Controller.Start();

        fixture.Controller.Dispose();
        fixture.Service.Fail(new OperationCanceledException());

        Assert.Empty(fixture.Log);
    }

    private sealed class Fixture
    {
        public Fixture(bool installed = true, string? pendingVersion = null)
        {
            Service = new FakeUpdateService(Log) { IsInstalled = installed, PendingVersion = pendingVersion };
            Controller = new UpdateController(
                Service,
                new FakeFailureReporter(Log),
                version => Log.Add("ready " + version),
                version =>
                {
                    Log.Add("confirm " + version);
                    WhileConfirming?.Invoke();
                    return Consent;
                },
                () => Log.Add("exit"),
                Time,
                action => action());
        }

        public List<string> Log { get; } = [];

        public ManualTimeProvider Time { get; } = new();

        public FakeUpdateService Service { get; }

        public UpdateController Controller { get; }

        public bool Consent { get; set; }

        /// <summary>Runs inside the confirmation, like a tray click handled by the modal dialog's message loop.</summary>
        public Action? WhileConfirming { get; set; }
    }

    private sealed class FakeUpdateService(List<string> log) : IUpdateService
    {
        private TaskCompletionSource<string?>? running;

        public bool IsInstalled { get; init; }

        public string? PendingVersion { get; init; }

        public int CheckCount { get; private set; }

        public CancellationToken LastToken { get; private set; }

        public Exception? CheckFailure { get; set; }

        public Exception? ApplyFailure { get; set; }

        public Task<string?> CheckAndDownloadAsync(CancellationToken cancellationToken)
        {
            CheckCount++;
            LastToken = cancellationToken;
            if (CheckFailure is { } failure)
            {
                throw failure;
            }

            running = new TaskCompletionSource<string?>();
            return running.Task;
        }

        public void ApplyAfterExit()
        {
            log.Add("apply");
            if (ApplyFailure is { } failure)
            {
                throw failure;
            }
        }

        public void Complete(string? version) => Take().SetResult(version);

        public void Fail(Exception exception) => Take().SetException(exception);

        private TaskCompletionSource<string?> Take()
        {
            var check = running ?? throw new InvalidOperationException("No check is running.");
            running = null;
            return check;
        }
    }

    private sealed class FakeFailureReporter(List<string> log) : IUpdateFailureReporter
    {
        public void Report(string operation, string reason) => log.Add($"failure {operation}: {reason}");
    }
}
