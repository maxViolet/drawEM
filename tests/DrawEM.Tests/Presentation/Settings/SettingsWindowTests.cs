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
using DrawEM.Tests.Application.Settings;

namespace DrawEM.Tests.Presentation.Settings;

/// <summary>Shows the real window off-screen, to catch XAML and binding mistakes.</summary>
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
                new SettingsEditor(() => active, new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), _ => { }),
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

            tabs.SelectedIndex = 1;
            Layout(window);
            var effects = ComboBoxes(slots, "Effect");
            Assert.Equal(ActionSlot.Count, effects.Length);
            Assert.Equal(
                ["Monitor: Confetti", "Cursor: Focus ring"],
                effects[1].Items.Groups!.Cast<CollectionViewGroup>().Select(group =>
                    $"{group.Name}: {string.Join(", ", group.Items.Cast<EffectOption>().Select(option => option.Name))}"));
            Assert.Equal("Focus ring", Assert.IsType<EffectOption>(effects[1].SelectedItem).Name);
            Assert.Equal(Visibility.Visible, effects[1].Visibility);
            Assert.Equal(Visibility.Collapsed, effects[0].Visibility);
            Assert.Null(effects[2].SelectedItem);

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
                new SettingsEditor(() => active, new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), _ => { }),
                new FakeCapture(),
                picker);
            Show(window, model);
            ((TabControl)window.FindName("Tabs")).SelectedIndex = 1;
            Layout(window);
            var kind = ComboBoxes(window, "Action type")[1];

            // As a user's choice does: the one-way SelectedItem binding stays.
            void ChooseSound()
            {
                kind.SetCurrentValue(Selector.SelectedItemProperty, SlotKind.Sound);
                Layout(window);
            }

            ChooseSound();

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
            ContentControl.ContentProperty, UIElement.IsEnabledProperty, TextBlock.TextProperty,
            FrameworkElement.DataContextProperty, FrameworkElement.ToolTipProperty, UIElement.VisibilityProperty,
            Selector.SelectedItemProperty, ItemsControl.ItemsSourceProperty,
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
