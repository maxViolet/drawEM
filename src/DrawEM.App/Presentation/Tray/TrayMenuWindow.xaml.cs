using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using DrawEM.App.Presentation.Theme;
using ITrayMenu = DrawEM.App.Infrastructure.ITrayMenu;
using TrayMenuCommand = DrawEM.App.Infrastructure.TrayMenuCommand;
using WinFormsCursor = System.Windows.Forms.Cursor;
using WinFormsScreen = System.Windows.Forms.Screen;

namespace DrawEM.App.Presentation.Tray;

/// <summary>
/// The tray menu: a glass window that opens next to the cursor and closes on a command, Escape, or a click
/// elsewhere. It is hidden rather than closed, so it opens again quickly.
/// </summary>
public partial class TrayMenuWindow : Window, ITrayMenu
{
    public TrayMenuWindow(UiThemeState theme)
    {
        InitializeComponent();
        GlassBackdrop.Attach(this, theme, captionHeight: 0, resizeBorder: default);
        SourceInitialized += (_, _) => HideFromAltTab();
        Deactivated += (_, _) => Hide();
        PreviewKeyDown += OnPreviewKeyDown;
    }

    public event Action<TrayMenuCommand>? Chosen;

    /// <summary>Opens the menu at the cursor, on the monitor the cursor is on.</summary>
    void ITrayMenu.Show()
    {
        var cursor = WinFormsCursor.Position;
        var workArea = WinFormsScreen.FromPoint(cursor).WorkingArea;
        var scale = MonitorScale(cursor);
        ShowAt(
            new Point(cursor.X / scale, cursor.Y / scale),
            new Rect(workArea.X / scale, workArea.Y / scale, workArea.Width / scale, workArea.Height / scale));
    }

    /// <summary>Opens the menu with a corner at <paramref name="cursor"/>, inside <paramref name="workArea"/>.</summary>
    internal void ShowAt(Point cursor, Rect workArea)
    {
        if (!IsVisible)
        {
            // Shown off-screen first, so the menu has its size before it is placed.
            Left = -32000;
            Top = -32000;
            Show();
        }

        UpdateLayout();
        var position = TrayMenuPlacement.Place(cursor, new Size(ActualWidth, ActualHeight), workArea);
        Left = position.X;
        Top = position.Y;
        if (ShowActivated)
        {
            // The menu closes when another window becomes active, as a system menu does.
            Activate();
            SettingsItem.Focus();
        }
    }

    private void Choose(TrayMenuCommand command)
    {
        Hide();
        Chosen?.Invoke(command);
    }

    private void OnSettings(object sender, RoutedEventArgs e) => Choose(TrayMenuCommand.Settings);

    private void OnStopEffects(object sender, RoutedEventArgs e) => Choose(TrayMenuCommand.StopEffects);

    private void OnExit(object sender, RoutedEventArgs e) => Choose(TrayMenuCommand.Exit);

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Hide();
            e.Handled = true;
        }
    }

    /// <summary>A menu is not a window to switch to, so it stays out of Alt+Tab.</summary>
    private void HideFromAltTab()
    {
        var handle = new WindowInteropHelper(this).Handle;
        var style = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE, style | NativeMethods.WS_EX_TOOLWINDOW);
    }

    /// <summary>The scale of the monitor at <paramref name="point"/>, in physical pixels per window unit.</summary>
    private static double MonitorScale(System.Drawing.Point point)
    {
        var monitor = NativeMethods.MonitorFromPoint(
            new NativeMethods.POINT { X = point.X, Y = point.Y }, NativeMethods.MONITOR_DEFAULTTONEAREST);
        return NativeMethods.GetDpiForMonitor(monitor, NativeMethods.MDT_EFFECTIVE_DPI, out var dpi, out _) == 0
            ? dpi / 96.0
            : 1.0;
    }
}
