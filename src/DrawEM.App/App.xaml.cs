using System.Windows;
using DrawEM.App.Infrastructure;
using DrawEM.App.Presentation;
using DrawingSessionController = DrawEM.App.Application.DrawingSessionController;
using MessageBox = System.Windows.MessageBox;

namespace DrawEM.App;

public partial class App : System.Windows.Application
{
    private DrawingSessionController? controller;
    private OverlayWindow? overlayWindow;
    private Win32KeyboardHookSource? keyboardHookSource;
    private Win32MouseHookSource? mouseHookSource;
    private TrayApplication? trayApplication;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        try
        {
            controller = new DrawingSessionController();
            overlayWindow = new OverlayWindow();
            _ = new OverlayWindowAdapter(controller, overlayWindow);

            var inputGate = new DrawingModeInputGate();
            keyboardHookSource = new Win32KeyboardHookSource();
            _ = new GlobalShortcutAdapter(
                keyboardHookSource,
                controller,
                inputGate,
                new Win32CursorPositionSource(),
                action => Dispatcher.BeginInvoke(action),
                new Win32MonitorBoundsSource());

            mouseHookSource = new Win32MouseHookSource();
            _ = new GlobalMouseInputAdapter(mouseHookSource, controller, inputGate, action => Dispatcher.BeginInvoke(action));

            overlayWindow.Show();
            trayApplication = new TrayApplication(
                new NotifyIconTrayHost(),
                new CompositeDisposable(keyboardHookSource, mouseHookSource),
                overlayWindow,
                new WpfApplicationLifetime(this),
                action => Dispatcher.BeginInvoke(action));
            trayApplication.Start();
        }
        catch (Exception exception)
        {
            if (trayApplication is not null)
            {
                trayApplication.Dispose();
            }
            else
            {
                keyboardHookSource?.Dispose();
                mouseHookSource?.Dispose();
                overlayWindow?.Close();
            }

            MessageBox.Show(exception.Message, "drawEM", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        trayApplication?.Dispose();
        base.OnExit(e);
    }
}
