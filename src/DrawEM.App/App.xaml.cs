using System.Windows;
using DrawEM.App.Application.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Effects;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;
using DrawEM.App.Presentation.Drawing;
using DrawEM.App.Presentation.Effects;
using DrawEM.App.Presentation.Settings;
using DrawingSessionController = DrawEM.App.Application.Drawing.DrawingSessionController;
using MessageBox = System.Windows.MessageBox;
using SoundChannelController = DrawEM.App.Application.Sound.SoundChannelController;
using EffectChannelController = DrawEM.App.Application.Effects.EffectChannelController;

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
    private EffectChannelController? effectChannel;
    private BuiltInEffectAnimations? effectAnimations;
    private LoggingEffectFailureReporter? effectFailures;
    private Win32DisplayChangeSource? displayChanges;
    private Win32SessionInterruptionSource? sessionInterruptions;

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
            Action<Action> dispatchToUi = action => Dispatcher.BeginInvoke(action);

            // Failures of a Settings window sample are also shown in the window.
            var playbackFailures = new SampleFailureRouter(soundLog, dispatchToUi);
            var channelHost = new SoundChannelHost(dispatch => new SoundChannelController(
                new MediaSoundPlayerFactory(),
                playbackFailures,
                TimeProvider.System,
                dispatch));
            soundChannel = channelHost;

            // Bindings and the sound and effect commands they hold come from one snapshot.
            var active = settings.Current;

            // Each built-in file is parsed here, once, outside the hook callback.
            var effectLog = new LoggingEffectFailureReporter(
                new EffectFailureLog(EffectFailureLog.DefaultPath).Append, TimeProvider.System);
            effectFailures = effectLog;
            var animations = new BuiltInEffectAnimations();
            effectAnimations = animations;
            var displays = new Win32DisplayChangeSource(dispatchToUi);
            displayChanges = displays;
            var sessions = new Win32SessionInterruptionSource(dispatchToUi);
            sessionInterruptions = sessions;
            var surface = new LottieEffectSurface(animations, displays.NotifyDpiChanged);

            // Failures of a Settings window sample are also shown in the window.
            var effectSampleFailures = new EffectSampleFailureRouter(effectLog);
            var effects = new EffectChannelController(
                surface, effectSampleFailures, TimeProvider.System, dispatchToUi);
            effectChannel = effects;

            var inputGate = new DrawingModeInputGate();
            var monitorBoundsSource = new Win32MonitorBoundsSource();
            var cursor = new Win32CursorPositionSource();
            keyboardHookSource = new Win32KeyboardHookSource();
            mouseHookSource = new Win32MouseHookSource();

            // Stops an effect when the cursor enters another monitor, drawing or not; frames recheck the cursor.
            // A display change, a session lock, or a system suspend also stops it and drops queued starts.
            var monitorGuard = new EffectMonitorGuard(
                mouseHookSource, displays, sessions, monitorBoundsSource, cursor, effects, dispatchToUi);
            surface.FrameStarting += monitorGuard.CheckCursor;
            var shortcuts = new GlobalShortcutAdapter(
                keyboardHookSource.Events,
                keyboardHookSource,
                controller,
                inputGate,
                cursor,
                dispatchToUi,
                monitorBoundsSource,
                ShortcutBindings.FromSnapshot(active, settings.CommandFor),
                channelHost.Play,
                monitorGuard);

            _ = new GlobalMouseInputAdapter(
                mouseHookSource, controller, inputGate, dispatchToUi, monitorBoundsSource);

            overlayWindow.Show();

            trayApplication = new TrayApplication(
                new NotifyIconTrayHost(),
                // The sound channel reports its last failures while it stops, so the log drains after it.
                // Effects stop and close their windows before their animations and failure log are released.
                new CompositeDisposable(
                    keyboardHookSource, mouseHookSource, displays, sessions, channelHost, soundLog, effects, animations, effectLog),
                overlayWindow,
                new WpfApplicationLifetime(this),
                dispatchToUi,
                failure => exitPolicy.Report("exit", failure));

            trayApplication.StopEffectsRequested += monitorGuard.StopEffects;

            var settingsSave = new SettingsSaveOperation(
                store, library, settings, channelHost, effects, shortcuts, saveNotifications);
            var sampler = new SoundSampler(channelHost.Play, channelHost.Stop, settings.CommandFor, playbackFailures);
            trayApplication.SettingsRequested += () => OpenSettings(() =>
            {
                var window = new SettingsWindow();
                // Each window samples effects on its own, so it stops only its own sample.
                var effectSampler = new EffectSampler(
                    cursor, monitorBoundsSource, monitorGuard, effects, effectSampleFailures);
                var editor = new SettingsEditor(
                    () => settings.Current, library, settingsSave, sampler, effectSampler, saveNotifications.CleanupFailed);
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
            () => displayChanges?.Dispose(),
            () => sessionInterruptions?.Dispose(),
            () => soundChannel?.Dispose(),
            () => soundFailures?.Dispose(),
            () => effectChannel?.Dispose(),
            () => effectAnimations?.Dispose(),
            () => effectFailures?.Dispose(),
            () => overlayWindow?.Close(),
        ]);
    }
}
