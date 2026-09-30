using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests.Application.Settings;

public class SettingsStartupTests
{
    [Fact]
    public void Load_UsesSavedSettings()
    {
        var saved = SettingsSnapshot.Validate(
            new DrawingStyle(new HexColor(0, 0, 255), new StrokeWidth(9)),
            SettingsSnapshot.Default.DrawShortcut,
            SettingsSnapshot.Default.ClearShortcut,
            SettingsSnapshot.Default.Slots).Snapshot!;
        var store = new FakeStore(new SettingsLoadResult.Loaded(saved));
        var failures = new FakeReporter();

        var startup = SettingsStartup.Load(store, failures);

        Assert.Same(saved, startup.Active);
        Assert.True(startup.SavedSettingsLoaded);
        Assert.Empty(failures.Reports);
    }

    [Fact]
    public void Load_FirstLaunch_UsesDefaultsWithoutReportingOrSaving()
    {
        var store = new FakeStore(new SettingsLoadResult.Missing());
        var failures = new FakeReporter();

        var startup = SettingsStartup.Load(store, failures);

        Assert.Same(SettingsSnapshot.Default, startup.Active);
        Assert.All(startup.Active.Slots, slot => Assert.True(slot.IsEmpty));
        Assert.False(startup.SavedSettingsLoaded);
        Assert.Empty(failures.Reports);
        Assert.Equal(0, store.Saves);
    }

    [Fact]
    public void Load_Unreadable_ReportsFailureUsesDefaultsAndDoesNotSave()
    {
        var store = new FakeStore(new SettingsLoadResult.Unreadable("broken", @"C:\copy.json"));
        var failures = new FakeReporter();

        var startup = SettingsStartup.Load(store, failures);

        Assert.Same(SettingsSnapshot.Default, startup.Active);
        Assert.False(startup.SavedSettingsLoaded);
        Assert.Equal([("broken", @"C:\copy.json")], failures.Reports);
        Assert.Equal(0, store.Saves);
    }

    private sealed class FakeStore(SettingsLoadResult result) : ISettingsStore
    {
        public int Saves { get; private set; }

        public SettingsLoadResult Load() => result;

        public void Save(SettingsSnapshot snapshot) => Saves++;
    }

    private sealed class FakeReporter : ISettingsFailureReporter
    {
        public List<(string Reason, string? RecoveryCopy)> Reports { get; } = [];

        public void SettingsUnreadable(string reason, string? recoveryCopy) => Reports.Add((reason, recoveryCopy));
    }
}
