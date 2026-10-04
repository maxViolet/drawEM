using System.Windows;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Sound;
using DrawEM.App.Presentation.Drawing;
using DrawingSessionController = DrawEM.App.Application.Drawing.DrawingSessionController;
using MessageBox = System.Windows.MessageBox;
using SoundChannelController = DrawEM.App.Application.Sound.SoundChannelController;

namespace DrawEM.App;

public partial class App : System.Windows.Application
{
    private DrawingSessionController? controller;
    private OverlayWindow? overlayWindow;
    private Win32KeyboardHookSource? keyboardHookSource;
    private Win32MouseHookSource? mouseHookSource;
    private SoundChannelHost? soundChannel;
    private LoggingSoundFailureReporter? soundFailures;
    private TrayApplication? trayApplication;
    private ApplicationExitPolicy? exitPolicy;
    private SingleInstanceGuard? instanceGuard;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        instanceGuard = SingleInstanceGuard.TryAcquire(SingleInstanceGuard.DefaultName);
        if (instanceGuard is null)
        {
            // Checked before any hook is installed, so the running instance keeps its shortcuts.
            MessageBox.Show(
                "drawEM is already running. Use its icon in the notification area.",
                "drawEM",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            Shutdown();
            return;
        }

        exitPolicy = new ApplicationExitPolicy(new AppFailureLog(AppFailureLog.DefaultPath), Environment.Exit);

        try
        {
            // Step 6 replaces the default with the active saved snapshot's style.
            controller = new DrawingSessionController(() => SettingsSnapshot.Default.Style);
            overlayWindow = new OverlayWindow();
            _ = new OverlayWindowAdapter(controller, overlayWindow);

            var soundConfiguration = SoundConfiguration.Default;
            soundFailures = new LoggingSoundFailureReporter(
                new SoundFailureLog(SoundFailureLog.DefaultPath).Append, soundConfiguration, TimeProvider.System);
            var failures = soundFailures;
            soundChannel = new SoundChannelHost(dispatch => new SoundChannelController(
                new MediaSoundPlayerFactory(soundConfiguration),
                failures,
                TimeProvider.System,
                dispatch));

            var inputGate = new DrawingModeInputGate();
            keyboardHookSource = new Win32KeyboardHookSource();
            _ = new GlobalShortcutAdapter(
                keyboardHookSource.Events,
                keyboardHookSource,
                controller,
                inputGate,
                new Win32CursorPositionSource(),
                action => Dispatcher.BeginInvoke(action),
                new Win32MonitorBoundsSource(),
                ShortcutBindings.ForCodeAssignments(soundConfiguration),
                soundChannel.Play);

            mouseHookSource = new Win32MouseHookSource();
            _ = new GlobalMouseInputAdapter(mouseHookSource, controller, inputGate, action => Dispatcher.BeginInvoke(action));

            overlayWindow.Show();
            trayApplication = new TrayApplication(
                new NotifyIconTrayHost(),
                // The sound channel reports its last failures while it stops, so the log drains after it.
                new CompositeDisposable(keyboardHookSource, mouseHookSource, soundChannel, soundFailures),
                overlayWindow,
                new WpfApplicationLifetime(this),
                action => Dispatcher.BeginInvoke(action),
                failure => exitPolicy.Report("exit", failure));
            trayApplication.Start();
        }
        catch (Exception exception)
        {
            // Construction of the sound host can itself time out before it is assigned to a field.
            exitPolicy.Report("startup", exception);
            StartupFailure.Handle(
                ReleaseStartedResources,
                failure => exitPolicy.Report("startup cleanup", failure),
                () => exitPolicy.ShowStartupError(
                    () => MessageBox.Show(exception.Message, "drawEM", MessageBoxButton.OK, MessageBoxImage.Error)),
                Shutdown);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        try
        {
            trayApplication?.Dispose();
            base.OnExit(e);
        }
        finally
        {
            // Released after cleanup so a new instance cannot start while this one still holds hooks.
            instanceGuard?.Dispose();

            // All resource cleanup has been attempted before a timed-out sound thread forces exit.
            exitPolicy?.Complete();
        }
    }

    private void ReleaseStartedResources()
    {
        if (trayApplication is not null)
        {
            trayApplication.Dispose();
            return;
        }

        CleanupSteps.RunAll(
        [
            () => keyboardHookSource?.Dispose(),
            () => mouseHookSource?.Dispose(),
            () => soundChannel?.Dispose(),
            () => soundFailures?.Dispose(),
            () => overlayWindow?.Close(),
        ]);
    }
}
