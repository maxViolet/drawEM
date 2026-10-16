using Velopack;

namespace DrawEM.App;

public static class Program
{
    /// <summary>
    /// Runs Velopack's install, update, and uninstall hooks before anything else. During a hook,
    /// <see cref="VelopackApp.Run"/> exits the process, so settings, sounds, logs, the single-instance guard,
    /// the tray, and global hooks never start. No hook callbacks are registered.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        // Updates install only after the user confirms a restart (R4-07), never silently on launch.
        VelopackApp.Build().SetAutoApplyOnStartup(false).Run();

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}
