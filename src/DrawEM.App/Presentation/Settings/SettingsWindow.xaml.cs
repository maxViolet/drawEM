using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DrawEM.App.Domain.Settings;
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

    public SettingsWindow()
    {
        InitializeComponent();
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

    private void OnRemoveSound(object sender, RoutedEventArgs e) => Model<SlotViewModel>(sender).RemoveSound();

    private void OnSample(object sender, RoutedEventArgs e) => Model<SlotViewModel>(sender).Sample();

    private void OnRestoreDefaults(object sender, RoutedEventArgs e) => ViewModel?.RestoreDefaults();

    private void OnCancel(object sender, RoutedEventArgs e) => Close();

    private void OnSave(object sender, RoutedEventArgs e)
    {
        if (ViewModel?.Save() == true)
        {
            Close();
        }
    }

    private void OnPickColor(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not { } model)
        {
            return;
        }

        using var dialog = new WinFormsColorDialog
        {
            FullOpen = true,
            Color = System.Drawing.Color.FromArgb(model.DraftColor.Red, model.DraftColor.Green, model.DraftColor.Blue),
        };
        if (dialog.ShowDialog() == WinFormsDialogResult.OK)
        {
            model.SetColor(new HexColor(dialog.Color.R, dialog.Color.G, dialog.Color.B));
        }
    }
}
