using System.Windows;
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

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        var appLog = new AppFailureLog(AppFailureLog.DefaultPath);

        try
        {
            controller = new DrawingSessionController();
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
                keyboardHookSource,
                controller,
                inputGate,
                new Win32CursorPositionSource(),
                action => Dispatcher.BeginInvoke(action),
                new Win32MonitorBoundsSource(),
                soundConfiguration.Resolve,
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
                failure => appLog.Append("exit", failure));
            trayApplication.Start();
        }
        catch (Exception exception)
        {
            StartupFailure.Handle(
                ReleaseStartedResources,
                failure => appLog.Append("startup cleanup", failure),
                () => MessageBox.Show(exception.Message, "drawEM", MessageBoxButton.OK, MessageBoxImage.Error),
                Shutdown);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        trayApplication?.Dispose();
        base.OnExit(e);
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
