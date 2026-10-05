using System.Windows;
using DrawEM.App.Application.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;
using DrawEM.App.Presentation.Drawing;
using DrawEM.App.Presentation.Settings;
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
    private SettingsWindow? settingsWindow;

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
            var store = new JsonSettingsStore(JsonSettingsStore.DefaultDirectory);
            var settingsFailures = new SettingsFailureDialog(message =>
                MessageBox.Show(message, "drawEM", MessageBoxButton.OK, MessageBoxImage.Warning));
            var startup = SettingsStartup.Load(store, settingsFailures);
            var library = new ManagedSoundLibrary(ManagedSoundLibrary.DefaultDirectory);
            var settings = new ActiveSettings(startup.Active, sound => library.PathFor(sound.LibraryFileName));

            var soundLog = new LoggingSoundFailureReporter(
                new SoundFailureLog(SoundFailureLog.DefaultPath).Append, TimeProvider.System);
            soundFailures = soundLog;
            var saveNotifications = new SettingsSaveNotifications(settingsFailures, soundLog, library);
            foreach (var failure in SettingsStartup.RemoveOrphanSounds(startup, library))
            {
                saveNotifications.CleanupFailed(failure);
            }

            controller = new DrawingSessionController(() => settings.Current.Style);
            overlayWindow = new OverlayWindow();
            _ = new OverlayWindowAdapter(controller, overlayWindow);

            // Failures of a Settings window sample are also shown in the window.
            var playbackFailures = new SampleFailureRouter(soundLog, action => Dispatcher.BeginInvoke(action));
            var channelHost = new SoundChannelHost(dispatch => new SoundChannelController(
                new MediaSoundPlayerFactory(),
                playbackFailures,
                TimeProvider.System,
                dispatch));
            soundChannel = channelHost;

            // Bindings and the sound commands they hold come from one snapshot.
            var active = settings.Current;

            var inputGate = new DrawingModeInputGate();
            var monitorBoundsSource = new Win32MonitorBoundsSource();
            keyboardHookSource = new Win32KeyboardHookSource();
            var shortcuts = new GlobalShortcutAdapter(
                keyboardHookSource.Events,
                keyboardHookSource,
                controller,
                inputGate,
                new Win32CursorPositionSource(),
                action => Dispatcher.BeginInvoke(action),
                monitorBoundsSource,
                ShortcutBindings.FromSnapshot(active, settings.CommandFor),
                channelHost.Play);

            mouseHookSource = new Win32MouseHookSource();
            _ = new GlobalMouseInputAdapter(
                mouseHookSource, controller, inputGate, action => Dispatcher.BeginInvoke(action), monitorBoundsSource);

            overlayWindow.Show();
            trayApplication = new TrayApplication(
                new NotifyIconTrayHost(),
                // The sound channel reports its last failures while it stops, so the log drains after it.
                new CompositeDisposable(keyboardHookSource, mouseHookSource, channelHost, soundLog),
                overlayWindow,
                new WpfApplicationLifetime(this),
                action => Dispatcher.BeginInvoke(action),
                failure => exitPolicy.Report("exit", failure));

            var settingsSave = new SettingsSaveOperation(store, library, settings, channelHost, shortcuts, saveNotifications);
            var sampler = new SoundSampler(channelHost.Play, channelHost.Stop, settings.CommandFor, playbackFailures);
            trayApplication.SettingsRequested += () => OpenSettings(() =>
            {
                var window = new SettingsWindow();
                var editor = new SettingsEditor(
                    () => settings.Current, library, settingsSave, sampler, saveNotifications.CleanupFailed);
                window.Attach(new SettingsViewModel(editor, shortcuts, window));
                return window;
            });
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
            CleanupSteps.RunAll([CloseSettings, () => trayApplication?.Dispose()]);
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

    /// <summary>Shows a Settings window over a fresh draft, or brings the open one to the front.</summary>
    /// <param name="create">Builds the window over a fresh draft of the active settings.</param>
    private void OpenSettings(Func<SettingsWindow> create)
    {
        if (settingsWindow is null)
        {
            var window = create();
            window.Closed += (_, _) => settingsWindow = null;
            settingsWindow = window;
            window.Show();
        }

        if (settingsWindow.WindowState == WindowState.Minimized)
        {
            settingsWindow.WindowState = WindowState.Normal;
        }

        settingsWindow.Activate();
    }

    /// <summary>Discards an open draft, so its imports are removed before the sound library is left.</summary>
    private void CloseSettings() => settingsWindow?.Close();

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
