using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Presentation.Theme;
using Microsoft.Win32;
using WinFormsColorDialog = System.Windows.Forms.ColorDialog;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;

namespace DrawEM.App.Presentation.Settings;

/// <summary>
/// The Settings window. Closing it any way other than a successful Save discards the draft.
/// </summary>
public partial class SettingsWindow : Window, ISoundFilePicker
{
    private SettingsViewModel? ViewModel => DataContext as SettingsViewModel;

    /// <summary>The title bar height in window units: 36 layout units at the window's 1.2 scale.</summary>
    private const double CaptionHeight = 36 * 1.2;

    /// <summary>Segoe MDL2 Assets glyphs of the maximize button.</summary>
    private const string MaximizeGlyph = "\uE922";
    private const string RestoreGlyph = "\uE923";

    /// <summary>Set once closing goes ahead; the draft has then ended.</summary>
    private bool closed;

    private readonly UiThemeState theme;

    public SettingsWindow()
        : this(SystemParameters.WorkArea.Height)
    {
    }

    /// <param name="workAreaHeight">
    /// The usable screen height. The default height is cut to it, so the title bar stays on screen.
    /// </param>
    /// <param name="theme">The shared theme; without one, the window follows the Windows app theme.</param>
    public SettingsWindow(double workAreaHeight, UiThemeState? theme = null)
    {
        InitializeComponent();
        this.theme = theme ?? UiThemeState.FromSystem();
        GlassBackdrop.Attach(this, this.theme, CaptionHeight, new Thickness(6));
        ShowTheme();
        this.theme.Changed += ShowTheme;
        Closed += (_, _) => this.theme.Changed -= ShowTheme;
        StateChanged += (_, _) => OnStateChanged();
        Height = Math.Min(Height, workAreaHeight);
        // A capture field must not take focus on open: focusing one starts capture.
        Loaded += (_, _) => HexBox.Focus();
    }

    /// <summary>Binds the window to its draft. Call once, before <see cref="Window.Show"/>.</summary>
    public void Attach(SettingsViewModel model)
    {
        DataContext = model;
    }

    public string? Pick()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Choose a sound",
            Filter = "Sound files (*.wav;*.mp3)|*.wav;*.mp3",
            CheckFileExists = true,
        };
        return dialog.ShowDialog(this) == true ? dialog.FileName : null;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        if (!e.Cancel)
        {
            closed = true;
            ViewModel?.Close();
        }
    }

    private static T Model<T>(object sender) => (T)((FrameworkElement)sender).DataContext;

    private void OnCaptureClick(object sender, RoutedEventArgs e) => Model<ShortcutFieldViewModel>(sender).BeginCapture();

    private void OnCaptureFocus(object sender, KeyboardFocusChangedEventArgs e) =>
        Model<ShortcutFieldViewModel>(sender).BeginCapture();

    private void OnCaptureFocusLost(object sender, KeyboardFocusChangedEventArgs e) =>
        Model<ShortcutFieldViewModel>(sender).EndCapture();

    private void OnChooseFile(object sender, RoutedEventArgs e) => Model<SlotViewModel>(sender).ChooseFile();

    // SelectedItem is bound one way, and the change runs after this selection change ends: a file dialog
    // then opens over a closed list, and a choice the model refuses, such as a cancelled dialog, is reverted
    // by the model's notification. That revert reaches here again as a no-op. A change still queued when
    // the window closes is dropped, so nothing is imported into the ended draft.
    private void OnKindChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 1 && e.AddedItems[0] is SlotKind kind)
        {
            var slot = Model<SlotViewModel>(sender);
            Dispatcher.BeginInvoke(() =>
            {
                if (!closed)
                {
                    slot.ChangeKind(kind);
                }
            });
        }
    }

    private void OnClearSlot(object sender, RoutedEventArgs e) => Model<SlotViewModel>(sender).ClearSlot();

    private void OnSample(object sender, RoutedEventArgs e) => Model<SlotViewModel>(sender).Sample();

    private void OnIncreaseMaxLength(object sender, RoutedEventArgs e) =>
        Model<SlotViewModel>(sender).StepMaxLength(1);

    private void OnDecreaseMaxLength(object sender, RoutedEventArgs e) =>
        Model<SlotViewModel>(sender).StepMaxLength(-1);

    /// <summary>Shows the shared theme on the switch; the switch then changes nothing.</summary>
    private void ShowTheme() => ThemeSwitch.IsChecked = theme.Current == UiTheme.Light;

    private void OnThemeSwitched(object sender, RoutedEventArgs e)
    {
        if ((ThemeSwitch.IsChecked == true) != (theme.Current == UiTheme.Light))
        {
            theme.Toggle();
        }
    }

    private void OnMinimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OnMaximize(object sender, RoutedEventArgs e) =>
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    /// <summary>
    /// A maximized window extends past the screen by its resize border, so the content is inset by it. The
    /// maximize button shows Restore while maximized.
    /// </summary>
    private void OnStateChanged()
    {
        var maximized = WindowState == WindowState.Maximized;
        Frame.Margin = maximized ? SystemParameters.WindowResizeBorderThickness : default;
        MaximizeButton.Content = maximized ? RestoreGlyph : MaximizeGlyph;
    }

    private void OnRestoreDefaults(object sender, RoutedEventArgs e) => ViewModel?.RestoreDefaults();

    private void OnCancel(object sender, RoutedEventArgs e) => Close();

    private void OnSave(object sender, RoutedEventArgs e)
    {
        if (ViewModel?.Save() == true)
        {
            Close();
        }
    }

    private void OnIncreaseWidth(object sender, RoutedEventArgs e) => ViewModel?.StepWidth(1);

    private void OnDecreaseWidth(object sender, RoutedEventArgs e) => ViewModel?.StepWidth(-1);

    private void OnWidthKeyDown(object sender, KeyEventArgs e)
    {
        if (WidthStep(e.Key) is { } pixels && ViewModel is { } model)
        {
            model.StepWidth(pixels);
            WidthBox.CaretIndex = WidthBox.Text.Length;
            e.Handled = true;
        }
    }

    /// <summary>Up and Down step the width by one pixel; other keys type into the box.</summary>
    private static int? WidthStep(Key key) => key switch
    {
        Key.Up => 1,
        Key.Down => -1,
        _ => null,
    };

    /// <summary>Asks for a color, starting from the given one; <c>null</c> when cancelled. Tests replace it.</summary>
    internal Func<HexColor, HexColor?> PickColor { get; set; } = ShowColorDialog;

    private void OnPickColor(object sender, RoutedEventArgs e)
    {
        if (ViewModel is { } model && PickColor(model.DraftColor) is { } color)
        {
            model.SetColor(color);
        }
    }

    private static HexColor? ShowColorDialog(HexColor current)
    {
        using var dialog = new WinFormsColorDialog
        {
            FullOpen = true,
            Color = System.Drawing.Color.FromArgb(current.Red, current.Green, current.Blue),
        };
        return dialog.ShowDialog() == WinFormsDialogResult.OK
            ? new HexColor(dialog.Color.R, dialog.Color.G, dialog.Color.B)
            : null;
    }
}
