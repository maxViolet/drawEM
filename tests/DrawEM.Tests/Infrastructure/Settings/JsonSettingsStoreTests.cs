using System.Text;
using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Settings;

namespace DrawEM.Tests.Infrastructure.Settings;

public sealed class JsonSettingsStoreTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
    private readonly ManualTimeProvider time = new();

    private string SettingsPath => Path.Combine(directory, "settings.json");

    public void Dispose()
    {
        if (!Directory.Exists(directory))
        {
            return;
        }

        foreach (var file in Directory.EnumerateFiles(directory))
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }

        Directory.Delete(directory, recursive: true);
    }

    [Fact]
    public void DefaultDirectory_IsDrawEMUnderCurrentUserLocalAppData()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.Equal(Path.Combine(localAppData, "drawEM"), JsonSettingsStore.DefaultDirectory);
    }

    [Fact]
    public void Load_FirstLaunch_IsMissingAndWritesNothing()
    {
        var result = Store().Load();

        Assert.IsType<SettingsLoadResult.Missing>(result);
        Assert.False(Directory.Exists(directory));
    }

    [Fact]
    public void FirstLaunch_StartsWithDefaultsAndEightEmptySlots()
    {
        var startup = SettingsStartup.Load(Store(), new NoReporter());

        Assert.Equal(SettingsSnapshot.Default, startup.Active);
        Assert.All(startup.Active.Slots, slot => Assert.True(slot.IsEmpty));
        Assert.False(File.Exists(SettingsPath));
    }

    [Fact]
    public void Save_ThenLoadInNewStore_RestoresSnapshot()
    {
        var snapshot = Custom();
        Store().Save(snapshot);

        var result = Store().Load();

        Assert.Equal(snapshot, Assert.IsType<SettingsLoadResult.Loaded>(result).Snapshot);
        Assert.Equal([SettingsPath], Directory.GetFiles(directory));
    }

    [Fact]
    public void SavedStyle_IsUsedByNewStrokesAfterRestart()
    {
        Store().Save(Custom());
        var startup = SettingsStartup.Load(Store(), new NoReporter());
        var controller = new DrawingSessionController(() => startup.Active.Style);

        controller.EnterDrawMode(new ScreenPoint(1, 1));
        controller.ExitDrawMode();

        Assert.Equal(Custom().Style, Assert.Single(controller.CompletedStrokes).Style);
    }

    [Fact]
    public void UnreadableSettings_ShowReasonAndRecoveryCopy()
    {
        Directory.CreateDirectory(directory);
        File.WriteAllText(SettingsPath, "damaged");
        var messages = new List<string>();

        SettingsStartup.Load(Store(), new SettingsFailureDialog(messages.Add));

        var message = Assert.Single(messages);
        Assert.Contains("started with defaults", message);
        Assert.Contains(Assert.Single(Directory.GetFiles(directory, "settings.unreadable-*.json")), message);
    }

    [Fact]
    public void Save_WritesVersionedSchema()
    {
        Store().Save(Custom());

        var json = File.ReadAllText(SettingsPath);

        Assert.Contains("\"schemaVersion\": 1", json);
        Assert.Contains("\"color\": \"#00FF7F\"", json);
        Assert.Contains("\"width\": 12", json);
        Assert.Contains("\"drawShortcut\": \"Ctrl+Shift+D\"", json);
        Assert.Contains("\"shortcut\": \"Alt+Shift+F3\"", json);
        Assert.Contains("\"action\": null", json);
    }

    [Fact]
    public void Save_ReplacesPreviousSettings()
    {
        Store().Save(Custom());
        Store().Save(SettingsSnapshot.Default);

        Assert.Equal(SettingsSnapshot.Default, Loaded(Store().Load()));
    }

    [Theory]
    [InlineData("not json")]
    [InlineData("")]
    [InlineData("[]")]
    [InlineData("{\"schemaVersion\": 1}")]
    [InlineData("{\"schemaVersion\": \"1\"}")]
    public void Load_MalformedFile_IsUnreadableAndKeepsOriginalAndRecoveryCopy(string content)
    {
        WriteSettings(content);

        var result = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Equal(content, File.ReadAllText(SettingsPath));
        Assert.Equal(Path.Combine(directory, "settings.unreadable-20260925T120000000Z.json"), result.RecoveryCopy);
        Assert.Equal(content, File.ReadAllText(result.RecoveryCopy!));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(0)]
    public void Load_UnsupportedSchemaVersion_IsUnreadableWithVersionInReason(int version)
    {
        var content = ValidJson().Replace("\"schemaVersion\": 1", $"\"schemaVersion\": {version}");
        WriteSettings(content);

        var result = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Contains($"version {version} is not supported", result.Reason);
        Assert.Equal(content, File.ReadAllText(SettingsPath));
    }

    [Theory]
    [InlineData("\"#FF4500\"", "\"orange\"", "drawing.color")]
    [InlineData("\"width\": 4", "\"width\": 21", "drawing.width 21")]
    [InlineData("\"width\": 4", "\"width\": 0", "drawing.width 0")]
    [InlineData("\"Ctrl+Alt+X\"", "\"Ctrl+X\"", "clearShortcut 'Ctrl+X'")]
    [InlineData("\"Ctrl+Alt+X\"", "\"Win+Ctrl+X\"", "clearShortcut 'Win+Ctrl+X'")]
    [InlineData("\"Ctrl+Alt+X\"", "\"Ctrl+Alt+Z\"", "same shortcut")]
    [InlineData("\"type\": \"sound\"", "\"type\": \"video\"", "'video' is not supported")]
    [InlineData("\"file\": \"abc.wav\"", "\"file\": \"..\\\\abc.wav\"", "Slot 1 sound is invalid")]
    [InlineData("{ \"slot\": 8, \"action\": null }", "{ \"slot\": 7, \"action\": null }", "slots 1–8")]
    public void Load_InvalidSettings_IsUnreadableWithReason(string find, string replace, string reason)
    {
        var content = ValidJson();
        Assert.Contains(find, content);
        WriteSettings(content.Replace(find, replace));

        var result = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Contains(reason, result.Reason);
    }

    [Fact]
    public void Load_HandWrittenValidFile_Loads()
    {
        WriteSettings(ValidJson());

        var snapshot = Loaded(Store().Load());

        var sound = Assert.IsType<SoundAction>(snapshot.Slots[0].Action);
        Assert.Equal(new SoundReference("abc.wav", "applause.wav"), sound.Sound);
        Assert.Equal("Ctrl+Alt+1", sound.Shortcut!.ToString());
    }

    [Fact]
    public void Load_SameDamagedFileTwice_KeepsOneRecoveryCopy()
    {
        WriteSettings("damaged");

        var first = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());
        time.Advance(TimeSpan.FromMinutes(1));
        var second = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Equal(first.RecoveryCopy, second.RecoveryCopy);
        Assert.Single(Directory.GetFiles(directory, "settings.unreadable-*.json"));
    }

    [Fact]
    public void Load_DifferentDamagedFileAtSameTime_DoesNotOverwriteEarlierCopy()
    {
        WriteSettings("damaged one");
        var first = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());
        WriteSettings("damaged two");

        var second = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.NotEqual(first.RecoveryCopy, second.RecoveryCopy);
        Assert.Equal("damaged one", File.ReadAllText(first.RecoveryCopy!));
        Assert.Equal("damaged two", File.ReadAllText(second.RecoveryCopy!));
    }

    [Fact]
    public void StartupWithDamagedFile_UsesDefaultsReportsAndLeavesFileUntilSave()
    {
        WriteSettings("damaged");
        var reporter = new RecordingReporter();

        var startup = SettingsStartup.Load(Store(), reporter);

        Assert.Equal(SettingsSnapshot.Default, startup.Active);
        Assert.False(startup.SavedSettingsLoaded);
        Assert.Single(reporter.Reasons);
        Assert.Equal("damaged", File.ReadAllText(SettingsPath));

        Store().Save(Custom());

        Assert.Equal(Custom(), Loaded(Store().Load()));
        Assert.Equal("damaged", File.ReadAllText(Assert.Single(Directory.GetFiles(directory, "settings.unreadable-*.json"))));
    }

    [Fact]
    public void Save_WhenTemporaryFileCannotBeWritten_ThrowsAndKeepsPreviousSettings()
    {
        Store().Save(Custom());
        var before = File.ReadAllBytes(SettingsPath);
        Directory.CreateDirectory(SettingsPath + ".tmp");

        Assert.Throws<SettingsStoreException>(() => Store().Save(SettingsSnapshot.Default));

        Assert.Equal(before, File.ReadAllBytes(SettingsPath));
        Assert.Equal(Custom(), Loaded(Store().Load()));
    }

    [Fact]
    public void Save_WhenReplaceFails_ThrowsKeepsPreviousSettingsAndRemovesTemporaryFile()
    {
        Store().Save(Custom());
        var before = File.ReadAllBytes(SettingsPath);
        File.SetAttributes(SettingsPath, FileAttributes.ReadOnly);

        Assert.Throws<SettingsStoreException>(() => Store().Save(SettingsSnapshot.Default));

        Assert.Equal(before, File.ReadAllBytes(SettingsPath));
        Assert.False(File.Exists(SettingsPath + ".tmp"));
    }

    [Fact]
    public void Save_OverwritesTemporaryFileLeftByCrash()
    {
        Directory.CreateDirectory(directory);
        File.WriteAllText(SettingsPath + ".tmp", "partial write");

        Store().Save(Custom());

        Assert.Equal(Custom(), Loaded(Store().Load()));
        Assert.False(File.Exists(SettingsPath + ".tmp"));
    }

    [Fact]
    public void Load_WhenFileIsLocked_IsUnreadableWithoutRecoveryCopy()
    {
        WriteSettings(ValidJson());
        using var locked = new FileStream(SettingsPath, FileMode.Open, FileAccess.Read, FileShare.None);

        var result = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Null(result.RecoveryCopy);
        Assert.Empty(Directory.GetFiles(directory, "settings.unreadable-*.json"));
    }

    private JsonSettingsStore Store() => new(directory, time);

    private void WriteSettings(string content)
    {
        Directory.CreateDirectory(directory);
        File.WriteAllText(SettingsPath, content, new UTF8Encoding(false));
    }

    private static SettingsSnapshot Loaded(SettingsLoadResult result) =>
        Assert.IsType<SettingsLoadResult.Loaded>(result).Snapshot;

    private static SettingsSnapshot Custom()
    {
        var slots = Enumerable.Range(1, 8).Select(ActionSlot.Empty).ToArray();
        slots[0] = new ActionSlot(1, new SoundAction(new SoundReference("abc.wav", "applause.wav"), Keys("Ctrl+Alt+1")));
        slots[2] = new ActionSlot(3, new SoundAction(new SoundReference("abc.wav", "applause.wav"), Keys("Alt+Shift+F3")));
        return SettingsSnapshot.Validate(
            new DrawingStyle(new HexColor(0x00, 0xFF, 0x7F), new StrokeWidth(12)),
            Keys("Ctrl+Shift+D"),
            Keys("Ctrl+Alt+Shift+C"),
            slots).Snapshot!;
    }

    private static Shortcut Keys(string text) =>
        Shortcut.TryParse(text, out var shortcut, out _) ? shortcut : throw new ArgumentException(text);

    private static string ValidJson() => """
        {
          "schemaVersion": 1,
          "drawing": {
            "color": "#FF4500",
            "width": 4,
            "drawShortcut": "Ctrl+Alt+Z",
            "clearShortcut": "Ctrl+Alt+X"
          },
          "slots": [
            { "slot": 1, "action": { "type": "sound", "file": "abc.wav", "name": "applause.wav", "shortcut": "Ctrl+Alt+1" } },
            { "slot": 2, "action": null },
            { "slot": 3, "action": null },
            { "slot": 4, "action": null },
            { "slot": 5, "action": null },
            { "slot": 6, "action": null },
            { "slot": 7, "action": null },
            { "slot": 8, "action": null }
          ]
        }
        """;

    private sealed class NoReporter : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy) =>
            throw new InvalidOperationException(reason);
    }

    private sealed class RecordingReporter : ISettingsFailureReporter
    {
        public List<string> Reasons { get; } = [];

        public void SettingsUnreadable(string reason, string? recoveryCopy) => Reasons.Add(reason);
    }
}
