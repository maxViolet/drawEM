using System.Text;
using System.Text.Json;
using DrawEM.App.Application.Drawing;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Infrastructure.Settings;

namespace DrawEM.Tests.Infrastructure.Settings;

public sealed class JsonSettingsStoreTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
    private readonly ManualTimeProvider time = new();

    private string SettingsPath => Path.Combine(directory, "settings.json");

    private string BackupPath => Path.Combine(directory, "settings.schema1-backup.json");

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

        Assert.Contains("\"schemaVersion\": 2", json);
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
    [InlineData(3)]
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
    public void Load_Schema1File_KeepsSlotsShortcutsAndFilesAndWritesNothing()
    {
        WriteSettings(Schema1CustomJson());
        var before = File.ReadAllBytes(SettingsPath);

        var snapshot = Loaded(Store().Load());

        Assert.Equal(Custom(), snapshot);
        Assert.Equal(before, File.ReadAllBytes(SettingsPath));
        Assert.Equal([SettingsPath], Directory.GetFiles(directory));
    }

    [Fact]
    public void Save_ThenLoad_RoundTripsMixedSoundEffectAndEmptySlots()
    {
        Store().Save(Mixed());

        Assert.Equal(Mixed(), Loaded(Store().Load()));
    }

    [Fact]
    public void Save_WritesEffectAsTypeEffectIdAndShortcutOnly()
    {
        Store().Save(Mixed());

        using var document = JsonDocument.Parse(File.ReadAllBytes(SettingsPath));
        var actions = document.RootElement.GetProperty("slots").EnumerateArray()
            .Select(slot => slot.GetProperty("action"))
            .ToArray();
        var effect = actions[1];
        Assert.Equal(["effectId", "shortcut", "type"], effect.EnumerateObject().Select(property => property.Name).Order());
        Assert.Equal("effect", effect.GetProperty("type").GetString());
        Assert.Equal("confetti", effect.GetProperty("effectId").GetString());
        Assert.Equal("Ctrl+Alt+2", effect.GetProperty("shortcut").GetString());
        Assert.Equal("focus-ring", actions[2].GetProperty("effectId").GetString());
    }

    [Fact]
    public void Load_HandWrittenSchema2File_Loads()
    {
        WriteSettings(Schema2Json());

        var snapshot = Loaded(Store().Load());

        Assert.Equal(new SoundReference("abc.wav", "applause.wav"), Assert.IsType<SoundAction>(snapshot.Slots[0].Action).Sound);
        var effect = Assert.IsType<EffectAction>(snapshot.Slots[1].Action);
        Assert.Equal(new EffectId("confetti"), effect.Effect);
        Assert.Equal("Ctrl+Alt+2", effect.Shortcut!.ToString());
        Assert.Equal(new EffectId("focus-ring"), Assert.IsType<EffectAction>(snapshot.Slots[2].Action).Effect);
        Assert.True(snapshot.Slots[3].IsEmpty);
    }

    [Theory]
    [InlineData("\"effectId\": \"confetti\"", "\"effectId\": \"sparkles\"", "Slot 2 uses unsupported effect \"sparkles\"")]
    [InlineData("\"effectId\": \"confetti\"", "\"effectId\": \" \"", "Slot 2 effect is invalid")]
    [InlineData("\"effectId\": \"confetti\"", "\"effect\": \"confetti\"", "Missing property 'effectId'")]
    [InlineData("\"type\": \"effect\"", "\"type\": \"video\"", "Slot 2 action type 'video' is not supported")]
    public void Load_InvalidSchema2Settings_IsUnreadableWithReasonAndKeepsFile(string find, string replace, string reason)
    {
        var content = Schema2Json();
        Assert.Contains(find, content);
        WriteSettings(content.Replace(find, replace));

        var result = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Contains(reason, result.Reason);
        Assert.Equal(content.Replace(find, replace), File.ReadAllText(SettingsPath));
        Assert.Equal(content.Replace(find, replace), File.ReadAllText(result.RecoveryCopy!));
    }

    [Fact]
    public void Load_EffectInSchema1File_IsUnreadable()
    {
        WriteSettings(Schema2Json().Replace("\"schemaVersion\": 2", "\"schemaVersion\": 1"));

        var result = Assert.IsType<SettingsLoadResult.Unreadable>(Store().Load());

        Assert.Contains("Slot 2 action type 'effect' is not supported in schema version 1", result.Reason);
    }

    [Fact]
    public void FirstSaveOverSchema1File_BacksItUpThenWritesSchema2()
    {
        WriteSettings(Schema1CustomJson());
        var original = File.ReadAllBytes(SettingsPath);
        var loaded = Loaded(Store().Load());

        Store().Save(loaded);

        Assert.Equal(original, File.ReadAllBytes(BackupPath));
        Assert.Equal(2, SchemaVersion(File.ReadAllBytes(SettingsPath)));
        Assert.Equal(loaded, Loaded(Store().Load()));
    }

    [Fact]
    public void LaterSaves_NeverOverwriteTheMigrationBackup()
    {
        WriteSettings(Schema1CustomJson());
        var original = File.ReadAllBytes(SettingsPath);
        Store().Save(Custom());
        Store().Save(Mixed());

        // A rollback restored an older schema 1 file, then the user upgraded again.
        WriteSettings(ValidJson());
        Store().Save(Mixed());

        Assert.Equal(original, File.ReadAllBytes(BackupPath));
        Assert.Equal(Mixed(), Loaded(Store().Load()));
    }

    [Fact]
    public void Save_WithoutSchema1File_CreatesNoBackup()
    {
        Store().Save(Custom());
        Store().Save(Mixed());

        Assert.Equal([SettingsPath], Directory.GetFiles(directory));
    }

    [Fact]
    public void Save_WhenBackupCannotBeCreated_ThrowsAndLeavesSchema1File()
    {
        WriteSettings(Schema1CustomJson());
        var before = File.ReadAllBytes(SettingsPath);
        Directory.CreateDirectory(BackupPath);

        var failure = Assert.Throws<SettingsStoreException>(() => Store().Save(Mixed()));

        Assert.Contains("backup", failure.Message);
        Assert.Equal(before, File.ReadAllBytes(SettingsPath));
        Assert.Equal([SettingsPath], Directory.GetFiles(directory));
    }

    [Fact]
    public void Save_WhenReplaceFailsAfterBackup_KeepsSchema1FileAndBackup()
    {
        WriteSettings(Schema1CustomJson());
        var before = File.ReadAllBytes(SettingsPath);
        File.SetAttributes(SettingsPath, FileAttributes.ReadOnly);

        Assert.Throws<SettingsStoreException>(() => Store().Save(Mixed()));

        Assert.Equal(before, File.ReadAllBytes(SettingsPath));
        Assert.Equal(before, File.ReadAllBytes(BackupPath));
        Assert.False(File.Exists(SettingsPath + ".tmp"));
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
        Assert.False(File.Exists(BackupPath));
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
        slots[2] = new ActionSlot(3, new SoundAction(new SoundReference("def.mp3", "horn.mp3"), Keys("Alt+Shift+F3")));
        return SettingsSnapshot.Validate(
            new DrawingStyle(new HexColor(0x00, 0xFF, 0x7F), new StrokeWidth(12)),
            Keys("Ctrl+Shift+D"),
            Keys("Ctrl+Alt+Shift+C"),
            slots).Snapshot!;
    }

    private static SettingsSnapshot Mixed()
    {
        var slots = Enumerable.Range(1, 8).Select(ActionSlot.Empty).ToArray();
        slots[0] = new ActionSlot(1, new SoundAction(new SoundReference("abc.wav", "applause.wav"), Keys("Ctrl+Alt+1")));
        slots[1] = new ActionSlot(2, new EffectAction(new EffectId("confetti"), Keys("Ctrl+Alt+2")));
        slots[2] = new ActionSlot(3, new EffectAction(new EffectId("focus-ring"), Keys("Alt+Shift+F3")));
        slots[4] = new ActionSlot(5, new SoundAction(new SoundReference("def.mp3", "horn.mp3"), Keys("Ctrl+Alt+5")));
        var defaults = SettingsSnapshot.Default;
        return SettingsSnapshot.Validate(defaults.Style, defaults.DrawShortcut, defaults.ClearShortcut, slots).Snapshot!;
    }

    private static int SchemaVersion(byte[] json)
    {
        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("schemaVersion").GetInt32();
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

    /// <summary><see cref="Custom"/> as a v3 app wrote it.</summary>
    private static string Schema1CustomJson() => """
        {
          "schemaVersion": 1,
          "drawing": {
            "color": "#00FF7F",
            "width": 12,
            "drawShortcut": "Ctrl+Shift+D",
            "clearShortcut": "Ctrl+Alt+Shift+C"
          },
          "slots": [
            { "slot": 1, "action": { "type": "sound", "file": "abc.wav", "name": "applause.wav", "shortcut": "Ctrl+Alt+1" } },
            { "slot": 2, "action": null },
            { "slot": 3, "action": { "type": "sound", "file": "def.mp3", "name": "horn.mp3", "shortcut": "Alt+Shift+F3" } },
            { "slot": 4, "action": null },
            { "slot": 5, "action": null },
            { "slot": 6, "action": null },
            { "slot": 7, "action": null },
            { "slot": 8, "action": null }
          ]
        }
        """;

    private static string Schema2Json() => """
        {
          "schemaVersion": 2,
          "drawing": {
            "color": "#FF4500",
            "width": 4,
            "drawShortcut": "Ctrl+Alt+Z",
            "clearShortcut": "Ctrl+Alt+X"
          },
          "slots": [
            { "slot": 1, "action": { "type": "sound", "file": "abc.wav", "name": "applause.wav", "shortcut": "Ctrl+Alt+1" } },
            { "slot": 2, "action": { "type": "effect", "effectId": "confetti", "shortcut": "Ctrl+Alt+2" } },
            { "slot": 3, "action": { "type": "effect", "effectId": "focus-ring", "shortcut": "Ctrl+Alt+3" } },
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
