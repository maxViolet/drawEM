using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;
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
            var active = TestSettings.WithSounds((1, new SoundReference("horn.wav", "Horn.wav")));
            var window = new SettingsWindow();
            var model = new SettingsViewModel(
                new SettingsEditor(() => active, new FakeLibrary(), new FakeSettingsSave(), new FakeSampler(), _ => { }),
                new FakeCapture(),
                window);
            window.Attach(model);
            window.ShowActivated = false;
            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = -20000;
            window.Top = -20000;
            window.Show();
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

            model.HexText = "#12";
            Layout(window);
            Assert.False(save.IsEnabled);
            window.Close();
        });
    }

    /// <summary>Properties bound by a style or a data template, which are not local values.</summary>
    private static readonly DependencyProperty[] StyledProperties =
        [
            ContentControl.ContentProperty, UIElement.IsEnabledProperty, TextBlock.TextProperty,
            FrameworkElement.DataContextProperty, FrameworkElement.ToolTipProperty,
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
}
