using System.Windows;
using DrawEM.App.Domain;
using DrawEM.App.Infrastructure;
using DrawEM.App.Presentation;

namespace DrawEM.App;

public partial class App : Application
{
    private DrawingSessionController? controller;
    private OverlayWindow? overlayWindow;
    private Win32KeyboardHookSource? keyboardHookSource;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        controller = new DrawingSessionController();
        overlayWindow = new OverlayWindow();
        _ = new OverlayWindowAdapter(controller, overlayWindow);

        keyboardHookSource = new Win32KeyboardHookSource();
        _ = new GlobalShortcutAdapter(keyboardHookSource, controller, action => Dispatcher.BeginInvoke(action));

        overlayWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        keyboardHookSource?.Dispose();
        base.OnExit(e);
    }
}
