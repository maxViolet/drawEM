using System.IO;
using System.Windows;
using DrawEM.App.Application.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Settings;
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

    /// <summary>Saves a settings draft and applies it to the running app. Call on the UI thread.</summary>
    internal SettingsSaveOperation? SettingsSave { get; private set; }

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
            string ManagedCopy(string libraryFileName) => Path.Combine(library.LibraryDirectory, libraryFileName);
            var settings = new ActiveSettings(startup.Active, sound => ManagedCopy(sound.LibraryFileName));

            var soundLog = new LoggingSoundFailureReporter(
                new SoundFailureLog(SoundFailureLog.DefaultPath).Append, TimeProvider.System);
            soundFailures = soundLog;
            void ReportCleanup(SoundCleanupFailure failure) => soundLog.ReportCleanup(
                failure.LibraryFileName is { } name ? ManagedCopy(name) : library.LibraryDirectory, failure.Reason);
            foreach (var failure in SettingsStartup.RemoveOrphanSounds(startup, library))
            {
                ReportCleanup(failure);
            }

            controller = new DrawingSessionController(() => settings.Current.Snapshot.Style);
            overlayWindow = new OverlayWindow();
            _ = new OverlayWindowAdapter(controller, overlayWindow);

            var channelHost = new SoundChannelHost(dispatch => new SoundChannelController(
                new MediaSoundPlayerFactory(),
                soundLog,
                TimeProvider.System,
                dispatch));
            soundChannel = channelHost;

            // Bindings and the sound commands they hold come from one configuration.
            var active = settings.Current;

            var inputGate = new DrawingModeInputGate();
            keyboardHookSource = new Win32KeyboardHookSource();
            var shortcuts = new GlobalShortcutAdapter(
                keyboardHookSource.Events,
                keyboardHookSource,
                controller,
                inputGate,
                new Win32CursorPositionSource(),
                action => Dispatcher.BeginInvoke(action),
                new Win32MonitorBoundsSource(),
                ShortcutBindings.FromSnapshot(active.Snapshot, active.CommandFor),
                channelHost.Play);

            mouseHookSource = new Win32MouseHookSource();
            _ = new GlobalMouseInputAdapter(mouseHookSource, controller, inputGate, action => Dispatcher.BeginInvoke(action));

            overlayWindow.Show();
            trayApplication = new TrayApplication(
                new NotifyIconTrayHost(),
                // The sound channel reports its last failures while it stops, so the log drains after it.
                new CompositeDisposable(keyboardHookSource, mouseHookSource, channelHost, soundLog),
                overlayWindow,
                new WpfApplicationLifetime(this),
                action => Dispatcher.BeginInvoke(action),
                failure => exitPolicy.Report("exit", failure));
            trayApplication.Start();

            SettingsSave = new SettingsSaveOperation(
                store, library, settings, channelHost.Stop, shortcuts, controller, settingsFailures, ReportCleanup,
                soundLog.ReportUnconfirmedStop);
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
