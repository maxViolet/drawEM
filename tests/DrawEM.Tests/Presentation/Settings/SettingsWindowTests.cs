using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Presentation.Settings;
using DrawEM.App.Presentation.Theme;
using DrawEM.Tests.Application.Settings;

namespace DrawEM.Tests.Presentation.Settings;

/// <summary>Shows the real window off-screen, to catch XAML and binding mistakes.</summary>
[Collection(WpfCollection.Name)]
public sealed class SettingsWindowTests
{
    [Fact]
    public void Window_BindsEveryFieldWithoutBindingErrors()
    {
        StaThread.Run(() =>
        {
            var active = TestSettings.WithActions(
                [(1, new SoundReference("horn.wav", "Horn.wav"))],
                [(2, new EffectId("focus-ring"))]);
            var window = new SettingsWindow();
            var model = new SettingsViewModel(
                new SettingsEditor(() => active, new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), new FakeEffectSampler(), _ => { }),
                new FakeCapture(),
                window);
            Show(window, model);
            var tabs = (TabControl)window.FindName("Tabs");
            var bindings = new List<string>();
            foreach (var tab in tabs.Items.Cast<TabItem>())
            {
                tabs.SelectedItem = tab;
                Layout(window);
                bindings.AddRange(Bindings(window));
            }

            var save = (Button)window.FindName("SaveButton");
            var slots = (ItemsControl)window.FindName("SlotList");
            Assert.DoesNotContain(bindings, binding => !binding.EndsWith(" Active", StringComparison.Ordinal));
            // The walk reaches the slot template and both tabs.
            Assert.Contains("TextBlock.Text: FileName Active", bindings);
            Assert.Contains("TextBox.Text: HexText Active", bindings);
            Assert.True(save.IsEnabled);
            Assert.Equal(ActionSlot.Count, slots.Items.Count);
            Assert.Equal("Ctrl+Alt+Z", ((Button)window.FindName("DrawShortcutButton")).Content);
            Assert.Contains("ComboBox.SelectedItem: Effect Active", bindings);
            Assert.Contains("ComboBox.SelectedItem: Kind Active", bindings);
            Assert.Contains("ComboBox.Visibility: IsEffect Active", bindings);
            Assert.Contains("CheckBox.IsChecked: Loop Active", bindings);
            Assert.Contains("CheckBox.IsChecked: NoLimit Active", bindings);
            Assert.Contains("RepeatButton.IsEnabled: CanIncreaseMaxLength Active", bindings);
            Assert.Contains("Button.Content: SampleText Active", bindings);

            Assert.Equal(3, tabs.Items.Count);
            Assert.Contains("CheckBox.IsChecked: BezierSmoothing Active", bindings);
            tabs.SelectedIndex = 2;
            Layout(window);
            var smoothing = Descendants<CheckBox>(window)
                .Single(box => AutomationProperties.GetName(box) == "Bezier smoothing");
            Assert.True(smoothing.IsChecked);
            tabs.SelectedIndex = 1;
            Layout(window);
            var effects = ComboBoxes(slots, "Effect");
            Assert.Equal(ActionSlot.Count, effects.Length);
            Assert.Equal(
                EffectOption.All.GroupBy(option => option.Group).Select(group =>
                    $"{group.Key}: {string.Join(", ", group.Select(option => option.Name))}"),
                effects[1].Items.Groups!.Cast<CollectionViewGroup>().Select(group =>
                    $"{group.Name}: {string.Join(", ", group.Items.Cast<EffectOption>().Select(option => option.Name))}"));
            Assert.Equal("Focus ring", Assert.IsType<EffectOption>(effects[1].SelectedItem).Name);
            Assert.Equal(Visibility.Visible, effects[1].Visibility);
            Assert.Equal(Visibility.Collapsed, effects[0].Visibility);
            Assert.Null(effects[2].SelectedItem);
            var loops = Descendants<CheckBox>(slots)
                .Where(box => AutomationProperties.GetName(box) == "Loop effect")
                .ToArray();
            var noLimits = Descendants<CheckBox>(slots)
                .Where(box => AutomationProperties.GetName(box) == "No effect length limit")
                .ToArray();
            var lengthSteppers = Descendants<RepeatButton>(slots)
                .Where(button => AutomationProperties.GetName(button) == "Increase max length")
                .ToArray();
            Assert.Equal(ActionSlot.Count, loops.Length);
            Assert.Equal([false, true, false],
                loops.Take(3).Select(box => box.IsVisible));
            Assert.Equal(Visibility.Visible, noLimits[1].Visibility);
            Assert.True(noLimits[1].IsEnabled);
            Assert.True(lengthSteppers[1].IsEnabled);
            var samples = Descendants<Button>(slots).Where(button => Equals(button.Content, "Sample")).ToArray();
            Assert.Equal([true, true, false], samples.Take(3).Select(button => button.IsEnabled));

            model.HexText = "#12";
            Layout(window);
            Assert.False(save.IsEnabled);
            window.Close();
        });
    }

    [Fact]
    public void TypeSelector_WhenTheFileDialogIsCancelled_ShowsTheKeptEffect()
    {
        StaThread.Run(() =>
        {
            var active = TestSettings.WithEffects((2, new EffectId("confetti")));
            var picker = new FakePicker();
            var window = new SettingsWindow();
            var model = new SettingsViewModel(
                new SettingsEditor(() => active, new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), new FakeEffectSampler(), _ => { }),
                new FakeCapture(),
                picker);
            Show(window, model);
            ((TabControl)window.FindName("Tabs")).SelectedIndex = 1;
            Layout(window);
            var kind = ComboBoxes(window, "Action type")[1];
            var listOpenAtPick = new List<bool>();
            picker.Picking = () => listOpenAtPick.Add(kind.IsDropDownOpen);

            // As a click on a list item does: select while the list is open, then close it. The selection keeps
            // the one-way SelectedItem binding.
            void ChooseSound()
            {
                kind.IsDropDownOpen = true;
                Layout(window);
                kind.SetCurrentValue(Selector.SelectedItemProperty, SlotKind.Sound);
                kind.IsDropDownOpen = false;
                Layout(window);
            }

            ChooseSound();

            Assert.Equal([false], listOpenAtPick);

            Assert.Single(picker.Picks);
            Assert.Equal(SlotKind.Effect, kind.SelectedItem);
            Assert.Equal(SlotKind.Effect, model.Slots[1].Kind);

            picker.Next = @"C:\sounds\Horn.wav";
            ChooseSound();

            Assert.Equal(2, picker.Picks.Count);
            Assert.Equal(SlotKind.Sound, kind.SelectedItem);
            Assert.Equal("Horn.wav", model.Slots[1].FileName);
            Assert.Equal("Ctrl+Alt+2", model.Slots[1].Shortcut.Text);
            window.Close();
        });
    }

    [Fact]
    public void TypeSelector_ChangeQueuedWhenTheWindowCloses_DoesNothing()
    {
        StaThread.Run(() =>
        {
            var active = TestSettings.WithEffects((2, new EffectId("confetti")));
            var picker = new FakePicker { Next = @"C:\sounds\Horn.wav" };
            var library = new FakeLibrary();
            var window = new SettingsWindow();
            var model = new SettingsViewModel(
                new SettingsEditor(() => active, library, new FakeSettingsSave(), new FakeSampler(), new FakeEffectSampler(), _ => { }),
                new FakeCapture(),
                picker);
            Show(window, model);
            ((TabControl)window.FindName("Tabs")).SelectedIndex = 1;
            Layout(window);

            ComboBoxes(window, "Action type")[1].SetCurrentValue(Selector.SelectedItemProperty, SlotKind.Sound);
            window.Close();
            Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);

            Assert.Empty(picker.Picks);
            Assert.Empty(library.Imported);
            Assert.Equal([active], library.Discarded);
        });
    }

    [Fact]
    public void DropDownsAndTooltips_AreDrawnAtTheWindowsScale()
    {
        StaThread.Run(() =>
        {
            var active = TestSettings.WithEffects((2, new EffectId("confetti")));
            var window = new SettingsWindow();
            var model = new SettingsViewModel(
                new SettingsEditor(() => active, new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), new FakeEffectSampler(), _ => { }),
                new FakeCapture(),
                window);
            Show(window, model);
            ((TabControl)window.FindName("Tabs")).SelectedIndex = 1;
            Layout(window);
            var effects = ComboBoxes(window, "Effect")[1];
            var scale = ScreenScale(effects);
            Assert.Equal(1.2, scale, 3);

            effects.IsDropDownOpen = true;
            Layout(window);
            var dropDown = ((Popup)effects.Template.FindName("PART_Popup", effects)).Child;
            var item = Descendants<ComboBoxItem>(dropDown).First();
            var header = Descendants<TextBlock>(dropDown).First(text => text.Text == "Monitor");
            var tip = new ToolTip { Content = "tip", PlacementTarget = effects };
            effects.ToolTip = tip;
            tip.IsOpen = true;
            Layout(window);

            Assert.Equal(scale, ScreenScale(item), 3);
            Assert.Equal(scale, ScreenScale(header), 3);
            Assert.Equal(scale, ScreenScale(tip), 3);
            tip.IsOpen = false;
            effects.IsDropDownOpen = false;
            window.Close();
        });
    }

    [Fact]
    public void ThemeSwitch_TogglesTheSharedThemeAndTheWindowBrushes()
    {
        StaThread.Run(() =>
        {
            var theme = new UiThemeState(UiTheme.Dark);
            var window = new SettingsWindow(1200, theme);
            var model = new SettingsViewModel(
                new SettingsEditor(() => TestSettings.WithSounds(), new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), new FakeEffectSampler(), _ => { }),
                new FakeCapture(),
                window);
            Show(window, model);
            Layout(window);
            var themeSwitch = (ToggleButton)window.FindName("ThemeSwitch");
            var dark = ((SolidColorBrush)window.FindResource("Glass.Foreground")).Color;
            Assert.False(themeSwitch.IsChecked);

            themeSwitch.IsChecked = true;
            Layout(window);

            Assert.Equal(UiTheme.Light, theme.Current);
            Assert.NotEqual(dark, ((SolidColorBrush)window.FindResource("Glass.Foreground")).Color);

            theme.Toggle();
            Layout(window);

            Assert.False(themeSwitch.IsChecked);
            Assert.Equal(dark, ((SolidColorBrush)window.FindResource("Glass.Foreground")).Color);
            window.Close();
        });
    }

    /// <summary>Work areas of a large display, 1366×768 at 100%, and 1366×768 at 125%.</summary>
    [Theory]
    [InlineData(1200, 768)]
    [InlineData(728, 728)]
    [InlineData(576, 576)]
    public void Window_FitsTheWorkAreaHeight(double workArea, double height)
    {
        StaThread.Run(() =>
        {
            var window = new SettingsWindow(workArea);
            var model = new SettingsViewModel(
                new SettingsEditor(() => TestSettings.WithSounds(), new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), new FakeEffectSampler(), _ => { }),
                new FakeCapture(),
                window);
            Show(window, model);
            Layout(window);

            Assert.Equal(height, window.ActualHeight, 3);
            Assert.True(window.MinHeight <= workArea, $"Minimum height {window.MinHeight} exceeds {workArea}.");
            window.Close();
        });
    }

    /// <summary>How many device-independent screen units one layout unit of <paramref name="element"/> spans.</summary>
    private static double ScreenScale(Visual element) =>
        (element.PointToScreen(new Point(100, 0)).X - element.PointToScreen(new Point(0, 0)).X)
        / 100 / VisualTreeHelper.GetDpi(element).DpiScaleX;

    private static void Show(SettingsWindow window, SettingsViewModel model)
    {
        window.Attach(model);
        window.ShowActivated = false;
        window.ShowInTaskbar = false;
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.Left = -20000;
        window.Top = -20000;
        window.Show();
    }

    /// <summary>Properties bound by a style or a data template, which are not local values.</summary>
    private static readonly DependencyProperty[] StyledProperties =
        [
            ContentControl.ContentProperty, ToggleButton.IsCheckedProperty, UIElement.IsEnabledProperty,
            TextBlock.TextProperty, FrameworkElement.DataContextProperty, FrameworkElement.ToolTipProperty,
            UIElement.VisibilityProperty, Selector.SelectedItemProperty, ItemsControl.ItemsSourceProperty,
        ];

    private static void Layout(Window window)
    {
        // Deferred bindings and item generation run on the dispatcher, then need another layout pass.
        for (var pass = 0; pass < 2; pass++)
        {
            window.UpdateLayout();
            Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
        }
    }

    /// <summary>
    /// Every binding in the visual tree, as <c>Type.Property: Path Status</c>: local ones, and those a style
    /// sets on <see cref="StyledProperties"/>.
    /// </summary>
    private static IEnumerable<string> Bindings(DependencyObject element)
    {
        var properties = new HashSet<DependencyProperty>(StyledProperties);
        var values = element.GetLocalValueEnumerator();
        while (values.MoveNext())
        {
            properties.Add(values.Current.Property);
        }

        foreach (var property in properties)
        {
            if (BindingOperations.GetBindingExpression(element, property) is { } expression)
            {
                yield return $"{element.GetType().Name}.{property.Name}: " +
                    $"{expression.ParentBinding.Path?.Path} {expression.Status}";
            }
        }

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            foreach (var binding in Bindings(VisualTreeHelper.GetChild(element, i)))
            {
                yield return binding;
            }
        }
    }

    /// <summary>The combo boxes under <paramref name="root"/> with this automation name, one per slot.</summary>
    private static ComboBox[] ComboBoxes(DependencyObject root, string name) =>
        Descendants<ComboBox>(root).Where(box => AutomationProperties.GetName(box) == name).ToArray();

    private static IEnumerable<T> Descendants<T>(DependencyObject element) where T : DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var child = VisualTreeHelper.GetChild(element, i);
            if (child is T match)
            {
                yield return match;
            }

            foreach (var descendant in Descendants<T>(child))
            {
                yield return descendant;
            }
        }
    }
}
